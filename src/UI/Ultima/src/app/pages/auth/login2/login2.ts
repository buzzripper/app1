import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';
import { AuthService } from '@/app/core/auth/auth.service';

@Component({
    selector: 'app-login-2',
    standalone: true,
    imports: [ButtonModule, RouterModule, RippleModule, AppConfigurator],
    templateUrl: './login2.html'
})
export class Login2 {
    private readonly authService = inject(AuthService);

    signIn(): void {
        this.authService.signIn();
    }
}
