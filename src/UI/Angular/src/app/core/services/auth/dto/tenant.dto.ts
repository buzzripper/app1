import { AuthMode } from './auth-mode';

export interface TenantDto {
    id: string;
    name: string;
    slug: string;
    authMode: AuthMode;
    externalAuthority: string | null;
    externalClientId: string | null;
    externalClientSecret: string | null;
    adDcHost: string | null;
    adDomain: string | null;
    adLdapPort: number | null;
    adBaseDn: string | null;
    isActive: boolean;
    createdAt: string;
}
