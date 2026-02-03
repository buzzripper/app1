//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/2/2026 8:28 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Models;
using Dyvenix.App1.Common.Shared.Exceptions;

namespace Dyvenix.App1.App.Shared.Queries.v1;

public class QueryByLastNamePagingQuery : IPagingQuery
{
	public int PageSize { get; set; }
	public int PageOffset { get; set; }
	public bool RecalcRowCount { get; set; }
	public bool GetRowCountOnly { get; set; }

	public string LastName { get; set; }
}
