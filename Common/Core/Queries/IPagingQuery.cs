namespace Dyvenix.App1.Common.Core.Queries;

public interface IPagingQuery
{
	int RowOffset { get; set; }
	int PageSize { get; set; }
	bool RecalcRowCount { get; set; }
	bool GetRowCountOnly { get; set; }
}
