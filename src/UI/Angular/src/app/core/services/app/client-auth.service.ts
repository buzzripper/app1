import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from 'environments/environment';
import { Result } from '../common/dtos';
import { unwrapResult, unwrapVoidResult } from '../common/unwrap-result';
import { TenantDto } from './dto';
import { CreateTenantReq, UpdateTenantReq } from './req';

@Injectable({ providedIn: 'root' })
export class ClientAuthService {
	private _httpClient = inject(HttpClient);
	private readonly _baseUrl = `${environment.apiBaseUrl}/api/app/v1/clientauth`;

	// Read - Single

	getTenantById(id: string): Promise<TenantDto | null> {
		return unwrapResult(this._httpClient.get<Result<TenantDto | null>>(`${this._baseUrl}/GetTenantById/${id}`));
	}

	getTenantBySlug(slug: string): Promise<TenantDto | null> {
		return unwrapResult(this._httpClient.get<Result<TenantDto | null>>(`${this._baseUrl}/GetTenantBySlug/${encodeURIComponent(slug)}`));
	}

	// Read - List

	getAllTenants(): Promise<TenantDto[]> {
		return unwrapResult(this._httpClient.get<Result<TenantDto[]>>(`${this._baseUrl}/GetAllTenants`));
	}

	// Create/Update

	createTenant(request: CreateTenantReq): Promise<string> {
		return unwrapResult(this._httpClient.post<Result<string>>(`${this._baseUrl}/CreateTenant`, request));
	}

	updateTenant(request: UpdateTenantReq): Promise<void> {
		return unwrapVoidResult(this._httpClient.put<Result<void>>(`${this._baseUrl}/UpdateTenant`, request));
	}

	// Delete

	deleteTenant(id: string): Promise<void> {
		return unwrapVoidResult(this._httpClient.delete<Result<void>>(`${this._baseUrl}/DeleteTenant`, { body: { id } }));
	}
}
