import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { AppRegistrationDto } from './dto';
import { CreateAppRegistrationReq, UpdateAppRegistrationReq } from './req';

@Injectable({ providedIn: 'root' })
export class AppRegistrationService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/appregistration`;

    getById(id: string): Observable<AppRegistrationDto | null> {
        return this._httpClient.get<AppRegistrationDto | null>(`${this._baseUrl}/GetById/${id}`);
    }

    getByClientId(clientId: string): Observable<AppRegistrationDto | null> {
        return this._httpClient.get<AppRegistrationDto | null>(`${this._baseUrl}/GetByClientId/${encodeURIComponent(clientId)}`);
    }

    getAll(): Observable<AppRegistrationDto[]> {
        return this._httpClient.get<AppRegistrationDto[]>(`${this._baseUrl}/GetAll`);
    }

    create(request: CreateAppRegistrationReq): Observable<string> {
        return this._httpClient.post<string>(`${this._baseUrl}/Create`, request);
    }

    update(request: UpdateAppRegistrationReq): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update`, request);
    }

    delete(id: string): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/Delete`, { body: { id } });
    }
}
