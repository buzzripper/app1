export interface CreateUserReq {
    tenantId: string;
    userName: string;
    email: string;
    password: string;
    phoneNumber: string | null;
}

export interface UpdateUserReq {
    id: string;
    email: string | null;
    phoneNumber: string | null;
    lockoutEnabled: boolean | null;
    lockoutEnd: string | null;
}
