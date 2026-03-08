export interface CreateAppRegistrationReq {
    clientId: string;
    clientSecret: string | null;
    displayName: string | null;
    consentType: string | null;
    permissions: string[];
    redirectUris: string[];
    postLogoutRedirectUris: string[];
}

export interface UpdateAppRegistrationReq {
    id: string;
    displayName: string | null;
    clientSecret: string | null;
    consentType: string | null;
    permissions: string[];
    redirectUris: string[];
    postLogoutRedirectUris: string[];
}
