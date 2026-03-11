//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/10/2026 9:58:05 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Requests;

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class SearchClientsByNameReq : IPagingRequest, ISortingRequest
{
	public int PageSize { get; set; }
	public int PageOffset { get; set; }
	public bool RecalcRowCount { get; set; }
	public bool GetRowCountOnly { get; set; }
	public string SortBy { get; set; } = null!;
	public bool SortDesc { get; set; }
	public string Name { get; set; } = null!;
}
