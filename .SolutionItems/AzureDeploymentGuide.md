# Azure Deployment Configuration Guide

## Angular to Azure Static Web Apps with Separate BFF Backend

**Generated:** December 2024  
**Repository:** app1  
**Branch:** angular-breakout

---

## Architecture Overview

```
???????????????????????????     ???????????????????????????
? Azure Static Web Apps   ??????? Azure App Service (BFF) ?
? (Angular SPA)           ?     ? (Portal.Server)         ?
? https://your-swa.net    ?     ? https://your-api.net    ?
???????????????????????????     ???????????????????????????
        Different Origins = CORS Required
```

---

## 1. Angular Environment Configuration

### Production: `UI/Angular/src/environments/environment.ts`

```typescript
export const environment = {
    production: true,
    apiBaseUrl: 'https://your-bff-app.azurewebsites.net' // CHANGE: Full BFF URL
};
```

### Development: `UI/Angular/src/environments/environment.development.ts`

```typescript
export const environment = {
    production: false,
    apiBaseUrl: 'https://localhost:5001' // BFF server URL in development
};
```

---

## 2. Angular Build Configuration

### `UI/Angular/angular.json` - Update output path

```json
"outputPath": {
    "base": "dist/fuse",
    "browser": ""
}
```

> **CHANGE:** Local dist folder instead of `../server/wwwroot`

### `UI/Angular/vite.config.ts` - Update build output

```typescript
import { defineConfig } from 'vite';

export default defineConfig({
  server: {
    port: 4201,
    host: 'localhost',
    strictPort: true,
    hmr: {
      host: 'localhost',
      port: 4201,
      protocol: 'wss',
      clientPort: 4201
    }
  },
  optimizeDeps: {
    force: true
  },
  build: {
    outDir: 'dist/fuse',  // CHANGE: Local output for SWA deployment
    emptyOutDir: true
  }
});
```

---

## 3. Azure Static Web Apps Configuration

### Create `UI/Angular/staticwebapp.config.json`

```json
{
    "navigationFallback": {
        "rewrite": "/index.html",
        "exclude": ["/images/*.{png,jpg,gif,svg}", "/css/*", "/assets/*"]
    },
    "globalHeaders": {
        "Cache-Control": "no-cache"
    },
    "mimeTypes": {
        ".json": "application/json",
        ".woff2": "font/woff2"
    }
}
```

---

## 4. BFF (Portal.Server) CORS Configuration

### Option A: Update `Portal/Portal.Server/appsettings.json`

```json
{
    "Cors": {
        "AllowedOrigins": [
            "https://your-static-web-app.azurestaticapps.net"
        ]
    }
}
```

### Option B: Create `Portal/Portal.Server/appsettings.Production.json`

```json
{
    "Cors": {
        "AllowedOrigins": [
            "https://your-static-web-app.azurestaticapps.net",
            "https://your-custom-domain.com"
        ]
    },
    "MicrosoftEntraID": {
        "CallbackPath": "/signin-oidc",
        "SignedOutCallbackPath": "/signout-callback-oidc"
    }
}
```

---

## 5. Authentication Cookie Configuration

Since cookies won't work cross-origin by default, update `Portal/Portal.Server/Program.cs`:

### Cookie Authentication Settings

```csharp
// Configure cookie settings for cross-origin Angular app
services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.SameSite = SameSiteMode.None;  // CHANGE: None for cross-origin
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
```

### Antiforgery Cookie Settings

```csharp
services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "__Host-X-XSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.None;  // CHANGE: None for cross-origin
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
```

---

## 6. Angular HTTP Interceptor Update

### `UI/Angular/src/app/core/auth/secure-api.interceptor.ts`

```typescript
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

  // CHANGE: Prepend apiBaseUrl for cross-origin requests
  let url = request.url;
  if (!request.url.startsWith('http') && apiBaseUrl) {
    url = `${apiBaseUrl}${request.url.startsWith('/') ? '' : '/'}${request.url}`;
  }

  // Clone request with credentials and XSRF token
  request = request.clone({
    url: url,
    withCredentials: true, // Required for cross-origin cookie authentication
    headers: request.headers.set('X-XSRF-TOKEN', token),
  });

  return next(request);
}
```

---

## 7. Azure App Registration Updates

Add redirect URIs for both origins:

| Redirect URI |
|--------------|
| `https://your-bff-app.azurewebsites.net/signin-oidc` |
| `https://your-bff-app.azurewebsites.net/signout-callback-oidc` |

---

## 8. Summary Checklist

| Category | Item | Action |
|----------|------|--------|
| **Angular** | `environment.ts` | ?? Add full BFF URL |
| **Angular** | `angular.json` output | ?? Change to local `dist` folder |
| **Angular** | `vite.config.ts` | ?? Update `outDir` |
| **Angular** | `staticwebapp.config.json` | ?? Create for SPA routing |
| **Angular** | HTTP interceptor | ?? Prepend `apiBaseUrl` to requests |
| **BFF** | CORS origins | ?? Add SWA URL |
| **BFF** | Cookie SameSite | ?? Change to `None` |
| **BFF** | Antiforgery SameSite | ?? Change to `None` |
| **Azure** | App Registration | ?? Verify redirect URIs |
| **SWA CLI** | Deploy | ?? Run `npx swa init` then `npx swa deploy` |

---

## 9. Deployment Commands

```bash
# In UI/Angular directory
npm install -g @azure/static-web-apps-cli
npx swa init --yes
npm run build
npx swa deploy --env production
```

---

## 10. Azure App Service Configuration Settings

| Setting | Value |
|---------|-------|
| `MicrosoftEntraID__Instance` | `https://login.microsoftonline.com/` or CIAM URL |
| `MicrosoftEntraID__Domain` | Your domain |
| `MicrosoftEntraID__TenantId` | Your tenant ID |
| `MicrosoftEntraID__ClientId` | Your client ID |
| `MicrosoftEntraID__ClientSecret` | *Use Key Vault reference* |
| `MicrosoftEntraID__CallbackPath` | `/signin-oidc` |
| `MicrosoftEntraID__SignedOutCallbackPath` | `/signout-callback-oidc` |

---

## 11. Important Security Notes

### Cross-Origin Cookie Requirements

When deploying Angular and BFF on different origins:

1. **SameSite=None** is required for cookies to be sent cross-origin
2. **Secure=true** is mandatory when using SameSite=None
3. **withCredentials: true** must be set on all HTTP requests from Angular
4. **CORS must explicitly allow credentials** in the BFF configuration

### CORS Configuration in BFF

The existing CORS configuration in `Program.cs` already includes `.AllowCredentials()`:

```csharp
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();  // This is already present
    });
});
```

---

## 12. Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| CORS errors in browser | Verify `AllowedOrigins` includes exact SWA URL |
| Cookies not sent | Check `SameSite=None` and `withCredentials: true` |
| Authentication redirect fails | Verify App Registration redirect URIs |
| 404 on SPA routes | Ensure `staticwebapp.config.json` has `navigationFallback` |
| API calls fail | Check `apiBaseUrl` is correct in `environment.ts` |

### Browser DevTools Checks

1. **Network tab:** Verify `Access-Control-Allow-Origin` header in responses
2. **Application tab:** Check if cookies are being set with correct attributes
3. **Console:** Look for CORS or cookie-related warnings

---

*End of Document*
