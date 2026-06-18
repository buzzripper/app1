import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';

@Component({
    selector: 'app-error-2',
    imports: [ButtonModule, RippleModule, RouterModule, AppConfigurator],
    standalone: true,
    templateUrl: './error2.html'
})
export class Error2 {}
