//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/27/2026 4:53 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;

namespace Dyvenix.App1.Auth.Shared.DTOs;

public record Dto1 (
	Guid Id,
	string Name,
	string Slug,
	string ExternalClientId,
	string ExternalClientSecret,
	string ADDcHost
);

public record Dto2 (
	AuthMode AuthMode,
	bool IsActive,
	DateTime CreatedAt,
	string ExternalAuthority,
	string ExternalClientSecret,
	string ADDomain
);
