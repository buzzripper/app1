import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';

@Component({
    selector: 'app-error',
    imports: [ButtonModule, RippleModule, RouterModule, AppConfigurator],
    standalone: true,
    templateUrl: './error.html'
})
export class Error {}
