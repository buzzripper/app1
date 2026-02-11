//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/10/2026 4:33 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Models;

namespace Dyvenix.App1.App.Shared.Requests.v1;

public class GetAllPagingReq : IPagingRequest
{
	public int PageSize { get; set; }
	public int PageOffset { get; set; }
	public bool RecalcRowCount { get; set; }
	public bool GetRowCountOnly { get; set; }
}
