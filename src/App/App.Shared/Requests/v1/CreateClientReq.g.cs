
namespace Dyvenix.App1.App.Shared.Requests.v1;

public class CreateClientReq
{
	public Guid Id { get; set; }
	public byte[] RowVersion { get; set; }

	// Required properties
	public string Key { get; set; }
	public string Name { get; set; }
	public string BaseUrl { get; set; }
}
