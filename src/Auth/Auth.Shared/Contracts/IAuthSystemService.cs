
using Dyvenix.App1.Common.Shared.DTOs;

namespace Dyvenix.App1.Auth.Shared.Contracts;

public interface IAuthSystemService
{
    Task<HealthStatus> Health();
}
