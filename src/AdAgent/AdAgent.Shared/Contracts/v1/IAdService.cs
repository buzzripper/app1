using Dyvenix.App1.AdAgent.Shared.DTOs;

namespace Dyvenix.App1.AdAgent.Shared.Contracts.v1;

public interface IAdService
{
    Task<AdAuthResult> AuthenticateUser(string userUpnOrDomainUser, string password, CancellationToken ct = default);
}
