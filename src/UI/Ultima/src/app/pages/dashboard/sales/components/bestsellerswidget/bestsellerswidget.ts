import { Component } from '@angular/core';
import { MenuModule } from 'primeng/menu';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'best-sellers-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, RippleModule, MenuModule],
    templateUrl: './bestsellerswidget.html'
})
export class BestSellersWidget {
    items = [
        { label: 'Update', icon: 'pi pi-fw pi-refresh' },
        { label: 'Edit', icon: 'pi pi-fw pi-pencil' }
    ];

    sellers = [
        {
            name: 'Blue Band',
            image: '/demo/images/product/blue-band.jpg'
        },
        {
            name: 'Bracelet',
            image: '/demo/images/product/bracelet.jpg'
        },
        {
            name: 'Black Watch',
            image: '/demo/images/product/black-watch.jpg'
        },
        {
            name: 'Bamboo Watch',
            image: '/demo/images/product/bamboo-watch.jpg'
        },
        {
            name: 'Blue T-Shirt',
            image: '/demo/images/product/blue-t-shirt.jpg'
        },
        {
            name: 'Game Controller',
            image: '/demo/images/product/game-controller.jpg'
        },
        {
            name: 'Phone Case',
            image: '/demo/images/product/gold-phone-case.jpg'
        }
    ];
}
