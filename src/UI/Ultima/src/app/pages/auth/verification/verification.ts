import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Ripple } from 'primeng/ripple';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { InputNumber } from 'primeng/inputnumber';
import { Fluid } from 'primeng/fluid';
import { AppConfigurator } from '@/app/layout/components/app.configurator/app.configurator';

@Component({
    selector: 'app-verification',
    standalone: true,
    imports: [ButtonModule, Ripple, RouterModule, FormsModule, InputNumber, Fluid, AppConfigurator],
    templateUrl: './verification.html'
})
export class Verification {
    value: string = '';

    focusOnNext(inputEl: InputNumber) {
        inputEl.input.nativeElement.focus();
    }
}
