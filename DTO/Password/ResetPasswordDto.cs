using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.DTO.Password
{
	public class ResetPasswordDto
	{
		[Required]
		public string Token { get; set; }
		[Required]
		[MinLength(6)]
		public string NewPassword { get; set; }
	}
}
