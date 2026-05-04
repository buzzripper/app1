export interface AppRegistrationDto {
    id: string;
    clientId: string;
    displayName: string | null;
    consentType: string | null;
    permissions: string[];
    redirectUris: string[];
    postLogoutRedirectUris: string[];
}
