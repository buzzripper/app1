namespace Dyvenix.App1.Common.Shared.Requests;

public interface IPagingRequest
{
	int PageOffset { get; set; }
	int PageSize { get; set; }
	bool RecalcRowCount { get; set; }
	bool GetRowCountOnly { get; set; }
}
