namespace Dyvenix.App1.Common.Core.Models;

public interface IPagingQuery
{
	int PageOffset { get; set; }
	int PageSize { get; set; }
	bool RecalcRowCount { get; set; }
	bool GetRowCountOnly { get; set; }
}
