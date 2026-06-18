import { afterNextRender, Component, DestroyRef, effect, inject, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule, UIChart } from 'primeng/chart';
import { LayoutService } from '@/app/layout/service/layout.service';

@Component({
    selector: 'stores-widget',
    standalone: true,
    imports: [CommonModule, ChartModule],
    templateUrl: './storeswidget.html'
})
export class StoresWidget {
    layoutService = inject(LayoutService);

    destroyRef = inject(DestroyRef);

    @ViewChild('storeA') storeAViewChild!: UIChart;

    @ViewChild('storeB') storeBViewChild!: UIChart;

    @ViewChild('storeC') storeCViewChild!: UIChart;

    @ViewChild('storeD') storeDViewChild!: UIChart;

    storeATotalValue = signal(100);
    storeADiff = signal(0);
    storeAData = signal<any>(null);

    storeBTotalValue = signal(120);
    storeBDiff = signal(0);
    storeBData = signal<any>(null);

    storeCTotalValue = signal(150);
    storeCDiff = signal(0);
    storeCData = signal<any>(null);

    storeDTotalValue = signal(80);
    storeDDiff = signal(0);
    storeDData = signal<any>(null);

    chartOptions = {
        plugins: {
            legend: {
                display: false
            }
        },
        maintainAspectRatio: false,
        responsive: true,
        aspectRatio: 4,
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

    storeInterval: any;

    private initialized = false;

    constructor() {
        afterNextRender(() => {
            setTimeout(() => {
                this.initCharts();
                this.startInterval();
                this.initialized = true;
            }, 150);
        });

        effect(() => {
            this.layoutService.layoutConfig().darkTheme;
            if (this.initialized) {
                setTimeout(() => {
                    this.initCharts();
                }, 150);
            }
        });

        this.destroyRef.onDestroy(() => {
            clearInterval(this.storeInterval);
        });
    }

    startInterval() {
        const calculateStore = (storeData: any, totalValue: number) => {
            let randomNumber = +(Math.random() * 500).toFixed(2);
            let data = [...storeData.datasets[0].data];
            let length = data.length;
            data.push(randomNumber);
            data.shift();
            storeData.datasets[0].data = data;

            let diff = +(data[length - 1] - data[length - 2]).toFixed(2);
            totalValue = +(totalValue + diff).toFixed(2);

            return { diff, totalValue, storeData };
        };

        this.storeInterval = setInterval(() => {
            requestAnimationFrame(() => {
                const storeAResult = calculateStore(this.storeAData(), this.storeATotalValue());
                this.storeADiff.set(storeAResult.diff);
                this.storeATotalValue.set(storeAResult.totalValue);
                this.storeAData.set({ ...storeAResult.storeData });
                this.storeAViewChild?.refresh();

                const storeBResult = calculateStore(this.storeBData(), this.storeBTotalValue());
                this.storeBDiff.set(storeBResult.diff);
                this.storeBTotalValue.set(storeBResult.totalValue);
                this.storeBData.set({ ...storeBResult.storeData });
                this.storeBViewChild?.refresh();

                const storeCResult = calculateStore(this.storeCData(), this.storeCTotalValue());
                this.storeCDiff.set(storeCResult.diff);
                this.storeCTotalValue.set(storeCResult.totalValue);
                this.storeCData.set({ ...storeCResult.storeData });
                this.storeCViewChild?.refresh();

                const storeDResult = calculateStore(this.storeDData(), this.storeDTotalValue());
                this.storeDDiff.set(storeDResult.diff);
                this.storeDTotalValue.set(storeDResult.totalValue);
                this.storeDData.set({ ...storeDResult.storeData });
                this.storeDViewChild?.refresh();
            });
        }, 2000);
    }

    initCharts() {
        this.storeAData.set({
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September'],
            datasets: [
                {
                    data: [55, 3, 45, 6, 44, 58, 84, 68, 64],
                    borderColor: ['#4DD0E1'],
                    backgroundColor: ['rgba(77, 208, 225, 0.8)'],
                    borderWidth: 2,
                    fill: true,
                    tension: 0.4
                }
            ]
        });

        this.storeBData.set({
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September'],
            datasets: [
                {
                    data: [81, 75, 63, 100, 69, 79, 38, 37, 76],
                    borderColor: ['#4DD0E1'],
                    backgroundColor: ['rgba(77, 208, 225, 0.8)'],
                    borderWidth: 2,
                    fill: true,
                    tension: 0.4
                }
            ]
        });

        this.storeCData.set({
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September'],
            datasets: [
                {
                    data: [99, 55, 22, 72, 24, 79, 35, 91, 48],
                    borderColor: ['#4DD0E1'],
                    backgroundColor: ['rgba(77, 208, 225, 0.8)'],
                    borderWidth: 2,
                    fill: true,
                    tension: 0.4
                }
            ]
        });

        this.storeDData.set({
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September'],
            datasets: [
                {
                    data: [5, 51, 68, 82, 28, 21, 29, 45, 44],
                    borderColor: ['#4DD0E1'],
                    backgroundColor: ['rgba(77, 208, 225, 0.8)'],
                    borderWidth: 2,
                    fill: true,
                    tension: 0.4
                }
            ]
        });
    }
}
