namespace Dyvenix.App1.Common.Shared.Models;

public interface ISortingQuery
{
	string SortBy { get; set; }
	bool SortDesc { get; set; }
}
