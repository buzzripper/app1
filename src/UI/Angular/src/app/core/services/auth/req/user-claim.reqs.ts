export interface CreateUserClaimReq {
    userId: string;
    claimType: string;
    claimValue: string;
}

export interface UpdateUserClaimReq {
    id: number;
    userId: string;
    newClaimType: string;
    newClaimValue: string;
}
