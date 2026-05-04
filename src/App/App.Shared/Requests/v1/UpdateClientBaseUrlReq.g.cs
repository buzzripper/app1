
namespace Dyvenix.App1.App.Shared.Requests.v1;

public class UpdateClientBaseUrlReq
{
	public Guid Id { get; set; }
	public byte[] RowVersion { get; set; }

	// Required properties
	public string BaseUrl { get; set; }
	public string Key { get; set; }
}
