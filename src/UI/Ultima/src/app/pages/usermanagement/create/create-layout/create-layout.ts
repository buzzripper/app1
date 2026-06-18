import { Component, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormStateService } from '../form-state.service';

interface MenuItem {
    label: string;
    shortLabel: string;
    icon: string;
    route: string;
}

@Component({
    selector: 'app-create-layout',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './create-layout.html'
})
export class CreateLayout {
    menuItems: MenuItem[] = [
        {
            label: 'Basic Information',
            shortLabel: 'Basic',
            icon: 'pi pi-user',
            route: '/profile/create/basic-information/basic-information'
        },
        {
            label: 'Business Information',
            shortLabel: 'Business',
            icon: 'pi pi-briefcase',
            route: '/profile/create/business-information/business-information'
        },
        {
            label: 'Location Information',
            shortLabel: 'Location',
            icon: 'pi pi-map-marker',
            route: '/profile/create/location-information/location-information'
        },
        {
            label: 'Authorization and Access',
            shortLabel: 'Access',
            icon: 'pi pi-key',
            route: '/profile/create/authorization/authorization'
        },
        {
            label: 'Account Status',
            shortLabel: 'Status',
            icon: 'pi pi-shield',
            route: '/profile/create/account-status/account-status'
        }
    ];

    currentRoute = '';

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        public formStateService: FormStateService
    ) {
        this.router.events.subscribe(() => {
            this.currentRoute = this.router.url;
        });
    }

    isActive(menuRoute: string): boolean {
        return this.currentRoute === menuRoute;
    }

    navigateTo(menuRoute: string) {
        this.router.navigate([menuRoute]);
    }

    getMenuButtonClass(route: string): string {
        const baseClass = 'pl-3 pr-2 py-2 rounded-xl flex items-center gap-2 transition-colors cursor-pointer';
        if (this.isActive(route)) {
            return `${baseClass} bg-primary text-surface-0 dark:text-surface-900 shadow-sm`;
        }
        return `${baseClass} text-surface-500 dark:text-surface-400 hover:bg-surface-100 dark:hover:bg-surface-700`;
    }

    getMobileMenuButtonClass(route: string): string {
        const baseClass = 'px-4 py-2 rounded-xl flex items-center gap-2 transition-colors cursor-pointer whitespace-nowrap';
        if (this.isActive(route)) {
            return `${baseClass} bg-primary text-surface-0 dark:text-surface-900 shadow-sm`;
        }
        return `${baseClass} bg-surface-100 dark:bg-surface-800 text-surface-500 dark:text-surface-400 hover:bg-surface-200 dark:hover:bg-surface-700`;
    }
}
