import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { AvatarGroup } from 'primeng/avatargroup';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'teams-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, AvatarModule, AvatarGroup, RippleModule],
    templateUrl: './teamswidget.html',
    host: {
        class: 'col-span-12 md:col-span-4'
    }
})
export class TeamsWidget {
    @Input() selectedTeam!: any;

    @Output() onTeamFilter: EventEmitter<any> = new EventEmitter<any>();

    teams = [
        {
            title: 'UX Researchers',
            avatar: ['/demo/images/avatar/circle/avatar-f-1.png', '/demo/images/avatar/circle/avatar-f-6.png', '/demo/images/avatar/circle/avatar-f-11.png', '/demo/images/avatar/circle/avatar-f-12.png'],
            avatarText: '+4',
            badgeClass: 'bg-pink-500'
        },
        {
            title: 'UX Designers',
            avatar: ['/demo/images/avatar/circle/avatar-f-2.png'],
            badgeClass: 'bg-blue-500'
        },
        {
            title: 'UI Designers',
            avatar: ['/demo/images/avatar/circle/avatar-f-3.png', '/demo/images/avatar/circle/avatar-f-8.png'],
            avatarText: '+1',
            badgeClass: 'bg-green-500'
        },
        {
            title: 'Front-End Developers',
            avatar: ['/demo/images/avatar/circle/avatar-f-4.png', '/demo/images/avatar/circle/avatar-f-9.png'],
            badgeClass: 'bg-yellow-500'
        },
        {
            title: 'Back-End Developers',
            avatar: ['/demo/images/avatar/circle/avatar-f-10.png'],
            badgeClass: 'bg-purple-500'
        }
    ];

    teamFilter(team: any) {
        this.onTeamFilter.emit(team);
    }
}
