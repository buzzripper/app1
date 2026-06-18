import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'saas-table-widget',
    standalone: true,
    imports: [CommonModule, AvatarModule, ButtonModule, RippleModule],
    templateUrl: './saastablewidget.html',
    host: {
        class: 'col-span-12 md:col-span-8'
    }
})
export class SaasTableWidget {
    @Input() filteredTeamMembers: any[] = [];
}
