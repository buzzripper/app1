namespace Dyvenix.App1.Auth.Shared.DTOs;

public record ScopeDto(
    string Id,
    string Name,
    string? DisplayName,
    string? Description
);
