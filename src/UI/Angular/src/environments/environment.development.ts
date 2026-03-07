/**
 * Derives the BFF base URL from the current hostname.
 * acme.localhost:4201 → https://acme.localhost:5001
 * localhost:4201       → https://localhost:5001 (no tenant — auth will be blocked by the guard)
 */
function getBffBaseUrl(): string {
    const hostname = window.location.hostname; // e.g. "acme.localhost"
    return `https://${hostname}:5001`;
}

export const environment = {
    production: false,
    apiBaseUrl: getBffBaseUrl()
};
