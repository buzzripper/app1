import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { MenuModule } from 'primeng/menu';

@Component({
    selector: 'expenses-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, RippleModule, MenuModule],
    templateUrl: './expenseswidget.html'
})
export class ExpensesWidget {
    expenses = [
        {
            icon: 'pi pi-cloud',
            amount: '30.247',
            category: 'Cloud Infrastructure'
        },
        {
            icon: 'pi pi-tag',
            amount: '29.550',
            category: 'General Goods'
        },
        {
            icon: 'pi pi-desktop',
            amount: '16.660',
            category: 'Consumer Electronics'
        },
        {
            icon: 'pi pi-compass',
            amount: '5.801',
            category: 'Incalculables'
        }
    ];

    items = [
        { label: 'View', icon: 'pi pi-eye' },
        { label: 'Export', icon: 'pi pi-upload' }
    ];
}
