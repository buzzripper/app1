namespace Dyvenix.App1.Common.Shared.DTOs;

public class ListPage<T>
{
	public ListPage()
	{ }

	public ListPage(IEnumerable<T> entityList)
	{
		Items.AddRange(entityList);
	}

	public List<T> Items { get; set; } = [];
	public int TotalRowCount { get; set; }
}
