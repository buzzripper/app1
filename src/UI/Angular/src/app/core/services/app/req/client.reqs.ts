export interface CreateClientReq {
    id: string;
    rowVersion: string | null;
    key: string;
    name: string;
    baseUrl: string;
}

export interface GetAllClientOptionsReq {
    sortBy: string;
    sortDesc: boolean;
}
