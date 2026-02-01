//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/1/2026 4:43 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Shared.Exceptions;

namespace Dyvenix.App1.App.Shared.DTOs.v1;

public class UpdateLastNameAndEmailReq
{
	public Guid Id { get; set; }

	// Required properties
	public string LastName { get; set; }
	public string Email { get; set; }
}
