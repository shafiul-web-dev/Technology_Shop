namespace Technology_Shop.Models
{
	public class Payment
	{
		public int ID { get; set; }
		public decimal Amount { get; set; }
		public DateTime PaymentDate { get; set; }
		public string PaymentMethod { get; set; }
		public int OrderID { get; set; }
		public Order Order { get; set; }

	}
}
