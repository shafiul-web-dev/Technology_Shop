using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.DTO
{
	public class UserOwnUpdateDto
	{
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }

	}
}
