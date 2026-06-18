import { Component, EventEmitter, Input, Output, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { AvatarModule } from 'primeng/avatar';

interface ComposeData {
    to: string;
    subject: string;
    message: string;
}

@Component({
    selector: 'app-compose-dialog',
    standalone: true,
    imports: [CommonModule, FormsModule, ButtonModule, DialogModule, InputTextModule, TextareaModule, AvatarModule],
    templateUrl: './compose-dialog.html'
})
export class ComposeDialog {
    @Input() visible = false;
    @Input() initialData: ComposeData = { to: '', subject: '', message: '' };
    @Output() visibleChange = new EventEmitter<boolean>();
    @Output() send = new EventEmitter<ComposeData>();
    @Output() close = new EventEmitter<void>();

    composeData: ComposeData = { to: '', subject: '', message: '' };

    ngOnChanges() {
        this.composeData = {
            to: this.initialData.to || '',
            subject: this.initialData.subject || '',
            message: this.initialData.message || ''
        };
    }

    closeCompose() {
        this.close.emit();
        this.visible = false;
        this.visibleChange.emit(false);
    }

    sendEmail() {
        this.send.emit({ ...this.composeData });
        this.closeCompose();
    }

    onHide() {
        this.closeCompose();
    }
}
