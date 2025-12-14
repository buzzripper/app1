import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { PortalHealthStatus, PingResult } from './system.types';

@Injectable({ providedIn: 'root' })
export class PortalSystemService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.apiBaseUrl}/api/portal/system`;

    ping(): Observable<PingResult> {
        return this._httpClient.get<PingResult>(`${this._baseUrl}/ping`);
    }

    health(): Observable<PortalHealthStatus> {
        return this._httpClient.get<PortalHealthStatus>(`${this._baseUrl}/health`);
    }
}
