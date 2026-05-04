import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from '../auth.service';

export const noAuthGuard: CanActivateFn | CanActivateChildFn = () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    return authService.check().pipe(
        map((authenticated) => {
            if (authenticated) {
                return router.parseUrl('/dashboards');
            }

            return true;
        })
    );
};
