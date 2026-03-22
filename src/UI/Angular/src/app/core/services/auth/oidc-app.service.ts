import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from 'environments/environment';
import { Result } from '../common/dtos';
import { unwrapResult, unwrapVoidResult } from '../common/unwrap-result';
import { OidcAppDto } from './dto';
import { CreateOidcAppReq, UpdateOidcAppReq } from './req';

@Injectable({ providedIn: 'root' })
export class OidcAppService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/oidcapp`;

    getById(id: string): Promise<OidcAppDto | null> {
        return unwrapResult(this._httpClient.get<Result<OidcAppDto | null>>(`${this._baseUrl}/GetById/${id}`));
    }

    getByClientId(clientId: string): Promise<OidcAppDto | null> {
        return unwrapResult(this._httpClient.get<Result<OidcAppDto | null>>(`${this._baseUrl}/GetByClientId/${encodeURIComponent(clientId)}`));
    }

    getAll(): Promise<OidcAppDto[]> {
        return unwrapResult(this._httpClient.get<Result<OidcAppDto[]>>(`${this._baseUrl}/GetAll`));
    }

    create(request: CreateOidcAppReq): Promise<string> {
        return unwrapResult(this._httpClient.post<Result<string>>(`${this._baseUrl}/Create`, request));
    }

    update(request: UpdateOidcAppReq): Promise<void> {
        return unwrapVoidResult(this._httpClient.put<Result<void>>(`${this._baseUrl}/Update`, request));
    }

    delete(id: string): Promise<void> {
        return unwrapVoidResult(this._httpClient.delete<Result<void>>(`${this._baseUrl}/Delete/${encodeURIComponent(id)}`));
    }
}
