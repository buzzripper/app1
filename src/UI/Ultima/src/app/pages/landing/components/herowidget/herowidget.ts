import { Component } from '@angular/core';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';

@Component({
    selector: 'hero-widget',
    standalone: true,
    imports: [ButtonModule, RippleModule],
    templateUrl: './herowidget.html',
    host: {
        style: 'display: contents;'
    }
})
export class HeroWidget {
    handleScroll(id: string) {
        const element = document.getElementById(id);
        element?.scrollIntoView({ behavior: 'smooth' });
    }
}
