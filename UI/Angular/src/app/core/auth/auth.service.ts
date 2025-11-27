import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserService } from 'app/core/user/user.service';
import { catchError, Observable, of, map, BehaviorSubject, interval, shareReplay, tap } from 'rxjs';

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
     * Sign in - Redirect to BFF login endpoint
     */
    signIn(): void {
        window.location.href = '/api/Account/Login';
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
        
        // Perform full page redirect to logout endpoint
        window.location.href = '/api/Account/Logout';
        
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

        this._authCheckCache$ = this._httpClient.get<UserProfile>('/api/User').pipe(
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
