using Dyvenix.App1.Auth.Shared.DTOs;

namespace Dyvenix.App1.Auth.Shared.Interfaces;

public interface IAuthSystemService
{
	//Task<string> Ping();
	Task<AuthHealthStatus> Health();
}
