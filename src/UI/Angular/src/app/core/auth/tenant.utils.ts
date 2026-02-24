/**
 * Extracts the tenant slug from the current hostname's subdomain.
 * "acme.localhost" → "acme"
 * "fabrikam.localhost" → "fabrikam"
 * "localhost" → null (no tenant)
 */
export function getTenantSlug(): string | null {
    const hostname = window.location.hostname; // e.g. "acme.localhost"
    const parts = hostname.split('.');

    // Expect at least 2 parts for a subdomain (e.g. "acme.localhost")
    if (parts.length >= 2 && parts[parts.length - 1] === 'localhost') {
        return parts.slice(0, -1).join('.') || null;
    }

    // Production: e.g. "acme.app1.example.com" → "acme"
    if (parts.length >= 3) {
        return parts[0];
    }

    return null;
}
