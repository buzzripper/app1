import { afterNextRender, Component, effect, inject, signal, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Ripple } from 'primeng/ripple';
import { MenuModule } from 'primeng/menu';
import { ChartModule, UIChart } from 'primeng/chart';
import { LayoutService } from '@/app/layout/service/layout.service';

@Component({
    selector: 'insights-widget',
    standalone: true,
    imports: [ButtonModule, ChartModule, MenuModule, Ripple],
    templateUrl: './insightswidget.html'
})
export class InsightsWidget {
    layoutService = inject(LayoutService);

    @ViewChild('doughnutChart') doughnutViewChild!: UIChart;

    colorKeys = ['indigo', 'blue', 'cyan', 'green', 'emerald', 'orange', 'yellow'];

    items = [
        { label: 'Update', icon: 'pi pi-fw pi-refresh' },
        { label: 'Edit', icon: 'pi pi-fw pi-pencil' }
    ];

    doughnutData = signal<any>(null);

    doughnutOptions = signal<any>(null);

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

    changeDoughnutDataView() {
        if (this.doughnutViewChild.chart.options.circumference === 180) {
            this.doughnutViewChild.chart.options.circumference = 360;
            this.doughnutViewChild.chart.options.rotation = -45;
        } else {
            this.doughnutViewChild.chart.options.circumference = 180;
            this.doughnutViewChild.chart.options.rotation = -90;
        }

        this.doughnutViewChild.chart.update();
    }

    initChart() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color') || 'rgba(0, 0, 0, 0.87)';
        const fontFamily = documentStyle.getPropertyValue('--font-family');
        const borderColor = documentStyle.getPropertyValue('--surface-border') || 'rgba(160, 167, 181, .3)';
        const suffix = this.layoutService.isDarkTheme() ? '400' : '500';
        const backgroundColor = this.colorKeys.map((color: string) => documentStyle.getPropertyValue(`--p-${color}-${suffix}`));

        this.doughnutData.set({
            labels: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
            datasets: [
                {
                    data: [11, 29, 71, 33, 28, 95, 6],
                    backgroundColor,
                    borderColor
                }
            ]
        });

        this.doughnutOptions.set({
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        font: {
                            family: fontFamily
                        },
                        color: textColor
                    }
                }
            },
            responsive: true,
            maintainAspectRatio: false,
            circumference: 180,
            rotation: -90,
            animation: {
                animateScale: true,
                animateRotate: true
            }
        });
    }
}
