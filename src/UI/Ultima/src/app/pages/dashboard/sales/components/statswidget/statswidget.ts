import { afterNextRender, Component, effect, inject, QueryList, signal, ViewChildren } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RippleModule } from 'primeng/ripple';
import { ChartModule } from 'primeng/chart';
import { ButtonModule } from 'primeng/button';
import { Menu, MenuModule } from 'primeng/menu';
import { LayoutService } from '@/app/layout/service/layout.service';

@Component({
    selector: 'stats-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, MenuModule, ChartModule, RippleModule],
    templateUrl: './statswidget.html',
    host: {
        class: 'col-span-12 grid grid-cols-12 gap-4'
    }
})
export class StatsWidget {
    @ViewChildren('menu') menu!: QueryList<Menu>;

    layoutService = inject(LayoutService);

    items = [
        { label: 'Update', icon: 'pi pi-fw pi-refresh' },
        { label: 'Edit', icon: 'pi pi-fw pi-pencil' }
    ];

    stats = [
        {
            icon: 'pi pi-shopping-cart',
            title: 'Orders',
            value: '640',
            subValue: '1420 Completed',
            colorClass: 'text-teal-600 dark:text-teal-200'
        },
        {
            icon: 'pi pi-dollar',
            title: 'Revenue',
            value: '$57K',
            subValue: '$9,640 Income',
            colorClass: 'text-teal-600 dark:text-teal-200'
        },
        {
            icon: 'pi pi-users',
            title: 'Customers',
            value: '8572',
            subValue: '25402 Registered',
            colorClass: 'text-pink-600 dark:text-pink-200'
        },
        {
            icon: 'pi pi-comments',
            title: 'Comments',
            value: '805',
            subValue: '85 Responded',
            colorClass: 'text-teal-600 dark:text-teal-200'
        }
    ];

    chartDatasets = [
        [50, 64, 32, 24, 18, 27, 20, 36, 30],
        [11, 30, 52, 35, 39, 20, 14, 18, 29],
        [20, 29, 39, 36, 45, 24, 28, 20, 15],
        [30, 39, 50, 21, 33, 18, 10, 24, 20]
    ];

    chartOptions = {
        plugins: {
            legend: {
                display: false
            }
        },
        responsive: true,
        scales: {
            y: {
                display: false
            },
            x: {
                display: false
            }
        },
        tooltips: {
            enabled: false
        },
        elements: {
            point: {
                radius: 0
            }
        }
    };

    overviewChartData1 = signal<any>(null);

    overviewChartData2 = signal<any>(null);

    overviewChartData3 = signal<any>(null);

    overviewChartData4 = signal<any>(null);

    private initialized = false;

    constructor() {
        afterNextRender(() => {
            setTimeout(() => {
                this.initChart();
                this.initialized = true;
            }, 150);
        });

        effect(() => {
            this.layoutService.layoutConfig().darkTheme;
            if (this.initialized) {
                setTimeout(() => {
                    this.initChart();
                }, 150);
            }
        });
    }

    toggleMenu(event: any, index: number) {
        const menu = this.menu.toArray()[index];
        menu.toggle(event);
    }

    initChart() {
        const colors = this.getOverviewColors();

        this.overviewChartData1.set(this.createChartData(this.chartDatasets[0], colors.tealBorderColor, colors.tealBgColor));
        this.overviewChartData2.set(this.createChartData(this.chartDatasets[1], colors.tealBorderColor, colors.tealBgColor));
        this.overviewChartData3.set(this.createChartData(this.chartDatasets[2], colors.pinkBorderColor, colors.pinkBgColor));
        this.overviewChartData4.set(this.createChartData(this.chartDatasets[3], colors.tealBorderColor, colors.tealBgColor));
    }

    getOverviewColors() {
        return {
            pinkBorderColor: !this.layoutService.isDarkTheme() ? '#E91E63' : '#EC407A',
            pinkBgColor: !this.layoutService.isDarkTheme() ? '#F48FB1' : '#F8BBD0',
            tealBorderColor: !this.layoutService.isDarkTheme() ? '#009688' : '#26A69A',
            tealBgColor: !this.layoutService.isDarkTheme() ? '#80CBC4' : '#B2DFDB'
        };
    }

    createChartData(data: any, borderColor: string, bgColor: string) {
        return {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September'],
            datasets: [
                {
                    data,
                    borderColor: [borderColor],
                    backgroundColor: [bgColor],
                    borderWidth: 2,
                    fill: true,
                    tension: 0.4
                }
            ]
        };
    }
}
