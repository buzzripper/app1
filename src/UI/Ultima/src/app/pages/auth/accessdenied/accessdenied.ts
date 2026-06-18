import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { LayoutService } from '@/app/layout/service/layout.service';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';

@Component({
    selector: 'app-access',
    standalone: true,
    imports: [ButtonModule, RouterModule, RippleModule, AppConfigurator],
    templateUrl: './accessdenied.html'
})
export class AccessDenied {
    layoutService = inject(LayoutService);
}
