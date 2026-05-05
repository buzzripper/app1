export interface User {
    id: string;
    name: string;
    email: string;
    avatar?: string;
    status?: string;
    tenantId?: string | null;
    roles?: string[];
    permissions?: string[];
}
