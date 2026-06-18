import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SliderModule } from 'primeng/slider';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { FormStateService } from '../form-state.service';

interface Department {
    name: string;
    code: string;
}

@Component({
    selector: 'app-business-information',
    standalone: true,
    imports: [CommonModule, FormsModule, ButtonModule, InputTextModule, MultiSelectModule, SelectButtonModule, SliderModule, ToggleSwitchModule],
    templateUrl: './business-information.html'
})
export class BusinessInformation {
    departmentOptions: Department[] = [
        { name: 'Sales', code: 'sales' },
        { name: 'HR', code: 'hr' },
        { name: 'Marketing', code: 'marketing' },
        { name: 'Engineering', code: 'engineering' },
        { name: 'Finance', code: 'finance' }
    ];

    positionOptions = ['Admin', 'Manager', 'Employee'];

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

    getSalaryRangeDisplay(): string {
        const range = this.formState().salaryRange;
        return `${range[0].toLocaleString()}-${range[1].toLocaleString()}`;
    }

    cancel() {
        this.router.navigate(['/profile/list/list']);
    }

    next() {
        this.router.navigate(['/profile/create/location-information/location-information']);
    }
}
