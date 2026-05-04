import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { RoleClaimDto } from './dto';
import { CreateRoleClaimReq, UpdateRoleClaimReq } from './req';

@Injectable({ providedIn: 'root' })
export class RoleClaimService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/roleclaim`;

    getById(id: number): Observable<RoleClaimDto | null> {
        return this._httpClient.get<RoleClaimDto | null>(`${this._baseUrl}/GetById/${id}`);
    }

    getAllByRole(roleId: string): Observable<RoleClaimDto[]> {
        return this._httpClient.get<RoleClaimDto[]>(`${this._baseUrl}/GetAllByRole/${roleId}`);
    }

    create(request: CreateRoleClaimReq): Observable<void> {
        return this._httpClient.post<void>(`${this._baseUrl}/Create`, request);
    }

    update(request: UpdateRoleClaimReq): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update`, request);
    }

    delete(id: number): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/Delete`, { body: { id } });
    }
}
