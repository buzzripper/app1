export interface OidcAppDto {
    id: string;
    applicationType: string | null;
    clientId: string;
    clientSecret: string | null;
    displayName: string | null;
    redirectUris: string[];
    postLogoutRedirectUris: string[];
}
