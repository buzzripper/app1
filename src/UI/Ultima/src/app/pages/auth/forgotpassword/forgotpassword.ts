import { Component } from '@angular/core';
import { InputText } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { RouterModule } from '@angular/router';
import { RippleModule } from 'primeng/ripple';
import { InputGroup } from 'primeng/inputgroup';
import { InputGroupAddon } from 'primeng/inputgroupaddon';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';

@Component({
    standalone: true,
    selector: 'app-forgot-password',
    imports: [InputText, ButtonModule, RippleModule, RouterModule, InputGroup, InputGroupAddon, AppConfigurator],
    templateUrl: './forgotpassword.html'
})
export class ForgotPassword {}
