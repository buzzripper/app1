import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { InputText } from 'primeng/inputtext';

@Component({
    selector: 'newsletter-widget',
    standalone: true,
    imports: [InputText, ButtonModule, RippleModule],
    templateUrl: './newsletterwidget.html',
    host: {
        class: 'py-12 px-6 mt-12 w-full flex justify-center'
    }
})
export class NewsletterWidget {}
