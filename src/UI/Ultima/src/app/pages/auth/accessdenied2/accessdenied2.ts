import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';

@Component({
    selector: 'app-access-2',
    standalone: true,
    imports: [ButtonModule, RouterModule, RippleModule, AppConfigurator],
    templateUrl: './accessdenied2.html'
})
export class AccessDenied2 {}
