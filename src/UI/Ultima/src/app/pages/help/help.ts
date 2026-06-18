import { Component } from '@angular/core';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';

@Component({
    selector: 'app-help',
    standalone: true,
    imports: [IconFieldModule, InputIconModule, InputTextModule],
    templateUrl: './help.html'
})
export class Help {}
