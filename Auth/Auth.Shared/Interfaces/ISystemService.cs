using Dyvenix.Auth.Shared.DTOs;

namespace Dyvenix.Auth.Shared.Interfaces;

public interface ISystemService
{
	//Task<string> Ping();
	Task<AuthHealthStatus> Health();
}
