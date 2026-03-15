import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { ListPage } from '../common/dtos';
import { ClientDto, ClientLookupDto, ClientRouteDto, Dto4 } from './dto';
import { GetAllClientLookupItemsReq, GetAllClientsReq, SearchClientsByNameReq, CreateClientReq, UpdateClientReq, UpdateClientBaseUrlReq } from './req';

@Injectable({ providedIn: 'root' })
export class ClientService {
	private _httpClient = inject(HttpClient);
	private readonly _baseUrl = `${environment.apiBaseUrl}/api/app/v1/client`;

	// Create/Update
	
	 createClient(request: CreateClientReq): Observable<Uint8Array> {
		return this._httpClient.patch<Uint8Array>(`${this._baseUrl}/CreateClient`, request);
	}
	
	 updateClient(request: UpdateClientReq): Observable<Uint8Array> {
		return this._httpClient.patch<Uint8Array>(`${this._baseUrl}/UpdateClient`, request);
	}
	
	 updateClientBaseUrl(request: UpdateClientBaseUrlReq): Observable<Uint8Array> {
		return this._httpClient.patch<Uint8Array>(`${this._baseUrl}/UpdateClientBaseUrl`, request);
	}

	// Delete
	
	delete(id: string): Observable<Uint8Array> {
		return this._httpClient.delete<Uint8Array>(`${this._baseUrl}/DeleteClient`, { body: { id } });
	}

	// Read Single
	
	getClientById(id: string): Observable<ClientDto> {
		return this._httpClient.get<ClientDto>(`${this._baseUrl}/GetClientById/${id}`);
	}
	
	getClientByKey(key: string): Observable<ClientDto> {
		return this._httpClient.get<ClientDto>(`${this._baseUrl}/GetClientByKey/${key}`);
	}

	// Read List
	
	getAllClientLookupItems(request: GetAllClientLookupItemsReq): Observable<ClientLookupDto[]> {
		return this._httpClient.post<ClientLookupDto[]>(`${this._baseUrl}/GetAllClientLookupItems`, request);
	}
	
	getAllClientRoutes(): Observable<ClientRouteDto[]> {
		return this._httpClient.get<ClientRouteDto[]>(`${this._baseUrl}/GetAllClientRoutes`);
	}
	
	getAllClients(request: GetAllClientsReq): Observable<ClientDto[]> {
		return this._httpClient.post<ClientDto[]>(`${this._baseUrl}/GetAllClients`, request);
	}
	
	searchClientsByName(request: SearchClientsByNameReq): Observable<ListPage<ClientLookupDto>> {
		return this._httpClient.post<ListPage<ClientLookupDto>>(`${this._baseUrl}/SearchClientsByName`, request);
	}
}
