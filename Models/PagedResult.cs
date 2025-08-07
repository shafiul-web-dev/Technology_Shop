namespace Technology_Shop.Models
{
	public class PagedResult<T>
	{
		public int TotalRecords { get; set; }
		public int PageNumbers { get; set; }
		public int PageSize { get; set; }
		public int TotalPages => (int) Math.Ceiling((double)TotalRecords / PageSize);
		public IEnumerable<T> Items { get; set; } 

	}
}
