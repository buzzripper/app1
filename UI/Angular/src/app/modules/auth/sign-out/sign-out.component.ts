import { Component, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'app/core/auth/auth.service';

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
    constructor(private _authService: AuthService) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Redirect to sign in
     */
    signInNow(): void {
        this._authService.signIn();
    }
}
