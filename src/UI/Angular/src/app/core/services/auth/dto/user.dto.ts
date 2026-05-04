export interface UserDto {
    id: string;
    tenantId: string;
    userName: string;
    email: string;
    phoneNumber: string | null;
    emailConfirmed: boolean;
    lockoutEnabled: boolean;
    lockoutEnd: string | null;
    twoFactorEnabled: boolean;
}

export interface UserSummaryDto {
    id: string;
    tenantId: string;
    userName: string;
    email: string;
}
