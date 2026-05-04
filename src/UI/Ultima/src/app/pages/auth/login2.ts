import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { AppConfigurator } from '@/app/layout/components/app.configurator';
import { AuthService } from '@/app/core/auth/auth.service';

@Component({
    selector: 'app-login-2',
    standalone: true,
    imports: [ButtonModule, RouterModule, RippleModule, AppConfigurator],
    template: `<div class="h-screen flex w-full bg-surface-50 dark:bg-surface-950">
            <div class="flex flex-1 flex-col bg-surface-50 dark:bg-surface-950 items-center justify-center">
                <div class="w-11/12 sm:w-120">
                    <div class="flex flex-col">
                        <div style="height: 56px; width: 56px" class="bg-primary rounded-full flex items-center justify-center">
                            <i class="pi pi-sign-in text-surface-0 dark:text-surface-900 text-4xl!"></i>
                        </div>
                        <div class="mt-6">
                            <h1 class="m-0 text-primary font-semibold text-4xl">Welcome back!</h1>
                            <span class="block text-surface-700 dark:text-surface-100 mt-2">Use the shared sign-in flow to access Ultima.</span>
                        </div>
                    </div>
                    <div class="flex flex-col gap-4 mt-12">
                        <div class="text-sm text-surface-700 dark:text-surface-100">The backend/BFF will prompt for credentials and return here when sign-in completes.</div>
                        <div>
                            <button pButton pRipple class="w-full" label="SIGN IN" (click)="signIn()"></button>
                        </div>
                        <div>
                            <button pButton pRipple class="w-full text-primary-500" text label="USE PRIMARY LOGIN" [routerLink]="['/auth/login']"></button>
                        </div>
                    </div>
                </div>
            </div>
            <div [style]="{ backgroundImage: 'url(/demo/images/pages/accessDenied-bg.jpg)' }" class="hidden lg:flex flex-1 items-center justify-center bg-cover">
                <img src="/layout/images/logo/vector_logo.png" alt="" />
            </div>
        </div>
        <app-configurator simple />`
})
export class Login2 {
    private readonly authService = inject(AuthService);

    signIn(): void {
        this.authService.signIn();
    }
}
