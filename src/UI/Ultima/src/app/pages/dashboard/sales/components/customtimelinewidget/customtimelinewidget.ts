import { Component } from '@angular/core';
import { TimelineModule } from 'primeng/timeline';
import { RippleModule } from 'primeng/ripple';
import { MenuModule } from 'primeng/menu';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';

@Component({
    selector: 'custom-timeline-widget',
    standalone: true,
    imports: [CommonModule, TimelineModule, ButtonModule, CardModule, MenuModule, RippleModule],
    templateUrl: './customtimelinewidget.html',
    styles: `
        :host ::ng-deep {
            .p-timeline-event-opposite {
                flex: 0;
                padding: 0 !important;
            }
            .p-card {
                box-shadow: none;
            }
        }
    `
})
export class CustomTimelineWidget {
    items = [
        { label: 'Update', icon: 'pi pi-fw pi-refresh' },
        { label: 'Edit', icon: 'pi pi-fw pi-pencil' }
    ];

    timelineEvents = [
        { status: 'Ordered', date: '15/10/2024 10:30', icon: 'pi pi-shopping-cart', color: '#E91E63', description: 'Richard Jones (C8012) has ordered a blue t-shirt for $79.' },
        { status: 'Processing', date: '15/10/2024 14:00', icon: 'pi pi-cog', color: '#FB8C00', description: 'Order #99207 has processed succesfully.' },
        { status: 'Shipped', date: '15/10/2024 16:15', icon: 'pi pi-compass', color: '#673AB7', description: 'Order #99207 has shipped with shipping code 2222302090.' },
        { status: 'Delivered', date: '16/10/2024 10:00', icon: 'pi pi-check-square', color: '#0097A7', description: 'Richard Jones (C8012) has recieved his blue t-shirt.' }
    ];
}
