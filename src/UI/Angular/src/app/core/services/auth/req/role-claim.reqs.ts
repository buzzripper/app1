export interface CreateRoleClaimReq {
    roleId: string;
    claimType: string;
    claimValue: string;
}

export interface UpdateRoleClaimReq {
    id: number;
    roleId: string;
    newClaimType: string;
    newClaimValue: string;
}
