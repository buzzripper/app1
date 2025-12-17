# JWT Authentication Troubleshooting Guide

## Quick Diagnostics

### 1. Check if User is Authenticated

Put a breakpoint in your controller and inspect:
```csharp
// Should be TRUE if authentication succeeded
User.Identity.IsAuthenticated

// Should be "Cookies" (Angular) or "Bearer" (Postman)
User.Identity.AuthenticationType

// Should contain claims
User.Claims.Any()
```

**If `IsAuthenticated = false`:**
- ? Cookie not being sent (check browser DevTools ? Application ? Cookies)
- ? JWT token not in Authorization header (check Postman Headers tab)
- ? Wrong authorization policy (check Program.cs)

---

### 2. Check YARP Configuration

Look at console output when Portal starts:

**In-Process (Expected):**
```
[YARP] Skipping route 'auth-api' - Auth running in-process (direct controller access with dual auth)
[YARP] Skipping route 'app-api' - App running in-process (direct controller access with dual auth)
[YARP] Total routes loaded: 0
```

**Out-of-Process (Expected):**
```
[YARP] Loading route 'auth-api' for out-of-process module
[YARP] Loading route 'app-api' for out-of-process module
[YARP] Total routes loaded: 2
[YARP] Cluster 'auth-cluster' configured for out-of-process module
[YARP] Cluster 'app-cluster' configured for out-of-process module
```

---

### 3. Check Authorization Policy

In `Portal.Server/Program.cs`, verify:
```csharp
services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(
            CookieAuthenticationDefaults.AuthenticationScheme,  // ? Must include this
            JwtBearerDefaults.AuthenticationScheme)             // ? And this
        .Build();
});
```

**If policy only has JWT Bearer:**
- ? Angular requests will fail (401) even though cookie is valid
- ? Postman requests will work

---

## Common Issues

### Issue 1: Angular Gets 401 on API Calls

**Symptom:**
- Login works
- `/api/Account/Login` succeeds
- `/api/auth/System/Ping` or `/api/app/System/Ping` returns 401

**Diagnosis:**
```csharp
// In controller, check:
User.Identity.IsAuthenticated  // FALSE
```

**Possible Causes:**

1. **Authorization policy is JWT-only** ?
   ```csharp
   // WRONG (JWT only):
   .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
   
   // CORRECT (Dual auth):
   .AddAuthenticationSchemes(
       CookieAuthenticationDefaults.AuthenticationScheme,
       JwtBearerDefaults.AuthenticationScheme)
   ```

2. **YARP is active for in-process modules** ?
   - Check console output
   - Should say "Skipping route" for in-process modules
   - If not, check `DynamicProxyConfigProvider.cs`

3. **Cookie not being sent**
   - Check browser DevTools ? Network ? Headers
   - Should see `Cookie:` header
   - If missing, check CORS configuration

---

### Issue 2: Postman Gets 401 on API Calls

**Symptom:**
- Postman request with `Authorization: Bearer {token}` returns 401

**Diagnosis:**
1. **Check token format:**
   ```
   Authorization: Bearer eyJ0eXAiOiJKV1QiLCJhbGc...
   ```
   - Must start with `Bearer ` (note the space)
   - Token should be a long string (JWT)

2. **Verify token is valid:**
   - Go to https://jwt.ms
   - Paste your token
   - Check expiration (`exp` claim)
   - Check audience (`aud` claim) matches your client ID

3. **Check authorization policy includes JWT Bearer:**
   ```csharp
   .AddAuthenticationSchemes(
       CookieAuthenticationDefaults.AuthenticationScheme,
       JwtBearerDefaults.AuthenticationScheme)  // ? Must be here
   ```

---

### Issue 3: Portal AccountController Not Working

**Symptom:**
- `/api/Account/Login` redirects to error page
- Login loop (keeps redirecting to Entra ID)

**Diagnosis:**
Check that AccountController explicitly uses Cookie scheme:
```csharp
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]  // ? Must be explicit
public class AccountController : ControllerBase
{
    [HttpGet("Login")]
    [AllowAnonymous]  // ? Must allow anonymous for initial redirect
    public ActionResult Login(string? returnUrl, string? claimsChallenge)
    {
        return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
    }
}
```

---

### Issue 4: Out-of-Process YARP Not Working

**Symptom:**
- When Auth/App run as separate services, Angular gets 401

**Diagnosis:**

1. **Check YARP routes are loaded:**
   ```
   [YARP] Total routes loaded: 2  // Should be > 0 for out-of-process
   ```

2. **Check JWT transform has scopes:**
   ```json
   {
     "DownstreamApi": {
       "Scopes": "api://your-app-id/.default"  // ? Must be configured
     }
   }
   ```

3. **Check token acquisition works:**
   - Put breakpoint in `JwtForwardingTransform.cs`
   - Verify `ITokenAcquisition.GetAccessTokenForUserAsync()` succeeds
   - Verify token is added to `Authorization` header

---

## Testing Scenarios

### Test 1: Angular ? In-Process Auth API
```
1. Login via Angular
2. Navigate to a page that calls /api/auth/System/Ping
3. Expected: 200 OK with User.Identity.IsAuthenticated = true
```

### Test 2: Angular ? In-Process App API
```
1. Login via Angular
2. Navigate to a page that calls /api/app/System/Ping
3. Expected: 200 OK with User.Identity.IsAuthenticated = true
```

### Test 3: Postman ? In-Process API
```
1. Get JWT token from Entra ID
2. Postman GET /api/auth/System/Ping
   Headers: Authorization: Bearer {token}
3. Expected: 200 OK
```

### Test 4: Postman ? In-Process API (No Token)
```
1. Postman GET /api/auth/System/Ping
   Headers: (no Authorization header)
2. Expected: 401 Unauthorized
```

### Test 5: Portal Login/Logout
```
1. Navigate to https://localhost:5001/api/Account/Login
2. Expected: Redirect to Entra ID
3. After login: Redirect back to app
4. Navigate to https://localhost:5001/api/Account/Logout
5. Expected: Sign out and redirect back
```

---

## Advanced Debugging

### Enable PII in Development

In `Program.cs`:
```csharp
if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;  // ? Shows full token details
    app.UseDeveloperExceptionPage();
}
```

### Add Logging for Authentication

```csharp
builder.Logging.AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.Identity.Web", LogLevel.Debug);
```

### Inspect Claims

In controller:
```csharp
var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
// Inspect in debugger
```

---

## Contact & Support

If issues persist:
1. Check `.SolutionItems/JWT-Architecture-Changes.md` for architecture details
2. Review `Portal/Portal.Server/JwtForwardingTransform.cs` for YARP transform logic
3. Check `Portal/Portal.Server/DynamicProxyConfigProvider.cs` for routing logic

---

*Last Updated: 2024*
