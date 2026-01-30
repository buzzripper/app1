//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/29/2026 10:34 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Shared.Exceptions;

namespace Dyvenix.App1.Auth.Shared.Queries.v1;

public class ReadMethod4Query : IPagingQuery
{
	public int PageSize { get; set; }
	public int PageOffset { get; set; }
	public bool RecalcRowCount { get; set; }
	public bool GetRowCountOnly { get; set; }

	public string LastName { get; set; }
}
