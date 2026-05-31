namespace Dyvenix.App1.App.Shared.DTOs.User;

public sealed class UserDto
{
	public required string Id { get; init; }

	public required string Email { get; init; }

	public required string DisplayName { get; init; }

	public string? GivenName { get; init; }

	public string? Surname { get; init; }

	public string? JobTitle { get; init; }

	public string? PreferredLanguage { get; init; }

	public string? TimeZone { get; init; }

	public bool IsInternalUser { get; init; }
}
