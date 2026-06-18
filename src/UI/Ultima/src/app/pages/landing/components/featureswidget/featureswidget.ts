import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    standalone: true,
    selector: 'features-widget',
    imports: [CommonModule],
    templateUrl: './featureswidget.html'
})
export class FeaturesWidget {
    handleMouseEnter(event: MouseEvent) {
        const target = event.target as HTMLElement;
        if (target) {
            target.style.background = 'linear-gradient(110.43deg, rgba(134,140,208,.5) 0.04%, rgba(255,87,89,.5) 100.11%)';
        }
    }

    handleMouseLeave(event: MouseEvent) {
        const target = event.target as HTMLElement;
        if (target) {
            target.style.background = 'unset';
        }
    }
}
