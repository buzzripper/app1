import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router } from '@angular/router';
import { AuthService } from 'app/core/auth/auth.service';
import { map } from 'rxjs';
import { getTenantSlug } from 'app/core/auth/tenant.utils';

export const AuthGuard: CanActivateFn | CanActivateChildFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    // If no tenant subdomain is present, don't initiate auth — show an error or landing page
    if (!getTenantSlug()) {
        console.warn('AuthGuard: No tenant subdomain detected — cannot authenticate.');
        return router.parseUrl('/sign-in');
    }

    // Check the authentication status via BFF
    return authService.check().pipe(
        map(isAuthenticated => {
            if (!isAuthenticated) {
                // User is not authenticated - redirect to BFF login
                authService.signIn();  // ← Redirects to /api/Account/Login
                return false;  // Block current navigation
            }

            // User is authenticated - allow navigation
            return true;
        })
    );
};
