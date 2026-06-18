import { Component, inject } from '@angular/core';
import { LayoutService } from '@/app/layout/service/layout.service';
import { FormsModule } from '@angular/forms';
import { DrawerModule } from 'primeng/drawer';

@Component({
    selector: '[app-right-menu]',
    standalone: true,
    imports: [DrawerModule, FormsModule],
    templateUrl: './app.rightmenu.html'
})
export class AppRightMenu {
    layoutService = inject(LayoutService);

    get rightMenuActive(): boolean {
        return this.layoutService.layoutState().rightMenuActive;
    }

    set rightMenuActive(_val: boolean) {
        this.layoutService.layoutState.update((prev) => ({ ...prev, rightMenuActive: _val }));
    }
}
