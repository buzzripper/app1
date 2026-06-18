import { afterNextRender, Component, effect, inject, signal } from '@angular/core';
import { ChartModule } from 'primeng/chart';
import { LayoutService } from '@/app/layout/service/layout.service';

@Component({
    selector: 'performance-widget',
    standalone: true,
    imports: [ChartModule],
    templateUrl: './performancewidget.html',
    host: {
        class: 'col-span-12 md:col-span-4'
    }
})
export class PerformanceWidget {
    layoutService = inject(LayoutService);

    basicData = signal<any>(null);

    basicOptions = signal<any>(null);

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

    initChart() {
        const documentStyle = getComputedStyle(document.documentElement);
        const textColor = documentStyle.getPropertyValue('--text-color');
        const surfaceBorder = documentStyle.getPropertyValue('--surface-border');

        this.basicData.set({
            labels: ['January', 'February', 'March', 'April', 'May'],
            datasets: [
                {
                    label: 'Previous Month',
                    borderColor: '#E0E0E0',
                    tension: 0.5,
                    data: [22, 36, 11, 33, 2]
                },
                {
                    label: 'Current Month',
                    borderColor: '#6366F1',
                    tension: 0.5,
                    data: [22, 16, 31, 11, 38]
                }
            ]
        });

        this.basicOptions.set({
            plugins: {
                legend: {
                    labels: {
                        color: textColor,
                        boxWidth: 12,
                        boxHeight: 4
                    },
                    position: 'bottom'
                }
            },
            maintainAspectRatio: false,
            elements: { point: { radius: 0 } },
            scales: {
                x: {
                    ticks: {
                        color: textColor
                    },
                    grid: {
                        color: surfaceBorder
                    }
                },
                y: {
                    ticks: {
                        color: textColor,
                        stepSize: 10
                    },
                    grid: {
                        color: surfaceBorder
                    }
                }
            }
        });
    }
}
