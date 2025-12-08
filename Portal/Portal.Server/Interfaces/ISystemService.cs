using Dyvenix.App1.Portal.Server.DTOs;

namespace Dyvenix.App1.Portal.Server.Interfaces;

public interface ISystemService
{
	Task<PortalHealthStatus> Health();
}
