export interface CreateOidcAppReq {
    applicationType: string | null;
    clientId: string;
    clientSecret: string | null;
    displayName: string | null;
    redirectUris: string[];
    postLogoutRedirectUris: string[];
}

export interface UpdateOidcAppReq {
    id: string;
    applicationType: string | null;
    clientId: string;
    clientSecret: string | null;
    displayName: string | null;
    redirectUris: string[];
    postLogoutRedirectUris: string[];
}
