namespace Dyvenix.App1.Tests.Integration.Fixtures
{
	public class Pager
	{
		private const int MIN_COUNT = 3;

		public Pager(int itemCount)
		{
			if (itemCount < MIN_COUNT)
				throw new ApplicationException($"Need at least {MIN_COUNT} items to test paging");

			var pageSize = itemCount / MIN_COUNT;
			var remainder = itemCount % MIN_COUNT;
			var totalPages = (remainder == 0) ? MIN_COUNT : MIN_COUNT + 1;
			for (var i = 0; i < (totalPages - 1); i++)
				Expected.Add(pageSize);
			Expected.Add(remainder);
		}

		public int PageSize { get; set; }
		public int TotalPages { get; set; }
		public List<int> Expected { get; set; } = new List<int>();
	}
}
