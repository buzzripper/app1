import { Component, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'auth-sign-out',
    templateUrl: './sign-out.component.html',
    encapsulation: ViewEncapsulation.None,
    imports: [],
})
export class AuthSignOutComponent {
    /**
     * Constructor
     */
    constructor() {}

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Redirect to Azure login
     */
    signInNow(): void {
        window.location.href = '/api/Account/Login';
    }
}
