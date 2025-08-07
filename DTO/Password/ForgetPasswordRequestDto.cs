using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.DTO.Password
{
	public class ForgetPasswordRequestDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
