import { Component } from '@angular/core';
import { DailyTaskWidget } from '@/app/pages/dashboard/saas/components/dailytaskwidget/dailytaskwidget';
import { PerformanceWidget } from '@/app/pages/dashboard/saas/components/performancewidget/performancewidget';
import { CalendarWidget } from '@/app/pages/dashboard/saas/components/calendarwidget/calendarwidget';

@Component({
    selector: 'my-workspace-widget',
    standalone: true,
    imports: [DailyTaskWidget, PerformanceWidget, CalendarWidget],
    templateUrl: './myworkspacewidget.html'
})
export class MyWorkspaceWidget {
    dailyTasks = [
        {
            id: 1,
            checked: true,
            label: 'Prepare personas',
            description: 'Create profiles of fictional users representing target audience for product or service.',
            avatar: '/demo/images/avatar/circle/avatar-f-6.png',
            borderColor: 'border-pink-500'
        },
        {
            id: 2,
            checked: false,
            label: 'Prepare a user journey map',
            description: 'Visual representation of steps a user takes to accomplish a goal within product or service.',
            avatar: '/demo/images/avatar/circle/avatar-f-7.png',
            borderColor: 'border-purple-500'
        },
        {
            id: 3,
            checked: false,
            label: 'Prepare wireframes for onboarding screen',
            description: 'Create low-fidelity mockups of onboarding screen. Include layout, hierarchy, functionality.',
            avatar: '/demo/images/avatar/circle/avatar-f-8.png',
            borderColor: 'border-blue-500'
        },
        {
            id: 4,
            checked: false,
            label: 'Review benchmarks',
            description: 'Conduct research on similar products or services to understand market standards and identify opportunities.',
            avatar: '/demo/images/avatar/circle/avatar-f-9.png',
            borderColor: 'border-green-500'
        },
        {
            id: 5,
            checked: false,
            label: 'Let a plan with UI Team',
            description: 'Collaborate with UI design team to create plan for visual design of product or service.',
            avatar: '/demo/images/avatar/circle/avatar-f-10.png',
            borderColor: 'border-yellow-500'
        }
    ];
}
