using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.DTO
{
	public class UserOwnUpdateDto
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string lastName { get; set; }

		[EmailAddress,Required]
		public string Email { get; set; }

	}
}
