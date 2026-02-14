//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/14/2026 5:02 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Requests;

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class GetAllSortingReq : ISortingRequest
{
	public string SortBy { get; set; } = null!;
	public bool SortDesc { get; set; }
}
