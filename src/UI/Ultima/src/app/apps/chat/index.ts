import { Component, computed, OnInit, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { MenuModule } from 'primeng/menu';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MenuItem } from 'primeng/api';
import { Menu } from 'primeng/menu';
import { ChatMenu } from './chat-menu/chat-menu';
import { ChatBox } from './chatbox/chatbox';
import { ChatSidebar } from './chatsidebar/chatsidebar';

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

interface CurrentChatUser {
    id: string;
    name: string;
    avatar?: string;
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

interface Contact {
    id: number;
    name: string;
    avatar?: string;
    role?: string;
    company?: string;
    status?: string;
}

@Component({
    selector: 'app-chat',
    standalone: true,
    imports: [CommonModule, ButtonModule, MenuModule, ConfirmDialogModule, ChatMenu, ChatBox, ChatSidebar],
    providers: [ConfirmationService],
    templateUrl: './index.html'
})
export class Chat implements OnInit {
    @ViewChild('menu') menu!: Menu;

    chatRooms = signal<ChatRoom[]>([]);
    currentChatUser = signal<CurrentChatUser>({ id: 'me', name: 'You' });
    userData = signal<Record<number, SelectedUser>>({});
    activeChatId = signal<number | null>(1);
    showContactInfo = false;
    showUserProfile = false;
    selectedUserId = signal<number | null>(null);
    showChatView = false;

    activeChat = computed(() => {
        return this.chatRooms().find((chat) => chat.id === this.activeChatId()) ?? null;
    });

    selectedUser = computed(() => {
        const userId = this.selectedUserId();
        return userId ? this.userData()[userId] : null;
    });

    menuItems = computed<MenuItem[]>(() => {
        this.chatRooms();
        const chat = this.activeChat();
        return [
            {
                label: chat?.pinned ? 'Unpin Chat' : 'Pin Chat',
                icon: 'pi pi-thumbtack',
                command: () => this.togglePin()
            },
            {
                label: 'Delete Chat',
                icon: 'pi pi-trash',
                command: () => this.deleteChat()
            },
            {
                label: chat?.archived ? 'Restore Chat' : 'Archive Chat',
                icon: chat?.archived ? 'pi pi-replay' : 'pi pi-inbox',
                command: () => (chat?.archived ? this.restoreChat() : this.archiveChat())
            }
        ];
    });

    constructor(private confirmationService: ConfirmationService) {}

    async ngOnInit() {
        const response = await fetch('/demo/data/chatData.json');
        const data = await response.json();
        this.chatRooms.set(data.chatRooms);
        this.currentChatUser.set(data.currentUser);
        this.userData.set(data.userData);
    }

    deleteChat() {
        this.confirmationService.confirm({
            message: `Are you sure you want to delete "${this.activeChat()?.name}"? This action cannot be undone.`,
            header: 'Delete Chat',
            rejectButtonProps: {
                label: 'Cancel',
                severity: 'secondary',
                outlined: true
            },
            acceptButtonProps: {
                label: 'Delete',
                severity: 'danger'
            },
            accept: () => {
                const rooms = this.chatRooms();
                const chatIndex = rooms.findIndex((chat) => chat.id === this.activeChatId());
                if (chatIndex !== -1) {
                    rooms.splice(chatIndex, 1);
                    this.chatRooms.set([...rooms]);
                    if (rooms.length > 0) {
                        this.activeChatId.set(rooms[0].id);
                    } else {
                        this.activeChatId.set(null);
                        this.showChatView = false;
                    }
                }
            }
        });
    }

    archiveChat() {
        const rooms = this.chatRooms();
        const chat = rooms.find((c) => c.id === this.activeChatId());
        if (chat) {
            chat.archived = true;
            this.chatRooms.set([...rooms]);
            const availableChats = rooms.filter((c) => !c.archived);
            if (availableChats.length > 0) {
                this.activeChatId.set(availableChats[0].id);
            } else {
                this.activeChatId.set(null);
                this.showChatView = false;
            }
        }
    }

    restoreChat() {
        const rooms = this.chatRooms();
        const chat = rooms.find((c) => c.id === this.activeChatId());
        if (chat) {
            chat.archived = false;
            this.chatRooms.set([...rooms]);
        }
    }

    togglePin() {
        const rooms = this.chatRooms();
        const chat = rooms.find((c) => c.id === this.activeChatId());
        if (chat) {
            chat.pinned = !chat.pinned;
            this.chatRooms.set([...rooms]);
        }
    }

    formatParticipants(participants: Participant[]): string {
        if (participants.length <= 3) {
            return participants.map((p) => p.name).join(', ');
        }
        const first3 = participants
            .slice(0, 3)
            .map((p) => p.name)
            .join(', ');
        return `${first3} ...`;
    }

    toggleContactInfo() {
        if (this.activeChat()?.type === 'individual') {
            if (this.showUserProfile) {
                this.closeUserProfile();
            } else {
                const participant = this.activeChat()?.participants?.[0];
                if (participant) {
                    this.openUserProfile(participant.id);
                }
            }
        } else {
            this.showContactInfo = !this.showContactInfo;
            this.showUserProfile = false;
        }
    }

    openUserProfile(userId: string | number) {
        this.selectedUserId.set(Number(userId));
        this.showUserProfile = true;
        this.showContactInfo = false;
    }

    closeUserProfile() {
        this.showUserProfile = false;
        this.selectedUserId.set(null);
    }

    showMenu(event: Event) {
        this.menu.toggle(event);
    }

    selectChat(chatId: number) {
        this.activeChatId.set(chatId);
        this.showChatView = true;
    }

    goBackToMenu() {
        this.showChatView = false;
    }

    createNewChat(contact: Contact) {
        const rooms = this.chatRooms();
        const newChatId = Math.max(...rooms.map((c) => c.id)) + 1;

        const newChat: ChatRoom = {
            id: newChatId,
            name: contact.name,
            type: 'individual',
            archived: false,
            avatar: contact.avatar,
            lastMessage: 'Start a conversation...',
            lastMessageSender: undefined,
            lastMessageTime: 'Now',
            unreadCount: 0,
            messages: []
        };

        this.chatRooms.set([newChat, ...rooms]);
        this.activeChatId.set(newChatId);
        this.showChatView = true;
    }

    handleSendMessage(message: Message) {
        const rooms = this.chatRooms();
        const chat = rooms.find((c) => c.id === this.activeChatId());
        if (chat) {
            chat.messages.push(message);
            this.chatRooms.set([...rooms]);
        }
    }

    encodeURIComponent(str: string): string {
        return encodeURIComponent(str);
    }
}
