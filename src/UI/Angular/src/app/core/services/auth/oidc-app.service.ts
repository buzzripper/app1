import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'environments/environment';
import { OidcAppDto } from './dto';
import { CreateOidcAppReq, UpdateOidcAppReq } from './req';

@Injectable({ providedIn: 'root' })
export class OidcAppService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/oidcapp`;

    getById(id: string): Observable<OidcAppDto | null> {
        return this._httpClient
            .get<OidcAppDto | null>(`${this._baseUrl}/GetById/${id}`)
            .pipe(map((response: any) => response?.data ?? response));
    }

    getByClientId(clientId: string): Observable<OidcAppDto | null> {
        return this._httpClient
            .get<OidcAppDto | null>(`${this._baseUrl}/GetByClientId/${encodeURIComponent(clientId)}`)
            .pipe(map((response: any) => response?.data ?? response));
    }

    getAll(): Observable<OidcAppDto[]> {
        return this._httpClient
            .get<OidcAppDto[]>(`${this._baseUrl}/GetAll`)
            .pipe(map((response: any) => response?.data ?? response));
    }

    create(request: CreateOidcAppReq): Observable<string> {
        return this._httpClient.post<string>(`${this._baseUrl}/Create`, request);
    }

    update(request: UpdateOidcAppReq): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update`, request);
    }

    delete(id: string): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/Delete/${encodeURIComponent(id)}`);
    }
}
