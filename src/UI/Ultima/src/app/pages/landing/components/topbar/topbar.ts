import { Component } from '@angular/core';
import { StyleClassModule } from 'primeng/styleclass';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'topbar',
    standalone: true,
    imports: [RouterModule, StyleClassModule, ButtonModule, RippleModule],
    templateUrl: './topbar.html'
})
export class Topbar {
    handleScroll(id: string) {
        const element = document.getElementById(id);
        if (element) {
            setTimeout(() => {
                element.scrollIntoView({ behavior: 'smooth', block: 'start', inline: 'nearest' });
            }, 200);
        }
    }
}
