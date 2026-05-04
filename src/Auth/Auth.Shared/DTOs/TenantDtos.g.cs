
namespace Dyvenix.App1.Auth.Shared.DTOs;

public record TenantDto(
    Guid Id,
    string Name,
    string Slug,
    AuthMode AuthMode,
    string? ExternalAuthority,
    string? ExternalClientId,
    string? ExternalClientSecret,
    string? ADDcHost,
    string? ADDomain,
    int? ADLdapPort,
    string? ADBaseDn,
    bool IsActive,
    DateTime CreatedAt
);
