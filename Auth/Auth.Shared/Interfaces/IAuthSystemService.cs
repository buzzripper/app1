using App1.Auth.Shared.DTOs;

namespace App1.Auth.Shared.Interfaces;

public interface IAuthSystemService
{
	//Task<string> Ping();
	Task<AuthHealthStatus> Health();
}
