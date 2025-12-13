using Dyvenix.App1.Portal.Server.DTOs;

namespace Dyvenix.App1.Portal.Server.Interfaces;

public interface IPortalSystemService
{
	Task<PortalHealthStatus> Health();
}
