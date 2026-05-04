namespace Dyvenix.App1.Auth.Shared.DTOs;

public record UserClaimDto(
    int Id,
    string UserId,
    string ClaimType,
    string ClaimValue
);
