import { Component } from '@angular/core';

@Component({
    selector: 'who-uses-widget',
    standalone: true,
    templateUrl: './whouses.html',
    host: {
        class: 'grid grid-cols-12 gap-4 grid-nogutter p-2 lg:p-8',
        style: 'background-color: #000'
    }
})
export class WhoUsesWidget {}
