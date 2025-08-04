using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.Models
{
	public class Admin
	{
		public int Id { get; set; }

		[Required, EmailAddress, StringLength(255)]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string PasswordHash { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}

}
