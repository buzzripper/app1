import { Component, computed, inject } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Ripple } from 'primeng/ripple';
import { RouterModule } from '@angular/router';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';
import { Avatar } from 'primeng/avatar';
import { InputGroup } from 'primeng/inputgroup';
import { InputGroupAddon } from 'primeng/inputgroupaddon';
import { InputText } from 'primeng/inputtext';
import { AuthService } from '@/app/core/auth/auth.service';
import { UserService } from '@/app/core/user/user.service';

@Component({
    standalone: true,
    selector: 'app-lockscreen',
    imports: [ButtonModule, Ripple, RouterModule, AppConfigurator, Avatar, InputGroup, InputGroupAddon, InputText],
    templateUrl: './lockscreen.html'
})
export class LockScreen {
    private readonly authService = inject(AuthService);
    private readonly userService = inject(UserService);

    readonly user = computed(() => this.userService.user());

    signOut(): void {
        this.authService.signOut();
    }
}
