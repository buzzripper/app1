import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'upgrade-widget',
    standalone: true,
    imports: [ButtonModule, RippleModule],
    templateUrl: './upgradewidget.html'
})
export class UpgradeWidget {}
