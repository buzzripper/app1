import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-about-us',
    imports: [CommonModule],
    templateUrl: './aboutus.html'
})
export class AboutUs {
    visibleMember = -1;

    teamMembers = [
        {
            name: 'Jeff Davies',
            role: 'Software Developer',
            image: '/demo/images/blocks/team/team-1.png',
            socialIcons: [
                {
                    iconClass: 'pi pi-twitter',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl mr-4'
                },
                {
                    iconClass: 'pi pi-github',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl mr-4'
                },
                {
                    iconClass: 'pi pi-facebook',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl'
                }
            ]
        },
        {
            name: 'Kristin Watson',
            role: 'UI/UX Designer',
            image: '/demo/images/blocks/team/team-2.png',
            socialIcons: [
                {
                    iconClass: 'pi pi-twitter',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl mr-4'
                },
                {
                    iconClass: 'pi pi-github',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl mr-4'
                },
                {
                    iconClass: 'pi pi-facebook',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl'
                }
            ]
        },
        {
            name: 'Jenna Williams',
            role: 'Marketing Specialist',
            image: '/demo/images/blocks/team/team-3.png',
            socialIcons: [
                {
                    iconClass: 'pi pi-twitter',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl mr-4'
                },
                {
                    iconClass: 'pi pi-github',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl mr-4'
                },
                {
                    iconClass: 'pi pi-facebook',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl'
                }
            ]
        },
        {
            name: 'Joe Clifford',
            role: 'Customer Relations',
            image: '/demo/images/blocks/team/team-4.png',
            socialIcons: [
                {
                    iconClass: 'pi pi-twitter',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl mr-4'
                },
                {
                    iconClass: 'pi pi-github',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl mr-4'
                },
                {
                    iconClass: 'pi pi-facebook',
                    additionalClasses: 'text-surface-600 dark:text-surface-200 text-xl'
                }
            ]
        }
    ];

    setVisibleMember(index: number): void {
        this.visibleMember = index;
    }
}
