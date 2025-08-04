using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required, StringLength(100)]
		public string FirstName { get; set; } = string.Empty;

		[Required, StringLength(100)]
		public string LastName { get; set; } = string.Empty;

		[Required, EmailAddress, StringLength(255)]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string PasswordHash { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation: one user can have many orders
		public ICollection<Order> Orders { get; set; } = new List<Order>();

	}

}
