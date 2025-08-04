using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.Models
{
	public class RegisterRequest
	{
		[Required, StringLength(100)]
		public string FirstName { get; set; } = string.Empty;

		[Required, StringLength(100)]
		public string LastName { get; set; } = string.Empty;

		[Required, EmailAddress, StringLength(255)]
		public string Email { get; set; } = string.Empty;

		[Required, MinLength(6)]
		public string Password { get; set; } = string.Empty;
	}
}
