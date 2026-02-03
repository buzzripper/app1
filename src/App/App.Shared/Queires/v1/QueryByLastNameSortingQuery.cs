//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/3/2026 9:41 AM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
using System;
using Dyvenix.App1.Common.Shared.Models;

namespace Dyvenix.App1.App.Shared.Queries.v1;

public class QueryByLastNameSortingQuery : ISortingQuery
{
	public string SortBy { get; set; } = null!;
	public bool SortDesc { get; set; }

	public string LastName { get; set; } = null!;
}
