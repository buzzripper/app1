import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'top-searches-widget',
    standalone: true,
    imports: [CommonModule, MenuModule, ButtonModule, RippleModule],
    templateUrl: './topsearcheswidget.html'
})
export class TopSearchesWidget {
    items = [
        { label: 'View', icon: 'pi pi-eye' },
        { label: 'Export', icon: 'pi pi-upload' }
    ];

    searches = [
        {
            name: 'Mat Orange Case',
            rate: 82
        },
        {
            name: 'Space T-Shirt',
            rate: 78
        },
        {
            name: 'Orange Black Hoodie',
            rate: 61
        },
        {
            name: 'Wonders Notebook',
            rate: 48
        },
        {
            name: 'Robots T-Shirt',
            rate: 34
        },
        {
            name: 'Green Portal Sticker',
            rate: 11
        }
    ];
}
