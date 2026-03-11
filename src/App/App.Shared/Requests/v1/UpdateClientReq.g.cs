//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/10/2026 9:58:05 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class UpdateClientReq
{
	public Guid Id { get; set; }
	public byte[] RowVersion { get; set; }

	// Required properties
	public string Name { get; set; }
	public string BaseUrl { get; set; }
	public string Key { get; set; }
}
