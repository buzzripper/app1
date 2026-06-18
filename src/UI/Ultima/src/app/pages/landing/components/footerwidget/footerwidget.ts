import { Component } from '@angular/core';

@Component({
    selector: 'footer-widget',
    standalone: true,
    templateUrl: './footerwidget.html',
    host: {
        style: 'max-width: 1200px',
        class: 'w-full px-6'
    }
})
export class FooterWidget {}
