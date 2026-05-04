
namespace Dyvenix.App1.App.Shared.Dtos;

public record ClientDto (
	Guid Id,
	string Key,
	string Name,
	string BaseUrl,
	string ExtAuthId,
	string ExtClientId,
	byte[] RowVersion
);

public record ClientLookupDto (
	Guid Id,
	string Key,
	string Name
);

public record ClientRouteDto (
	Guid Id,
	string BaseUrl,
	string Key
);
