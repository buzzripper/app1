import { Component } from '@angular/core';
import { StatsWidget } from '@/app/pages/dashboard/sales/components/statswidget/statswidget';
import { ContactWidget } from '@/app/pages/dashboard/sales/components/contactwidget/contactwidget';
import { OrderGraphWidget } from '@/app/pages/dashboard/sales/components/ordergraphwidget/ordergraphwidget';
import { CustomTimelineWidget } from '@/app/pages/dashboard/sales/components/customtimelinewidget/customtimelinewidget';
import { SalesTableWidget } from '@/app/pages/dashboard/sales/components/salestablewidget/salestablewidget';
import { ChatWidget } from '@/app/pages/dashboard/sales/components/chatwidget/chatwidget';
import { ActivityWidget } from '@/app/pages/dashboard/sales/components/activitywidget/activitywidget';
import { BestSellersWidget } from '@/app/pages/dashboard/sales/components/bestsellerswidget/bestsellerswidget';

@Component({
    selector: 'app-dashboard-sales',
    standalone: true,
    imports: [StatsWidget, ContactWidget, OrderGraphWidget, CustomTimelineWidget, SalesTableWidget, ChatWidget, ActivityWidget, BestSellersWidget],
    templateUrl: './dashboardsales.html'
})
export class DashboardSales {}
