//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/10/2026 11:50 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.Models;

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class UpdateLastNameAndEmailReq
{
	public Guid Id { get; set; }

	// Required properties
	public string LastName { get; set; }
	public string Email { get; set; }
}
