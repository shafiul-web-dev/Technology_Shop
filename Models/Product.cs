using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.Models
{
	public class Product
	{
		public int ID { get; set; }
		[Required, MaxLength(30)]
		public string Name { get; set; }
		[Required, MaxLength(100)]
		public string Description { get; set; }
		[Required]
		public decimal Price { get; set; }
		public int StockQuantity { get; set; }
		public int CategoryID { get; set; }
		public Category Category { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
	}
}
