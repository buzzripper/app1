import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { AppHealthStatus, PingResult } from './system.types';

@Injectable({ providedIn: 'root' })
export class AppSystemService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.apiBaseUrl}/api/app/system`;

    ping(): Observable<PingResult> {
        return this._httpClient.get<PingResult>(`${this._baseUrl}/ping`);
    }

    health(): Observable<AppHealthStatus> {
        return this._httpClient.get<AppHealthStatus>(`${this._baseUrl}/health`);
    }
}
