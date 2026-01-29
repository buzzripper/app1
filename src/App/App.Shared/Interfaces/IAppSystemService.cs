using App1.App.Shared.DTOs;

namespace App1.App.Shared.Interfaces;

public interface IAppSystemService
{
	Task<AppHealthStatus> Health();
}
