import { Injectable, signal } from '@angular/core';
import { LoggedInUserDto } from './user.types';

@Injectable({ providedIn: 'root' })
export class UserService {
    private readonly _user = signal<LoggedInUserDto | null>(null);

    readonly user = this._user.asReadonly();

    setUser(value: LoggedInUserDto | null): void {
        this._user.set(value);
    }
}
