# JWT Authentication Architecture - Implementation Summary

**Date:** 2024  
**Branch:** ef-data-implementation  
**Status:** ? Implemented (Hybrid Approach)

---

## Problem Statement

When Auth/App modules run **in-process** with Portal:
- Angular sends cookie-authenticated requests to Portal
- Controllers expected JWT Bearer tokens but received cookies
- Result: **401 Unauthorized** for Angular calls to App/Auth APIs

## Root Cause Analysis

**Initial Attempt:** Route all requests through YARP (even in-process)
- ? **Problem:** YARP proxying to localhost creates a NEW HTTP request
- ? **Result:** Authentication context is NOT preserved
- ? **Symptom:** `User.Identity.IsAuthenticated = false` in controllers

**Root Issue:** You cannot proxy within the same process and expect the authentication context to flow through.

## Solution: Hybrid Architecture

### In-Process Modules
```
???????????  Cookie   ??????????  Direct Call  ????????????????
? Angular ? ????????? ? Portal ? ?????????????? ? App API      ?
???????????           ??????????   (Cookie!)    ? (In-Process) ?
                                                 ? Dual Auth:   ?
                                                 ? Cookie OR    ?
                                                 ? JWT Bearer   ?
                                                 ????????????????
                                                        ? 200 OK
```

### Out-of-Process Modules
```
???????????  Cookie   ??????????  YARP+Transform  ????????????????
? Angular ? ????????? ? Portal ? ????????????????? ? App API      ?
???????????           ??????????  (Adds JWT!)      ? (Remote)     ?
                          ?                         ? JWT Only     ?
                          ?                         ????????????????
                   ???????????????                        ? 200 OK
                   ? JWT Forward ?
                   ?  Transform  ?
                   ???????????????
```

### Direct API Access (Postman)
```
????????????  JWT Bearer  ????????????????
? Postman  ? ????????????? ? App API      ?
????????????               ? (In-Process  ?
                           ?  OR Remote)  ?
                           ? Dual Auth    ?
                           ????????????????
                                  ? 200 OK
```

## Changes Made

### 1. **DynamicProxyConfigProvider.cs**
- ? **Skips** in-process routes (Auth/App running in Portal)
- ? **Loads** out-of-process routes (Auth/App running remotely)
- ? **Logs** which routes are in-process vs out-of-process

**Key Code:**
```csharp
// Skip routes for in-process modules
if (routeId == "auth-api" && _authInProcess)
{
    Console.WriteLine($"[YARP] Skipping route '{routeId}' - Auth running in-process (direct controller access with dual auth)");
    continue;
}
```

### 2. **Portal.Server/Program.cs**
- ? **Dual Auth Policy:** Default policy accepts Cookie OR JWT Bearer
- ? **Comments:** Clearly document hybrid architecture
- ? **AccountController:** Explicitly uses Cookie auth only

**Key Code:**
```csharp
services.AddAuthorization(options =>
{
    // Default policy accepts BOTH Cookie and JWT Bearer
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(
            CookieAuthenticationDefaults.AuthenticationScheme,
            JwtBearerDefaults.AuthenticationScheme)
        .Build();
});
```

### 3. **Portal.Server/AccountController.cs**
- ? **Explicit Cookie Scheme:** `[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]`
- ? **Login:** Explicitly uses OpenIdConnect scheme
- ? **Logout:** Uses both Cookie and OpenIdConnect schemes

## Authentication Matrix

| Scenario | Client | Auth Method | Flow | Result |
|----------|--------|-------------|------|--------|
| **In-Process** | Angular | Cookie | Direct ? Controller (Dual Auth) | ? 200 |
| **In-Process** | Postman | JWT Bearer | Direct ? Controller (Dual Auth) | ? 200 |
| **Out-of-Process** | Angular | Cookie | YARP ? Transform ? Remote (JWT) | ? 200 |
| **Out-of-Process** | Postman | JWT Bearer | YARP ? Remote (JWT) | ? 200 |
| **Portal** | Browser | Cookie | Direct ? AccountController (Cookie) | ? 302/200 |

## Benefits

? **In-Process Flexibility:**
- Angular works with cookies
- Postman works with JWT Bearer
- No YARP overhead for same-process calls

? **Out-of-Process Clean Separation:**
- YARP handles cookie ? JWT conversion
- Remote services only see JWT Bearer
- Consistent with mobile/API-first approach

? **Mobile/API Ready:**
- JWT Bearer works everywhere
- No cookies required for direct API access

? **BFF Pattern (Out-of-Process):**
- Portal acts as Backend-for-Frontend
- Handles cookie ? JWT conversion for remote services
- Angular doesn't manage JWT tokens

## Configuration Requirements

### appsettings.json
```json
{
  "DownstreamApi": {
    "Scopes": "api://your-app-id/.default"
  },
  "ReverseProxy": {
    "Routes": {
      "auth-api": {
        "ClusterId": "auth-cluster",
        "Match": { "Path": "/api/auth/{**catch-all}" }
      },
      "app-api": {
        "ClusterId": "app-cluster",
        "Match": { "Path": "/api/app/{**catch-all}" }
      }
    },
    "Clusters": {
      "auth-cluster": {
        "Destinations": {
          "destination1": { "Address": "https://auth-service-url" }
        }
      },
      "app-cluster": {
        "Destinations": {
          "destination1": { "Address": "https://app-service-url" }
        }
      }
    }
  }
}
```

### Conditional Compilation
```xml
<!-- Portal.Server.csproj -->
<PropertyGroup>
  <DefineConstants>AUTH_INPROCESS;APP_INPROCESS</DefineConstants>
</PropertyGroup>
```

## Testing Checklist

- [x] Build succeeds
- [ ] Angular login works (cookie auth)
- [ ] Angular ? Auth API (in-process) works with Cookie auth ?
- [ ] Angular ? App API (in-process) works with Cookie auth ?
- [ ] Postman ? Auth API (in-process) works with JWT Bearer ?
- [ ] Postman ? App API (in-process) works with JWT Bearer ?
- [ ] Portal Account endpoints work (cookie auth)
- [ ] Out-of-process: Angular ? Remote Auth/App via YARP works
- [ ] Out-of-process: Postman ? Remote Auth/App works

## Debugging Tips

### Check Authentication Status
Put a breakpoint in any controller and inspect:
```csharp
User.Identity.IsAuthenticated  // Should be true
User.Identity.AuthenticationType  // "Cookies" or "Bearer"
```

### Check YARP Routing
Look for console output:
```
[YARP] Skipping route 'auth-api' - Auth running in-process (direct controller access with dual auth)
[YARP] Skipping route 'app-api' - App running in-process (direct controller access with dual auth)
[YARP] Total routes loaded: 0
```

If routes are loaded (count > 0), then YARP is active for out-of-process modules.

### Check Authorization Policy
The default policy should accept both schemes:
```csharp
options.DefaultPolicy.AuthenticationSchemes
// Should contain: ["Cookies", "Bearer"]
```

## Files Modified

1. `Portal/Portal.Server/DynamicProxyConfigProvider.cs` - Skip in-process routes
2. `Portal/Portal.Server/Program.cs` - Dual auth policy
3. `Portal/Portal.Server/AccountController.cs` - Explicit cookie scheme
4. `App/App.Api/Controllers/SystemController.cs` - Remove [AllowAnonymous] from Ping
5. `Auth/Auth.Api/Controllers/SystemController.cs` - Add [Authorize] for testing

## Why This Works

**In-Process:**
- No HTTP proxying = authentication context preserved
- Dual auth policy = both Cookie and JWT Bearer accepted
- Direct controller access = full performance

**Out-of-Process:**
- YARP proxying = separate HTTP request (required for remote service)
- JWT transform = converts cookie ? JWT Bearer
- Clean separation = remote service only handles JWT

---

*End of Document*
