import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { AppConfigurator } from '@/app/layout/components/app.configurator';
import { AuthService } from '@/app/core/auth/auth.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [ButtonModule, RouterModule, RippleModule, AppConfigurator],
    template: `<div class="min-h-screen flex flex-col bg-cover" [style]="{ backgroundImage: 'url(/demo/images/pages/login-bg.jpg)' }">
            <div class="self-center mt-auto mb-auto">
                <div class="text-center z-50 flex flex-col border rounded-md border-surface bg-surface-0 dark:bg-surface-900 p-12">
                    <span class="text-2xl font-semibold">Welcome</span>
                    <div class="text-muted-color mb-12 px-12">Sign in with your existing App1 account to continue.</div>

                    <div class="w-full flex flex-col gap-4 px-4">
                        <div class="text-sm text-muted-color">Authentication is handled by the shared backend/BFF.</div>
                        <button pButton pRipple class="w-full mt-4 px-4" label="SIGN IN" (click)="signIn()"></button>
                    </div>
                </div>
            </div>
        </div>
        <app-configurator simple />`
})
export class Login {
    private readonly authService = inject(AuthService);

    signIn(): void {
        this.authService.signIn();
    }
}
