import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { ClientDto, ClientOptionDto, ClientRouteDto } from './dto';
import { CreateClientReq, GetAllClientOptionsReq } from './req';

@Injectable({ providedIn: 'root' })
export class ClientService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.apiBaseUrl}/api/App/v1/Client`;

    getById(id: string): Observable<ClientDto> {
        return this._httpClient.get<ClientDto>(`${this._baseUrl}/GetById/${id}`);
    }

    getByKey(key: string): Observable<ClientDto> {
        return this._httpClient.get<ClientDto>(`${this._baseUrl}/GetByKey/${encodeURIComponent(key)}`);
    }

    getAllClientOptions(request: GetAllClientOptionsReq): Observable<ClientOptionDto[]> {
        return this._httpClient.post<ClientOptionDto[]>(`${this._baseUrl}/GetAllClientOptions`, request);
    }

    getAllRoutes(): Observable<ClientRouteDto[]> {
        return this._httpClient.get<ClientRouteDto[]>(`${this._baseUrl}/GetAllRoutes`);
    }

    create(request: CreateClientReq): Observable<string> {
        return this._httpClient.patch<string>(`${this._baseUrl}/Create`, request);
    }

    update(id: string, client: { key: string; name: string; baseUrl: string }): Observable<void> {
        return this._httpClient.put<void>(`${this._baseUrl}/Update/${encodeURIComponent(id)}`, client);
    }

    delete(id: string): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/DeleteClient`, { body: { id } });
    }
}
