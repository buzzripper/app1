import { NgClass } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';

interface Section {
    id: string;
    title: string;
}

@Component({
    selector: 'app-detail2',
    imports: [NgClass, ButtonModule, TagModule],
    templateUrl: './detail2.html',
    styles: `
        h1,
        h2,
        h3,
        h4,
        h5,
        h6,
        p {
            overflow-wrap: break-word;
            word-wrap: break-word;
            word-break: break-word;
        }
    `
})
export class Detail2 implements OnInit, OnDestroy {
    @ViewChild('scrollContainer') scrollContainer!: ElementRef;

    activeSection = signal('manufacturing-giants');
    isScrollBlocked = false;
    private scrollHandler: (() => void) | null = null;

    sections: Section[] = [
        { id: 'manufacturing-giants', title: 'How Manufacturing Giants Drive Economic Growth' },
        { id: 'manufacturing-investment', title: 'Manufacturing Investment: Risk or Opportunity?' },
        { id: 'strategic-manufacturing', title: 'Strategic Manufacturing Investment' },
        { id: 'workforce-development', title: 'Workforce Development and Skills Training' }
    ];

    ngOnInit() {
        this.scrollHandler = this.handleScroll.bind(this);
        setTimeout(() => {
            this.handleScroll();
            const container = this.scrollContainer?.nativeElement;
            if (container) {
                container.addEventListener('scroll', this.scrollHandler, { passive: true });
            }
        }, 0);
    }

    ngOnDestroy() {
        const container = this.scrollContainer?.nativeElement;
        if (container && this.scrollHandler) {
            container.removeEventListener('scroll', this.scrollHandler);
        }
    }

    scrollToSection(sectionId: string) {
        this.activeSection.set(sectionId);
        this.isScrollBlocked = true;

        const element = document.getElementById(sectionId);
        const container = this.scrollContainer?.nativeElement;

        if (element && container) {
            const yOffset = -80;
            const elementPosition = element.getBoundingClientRect().top;
            const containerPosition = container.getBoundingClientRect().top;
            const offsetPosition = container.scrollTop + elementPosition - containerPosition + yOffset;

            container.scrollTo({
                top: offsetPosition,
                behavior: 'smooth'
            });

            setTimeout(() => {
                this.isScrollBlocked = false;
                setTimeout(() => {
                    this.handleScroll();
                }, 50);
            }, 800);
        } else {
            this.isScrollBlocked = false;
        }
    }

    getIndicatorOffset(): number {
        const index = this.sections.findIndex((s) => s.id === this.activeSection());
        const baseOffset = index * 44;
        const isFirst = index === 0;
        const isLast = index === this.sections.length - 1;
        const adjustment = isFirst ? 13 : isLast ? -4 : 0;
        return baseOffset + adjustment;
    }

    private getElementTop(element: HTMLElement): number {
        const container = this.scrollContainer?.nativeElement;
        if (!container) return 0;
        const rect = element.getBoundingClientRect();
        const containerRect = container.getBoundingClientRect();
        return rect.top - containerRect.top + container.scrollTop;
    }

    private getScrollTop(): number {
        const container = this.scrollContainer?.nativeElement;
        return container ? container.scrollTop : 0;
    }

    private handleScroll() {
        if (this.isScrollBlocked) {
            return;
        }

        const scrollTop = this.getScrollTop();
        const threshold = 100;
        const oldActiveSection = this.activeSection();

        const sectionElements = this.sections
            .map((section) => ({
                ...section,
                element: document.getElementById(section.id)
            }))
            .filter((section) => section.element)
            .map((section) => ({
                ...section,
                top: this.getElementTop(section.element!)
            }))
            .sort((a, b) => a.top - b.top);

        if (sectionElements.length === 0) {
            return;
        }

        let newActiveSection = sectionElements[0].id;

        for (let i = 0; i < sectionElements.length; i++) {
            const section = sectionElements[i];
            const hasPassedSection = scrollTop >= section.top - threshold;

            if (hasPassedSection) {
                newActiveSection = section.id;
            } else {
                break;
            }
        }

        if (oldActiveSection !== newActiveSection) {
            this.activeSection.set(newActiveSection);
        }
    }
}
