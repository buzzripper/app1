import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from '../auth.service';
import { getTenantSlug } from '../tenant.utils';

export const authGuard: CanActivateFn | CanActivateChildFn = (_route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!getTenantSlug()) {
        return router.parseUrl('/auth/login');
    }

    return authService.check().pipe(
        map((isAuthenticated) => {
            if (!isAuthenticated) {
                authService.signIn(`${window.location.origin}${state.url}`);
                return false;
            }

            return true;
        })
    );
};
