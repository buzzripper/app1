import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { ScopeDto } from './dto';
import { CreateScopeReq, UpdateScopeReq } from './req';

@Injectable({ providedIn: 'root' })
export class ScopeService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/scope`;

    getById(id: string): Observable<ScopeDto | null> {
        return this._httpClient.get<ScopeDto | null>(`${this._baseUrl}/GetById/${id}`);
    }

    getByName(name: string): Observable<ScopeDto | null> {
        return this._httpClient.get<ScopeDto | null>(`${this._baseUrl}/GetByName/${encodeURIComponent(name)}`);
    }

    getAll(): Observable<ScopeDto[]> {
        return this._httpClient.get<ScopeDto[]>(`${this._baseUrl}/GetAll`);
    }

    create(request: CreateScopeReq): Observable<string> {
        return this._httpClient.post<string>(`${this._baseUrl}/Create`, request);
    }

    update(request: UpdateScopeReq): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update`, request);
    }

    delete(id: string): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/Delete`, { body: { id } });
    }
}
