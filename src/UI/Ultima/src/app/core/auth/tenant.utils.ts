export function getTenantSlug(): string | null {
    const hostname = window.location.hostname;
    const parts = hostname.split('.');

    if (parts.length >= 2 && parts[parts.length - 1] === 'localhost') {
        return parts.slice(0, -1).join('.') || null;
    }

    if (parts.length >= 3) {
        return parts[0];
    }

    return null;
}
