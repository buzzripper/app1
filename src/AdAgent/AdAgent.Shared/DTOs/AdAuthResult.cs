
namespace Dyvenix.App1.AdAgent.Shared.DTOs;

public sealed record AdAuthResult(
    AdAuthStatus Status,
    string? Message = null,
    Guid? UserObjectGuid = null
);
