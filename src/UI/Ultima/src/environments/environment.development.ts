function getBffBaseUrl(): string {
    const hostname = window.location.hostname;
    return `https://${hostname}:5001`;
}

export const environment = {
    production: false,
    apiBaseUrl: getBffBaseUrl()
};
