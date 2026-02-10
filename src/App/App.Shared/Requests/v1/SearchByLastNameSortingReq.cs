//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/10/2026 11:50 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Models;

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class SearchByLastNameSortingReq : ISortingRequest
{
	public string SortBy { get; set; } = null!;
	public bool SortDesc { get; set; }
	public string LastName { get; set; } = null!;
}
