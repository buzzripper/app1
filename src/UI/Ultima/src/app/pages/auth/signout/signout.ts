import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '@/app/core/auth/auth.service';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';

@Component({
    selector: 'app-sign-out',
    standalone: true,
    imports: [ButtonModule, RouterModule, AppConfigurator],
    templateUrl: './signout.html'
})
export class SignOut {
    private readonly authService = inject(AuthService);

    signInNow(): void {
        this.authService.signIn();
    }
}
