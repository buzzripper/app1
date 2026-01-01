import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { AuthHealthStatus, PingResult } from './system.types';

@Injectable({ providedIn: 'root' })
export class AuthSystemService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.apiBaseUrl}/api/auth/v1/system`;

    ping(): Observable<PingResult> {
        return this._httpClient.get<PingResult>(`${this._baseUrl}/ping`);
    }

    health(): Observable<AuthHealthStatus> {
        return this._httpClient.get<AuthHealthStatus>(`${this._baseUrl}/health`);
    }
}
