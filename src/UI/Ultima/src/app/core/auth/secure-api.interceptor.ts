import { HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { environment } from '@/environments/environment';
import { getCookie } from './get-cookie';

export function secureApiInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn) {
    const apiBaseUrl = environment.apiBaseUrl;
    const isApiRequest =
        request.url.startsWith('/api/') ||
        request.url.startsWith('api/') ||
        (apiBaseUrl && request.url.startsWith(apiBaseUrl));

    if (!isApiRequest) {
        return next(request);
    }

    const token = getCookie('X-XSRF-TOKEN');

    return next(
        request.clone({
            withCredentials: true,
            headers: request.headers.set('X-XSRF-TOKEN', token)
        })
    );
}
