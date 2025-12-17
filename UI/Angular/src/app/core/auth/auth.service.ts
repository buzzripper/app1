import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserService } from 'app/core/user/user.service';
import { catchError, Observable, of, map, BehaviorSubject, interval, shareReplay } from 'rxjs';
import { environment } from '../../../environments/environment';

interface Claim {
    type: string;
    value: string;
}

interface UserProfile {
    isAuthenticated: boolean;
    nameClaimType: string;
    roleClaimType: string;
    claims: Claim[];
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private _authenticatedSubject = new BehaviorSubject<boolean | null>(null);
    public authenticated$ = this._authenticatedSubject.asObservable();

    private _authCheckCache$: Observable<boolean> | null = null;
    private _cacheTimeout = 5000; // 5 seconds
    private _lastCheckTime = 0;

    private _httpClient = inject(HttpClient);
    private _userService = inject(UserService);

    private readonly _apiBaseUrl = environment.apiBaseUrl;

    constructor() {
        // Initial check on startup
        this.check().subscribe();

        // Periodic refresh (optional)
        interval(60000).subscribe(() => this.forceRefresh());
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Sign in - Redirect to BFF login endpoint with return URL back to Angular app
     */
    signIn(returnUrl?: string): void {
        const currentUrl = returnUrl || window.location.href;
        const encodedReturnUrl = encodeURIComponent(currentUrl);
        window.location.href = `${this._apiBaseUrl}/api/Account/Login?returnUrl=${encodedReturnUrl}`;
    }

    /**
     * Sign out - Redirect to BFF logout endpoint (full page navigation)
     * Note: Using GET request since the server redirect chain doesn't work with AJAX POST
     */
    signOut(): Observable<any> {
        // Clear local state immediately
        this._authenticatedSubject.next(false);
        this._userService.user = null;
        this._authCheckCache$ = null;
        
        // Redirect to Angular's sign-out page after BFF logout completes
        const signOutPageUrl = `${window.location.origin}/sign-out`;
        const returnUrl = encodeURIComponent(signOutPageUrl);
        
        // Perform full page redirect to logout endpoint
        window.location.href = `${this._apiBaseUrl}/api/Account/Logout?returnUrl=${returnUrl}`;
        
        // Return observable for compatibility with existing code
        return of(true);
    }

    check(): Observable<boolean> {
        const now = Date.now();

        // If we have a recent cached request, reuse it
        if (this._authCheckCache$ && (now - this._lastCheckTime < this._cacheTimeout)) {
            return this._authCheckCache$;
        }

        // Make fresh request
        return this.forceRefresh();
    }

    forceRefresh(): Observable<boolean> {
        this._lastCheckTime = Date.now();

        // Call the Auth API user endpoint (routed through Portal YARP)
        this._authCheckCache$ = this._httpClient.get<UserProfile>(`${this._apiBaseUrl}/api/auth/user`).pipe(
            map((profile) => {
                this._authenticatedSubject.next(profile.isAuthenticated);

                if (profile.isAuthenticated) {
                    // Update user info...
                    const userName = profile.claims?.find(
                        c => c.type === profile.nameClaimType
                    )?.value || 'User';

                    const email = profile.claims?.find(
                        c => c.type === 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
                    )?.value || '';

                    this._userService.user = {
                        id: email || userName,
                        name: userName,
                        email: email,
                        avatar: 'assets/images/avatars/brian-hughes.jpg',
                        status: 'online'
                    };
                } else {
                    this._userService.user = null;
                }

                return profile.isAuthenticated;
            }),
            catchError(() => {
                this._authenticatedSubject.next(false);
                this._userService.user = null;
                return of(false);
            }),
            shareReplay(1) // Share with multiple subscribers
        );

        return this._authCheckCache$;
    }
}
