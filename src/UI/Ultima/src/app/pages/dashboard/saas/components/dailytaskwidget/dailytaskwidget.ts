import { Component, Input } from '@angular/core';
import { AvatarModule } from 'primeng/avatar';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { KnobModule } from 'primeng/knob';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { AvatarGroup } from 'primeng/avatargroup';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'daily-task-widget',
    standalone: true,
    imports: [CommonModule, FormsModule, KnobModule, ButtonModule, RippleModule, CheckboxModule, AvatarModule, AvatarGroup],
    templateUrl: './dailytaskwidget.html',
    host: {
        class: 'col-span-12 md:col-span-4'
    }
})
export class DailyTaskWidget {
    @Input() dailyTasks: any[] = [];

    completeTask = 1;

    changeChecked() {
        this.completeTask = this.dailyTasks.filter((task) => task.checked).length;
    }
}
