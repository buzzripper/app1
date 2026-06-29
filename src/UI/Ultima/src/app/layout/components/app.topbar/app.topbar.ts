import { Component, computed, ElementRef, inject, ViewChild } from '@angular/core';
import { ConfirmationService, MegaMenuItem } from 'primeng/api';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StyleClassModule } from 'primeng/styleclass';
import { LayoutService } from '@/app/layout/service/layout.service';
import { Ripple } from 'primeng/ripple';
import { InputText } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { MegaMenuModule } from 'primeng/megamenu';
import { BadgeModule } from 'primeng/badge';
import { OverlayBadge } from 'primeng/overlaybadge';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { AuthService } from '@/app/core/auth/auth.service';
import { UserService } from '@/app/core/user/user.service';

@Component({
    selector: '[app-topbar]',
    standalone: true,
    imports: [RouterModule, CommonModule, StyleClassModule, FormsModule, Ripple, InputText, ButtonModule, MegaMenuModule, BadgeModule, OverlayBadge, ConfirmPopupModule],
    templateUrl: './app.topbar.html',
    host: {
        class: 'layout-topbar'
    },
    styles: `
        :host ::ng-deep .p-overlaybadge .p-badge {
            outline-width: 0px;
        }
    `,
    providers: [ConfirmationService]
})
export class AppTopbar {
    layoutService = inject(LayoutService);
    private readonly authService = inject(AuthService);
    private readonly userService = inject(UserService);
    private readonly confirmationService = inject(ConfirmationService);

    @ViewChild('searchInput') searchInput!: ElementRef<HTMLInputElement>;

    @ViewChild('menuButton') menuButton!: ElementRef<HTMLButtonElement>;

    @ViewChild('mobileMenuButton') mobileMenuButton!: ElementRef<HTMLButtonElement>;

    user = computed(() => this.userService.user());

    model: MegaMenuItem[] = [
        {
            label: 'UI KIT',
            items: [
                [
                    {
                        label: 'UI KIT 1',
                        items: [
                            { label: 'Form Layout', icon: 'pi pi-fw pi-id-card', routerLink: '/uikit/formlayout' },
                            { label: 'Input', icon: 'pi pi-fw pi-check-square', routerLink: '/uikit/input' },
                            { label: 'Timeline', icon: 'pi pi-fw pi-bookmark', routerLink: '/uikit/timeline' },
                            { label: 'Button', icon: 'pi pi-fw pi-mobile', routerLink: '/uikit/button' },
                            { label: 'File', icon: 'pi pi-fw pi-file', routerLink: '/uikit/file' }
                        ]
                    }
                ],
                [
                    {
                        label: 'UI KIT 2',
                        items: [
                            { label: 'Table', icon: 'pi pi-fw pi-table', routerLink: '/uikit/table' },
                            { label: 'List', icon: 'pi pi-fw pi-list', routerLink: '/uikit/list/list' },
                            { label: 'Tree', icon: 'pi pi-fw pi-share-alt', routerLink: '/uikit/tree' },
                            { label: 'Panel', icon: 'pi pi-fw pi-tablet', routerLink: '/uikit/panel' },
                            { label: 'Chart', icon: 'pi pi-fw pi-chart-bar', routerLink: '/uikit/charts' }
                        ]
                    }
                ],
                [
                    {
                        label: 'UI KIT 3',
                        items: [
                            { label: 'Overlay', icon: 'pi pi-fw pi-clone', routerLink: '/uikit/overlay' },
                            { label: 'Media', icon: 'pi pi-fw pi-image', routerLink: '/uikit/media' },
                            { label: 'Menu', icon: 'pi pi-fw pi-bars', routerLink: '/uikit/menu' },
                            { label: 'Message', icon: 'pi pi-fw pi-comment', routerLink: '/uikit/message' },
                            { label: 'Misc', icon: 'pi pi-fw pi-circle-off', routerLink: '/uikit/misc' }
                        ]
                    }
                ]
            ]
        },
        {
            label: 'UTILITIES',
            items: [
                [
                    {
                        label: 'UTILITIES 1',
                        items: [
                            {
                                label: 'Buy Now',
                                icon: 'pi pi-fw pi-shopping-cart',
                                url: 'https://www.primefaces.org/store',
                                target: '_blank'
                            },
                            {
                                label: 'Documentation',
                                icon: 'pi pi-fw pi-info-circle',
                                routerLink: '/documentation'
                            }
                        ]
                    }
                ]
            ]
        }
    ];

    onMenuButtonClick() {
        this.layoutService.onMenuToggle();
    }

    onRightMenuButtonClick() {
        this.layoutService.openRightMenu();
    }

    toggleConfigSidebar() {
        if (this.layoutService.isSidebarActive()) {
            this.layoutService.layoutState.update((prev) => ({
                ...prev,
                overlayMenuActive: false,
                staticMenuMobileActive: false,
                menuHoverActive: false,
                configSidebarVisible: true
            }));
        } else {
            this.layoutService.toggleConfigSidebar();
        }
    }

    focusSearchInput() {
        setTimeout(() => {
            this.searchInput.nativeElement.focus();
        }, 150);
    }

    onTopbarMenuToggle() {
        this.layoutService.layoutState.update((val) => ({ ...val, topbarMenuActive: !val.topbarMenuActive }));
    }

    confirmSignOut(event: Event): void {
        this.confirmationService.confirm({
            key: 'topbar-signout',
            target: event.target as EventTarget,
            message: 'Are you sure you want to log out of the program?',
            rejectButtonProps: {
                label: 'Cancel',
                severity: 'secondary',
                outlined: true
            },
            acceptButtonProps: {
                label: 'Log Out',
                severity: 'danger'
            },
            accept: () => {
                this.authService.signOut();
            }
        });
    }
}
