import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { TagModule } from 'primeng/tag';

@Component({
    selector: 'contact-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, RippleModule, TagModule, MenuModule],
    templateUrl: './contactwidget.html'
})
export class ContactWidget {
    items = [
        { label: 'New', icon: 'pi pi-fw pi-plus' },
        { label: 'Edit', icon: 'pi pi-fw pi-pencil' },
        { label: 'Delete', icon: 'pi pi-fw pi-trash' }
    ];

    contacts = [
        {
            name: 'Xuxue Feng',
            email: 'feng@ultima.org',
            avatar: '/demo/images/avatar/xuxuefeng.png',
            tags: ['Accounting', 'Sales']
        },
        {
            name: 'Elwin Sharvill',
            email: 'sharvill@ultima.org',
            avatar: '/demo/images/avatar/elwinsharvill.png',
            tags: ['Finance', 'Sales']
        },
        {
            name: 'Anna Fali',
            email: 'fali@ultima.org',
            avatar: '/demo/images/avatar/asiyajavayant.png',
            tags: ['Management']
        },
        {
            name: 'Jon Stone',
            email: 'stone@ultima.org',
            avatar: '/demo/images/avatar/ivanmagalhaes.png',
            tags: ['Management', 'Finance']
        },
        {
            name: 'Stephen Shaw',
            email: 'shaw@ultima.org',
            avatar: '/demo/images/avatar/stephenshaw.png',
            tags: ['Finance']
        }
    ];
}
