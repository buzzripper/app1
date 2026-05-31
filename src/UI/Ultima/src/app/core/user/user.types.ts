
export interface LoggedInUserDto {
    isAuthenticated: boolean;
    tenantId: string | null;
    userId: string;
    name: string;
    email: string;
    roles: string[];
    permissions: string[];
    avatar?: string;
}

