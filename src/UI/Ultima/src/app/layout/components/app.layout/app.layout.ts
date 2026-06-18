import { Component, computed, effect, inject, OnDestroy, Renderer2, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { filter } from 'rxjs';
import { AppTopbar } from '../app.topbar/app.topbar';
import { AppFooter } from '../app.footer/app.footer';
import { LayoutService } from '@/app/layout/service/layout.service';
import { AppConfigurator } from '../app.configurator/app.configurator';
import { AppBreadcrumb } from '../app.breadcrumb/app.breadcrumb';
import { AppSidebar } from '../app.sidebar/app.sidebar';
import { AppRightMenu } from '@/app/layout/components/app.rightmenu/app.rightmenu';
import { Toast } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
    selector: 'app-layout',
    standalone: true,
    imports: [CommonModule, AppTopbar, AppSidebar, RouterModule, AppFooter, AppConfigurator, AppBreadcrumb, AppRightMenu, Toast],
    templateUrl: './app.layout.html',
    providers: [MessageService]
})
export class AppLayout {
    layoutService = inject(LayoutService);

    constructor() {
        effect(() => {
            const state = this.layoutService.layoutState();
            if (state.mobileMenuActive) {
                document.body.classList.add('blocked-scroll');
            } else {
                document.body.classList.remove('blocked-scroll');
            }
        });
    }
    containerClass = computed(() => {
        const layoutConfig = this.layoutService.layoutConfig();
        const layoutState = this.layoutService.layoutState();

        return {
            'layout-overlay': layoutConfig.menuMode === 'overlay',
            'layout-static': layoutConfig.menuMode === 'static',
            'layout-slim': layoutConfig.menuMode === 'slim',
            'layout-slim-plus': layoutConfig.menuMode === 'slim-plus',
            'layout-horizontal': layoutConfig.menuMode === 'horizontal',
            'layout-reveal': layoutConfig.menuMode === 'reveal',
            'layout-drawer': layoutConfig.menuMode === 'drawer',
            'layout-sidebar-dark': layoutConfig.darkTheme,
            'layout-static-inactive': layoutState.staticMenuDesktopInactive && layoutConfig.menuMode === 'static',
            'layout-overlay-active': layoutState.overlayMenuActive,
            'layout-mobile-active': layoutState.mobileMenuActive,
            'layout-topbar-menu-active': layoutState.topbarMenuActive,
            'layout-menu-profile-active': layoutState.rightMenuActive,
            'layout-sidebar-expanded': layoutState.sidebarExpanded,
            'layout-sidebar-anchored': layoutState.anchored,
            [`layout-topbar-${layoutConfig.topbarTheme}`]: true,
            [`layout-menu-${layoutConfig.menuTheme}`]: true,
            [`layout-menu-profile-${layoutConfig.menuProfilePosition}`]: true
        };
    });
}
