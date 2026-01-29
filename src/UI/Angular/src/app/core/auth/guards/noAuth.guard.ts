import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router } from '@angular/router';
import { AuthService } from 'app/core/auth/auth.service';
import { of, switchMap } from 'rxjs';

export const NoAuthGuard: CanActivateFn | CanActivateChildFn = (
    route,
    state
) => {
    const router: Router = inject(Router);

    // Check the authentication status via BFF
    return inject(AuthService)
        .check()
        .pipe(
            switchMap((authenticated) => {
                // If the user is authenticated, redirect to home
                if (authenticated) {
                    return of(router.parseUrl(''));
                }

                // Allow the access (for login pages, etc.)
                return of(true);
            })
        );
};
