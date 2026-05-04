import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'environments/environment';
import { TenantDto } from './dto';
import { CreateTenantReq, UpdateTenantReq } from './req';

@Injectable({ providedIn: 'root' })
export class TenantService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/tenant`;

    getById(id: string): Observable<TenantDto | null> {
        return this._httpClient
            .get<TenantDto | null>(`${this._baseUrl}/GetById/${id}`)
            .pipe(map((response: any) => response?.data ?? response));
    }

    getBySlug(slug: string): Observable<TenantDto | null> {
        return this._httpClient
            .get<TenantDto | null>(`${this._baseUrl}/GetBySlug/${encodeURIComponent(slug)}`)
            .pipe(map((response: any) => response?.data ?? response));
    }

    getAll(): Observable<TenantDto[]> {
        return this._httpClient
            .get<TenantDto[]>(`${this._baseUrl}/GetAll`)
            .pipe(map((response: any) => response?.data ?? response));
    }

    create(request: CreateTenantReq): Observable<string> {
        return this._httpClient.post<string>(`${this._baseUrl}/Create`, request);
    }

    update(request: UpdateTenantReq): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update`, request);
    }

    delete(id: string): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/Delete`, { body: { id } });
    }
}
