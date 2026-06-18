import { Component } from '@angular/core';
import { MonthlyComparisonWidget } from '@/app/pages/dashboard/analytics/components/monthlycomparisonwidget/monthlycomparisonwidget';
import { InsightsWidget } from '@/app/pages/dashboard/analytics/components/insightswidget/insightswidget';
import { StatsWidget } from '@/app/pages/dashboard/analytics/components/statswidget/statswidget';
import { StoresWidget } from '@/app/pages/dashboard/analytics/components/storeswidget/storeswidget';
import { TopSearchesWidget } from '@/app/pages/dashboard/analytics/components/topsearcheswidget/topsearcheswidget';
import { AnalyticsTableWidget } from '@/app/pages/dashboard/analytics/components/analyticstablewidget/analyticstablewidget';
import { ExpensesWidget } from '@/app/pages/dashboard/analytics/components/expenseswidget/expenseswidget';
import { RatingsWidget } from '@/app/pages/dashboard/analytics/components/ratingswidget/ratingswidget';

@Component({
    selector: 'app-dashboard-analytics',
    standalone: true,
    imports: [MonthlyComparisonWidget, InsightsWidget, StatsWidget, StoresWidget, TopSearchesWidget, AnalyticsTableWidget, ExpensesWidget, RatingsWidget],
    templateUrl: './dashboardanalytics.html'
})
export class DashboardAnalytics {}
