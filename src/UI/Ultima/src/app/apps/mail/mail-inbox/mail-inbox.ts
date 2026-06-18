import { Component, OnInit, signal, computed, ViewChild, model, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { AvatarModule } from 'primeng/avatar';
import { TagModule } from 'primeng/tag';
import { MenuModule } from 'primeng/menu';
import { Menu } from 'primeng/menu';
import { PaginatorModule } from 'primeng/paginator';
import { DrawerModule } from 'primeng/drawer';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { MenuItem } from 'primeng/api';
import { ComposeDialog } from '../compose-dialog/compose-dialog';
import { MailService, Email } from '../mail.service';

interface MenuItemData {
    label: string;
    icon: string;
    count?: number;
}

@Component({
    selector: 'app-mail-inbox',
    standalone: true,
    imports: [CommonModule, FormsModule, ButtonModule, TableModule, AvatarModule, TagModule, MenuModule, PaginatorModule, DrawerModule, InputTextModule, IconFieldModule, InputIconModule, ComposeDialog],
    templateUrl: './mail-inbox.html'
})
export class MailInbox implements OnInit {
    @ViewChild('actionMenu') actionMenu!: Menu;
    @ViewChild('bulkActionMenu') bulkActionMenu!: Menu;

    private mailService = inject(MailService);
    private router = inject(Router);
    private route = inject(ActivatedRoute);

    emailsData = this.mailService.emailsData;
    menuItems = signal<MenuItemData[]>([]);
    categoryItems = signal<MenuItemData[]>([]);
    selectedMenuItem = signal<string | null>('Inbox');
    selectedCategory = signal<string | null>(null);

    selectedEmails = model<Email[]>([]);
    searchQuery = model('');
    showComposeOverlay = model(false);
    showMenuDrawer = model(false);
    first = signal(0);
    rows = signal(15);
    selectedEmailId = signal<number | null>(null);
    selectedEmailData = signal<Email | null>(null);

    Math = Math;

    async ngOnInit() {
        await this.mailService.loadEmails();

        const response = await fetch('/demo/data/emailData.json');
        const data = await response.json();
        this.menuItems.set(data.menuItems);
        this.categoryItems.set(data.categoryItems);

        const viewFromQuery = this.route.snapshot.queryParams['view'];
        if (viewFromQuery) {
            const isMenuItem = this.menuItems().some((item) => item.label === viewFromQuery);
            const isCategoryItem = this.categoryItems().some((item) => item.label === viewFromQuery);

            if (isMenuItem) {
                this.selectedMenuItem.set(viewFromQuery);
                this.selectedCategory.set(null);
            } else if (isCategoryItem) {
                this.selectedCategory.set(viewFromQuery);
                this.selectedMenuItem.set(null);
            }
        }
    }

    // Filter functions
    getInboxEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => !email.deleted && !email.spam && !email.archived);
    }

    getStarredEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.starred && !email.deleted && !email.spam);
    }

    getImportantEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.important && !email.deleted && !email.spam);
    }

    getArchivedEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.archived && !email.deleted);
    }

    getSpamEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.spam && !email.deleted);
    }

    getDeletedEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.deleted);
    }

    getCategoryEmails(emailList: Email[], category: string): Email[] {
        return emailList.filter((email) => email.category === category && !email.deleted && !email.spam && !email.archived);
    }

    getUnreadInboxEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => !email.deleted && !email.spam && !email.archived && !email.read);
    }

    getUnreadStarredEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.starred && !email.deleted && !email.spam && !email.read);
    }

    getUnreadImportantEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.important && !email.deleted && !email.spam && !email.read);
    }

    getUnreadArchivedEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.archived && !email.deleted && !email.read);
    }

    getUnreadSpamEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.spam && !email.deleted && !email.read);
    }

    getUnreadDeletedEmails(emailList: Email[]): Email[] {
        return emailList.filter((email) => email.deleted && !email.read);
    }

    getUnreadCategoryEmails(emailList: Email[], category: string): Email[] {
        return emailList.filter((email) => email.category === category && !email.deleted && !email.spam && !email.archived && !email.read);
    }

    baseFilteredEmails = computed(() => {
        const menuItem = this.selectedMenuItem();
        const category = this.selectedCategory();

        if (menuItem) {
            switch (menuItem) {
                case 'Inbox':
                    return this.getInboxEmails(this.emailsData());
                case 'Starred':
                    return this.getStarredEmails(this.emailsData());
                case 'Important':
                    return this.getImportantEmails(this.emailsData());
                case 'Sent':
                    return [];
                case 'Archived':
                    return this.getArchivedEmails(this.emailsData());
                case 'Spam':
                    return this.getSpamEmails(this.emailsData());
                case 'Trash':
                    return this.getDeletedEmails(this.emailsData());
                default:
                    return this.getInboxEmails(this.emailsData());
            }
        } else if (category) {
            return this.getCategoryEmails(this.emailsData(), category);
        }
        return this.getInboxEmails(this.emailsData());
    });

    filteredEmails = computed(() => {
        const emails = this.baseFilteredEmails();
        const query = this.searchQuery();
        if (!query.trim()) {
            return emails;
        }
        return emails.filter((email) => email.sender.toLowerCase().includes(query.toLowerCase().trim()));
    });

    paginatedEmails = computed(() => {
        const start = this.first();
        const end = start + this.rows();
        return this.filteredEmails().slice(start, end);
    });

    menuItemsWithCounts = computed(() => {
        return this.menuItems().map((item) => ({
            ...item,
            count: this.getMenuItemCount(item.label)
        }));
    });

    categoryItemsWithCounts = computed(() => {
        return this.categoryItems().map((item) => ({
            ...item,
            count: this.getCategoryItemCount(item.label)
        }));
    });

    actionMenuItems = computed<MenuItem[]>(() => {
        const email = this.selectedEmailData();
        const emailId = this.selectedEmailId();
        const isInTrash = email?.deleted;

        if (isInTrash) {
            return [{ label: 'Recover', icon: 'pi pi-replay', command: () => this.recoverEmail(emailId!) }];
        }

        return [
            { label: 'Forward', icon: 'pi pi-reply', command: () => {} },
            {
                label: email?.archived ? 'Unarchive' : 'Archive',
                icon: email?.archived ? 'pi pi-replay' : 'pi pi-inbox',
                command: () => (email?.archived ? this.unarchiveEmail(emailId!) : this.archiveEmail(emailId!))
            },
            { label: 'Spam', icon: 'pi pi-ban', command: () => this.markAsSpam(emailId!) },
            { label: 'Delete', icon: 'pi pi-trash', command: () => this.deleteEmail(emailId!) }
        ];
    });

    bulkActionMenuItems = computed<MenuItem[]>(() => {
        const selected = this.selectedEmails();
        const hasArchivedEmails = selected.some((email) => email.archived);
        const hasNonArchivedEmails = selected.some((email) => !email.archived);
        const hasDeletedEmails = selected.some((email) => email.deleted);

        if (this.selectedMenuItem() === 'Trash' || hasDeletedEmails) {
            return [{ label: 'Recover', icon: 'pi pi-replay', command: () => this.bulkRecover() }];
        }

        const items: MenuItem[] = [
            { label: 'Mark as Read', icon: 'pi pi-eye', command: () => this.bulkMarkAsRead() },
            { label: 'Mark as Unread', icon: 'pi pi-eye-slash', command: () => this.bulkMarkAsUnread() },
            { separator: true },
            { label: 'Star', icon: 'pi pi-star', command: () => this.bulkToggleStar(true) },
            { label: 'Unstar', icon: 'pi pi-star-fill', command: () => this.bulkToggleStar(false) },
            { separator: true },
            { label: 'Mark as Important', icon: 'pi pi-bookmark', command: () => this.bulkToggleImportant(true) },
            { label: 'Mark as Not Important', icon: 'pi pi-bookmark-fill', command: () => this.bulkToggleImportant(false) },
            { separator: true }
        ];

        if (hasNonArchivedEmails) {
            items.push({ label: 'Archive', icon: 'pi pi-inbox', command: () => this.bulkArchive() });
        }
        if (hasArchivedEmails) {
            items.push({ label: 'Unarchive', icon: 'pi pi-replay', command: () => this.bulkUnarchive() });
        }
        items.push({ label: 'Mark as Spam', icon: 'pi pi-ban', command: () => this.bulkMarkAsSpam() });
        items.push({ label: 'Delete', icon: 'pi pi-trash', command: () => this.bulkDelete() });

        return items;
    });

    getMenuItemCount(label: string): number {
        switch (label) {
            case 'Inbox':
                return this.getUnreadInboxEmails(this.emailsData()).length;
            case 'Starred':
                return this.getUnreadStarredEmails(this.emailsData()).length;
            case 'Important':
                return this.getUnreadImportantEmails(this.emailsData()).length;
            case 'Sent':
                return 0;
            case 'Archived':
                return this.getUnreadArchivedEmails(this.emailsData()).length;
            case 'Spam':
                return this.getUnreadSpamEmails(this.emailsData()).length;
            case 'Trash':
                return this.getUnreadDeletedEmails(this.emailsData()).length;
            default:
                return 0;
        }
    }

    getCategoryItemCount(label: string): number {
        return this.getUnreadCategoryEmails(this.emailsData(), label).length;
    }

    getAvatarInitials(name: string): string {
        return name
            .split(' ')
            .map((n) => n[0])
            .join('')
            .toUpperCase();
    }

    getAvatarColor(name: string): string {
        const colors = ['bg-violet-100 text-violet-950', 'bg-lime-100 text-lime-950', 'bg-red-100 text-rose-950', 'bg-cyan-100 text-cyan-950', 'bg-indigo-100 text-indigo-950'];
        const index = name.charCodeAt(0) % colors.length;
        return colors[index];
    }

    onPageChange(event: any) {
        this.first.set(event.first);
    }

    toggleActionMenu(event: Event, emailId: number) {
        this.selectedEmailId.set(emailId);
        this.selectedEmailData.set(this.emailsData().find((e) => e.id === emailId) ?? null);
        this.actionMenu.toggle(event);
    }

    toggleBulkActionMenu(event: Event) {
        this.bulkActionMenu.toggle(event);
    }

    openCompose() {
        this.showComposeOverlay.set(true);
    }

    closeCompose() {
        this.showComposeOverlay.set(false);
    }

    sendEmail() {
        this.closeCompose();
    }

    navigateToEmail(email: Email) {
        if (!email.read) {
            this.mailService.markAsRead(email.id);
        }
        const from = this.selectedMenuItem() || this.selectedCategory() || 'Inbox';
        this.router.navigate(['/apps/mail/detail/detail', email.id], { queryParams: { from } });
    }

    toggleMenuDrawer() {
        this.showMenuDrawer.set(!this.showMenuDrawer());
    }

    selectMenuItem(label: string) {
        this.selectedMenuItem.set(label);
        this.selectedCategory.set(null);
        this.first.set(0);
    }

    selectCategory(label: string) {
        this.selectedCategory.set(label);
        this.selectedMenuItem.set(null);
        this.first.set(0);
    }

    toggleStar(emailId: number) {
        this.mailService.toggleStar(emailId);
    }

    toggleImportant(emailId: number) {
        this.mailService.toggleImportant(emailId);
    }

    archiveEmail(emailId: number) {
        this.mailService.archiveEmail(emailId);
    }

    unarchiveEmail(emailId: number) {
        this.mailService.unarchiveEmail(emailId);
    }

    markAsSpam(emailId: number) {
        this.mailService.markAsSpam(emailId);
    }

    deleteEmail(emailId: number) {
        this.mailService.deleteEmail(emailId);
    }

    recoverEmail(emailId: number) {
        this.mailService.recoverEmail(emailId);
    }

    bulkMarkAsRead() {
        this.selectedEmails().forEach((selected) => {
            this.mailService.markAsRead(selected.id);
        });
        this.selectedEmails.set([]);
    }

    bulkMarkAsUnread() {
        this.selectedEmails().forEach((selected) => {
            this.mailService.markAsUnread(selected.id);
        });
        this.selectedEmails.set([]);
    }

    bulkToggleStar(starred: boolean) {
        this.selectedEmails().forEach((selected) => {
            this.mailService.updateEmail(selected.id, { starred });
        });
        this.selectedEmails.set([]);
    }

    bulkToggleImportant(important: boolean) {
        this.selectedEmails().forEach((selected) => {
            this.mailService.updateEmail(selected.id, { important });
        });
        this.selectedEmails.set([]);
    }

    bulkArchive() {
        this.selectedEmails().forEach((selected) => {
            this.mailService.archiveEmail(selected.id);
        });
        this.selectedEmails.set([]);
    }

    bulkUnarchive() {
        this.selectedEmails().forEach((selected) => {
            this.mailService.unarchiveEmail(selected.id);
        });
        this.selectedEmails.set([]);
    }

    bulkMarkAsSpam() {
        this.selectedEmails().forEach((selected) => {
            this.mailService.markAsSpam(selected.id);
        });
        this.selectedEmails.set([]);
    }

    bulkDelete() {
        this.selectedEmails().forEach((selected) => {
            this.mailService.deleteEmail(selected.id);
        });
        this.selectedEmails.set([]);
    }

    bulkRecover() {
        this.selectedEmails().forEach((selected) => {
            this.mailService.recoverEmail(selected.id);
        });
        this.selectedEmails.set([]);
    }
}
