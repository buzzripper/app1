import { Component, OnInit, signal, computed, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { MenuModule } from 'primeng/menu';
import { Menu } from 'primeng/menu';
import { PopoverModule } from 'primeng/popover';
import { Popover } from 'primeng/popover';
import { TextareaModule } from 'primeng/textarea';
import { MenuItem } from 'primeng/api';
import { MailService, Email } from '../mail.service';

@Component({
    selector: 'app-mail-detail',
    standalone: true,
    imports: [CommonModule, FormsModule, ButtonModule, AvatarModule, MenuModule, PopoverModule, TextareaModule],
    styles: [
        `
            .flip-icon-horizontal {
                transform: scaleX(-1);
            }
        `
    ],
    templateUrl: './mail-detail.html'
})
export class MailDetail implements OnInit {
    @ViewChild('actionMenu') actionMenu!: Menu;
    @ViewChild('recipientPanel') recipientPanel!: Popover;

    private mailService = inject(MailService);
    private router = inject(Router);
    private route = inject(ActivatedRoute);

    emailId = signal<number | null>(null);
    fromView = signal<string>('Inbox');

    replyMessage = '';
    showReplyEditor = false;

    async ngOnInit() {
        await this.mailService.loadEmails();

        const id = this.route.snapshot.params['id'];
        this.emailId.set(parseInt(id));
        this.fromView.set(this.route.snapshot.queryParams['from'] || 'Inbox');
    }

    currentEmail = computed(() => {
        const id = this.emailId();
        if (!id) return null;
        return this.mailService.getEmailById(id);
    });

    actionMenuItems = computed<MenuItem[]>(() => {
        const email = this.currentEmail();
        const isInTrash = email?.deleted;

        if (isInTrash) {
            return [{ label: 'Recover', icon: 'pi pi-replay', command: () => this.recoverEmail() }];
        }

        return [
            { label: 'Forward', icon: 'pi pi-reply', command: () => {} },
            {
                label: email?.archived ? 'Unarchive' : 'Archive',
                icon: email?.archived ? 'pi pi-replay' : 'pi pi-inbox',
                command: () => (email?.archived ? this.unarchiveEmail() : this.archiveEmail())
            },
            { label: 'Mark as Spam', icon: 'pi pi-ban', command: () => this.markAsSpam() },
            { label: 'Delete', icon: 'pi pi-trash', command: () => this.deleteEmail() }
        ];
    });

    getAvatarInitials(name: string): string {
        return name
            .split(' ')
            .map((n) => n[0])
            .join('')
            .toUpperCase();
    }

    goBack() {
        this.router.navigate(['/apps/mail/inbox'], { queryParams: { view: this.fromView() } });
    }

    toggleReply() {
        this.showReplyEditor = !this.showReplyEditor;
    }

    sendReply() {
        this.showReplyEditor = false;
        this.replyMessage = '';
    }

    toggleStar() {
        const email = this.currentEmail();
        if (email) {
            this.mailService.toggleStar(email.id);
        }
    }

    toggleImportant() {
        const email = this.currentEmail();
        if (email) {
            this.mailService.toggleImportant(email.id);
        }
    }

    archiveEmail() {
        const email = this.currentEmail();
        if (email) {
            this.mailService.archiveEmail(email.id);
            this.goBack();
        }
    }

    unarchiveEmail() {
        const email = this.currentEmail();
        if (email) {
            this.mailService.unarchiveEmail(email.id);
        }
    }

    markAsSpam() {
        const email = this.currentEmail();
        if (email) {
            this.mailService.markAsSpam(email.id);
            this.goBack();
        }
    }

    deleteEmail() {
        const email = this.currentEmail();
        if (email) {
            this.mailService.deleteEmail(email.id);
            this.goBack();
        }
    }

    recoverEmail() {
        const email = this.currentEmail();
        if (email) {
            this.mailService.recoverEmail(email.id);
            this.goBack();
        }
    }

    showActionMenu(event: Event) {
        this.actionMenu.toggle(event);
    }

    showRecipientDetails(event: Event) {
        this.recipientPanel.toggle(event);
    }
}
