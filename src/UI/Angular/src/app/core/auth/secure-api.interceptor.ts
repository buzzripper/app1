import { HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { getCookie } from './get-cookie';
import { environment } from '../../../environments/environment';

export function secureApiInterceptor(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
) {
  const apiBaseUrl = environment.apiBaseUrl;

  // Check if the request URL is an API request
  const isApiRequest = request.url.startsWith('/api/') || 
                       request.url.startsWith('api/') ||
                       (apiBaseUrl && request.url.startsWith(apiBaseUrl));

  if (!isApiRequest) {
    return next(request);
  }

  const token = getCookie('X-XSRF-TOKEN');

  // Clone request with credentials and XSRF token
  request = request.clone({
    withCredentials: true, // Required for cross-origin cookie authentication
    headers: request.headers.set('X-XSRF-TOKEN', token),
  });

  return next(request);
}
