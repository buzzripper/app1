import { HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { getCookie } from './get-cookie';

export function secureApiInterceptor(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
) {
  // Check if the request URL starts with /api/ (relative) or contains /api/ (absolute)
  const isApiRequest = request.url.startsWith('/api/') || 
                       request.url.startsWith('api/') ||
                       request.url.includes('://') && request.url.includes('/api/');

  if (!isApiRequest) {
    return next(request);
  }

  const token = getCookie('X-XSRF-TOKEN');

  request = request.clone({
    headers: request.headers.set(
      'X-XSRF-TOKEN',
      token
    ),
  });

  return next(request);
}

function getApiUrl() {
  const backendHost = getCurrentHost();

  return `${backendHost}/api/`;
}

function getCurrentHost() {
  const host = globalThis.location.host;
  const url = `${globalThis.location.protocol}//${host}`;
  return url;
}
