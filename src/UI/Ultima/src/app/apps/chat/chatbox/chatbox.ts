import { AfterViewChecked, Component, ElementRef, EventEmitter, Input, model, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';

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

interface Participant {
    id: number;
    name: string;
    avatar?: string;
    status: string;
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

interface CurrentChatUser {
    id: string;
    name: string;
    avatar?: string;
}

@Component({
    selector: 'app-chat-box',
    standalone: true,
    imports: [CommonModule, FormsModule, ButtonModule, AvatarModule, InputTextModule],
    templateUrl: './chatbox.html',
    host: {
        class: 'flex flex-1'
    }
})
export class ChatBox implements AfterViewChecked {
    @Input() activeChat: ChatRoom | null = null;
    @Input() currentChatUser: CurrentChatUser = { id: 'me', name: 'You' };
    @Output() openUserProfileEvent = new EventEmitter<string | number>();
    @Output() sendMessageEvent = new EventEmitter<Message>();

    @ViewChild('messagesContainer') messagesContainer!: ElementRef;

    newMessage = model('');
    private shouldScrollToBottom = false;

    ngAfterViewChecked() {
        if (this.shouldScrollToBottom) {
            this.scrollToBottom();
            this.shouldScrollToBottom = false;
        }
    }

    scrollToBottom() {
        if (this.messagesContainer?.nativeElement) {
            this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
        }
    }

    sendMessage() {
        if (!this.newMessage().trim()) return;

        const message: Message = {
            id: Date.now(),
            senderId: this.currentChatUser.id,
            senderName: this.currentChatUser.name,
            content: this.newMessage().trim(),
            timestamp: new Date().toISOString(),
            time: new Date().toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' }),
            type: 'text'
        };

        this.sendMessageEvent.emit(message);
        this.newMessage.set('');
        this.shouldScrollToBottom = true;
    }

    getAvatarInitials(name: string): string {
        return name
            .split(' ')
            .map((n) => n[0])
            .join('')
            .toUpperCase();
    }

    openUserProfile(userId: string | number) {
        this.openUserProfileEvent.emit(userId);
    }
}
