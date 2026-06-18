import { Component } from '@angular/core';
import { IntroduceWidget } from '@/app/pages/dashboard/saas/components/introducewidget/introducewidget';
import { UpgradeWidget } from '@/app/pages/dashboard/saas/components/upgradewidget/upgradewidget';
import { MyWorkspaceWidget } from '@/app/pages/dashboard/saas/components/myworkspacewidget/myworkspacewidget';
import { ProjectOverviewWidget } from '@/app/pages/dashboard/saas/components/projectoverviewwidget/projectoverviewwidget';

@Component({
    selector: 'app-dashboard-saas',
    standalone: true,
    imports: [IntroduceWidget, UpgradeWidget, MyWorkspaceWidget, ProjectOverviewWidget],
    templateUrl: './dashboardsaas.html'
})
export class DashboardSaas {}
