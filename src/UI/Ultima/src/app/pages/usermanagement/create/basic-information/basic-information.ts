import { Component, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { FormStateService } from '../form-state.service';

@Component({
    selector: 'app-basic-information',
    standalone: true,
    imports: [CommonModule, FormsModule, ButtonModule, InputTextModule, TextareaModule],
    templateUrl: './basic-information.html'
})
export class BasicInformation {
    @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

    constructor(
        private router: Router,
        private formStateService: FormStateService
    ) {}

    get formState() {
        return this.formStateService.formState;
    }

    updateField<K extends keyof ReturnType<typeof this.formState>>(field: K, value: ReturnType<typeof this.formState>[K]) {
        this.formStateService.updateField(field, value);
    }

    triggerFileUpload() {
        this.fileInput.nativeElement.click();
    }

    handleFileUpload(event: Event) {
        const input = event.target as HTMLInputElement;
        const file = input.files?.[0];
        if (file) {
            this.formStateService.updateField('profilePhoto', file);
            this.formStateService.updateField('profilePhotoUrl', URL.createObjectURL(file));
        }
    }

    cancel() {
        this.router.navigate(['/profile/list/list']);
    }

    next() {
        this.router.navigate(['/profile/create/business-information/business-information']);
    }
}
