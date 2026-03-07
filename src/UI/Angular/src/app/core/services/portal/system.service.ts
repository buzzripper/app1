import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { HealthStatus, PingResult } from '../common/dtos';

@Injectable({ providedIn: 'root' })
export class PortalSystemService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.apiBaseUrl}/api/portal/system`;

    ping(): Observable<PingResult> {
        return this._httpClient.get<PingResult>(`${this._baseUrl}/ping`);
    }

    health(): Observable<HealthStatus> {
        return this._httpClient.get<HealthStatus>(`${this._baseUrl}/health`);
    }
}
