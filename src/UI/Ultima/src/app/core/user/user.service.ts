import { Injectable, signal } from '@angular/core';
import { User } from './user.types';

@Injectable({ providedIn: 'root' })
export class UserService {
    private readonly _user = signal<User | null>(null);

    readonly user = this._user.asReadonly();

    setUser(value: User | null): void {
        this._user.set(value);
    }
}
