namespace Dyvenix.App1.Common.Core.Queries;

public interface ISortingQuery
{
	string SortBy { get; set; }
	bool SortDesc { get; set; }
}
