import { Component, ElementRef, ViewChild } from '@angular/core';
import { PopoverModule } from 'primeng/popover';
import { InputGroupModule } from 'primeng/inputgroup';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { MenuModule } from 'primeng/menu';
import { InputTextModule } from 'primeng/inputtext';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';

@Component({
    selector: 'chat-widget',
    standalone: true,
    imports: [CommonModule, ButtonModule, RippleModule, MenuModule, InputTextModule, InputGroupModule, InputGroupAddonModule, PopoverModule],
    templateUrl: './chatwidget.html'
})
export class ChatWidget {
    @ViewChild('chatcontainer') chatContainerViewChild!: ElementRef;

    items = [
        { label: 'View Media', icon: 'pi pi-fw pi-images' },
        { label: 'Starred Messages', icon: 'pi pi-fw pi-star' },
        { label: 'Search', icon: 'pi pi-fw pi-search' }
    ];

    chatMessages: any[] = [
        {
            from: 'Ioni Bowcher',
            url: '/demo/images/avatar/ionibowcher.png',
            messages: ['Hey M. hope you are well.', 'Our idea is accepted by the board. Now it’s time to execute it']
        },
        { messages: ['We did it! 🤠'] },
        {
            from: 'Ioni Bowcher',
            url: '/demo/images/avatar/ionibowcher.png',
            messages: ["That's really good!"]
        },
        { messages: ['But it’s important to ship MVP ASAP'] },
        {
            from: 'Ioni Bowcher',
            url: '/demo/images/avatar/ionibowcher.png',
            messages: ['I’ll be looking at the process then, just to be sure 🤓']
        },
        { messages: ['That’s awesome. Thanks!'] }
    ];

    chatEmojis: any[] = [
        '😀',
        '😃',
        '😄',
        '😁',
        '😆',
        '😅',
        '😂',
        '🤣',
        '😇',
        '😉',
        '😊',
        '🙂',
        '🙃',
        '😋',
        '😌',
        '😍',
        '🥰',
        '😘',
        '😗',
        '😙',
        '😚',
        '🤪',
        '😜',
        '😝',
        '😛',
        '🤑',
        '😎',
        '🤓',
        '🧐',
        '🤠',
        '🥳',
        '🤗',
        '🤡',
        '😏',
        '😶',
        '😐',
        '😑',
        '😒',
        '🙄',
        '🤨',
        '🤔',
        '🤫',
        '🤭',
        '🤥',
        '😳',
        '😞',
        '😟',
        '😠',
        '😡',
        '🤬',
        '😔',
        '😟',
        '😠',
        '😡',
        '🤬',
        '😔',
        '😕',
        '🙁',
        '😬',
        '🥺',
        '😣',
        '😖',
        '😫',
        '😩',
        '🥱',
        '😤',
        '😮',
        '😱',
        '😨',
        '😰',
        '😯',
        '😦',
        '😧',
        '😢',
        '😥',
        '😪',
        '🤤'
    ];

    onEmojiClick(chatInput: any, emoji: string) {
        if (chatInput) {
            chatInput.value += emoji;
            chatInput.focus();
        }
    }

    onChatKeydown(event: KeyboardEvent) {
        if (event.key === 'Enter') {
            const message = (<HTMLInputElement>event.currentTarget).value;
            const lastMessage = this.chatMessages[this.chatMessages.length - 1];

            if (lastMessage.from) {
                this.chatMessages.push({ messages: [message] });
            } else {
                lastMessage.messages.push(message);
            }

            if (message.match(/primeng|primereact|primefaces|primevue/i)) {
                this.chatMessages.push({
                    from: 'Ioni Bowcher',
                    url: '/demo/images/avatar/ionibowcher.png',
                    messages: ['Always bet on Prime!']
                });
            }

            (<HTMLInputElement>event.currentTarget).value = '';

            const el = this.chatContainerViewChild.nativeElement;
            setTimeout(() => {
                el.scroll({
                    top: el.scrollHeight,
                    behavior: 'smooth'
                });
            }, 1);
        }
    }
}
