namespace Technology_Shop.Models
{
	public class Order
	{
		public int ID { get; set; }
		public DateTime OrderDate { get; set; }

		// FK to User
		public int UserId { get; set; }
		public User User { get; set; } = null!;

		// Navigation
		public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
		public Payment Payment { get; set; }

	}
}
