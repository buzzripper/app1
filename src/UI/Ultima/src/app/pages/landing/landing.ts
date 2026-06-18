import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FeaturesWidget } from './components/featureswidget/featureswidget';
import { FooterWidget } from './components/footerwidget/footerwidget';
import { Topbar } from '@/app/pages/landing/components/topbar/topbar';
import { HeroWidget } from '@/app/pages/landing/components/herowidget/herowidget';
import { WhoUsesWidget } from '@/app/pages/landing/components/whouses/whouses';
import { CollaborationWidget } from '@/app/pages/landing/components/collaborationwidget/collaborationwidget';
import { EasyFollowWidget } from '@/app/pages/landing/components/easyfollowwidget/easyfollowwidget';
import { ShowReelsWidget } from '@/app/pages/landing/components/showreelswidget/showreelswidget';
import { NewsletterWidget } from '@/app/pages/landing/components/newsletterwidget/newsletterwidget';
import { LayoutService } from '@/app/layout/service/layout.service';

@Component({
    selector: 'app-landing',
    standalone: true,
    imports: [RouterModule, Topbar, HeroWidget, WhoUsesWidget, FeaturesWidget, CollaborationWidget, EasyFollowWidget, ShowReelsWidget, NewsletterWidget, FooterWidget],
    templateUrl: './landing.html',
    styles: `
        ::placeholder {
            color: #fff;
        }
    `
})
export class Landing implements OnInit, OnDestroy {
    layoutService = inject(LayoutService);

    ngOnInit() {
        if (!this.layoutService.isDarkTheme()) {
            this.layoutService.layoutConfig.update((state) => ({
                ...state,
                darkTheme: true
            }));
        }
    }

    ngOnDestroy() {
        this.layoutService.layoutConfig.update((state) => ({
            ...state,
            menuTheme: state.darkTheme ? 'dark' : 'light'
        }));
    }
}
