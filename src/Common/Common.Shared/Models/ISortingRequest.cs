namespace Dyvenix.App1.Common.Shared.Models;

public interface ISortingRequest
{
	string SortBy { get; set; }
	bool SortDesc { get; set; }
}
