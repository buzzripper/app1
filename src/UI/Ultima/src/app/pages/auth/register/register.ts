import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { InputText } from 'primeng/inputtext';
import { Ripple } from 'primeng/ripple';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputNumberModule } from 'primeng/inputnumber';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [ButtonModule, RouterModule, FormsModule, InputText, Ripple, AppConfigurator, InputGroupModule, InputGroupAddonModule, InputNumberModule],
    templateUrl: './register.html'
})
export class Register {
    value1: string = '';

    value2: string = '';
}
