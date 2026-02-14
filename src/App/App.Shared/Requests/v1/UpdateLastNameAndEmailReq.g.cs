//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 5:02 PM. Any changes made to it will be lost.
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
