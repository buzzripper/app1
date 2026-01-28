namespace Dyvenix.App1.Common.Core.Models;

public interface ISortingQuery
{
	string SortBy { get; set; }
	bool SortDesc { get; set; }
}
