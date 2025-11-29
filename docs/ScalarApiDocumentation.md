# Scalar API Documentation Setup

Scalar has been successfully added to both **App1.Api** and **Auth.Api** projects for interactive API testing and documentation.

## ?? What Was Added

### Packages
- **Scalar.AspNetCore** (v1.2.52) - Interactive OpenAPI documentation UI
- **Microsoft.AspNetCore.OpenApi** (v10.0.0) - OpenAPI document generation

### Projects Updated
1. **App1.Api**
2. **Auth.Api**
3. **App1.Server**
4. **Auth.Server**

## ?? Accessing Scalar UI

When running in **Development** mode, Scalar UI is automatically available:

### App1 API
- **Scalar UI**: `https://localhost:{port}/scalar/v1`
- **OpenAPI Spec**: `https://localhost:{port}/openapi/v1.json`

### Auth API
- **Scalar UI**: `https://localhost:{port}/scalar/v1`
- **OpenAPI Spec**: `https://localhost:{port}/openapi/v1.json`

> **Note**: Replace `{port}` with the actual port your service is running on.

## ?? Features Configured

### 1. OpenAPI Generation
Both API projects automatically generate OpenAPI specifications from your endpoints.

### 2. Scalar UI Configuration
- **Theme**: Purple
- **Default HTTP Client**: C# HttpClient
- **Interactive Testing**: Test APIs directly from the browser
- **Code Generation**: View request examples in multiple languages

### 3. Development-Only Exposure
Scalar UI is only enabled when running in Development environment for security.

## ?? Usage in Code

### Service Registration (Automatic)
```csharp
// Already configured in AddApp1ApiServices() and AddAuthApiServices()
services.AddOpenApi();
```

### Endpoint Mapping (Automatic in Development)
```csharp
// App1.Server
if (app.Environment.IsDevelopment())
{
    app.MapApp1ApiDocumentation();
}

// Auth.Server
if (app.Environment.IsDevelopment())
{
    app.MapAuthApiDocumentation();
}
```

## ?? API Endpoint Documentation

Your endpoints already use `.WithOpenApi()` which makes them appear in Scalar:

```csharp
app.MapGet("/api/app1/health", GetHealthAsync)
    .WithName("GetApp1Health")
    .WithOpenApi()
    .WithTags("System");
```

## ?? Next Steps

1. **Run your services** using the Aspire AppHost
2. **Navigate to** `/scalar/v1` on each service
3. **Test your APIs** directly from the Scalar UI
4. **View generated code** in various languages (C#, JavaScript, Python, etc.)

## ?? Production Considerations

- Scalar UI is **disabled in Production** by default
- To enable in other environments, modify the condition in `Program.cs`:
  ```csharp
  if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
  {
      app.MapApp1ApiDocumentation();
  }
  ```

## ?? Additional Resources

- [Scalar Documentation](https://github.com/scalar/scalar)
- [ASP.NET Core OpenAPI](https://learn.microsoft.com/aspnet/core/fundamentals/openapi)
- [.NET 10 Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
