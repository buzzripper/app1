//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/10/2026 9:58:05 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Requests;

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class GetAllClientsReq : ISortingRequest
{
	public string SortBy { get; set; } = null!;
	public bool SortDesc { get; set; }
}
