
using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.Models
{
	public class RegisterRequest
	{
		[Required, StringLength(100)]
		[Display(Name = "First Name")]
		public string FirstName { get; set; } = string.Empty;

		[Required, StringLength(100)]
		[Display(Name = "Last Name")]
		public string LastName { get; set; } = string.Empty;

		[Required, EmailAddress, StringLength(255)]

		[Display(Name = "Email Address")]
		public string Email { get; set; } = string.Empty;

		[Required, MinLength(6)]
		public string Password { get; set; } = string.Empty;
		[Compare("Password", ErrorMessage = "Password do not match")]
		[Display(Name = "Compare Password")]
		public string ComparePassword { get; set; } = string.Empty;
	}
}
