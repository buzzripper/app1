import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { UserDto, UserSummaryDto } from './dto';
import { CreateUserReq, UpdateUserReq } from './req';

@Injectable({ providedIn: 'root' })
export class UserService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/user`;

    getById(id: string): Observable<UserDto | null> {
        return this._httpClient.get<UserDto | null>(`${this._baseUrl}/GetById/${id}`);
    }

    getByEmail(email: string): Observable<UserDto | null> {
        return this._httpClient.get<UserDto | null>(`${this._baseUrl}/GetByEmail/${encodeURIComponent(email)}`);
    }

    getAllByTenant(tenantId: string): Observable<UserSummaryDto[]> {
        return this._httpClient.get<UserSummaryDto[]>(`${this._baseUrl}/GetAllByTenant/${tenantId}`);
    }

    create(request: CreateUserReq): Observable<string> {
        return this._httpClient.post<string>(`${this._baseUrl}/Create`, request);
    }

    update(request: UpdateUserReq): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update`, request);
    }

    delete(id: string): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/Delete`, { body: { id } });
    }
}
