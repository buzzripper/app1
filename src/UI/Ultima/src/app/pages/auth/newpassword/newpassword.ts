import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { InputGroup } from 'primeng/inputgroup';
import { InputGroupAddon } from 'primeng/inputgroupaddon';
import { Ripple } from 'primeng/ripple';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';

@Component({
    selector: 'app-new-password',
    standalone: true,
    imports: [InputTextModule, ButtonModule, RouterModule, FormsModule, InputGroup, InputGroupAddon, Ripple, AppConfigurator],
    templateUrl: './newpassword.html'
})
export class NewPassword {}
