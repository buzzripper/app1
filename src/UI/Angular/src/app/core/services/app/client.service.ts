import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from 'environments/environment';
import { ListPage, Result } from '../common/dtos';
import { unwrapResult, unwrapVoidResult } from '../common/unwrap-result';
import { ClientDto, ClientLookupDto, ClientRouteDto } from './dto';
import { GetAllClientLookupItemsReq, GetAllClientsReq, SearchClientsByNameReq, CreateClientReq, UpdateClientReq, UpdateClientBaseUrlReq } from './req';

@Injectable({ providedIn: 'root' })
export class ClientService {
	private _httpClient = inject(HttpClient);
	private readonly _baseUrl = `${environment.apiBaseUrl}/api/app/v1/client`;

	// Create/Update
	
	createClient(request: CreateClientReq): Promise<void> {
		return unwrapVoidResult(this._httpClient.patch<Result<void>>(`${this._baseUrl}/CreateClient`, request));
	}
	
	updateClient(request: UpdateClientReq): Promise<void> {
		return unwrapVoidResult(this._httpClient.patch<Result<void>>(`${this._baseUrl}/UpdateClient`, request));
	}
	
	updateClientBaseUrl(request: UpdateClientBaseUrlReq): Promise<void> {
		return unwrapVoidResult(this._httpClient.patch<Result<void>>(`${this._baseUrl}/UpdateClientBaseUrl`, request));
	}

	// Delete
	
	delete(id: string): Promise<void> {
		return unwrapVoidResult(this._httpClient.delete<Result<void>>(`${this._baseUrl}/DeleteClient`, { body: { id } }));
	}

	// Read Single
	
	getClientById(id: string): Promise<ClientDto> {
		return unwrapResult(this._httpClient.get<Result<ClientDto>>(`${this._baseUrl}/GetClientById/${id}`));
	}
	
	getClientByKey(key: string): Promise<ClientDto> {
		return unwrapResult(this._httpClient.get<Result<ClientDto>>(`${this._baseUrl}/GetClientByKey/${key}`));
	}

	// Read List
	
	getAllClientLookupItems(request: GetAllClientLookupItemsReq): Promise<ClientLookupDto[]> {
		return unwrapResult(this._httpClient.post<Result<ClientLookupDto[]>>(`${this._baseUrl}/GetAllClientLookupItems`, request));
	}
	
	getAllClientRoutes(): Promise<ClientRouteDto[]> {
		return unwrapResult(this._httpClient.get<Result<ClientRouteDto[]>>(`${this._baseUrl}/GetAllClientRoutes`));
	}
	
	getAllClients(request: GetAllClientsReq): Promise<ClientDto[]> {
		return unwrapResult(this._httpClient.post<Result<ClientDto[]>>(`${this._baseUrl}/GetAllClients`, request));
	}
	
	searchClientsByName(request: SearchClientsByNameReq): Promise<ListPage<ClientLookupDto>> {
		return unwrapResult(this._httpClient.post<Result<ListPage<ClientLookupDto>>>(`${this._baseUrl}/SearchClientsByName`, request));
	}
}
