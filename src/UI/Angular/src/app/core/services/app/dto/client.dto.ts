
export interface ClientDto {
	id: string;
	key: string;
	name: string;
	baseUrl: string;
	extAuthId: string;
	extClientId: string;
	rowVersion: Uint8Array;
}

export interface ClientLookupDto {
	id: string;
	key: string;
	name: string;
}

export interface ClientRouteDto {
	id: string;
	baseUrl: string;
	key: string;
}
