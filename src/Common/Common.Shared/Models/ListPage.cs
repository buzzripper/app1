namespace Dyvenix.App1.Common.Shared.Models
{
	public class ListPage<T> where T : new()
	{
		#region  Constructors

		public ListPage()
		{ }

		public ListPage(IEnumerable<T> entityList)
		{
			Items.AddRange(entityList);
		}

		#endregion

		public List<T> Items { get; set; } = new List<T>();
		public int TotalRowCount { get; set; }
	}
}
