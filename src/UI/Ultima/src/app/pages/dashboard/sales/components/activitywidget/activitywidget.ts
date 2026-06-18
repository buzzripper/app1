import { Component } from '@angular/core';
import { MenuModule } from 'primeng/menu';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'activity-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, RippleModule, MenuModule],
    templateUrl: './activitywidget.html'
})
export class ActivityWidget {
    items = [
        { label: 'Update', icon: 'pi pi-fw pi-refresh' },
        { label: 'Edit', icon: 'pi pi-fw pi-pencil' }
    ];

    activities = [
        {
            title: 'Income',
            description: '30 November, 16.20',
            progressColor: 'bg-yellow-500'
        },
        {
            title: 'Tax',
            description: '1 December, 15.27',
            progressColor: 'bg-pink-500'
        },
        {
            title: 'Invoices',
            description: '1 December, 15.28',
            progressColor: 'bg-cyan-600'
        },
        {
            title: 'Expanses',
            description: '3 December, 09.15',
            progressColor: 'bg-cyan-600'
        },
        {
            title: 'Bonus',
            description: '1 December, 23.55',
            progressColor: 'bg-cyan-600'
        },
        {
            title: 'Revenue',
            description: '30 November, 16.20',
            progressColor: 'bg-pink-500'
        }
    ];
}
