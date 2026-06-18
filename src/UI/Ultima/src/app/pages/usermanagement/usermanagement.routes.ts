import { Routes } from '@angular/router';
import { UserList } from './user-list/user-list';
import { UserCreate } from './user-create/user-create';
import { CreateLayout } from './create/create-layout/create-layout';
import { BasicInformation } from './create/basic-information/basic-information';
import { BusinessInformation } from './create/business-information/business-information';
import { LocationInformation } from './create/location-information/location-information';
import { Authorization } from './create/authorization/authorization';
import { AccountStatus } from './create/account-status/account-status';

export default [
    { path: 'list', component: UserList },
    { path: 'create-simple', component: UserCreate },
    {
        path: 'create',
        component: CreateLayout,
        children: [
            { path: '', redirectTo: 'basic-information', pathMatch: 'full' },
            { path: 'basic-information', component: BasicInformation },
            { path: 'business-information', component: BusinessInformation },
            { path: 'location-information', component: LocationInformation },
            { path: 'authorization', component: Authorization },
            { path: 'account-status', component: AccountStatus }
        ]
    },
    { path: '', redirectTo: 'list', pathMatch: 'full' }
] as Routes;
