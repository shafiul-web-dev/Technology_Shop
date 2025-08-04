using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.Models
{
	public class Category
	{
		public int ID { get; set; }

		[Required, MaxLength(20)]
		public string Name { get; set; }
		public ICollection<Product> Products { get; set; }

	}
}
