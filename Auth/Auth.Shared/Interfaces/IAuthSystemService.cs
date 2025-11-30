using Dyvenix.Auth.Shared.DTOs;

namespace Dyvenix.Auth.Shared.Interfaces;

public interface IAuthSystemService
{
	Task<string> Alive();
	Task<AuthHealthStatus> Health();
}
