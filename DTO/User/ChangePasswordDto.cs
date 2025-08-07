using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.DTO.User
{
	public class ChangePasswordDto
	{
		[Required]
		public string CurrentPassword { get; set; }
		[Required,MinLength(6)]
		public string NewPassword { get; set; }
	}
}
