import { Routes } from '@angular/router';
import { ClientsComponent } from './clients.component';
import { ClientDetailsComponent } from './details/client-details.component';
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
            {
                path: ':id',
                component: ClientDetailsComponent,
            },
        ],
    },
] as Routes;
