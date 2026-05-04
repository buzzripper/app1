
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
