import { Component, inject } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { LayoutService } from '@/app/layout/service/layout.service';

@Component({
    standalone: true,
    selector: '[app-footer]',
    imports: [ButtonModule],
    templateUrl: './app.footer.html',
    host: {
        class: 'layout-footer'
    }
})
export class AppFooter {
    layoutService = inject(LayoutService);
}
