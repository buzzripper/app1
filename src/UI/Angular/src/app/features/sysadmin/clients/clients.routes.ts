import { Routes } from '@angular/router';
import { ClientsComponent } from './clients.component';
import { ClientsListComponent } from './list/clients-list.component';

export default [
    {
        path: '',
        component: ClientsComponent,
        children: [
            {
                path: '',
                component: ClientsListComponent,
            },
        ],
    },
] as Routes;
