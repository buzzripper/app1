export interface User {
    id: string;
    name: string;
    email: string;
    avatar?: string;
    status?: string;
    tenantId?: string;
    roles?: string[];
    permissions?: string[];
}
