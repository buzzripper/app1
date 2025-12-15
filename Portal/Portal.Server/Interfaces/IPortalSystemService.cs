using App1.Portal.Server.DTOs;

namespace App1.Portal.Server.Interfaces;

public interface IPortalSystemService
{
	Task<PortalHealthStatus> Health();
}
