import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '@/app/core/auth/auth.service';
import { AppConfigurator } from '@/app/layout/components/app.configurator';

@Component({
    selector: 'app-sign-out',
    standalone: true,
    imports: [ButtonModule, RouterModule, AppConfigurator],
    template: `<div class="h-screen flex w-full bg-surface-50 dark:bg-surface-950">
            <div class="flex flex-1 flex-col items-center justify-center">
                <div class="w-11/12 sm:w-120 text-center border rounded-md border-surface bg-surface-0 dark:bg-surface-900 p-12">
                    <div class="text-2xl font-semibold">You have signed out</div>
                    <div class="text-muted-color mt-4 mb-8">Sign in again to continue.</div>
                    <button pButton type="button" label="SIGN IN" (click)="signInNow()"></button>
                </div>
            </div>
        </div>
        <app-configurator simple />`
})
export class SignOut {
    private readonly authService = inject(AuthService);

    signInNow(): void {
        this.authService.signIn();
    }
}
