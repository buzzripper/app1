//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.App.Shared.Dtos;

public record ClientDto (
	Guid Id,
	string Key,
	string Name,
	string BaseUrl
);

public record ClientOptionDto (
	Guid Id,
	string Key,
	string Name
);

public record ClientRouteDto (
	Guid Id,
	string Key,
	string BaseUrl
);
