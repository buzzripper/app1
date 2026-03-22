import { AuthMode } from '../dto';

export interface CreateTenantReq {
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

export interface UpdateTenantReq {
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
