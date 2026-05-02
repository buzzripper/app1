import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { UserService } from '@/app/core/user/user.service';
import { catchError, interval, map, Observable, of, shareReplay, tap } from 'rxjs';
import { environment } from '@/environments/environment';

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
    private readonly httpClient = inject(HttpClient);
    private readonly userService = inject(UserService);

    private readonly authenticatedState = signal<boolean | null>(null);
    readonly authenticated = this.authenticatedState.asReadonly();

    private authCheckCache$: Observable<boolean> | null = null;
    private readonly cacheTimeout = 5000;
    private lastCheckTime = 0;
    private readonly apiBaseUrl = environment.apiBaseUrl;

    constructor() {
        this.check().subscribe();
        interval(60000).subscribe(() => this.forceRefresh().subscribe());
    }

    signIn(returnUrl?: string): void {
        const currentUrl = returnUrl || window.location.href;
        const encodedReturnUrl = encodeURIComponent(currentUrl);
        window.location.href = `${this.apiBaseUrl}/api/Account/Login?returnUrl=${encodedReturnUrl}`;
    }

    signOut(returnUrl?: string): Observable<boolean> {
        this.authenticatedState.set(false);
        this.userService.setUser(null);
        this.authCheckCache$ = null;

        const redirectUrl = returnUrl || `${window.location.origin}/auth/sign-out`;
        const encodedReturnUrl = encodeURIComponent(redirectUrl);
        window.location.href = `${this.apiBaseUrl}/api/Account/Logout?returnUrl=${encodedReturnUrl}`;

        return of(true);
    }

    check(): Observable<boolean> {
        const now = Date.now();
        if (this.authCheckCache$ && now - this.lastCheckTime < this.cacheTimeout) {
            return this.authCheckCache$;
        }

        return this.forceRefresh();
    }

    forceRefresh(): Observable<boolean> {
        this.lastCheckTime = Date.now();

        this.authCheckCache$ = this.httpClient.get<UserProfile>(`${this.apiBaseUrl}/api/User`).pipe(
            tap((profile) => {
                this.authenticatedState.set(profile.isAuthenticated);
                this.userService.setUser(profile.isAuthenticated ? this.mapUser(profile) : null);
            }),
            map((profile) => profile.isAuthenticated),
            catchError(() => {
                this.authenticatedState.set(false);
                this.userService.setUser(null);
                return of(false);
            }),
            shareReplay(1)
        );

        return this.authCheckCache$;
    }

    private mapUser(profile: UserProfile) {
        const name = profile.claims?.find((claim) => claim.type === profile.nameClaimType)?.value || 'User';
        const email =
            profile.claims?.find(
                (claim) =>
                    claim.type === 'email' || claim.type === 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
            )?.value || '';

        return {
            id: email || name,
            name,
            email,
            avatar: '/demo/images/avatar/amyelsner.png',
            status: 'Authenticated'
        };
    }
}
