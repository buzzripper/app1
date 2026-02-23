namespace Dyvenix.App1.Portal.Server.Middleware;

/// <summary>
/// Extracts the tenant slug from the first subdomain of the request host
/// and stores it in HttpContext.Items["TenantSlug"] for downstream use.
/// </summary>
public class TenantMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context)
    {
        var host = context.Request.Host.Host;
        var slug = ExtractTenantSlug(host);

        if (!string.IsNullOrEmpty(slug))
        {
            context.Items["TenantSlug"] = slug;
        }

        return next(context);
    }

    private static string? ExtractTenantSlug(string host)
    {
        // "acme.dyvenix.com" ? "acme"
        // "acme.localhost"   ? "acme"
        // "localhost"        ? null (no subdomain)
        // "127.0.0.1"       ? null (IP address)

        if (System.Net.IPAddress.TryParse(host, out _))
            return null;

        var parts = host.Split('.');
        return parts.Length >= 2 ? parts[0] : null;
    }
}
