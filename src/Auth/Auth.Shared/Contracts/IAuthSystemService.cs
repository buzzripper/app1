using Dyvenix.App1.Auth.Shared.DTOs;

namespace Dyvenix.App1.Auth.Shared.Contracts;

public interface IAuthSystemService
{
	Task<AuthHealthStatus> Health();
}
