//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/13/2026 8:31 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Models;

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class GetAllSortingReq : ISortingRequest
{
	public string SortBy { get; set; } = null!;
	public bool SortDesc { get; set; }
}
