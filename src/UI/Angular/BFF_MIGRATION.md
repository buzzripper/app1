# BFF Authentication Integration - AngularUI Migration

## What Was Changed

### 1. **Authentication Architecture**
- **Removed**: JWT token-based authentication with localStorage
- **Added**: Cookie-based BFF authentication with XSRF tokens

### 2. **Files Created**
- `ui/src/app/core/auth/secure-api.interceptor.ts` - Adds XSRF token to API requests
- `ui/src/app/core/auth/get-cookie.ts` - Utility to read cookies
- `ui/certs/` - SSL certificates copied from Angular project

### 3. **Files Modified**
- `ui/src/app/core/auth/auth.service.ts` - Now uses `/api/User` endpoint and BFF login/logout
- `ui/src/app/core/auth/auth.provider.ts` - Uses BFF interceptor instead of JWT interceptor
- `ui/src/app/core/auth/guards/auth.guard.ts` - Checks auth via `/api/User` endpoint
- `ui/src/app/core/auth/guards/noAuth.guard.ts` - Updated for BFF auth
- `ui/src/app/app.config.ts` - Added CSP_NONCE provider
- `ui/src/index.html` - Added CSP nonce placeholder
- `ui/angular.json` - Updated build output to `../server/wwwroot` and SSL config

### 4. **Files Deleted**
- `ui/src/app/core/auth/auth.interceptor.ts` - Old JWT Bearer token interceptor
- `ui/src/app/core/auth/auth.utils.ts` - JWT token decode utilities

## How It Works Now

### Authentication Flow:
1. **Login**: User clicks login → redirected to `/api/Account/Login` → Microsoft Entra ID → back to app
2. **Auth Check**: App calls `/api/User` to get authentication status and user claims
3. **API Calls**: All `/api/*` requests automatically include `X-XSRF-TOKEN` header
4. **Logout**: App calls `POST /api/Account/Logout` with XSRF token → redirected to home

### Key Differences from Old Approach:
| Old (JWT) | New (BFF) |
|-----------|-----------|
| JWT token in localStorage | HTTP-only authentication cookie |
| Bearer token in Authorization header | XSRF token in X-XSRF-TOKEN header |
| Client-side token validation | Server-side session validation |
| `signIn(credentials)` | `window.location.href = '/api/Account/Login'` |
| Tokens expire on client | Session managed by server |

## Configuration

### Build Configuration (`ui/angular.json`):
```json
{
  "outputPath": {
    "base": "../server/wwwroot",
    "browser": ""
  },
  "serve": {
    "options": {
      "sslKey": "certs/dev_localhost.key",
      "sslCert": "certs/dev_localhost.pem",
      "port": 4201
    }
  }
}
```

### Development Server:
```bash
cd ui
ng serve --ssl
```

### Production Build:
```bash
cd ui
npm run build
# Output goes to: ../server/wwwroot
```

## Backend Endpoints Used

- `GET /api/User` - Get current user authentication status and claims
- `GET /api/Account/Login` - Initiate login flow (redirects to Entra ID)
- `POST /api/Account/Logout` - Logout (requires XSRF token)

## Security Features

1. **XSRF Protection**: All POST/PUT/DELETE requests include XSRF token
2. **CSP Nonce**: Content Security Policy nonce for inline scripts
3. **HTTP-Only Cookies**: Authentication cookie not accessible via JavaScript
4. **Secure Cookies**: All cookies marked as Secure and SameSite=Strict

## Testing Checklist

- [ ] Build Angular app: `cd ui && npm run build`
- [ ] Run server: `cd server && dotnet run`
- [ ] Access app at `https://localhost:5001`
- [ ] Click login - should redirect to Microsoft Entra ID
- [ ] After login - should show authenticated user info
- [ ] API calls should work with auth
- [ ] Logout should clear session
- [ ] No JWT tokens in localStorage/sessionStorage

## Mock API

The Fuse Mock API is still enabled and will simulate backend responses for development. To use real BFF endpoints, ensure the backend server is running.

## Next Steps

1. Remove or update Fuse auth UI pages (sign-in, sign-up forms) to redirect to BFF
2. Test all protected routes
3. Update any components that reference old JWT auth patterns
4. Consider removing Mock Auth API if not needed
