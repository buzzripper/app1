export interface CreateScopeReq {
    name: string;
    displayName: string | null;
    description: string | null;
}

export interface UpdateScopeReq {
    id: string;
    displayName: string | null;
    description: string | null;
}
