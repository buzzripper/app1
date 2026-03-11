//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/10/2026 9:58:05 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

// Read methods

export interface GetAllClientLookupItemsReq {

	sortBy : string
	sortDesc : boolean
}

export interface GetAllClientsReq {

	sortBy : string
	sortDesc : boolean
}

export interface SearchClientsByNameReq {
	name : string;

	pageSize : number;
	pageOffset : number;
	recalcRowCount : boolean;
	getRowCountOnly : boolean;

	sortBy : string
	sortDesc : boolean
}

// Update methods

export interface CreateClientReq {
	id: string
	rowVersion: Uint8Array

	// Required properties
	key : string;
	name : string;
	baseUrl : string;
}

export interface UpdateClientReq {
	id: string
	rowVersion: Uint8Array

	// Required properties
	name : string;
	baseUrl : string;
	key : string;
}

export interface UpdateClientBaseUrlReq {
	id: string
	rowVersion: Uint8Array

	// Required properties
	baseUrl : string;
	key : string;
}
