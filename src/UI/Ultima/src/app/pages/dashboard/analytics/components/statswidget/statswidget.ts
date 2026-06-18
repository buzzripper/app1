import { Component } from '@angular/core';
import { ProgressBar } from 'primeng/progressbar';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'stats-widget',
    standalone: true,
    imports: [CommonModule, ProgressBar],
    templateUrl: './statswidget.html',
    host: {
        class: 'col-span-12 grid grid-cols-12 gap-4'
    }
})
export class StatsWidget {
    stats = [
        {
            icon: 'pi pi-twitter',
            iconClass: 'text-muted-color',
            count: '44.995',
            label: 'Retweets',
            target: {
                value: '10.000',
                progress: 50
            },
            record: {
                value: '50.702',
                progress: 24
            }
        },
        {
            icon: 'pi pi-facebook',
            iconClass: 'text-muted-color',
            count: '63.573',
            label: 'Facebook Interactions',
            target: {
                value: '10.000',
                progress: 23
            },
            record: {
                value: '99.028',
                progress: 38
            }
        },
        {
            icon: 'pi pi-github',
            iconClass: 'text-muted-color',
            count: '81.002',
            label: 'Stars',
            target: {
                value: '10.000',
                progress: 62
            },
            record: {
                value: '162.550',
                progress: 14
            }
        }
    ];
}
