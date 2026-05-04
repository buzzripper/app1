namespace Dyvenix.App1.Auth.Shared.DTOs;

public record RoleDto(
    string Id,
    Guid TenantId,
    string Name
);
