import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { FormStateService } from '../form-state.service';

@Component({
    selector: 'app-authorization',
    standalone: true,
    imports: [CommonModule, FormsModule, ButtonModule, CheckboxModule, RadioButtonModule, ToggleSwitchModule],
    templateUrl: './authorization.html'
})
export class Authorization {
    authorizationOptions = ['Download Reports', 'Export Data', 'Edit Users', 'Access Custom Reports', 'Use API'];

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

    next() {
        this.router.navigate(['/profile/create/account-status/account-status']);
    }
}
