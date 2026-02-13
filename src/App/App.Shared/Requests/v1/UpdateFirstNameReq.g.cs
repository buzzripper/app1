//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/13/2026 8:31 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.Models;

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class UpdateFirstNameReq
{
	public Guid Id { get; set; }
	public byte[] RowVersion { get; set; }

	// Required properties
	public string FirstName { get; set; }
}
