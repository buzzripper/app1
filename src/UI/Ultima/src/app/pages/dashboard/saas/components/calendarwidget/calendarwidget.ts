import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'calendar-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, RippleModule],
    templateUrl: './calendarwidget.html',
    host: {
        class: 'col-span-12 md:col-span-4'
    }
})
export class CalendarWidget {
    @Input() dailyTasks: any[] = [];
}
