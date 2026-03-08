import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from 'environments/environment';

@Injectable({ providedIn: 'root' })
export class TenantService {
    private _httpClient = inject(HttpClient);
    private readonly _baseUrl = `${environment.authApiBaseUrl}/api/auth/v1/tenant`;
}
