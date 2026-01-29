import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn } from '@angular/router';
import { AuthService } from 'app/core/auth/auth.service';
import { map } from 'rxjs';

export const AuthGuard: CanActivateFn | CanActivateChildFn = (route, state) => {
    const authService = inject(AuthService);

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
