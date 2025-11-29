# Service Clients Configuration Guide

## Overview

The Portal.Server project now includes a `ServiceClients` configuration section that controls how the BFF (Backend for Frontend) communicates with the App1 and Auth microservices.

## Configuration Structure

```json
"ServiceClients": {
  "App1": {
    "InProcess": true,
    "Url": "https://localhost:7001"
  },
  "Auth": {
    "InProcess": true,
    "Url": "https://localhost:7002"
  }
}
```

## Configuration Properties

### InProcess (boolean)
Determines how the service is hosted and accessed:

- **`true` (In-Process)**: 
  - The service API is hosted **within** the Portal.Server process
  - No HTTP calls are made - direct method invocation
  - Better performance (no network overhead)
  - Simplified deployment (single process)
  - Uses dependency injection to resolve services directly
  
- **`false` (HTTP/Remote)**:
  - The service runs as a **separate process**
  - HTTP calls are made using HttpClient
  - Services can scale independently
  - Can be deployed on different machines/containers
  - Uses the `Url` property to connect

### Url (string)
The base URL for HTTP communication when `InProcess` is `false`.

**Format**: `https://localhost:{port}` or `https://your-service-domain.com`

**Examples**:
- Development: `https://localhost:7001`
- Production: `https://app1-service.yourdomain.com`

## Deployment Scenarios

### Scenario 1: Monolithic Deployment (In-Process)
**Best for**: Small to medium applications, simplified hosting, single server deployment

```json
"ServiceClients": {
  "App1": {
    "InProcess": true,
    "Url": "https://localhost:7001"  // Not used but required for future flexibility
  },
  "Auth": {
    "InProcess": true,
    "Url": "https://localhost:7002"  // Not used but required for future flexibility
  }
}
```

**How it works**:
```
???????????????????????????????????????
?      Portal.Server Process          ?
?  ????????????  ??????????????????  ?
?  ? Angular  ?  ? ASP.NET Core   ?  ?
?  ?   BFF    ?  ?   Endpoints    ?  ?
?  ????????????  ??????????????????  ?
?                      ?              ?
?       ?????????????????????????    ?
?       ?              ?        ?    ?
?  ???????????   ???????????  ?    ?
?  ? App1.Api?   ?Auth.Api ?  ?    ?
?  ?Services ?   ?Services ?  ?    ?
?  ???????????   ???????????  ?    ?
???????????????????????????????????????
```

### Scenario 2: Microservices Deployment (HTTP/Remote)
**Best for**: Large applications, independent scaling, distributed systems

```json
"ServiceClients": {
  "App1": {
    "InProcess": false,
    "Url": "https://app1-service:7001"
  },
  "Auth": {
    "InProcess": false,
    "Url": "https://auth-service:7002"
  }
}
```

**How it works**:
```
????????????????????       ????????????????       ????????????????
? Portal.Server    ?       ? App1.Server  ?       ? Auth.Server  ?
?                  ?       ?              ?       ?              ?
? ????????????    ?       ? ???????????? ?       ? ???????????? ?
? ? Angular  ?    ?       ? ? App1.Api ? ?       ? ? Auth.Api ? ?
? ?   BFF    ?    ?       ? ? Services ? ?       ? ? Services ? ?
? ????????????    ?       ? ???????????? ?       ? ???????????? ?
?                  ?       ?              ?       ?              ?
? HTTP Client ?????????????? Endpoints    ?       ? Endpoints    ?
? Proxy            ?       ????????????????       ????????????????
????????????????????
```

### Scenario 3: Hybrid Deployment
**Best for**: Gradual migration, performance optimization

```json
"ServiceClients": {
  "App1": {
    "InProcess": true,   // Critical/frequently-used service
    "Url": "https://localhost:7001"
  },
  "Auth": {
    "InProcess": false,  // Centralized auth service
    "Url": "https://auth-service.internal:7002"
  }
}
```

## Environment-Specific Configuration

### Development (appsettings.Development.json)
```json
"ServiceClients": {
  "App1": {
    "InProcess": true,
    "Url": "https://localhost:7001"
  },
  "Auth": {
    "InProcess": true,
    "Url": "https://localhost:7002"
  }
}
```

### Production (appsettings.json or Azure App Settings)
```json
"ServiceClients": {
  "App1": {
    "InProcess": false,
    "Url": "https://app1-api.yourdomain.com"
  },
  "Auth": {
    "InProcess": false,
    "Url": "https://auth-api.yourdomain.com"
  }
}
```

## Azure App Service Configuration

Set these as **Application Settings** in Azure Portal:

```
ServiceClients__App1__InProcess = false
ServiceClients__App1__Url = https://app1-service.azurewebsites.net

ServiceClients__Auth__InProcess = false
ServiceClients__Auth__Url = https://auth-service.azurewebsites.net
```

## Aspire Orchestration

When using .NET Aspire, the URLs can be automatically configured:

```csharp
// In AppHost Program.cs
var app1 = builder.AddProject<Projects.App1_Server>("app1-server");
var auth = builder.AddProject<Projects.Auth_Server>("auth-server");

var portal = builder.AddProject<Projects.Portal_Server>("portal-server")
    .WithEnvironment("ServiceClients__App1__InProcess", "false")
    .WithEnvironment("ServiceClients__App1__Url", app1.GetEndpoint("https"))
    .WithEnvironment("ServiceClients__Auth__InProcess", "false")
    .WithEnvironment("ServiceClients__Auth__Url", auth.GetEndpoint("https"));
```

## Code Implementation

### Program.cs Logic

```csharp
// Read configuration
bool hostApp1InProcess = configuration.GetValue<bool>("ServiceClients:App1:InProcess", true);
bool hostAuthInProcess = configuration.GetValue<bool>("ServiceClients:Auth:InProcess", true);

// Register API services if in-process
if (hostApp1InProcess)
{
    services.AddApp1ApiServices();
}

if (hostAuthInProcess)
{
    services.AddAuthApiServices();
}

// Register service clients (proxies)
services.AddApp1Client(configuration);
services.AddAuthClient(configuration);

// Map endpoints if in-process
if (hostApp1InProcess)
{
    app.MapApp1Endpoints();
}

if (hostAuthInProcess)
{
    app.MapAuthEndpoints();
}
```

### How Client Extensions Work

**When InProcess = true**:
```csharp
// App1ClientExtensions.cs
if (!inProcess)
{
    // Skip HTTP client registration
    // The ISystemService implementation is provided by AddApp1ApiServices()
}
```

**When InProcess = false**:
```csharp
// App1ClientExtensions.cs
services.AddHttpClient<ISystemService, SystemServiceHttpClient>(client =>
{
    client.BaseAddress = new Uri(baseUrl); // Uses Url from config
});
```

## Performance Considerations

| Aspect | In-Process | HTTP/Remote |
|--------|-----------|-------------|
| Latency | ~1ms | ~10-100ms |
| Network I/O | None | Yes |
| Serialization | None | JSON |
| Scalability | Single instance | Independent scaling |
| Deployment | Simple | Complex |
| Debugging | Easy | Requires distributed tracing |

## Migration Path

1. **Start with In-Process** (Development)
   - Simplest setup
   - Fast iteration
   - Easy debugging

2. **Test HTTP Mode** (Staging)
   - Switch `InProcess` to `false`
   - Verify URLs are correct
   - Test with separate service instances

3. **Deploy Microservices** (Production)
   - Deploy each service independently
   - Use load balancers
   - Configure service discovery (if needed)

## Troubleshooting

### Issue: "Service not registered" error in In-Process mode
**Solution**: Ensure `AddApp1ApiServices()` / `AddAuthApiServices()` is called before `AddApp1Client()` / `AddAuthClient()`.

### Issue: HTTP connection errors in Remote mode
**Solution**: 
- Verify the `Url` is correct
- Check service is running
- Verify firewall/network policies
- Check SSL certificates

### Issue: Services not accessible after deployment
**Solution**: Check Azure App Settings or environment variables are set correctly with the `ServiceClients__` prefix.

## Best Practices

1. ? **Use In-Process for development** - faster iteration, easier debugging
2. ? **Use HTTP/Remote for production microservices** - better scalability
3. ? **Always provide both InProcess and Url** - enables easy switching
4. ? **Use environment variables for production URLs** - avoid hardcoding
5. ? **Test both modes** - ensure your application works in both configurations
6. ? **Monitor performance** - measure actual impact before choosing architecture

## Related Files

- `Portal/Portal.Server/appsettings.json` - Base configuration
- `Portal/Portal.Server/appsettings.Development.json` - Development overrides
- `Portal/Portal.Server/Program.cs` - Service registration logic
- `App1/App1.Shared/Extensions/App1ClientExtensions.cs` - Client proxy setup
- `Auth/Auth.Shared/Extensions/AuthClientExtensions.cs` - Client proxy setup
