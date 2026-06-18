import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { TagModule } from 'primeng/tag';

interface Participant {
    id: number;
    name: string;
    avatar?: string;
    status: string;
}

interface Message {
    id: number;
    senderId: string | number;
    senderName: string;
    senderAvatar?: string;
    content: string;
    timestamp: string;
    time: string;
    type: string;
    isNewDay?: boolean;
    dateLabel?: string;
}

interface ChatRoom {
    id: number;
    name: string;
    type: string;
    avatar?: string;
    archived?: boolean;
    pinned?: boolean;
    participants?: Participant[];
    lastMessage?: string;
    lastMessageSender?: string;
    lastMessageTime?: string;
    unreadCount?: number;
    messages: Message[];
}

interface SelectedUser {
    id: number;
    name: string;
    avatar?: string;
    company?: string;
    role?: string;
    phone?: string;
    email?: string;
    firstContact?: string;
    createdBy?: string;
    statusTag?: string;
    access?: string;
    linkedThreads?: string[];
}

@Component({
    selector: 'app-chat-sidebar',
    standalone: true,
    imports: [CommonModule, ButtonModule, AvatarModule, TagModule],
    templateUrl: './chatsidebar.html'
})
export class ChatSidebar {
    @Input() showContactInfo = false;
    @Input() showUserProfile = false;
    @Input() activeChat: ChatRoom | null = null;
    @Input() selectedUser: SelectedUser | null = null;
    @Output() openUserProfileEvent = new EventEmitter<number>();
    @Output() closeUserProfileEvent = new EventEmitter<void>();
    @Output() toggleContactInfoEvent = new EventEmitter<void>();

    getAvatarInitials(name: string): string {
        return name
            .split(' ')
            .map((n) => n[0])
            .join('')
            .toUpperCase();
    }

    openUserProfile(userId: number) {
        this.openUserProfileEvent.emit(userId);
    }

    closeUserProfile() {
        this.closeUserProfileEvent.emit();
    }

    toggleContactInfo() {
        this.toggleContactInfoEvent.emit();
    }
}
