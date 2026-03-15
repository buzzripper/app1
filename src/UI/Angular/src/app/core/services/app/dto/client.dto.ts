
export interface ClientDto {
	id: string;
	key: string;
	name: string;
	baseUrl: string;
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

export interface Dto4 {
	key: string;
	rowVersion: Uint8Array;
	name: string;
}
