
namespace Dyvenix.App1.App.Shared.Dtos;

public record ClientDto (
	Guid Id,
	string Key,
	string Name,
	string BaseUrl
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

public record Dto4 (
	string Key,
	byte[] RowVersion,
	string Name
);
