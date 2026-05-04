import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { UserClaimDto } from './dto';
import { CreateUserClaimReq, UpdateUserClaimReq } from './req';

@Injectable({ providedIn: 'root' })
export class UserClaimService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/userclaim`;

    getById(id: number): Observable<UserClaimDto | null> {
        return this._httpClient.get<UserClaimDto | null>(`${this._baseUrl}/GetById/${id}`);
    }

    getAllByUser(userId: string): Observable<UserClaimDto[]> {
        return this._httpClient.get<UserClaimDto[]>(`${this._baseUrl}/GetAllByUser/${userId}`);
    }

    create(request: CreateUserClaimReq): Observable<void> {
        return this._httpClient.post<void>(`${this._baseUrl}/Create`, request);
    }

    update(request: UpdateUserClaimReq): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update`, request);
    }

    delete(id: number): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/Delete`, { body: { id } });
    }
}
