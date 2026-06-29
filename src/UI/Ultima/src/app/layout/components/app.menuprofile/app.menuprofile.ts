import { Component, computed, inject } from '@angular/core';
import { LayoutService } from '@/app/layout/service/layout.service';
import { ConfirmationService } from 'primeng/api';
import { TooltipModule } from 'primeng/tooltip';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { AuthService } from '@/app/core/auth/auth.service';
import { UserService } from '@/app/core/user/user.service';

@Component({
    selector: '[app-menu-profile]',
    standalone: true,
    imports: [CommonModule, TooltipModule, ButtonModule, RouterModule, ConfirmPopupModule],
    templateUrl: './app.menuprofile.html',
    host: {
        class: 'layout-menu-profile'
    },
    providers: [ConfirmationService],
    styles: [
        `
            /* Menu Profile Enter Animation */
            .p-menuprofile-enter {
                animation: p-animate-menuprofile-expand 400ms cubic-bezier(0.86, 0, 0.07, 1) forwards;
            }

            /* Menu Profile Leave Animation */
            .p-menuprofile-leave {
                animation: p-animate-menuprofile-collapse 400ms cubic-bezier(0.86, 0, 0.07, 1) forwards;
            }

            /* Overlay Menu Enter Animation */
            ul.overlay-menu.p-menuprofile-enter {
                animation: p-animate-menuprofile-overlay-enter 120ms cubic-bezier(0, 0, 0.2, 1) forwards;
            }

            /* Overlay Menu Leave Animation */
            ul.overlay-menu.p-menuprofile-leave {
                animation: p-animate-menuprofile-overlay-leave 100ms linear forwards;
            }

            @keyframes p-animate-menuprofile-expand {
                from {
                    max-height: 0;
                    opacity: 0;
                    overflow: hidden;
                }
                to {
                    max-height: 500px;
                    opacity: 1;
                    overflow: visible;
                }
            }

            @keyframes p-animate-menuprofile-collapse {
                from {
                    max-height: 500px;
                    opacity: 1;
                    overflow: hidden;
                }
                to {
                    max-height: 0;
                    opacity: 0;
                    overflow: hidden;
                }
            }

            @keyframes p-animate-menuprofile-overlay-enter {
                from {
                    opacity: 0;
                    transform: scaleY(0.8);
                }
                to {
                    opacity: 1;
                    transform: scaleY(1);
                }
            }

            @keyframes p-animate-menuprofile-overlay-leave {
                from {
                    opacity: 1;
                }
                to {
                    opacity: 0;
                }
            }
        `
    ]
})
export class AppMenuProfile {
    layoutService = inject(LayoutService);
    private readonly authService = inject(AuthService);
    private readonly userService = inject(UserService);
    private readonly confirmationService = inject(ConfirmationService);

    isHorizontal = computed(() => this.layoutService.isHorizontal() && this.layoutService.isDesktop());

    menuProfileActive = computed(() => this.layoutService.layoutState().menuProfileActive);

    menuProfilePosition = computed(() => this.layoutService.layoutConfig().menuProfilePosition);

    isTooltipDisabled = computed(() => !this.layoutService.isSlim());

    user = computed(() => this.userService.user());

    toggleMenu() {
        if (this.isHorizontal() && this.layoutService.layoutState().activePath) {
            this.layoutService.layoutState.update((val) => ({
                ...val,
                activePath: null,
                menuHoverActive: false
            }));
        }
        this.layoutService.onMenuProfileToggle();
    }

    confirmSignOut(event: Event): void {
        this.confirmationService.confirm({
            key: 'menuprofile-signout',
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
