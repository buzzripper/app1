//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/18/2026 7:27 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class UpdateLastNameAndEmailReq
{
	public Guid Id { get; set; }
	public byte[] RowVersion { get; set; }

	// Required properties
	public string LastName { get; set; }
	public string Email { get; set; }
}
