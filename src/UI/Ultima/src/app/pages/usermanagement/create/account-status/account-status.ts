import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { TagModule } from 'primeng/tag';
import { TextareaModule } from 'primeng/textarea';
import { FormStateService } from '../form-state.service';

@Component({
    selector: 'app-account-status',
    standalone: true,
    imports: [CommonModule, FormsModule, ButtonModule, CheckboxModule, TagModule, TextareaModule],
    templateUrl: './account-status.html'
})
export class AccountStatus {
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

    cancel() {
        this.router.navigate(['/profile/list/list']);
    }

    save() {
        this.router.navigate(['/profile/list/list']);
    }
}
