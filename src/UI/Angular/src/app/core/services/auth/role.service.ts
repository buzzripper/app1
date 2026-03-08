import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { RoleDto } from './dto';
import { CreateRoleReq, UpdateRoleReq } from './req';

@Injectable({ providedIn: 'root' })
export class RoleService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/role`;

    getById(id: string): Observable<RoleDto | null> {
        return this._httpClient.get<RoleDto | null>(`${this._baseUrl}/GetById/${id}`);
    }

    getByName(name: string): Observable<RoleDto | null> {
        return this._httpClient.get<RoleDto | null>(`${this._baseUrl}/GetByName/${encodeURIComponent(name)}`);
    }

    getAllByTenant(tenantId: string): Observable<RoleDto[]> {
        return this._httpClient.get<RoleDto[]>(`${this._baseUrl}/GetAllByTenant/${tenantId}`);
    }

    create(request: CreateRoleReq): Observable<string> {
        return this._httpClient.post<string>(`${this._baseUrl}/Create`, request);
    }

    update(request: UpdateRoleReq): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update`, request);
    }

    delete(id: string): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/Delete`, { body: { id } });
    }
}
