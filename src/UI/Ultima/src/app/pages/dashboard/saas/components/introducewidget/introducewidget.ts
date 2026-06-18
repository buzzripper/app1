import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'introduce-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, RippleModule],
    templateUrl: './introducewidget.html'
})
export class IntroduceWidget {
    todos = [
        {
            name: 'tasks',
            quantity: 72
        },
        {
            name: 'production',
            quantity: 4
        },
        {
            name: 'tests',
            quantity: 18
        },
        {
            name: 'meetings',
            quantity: 13
        }
    ];
}
