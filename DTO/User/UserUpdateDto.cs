using System.ComponentModel.DataAnnotations;

namespace Technology_Shop.DTO.User
{
	public class UserUpdateDto
	{
		[Required]
		public int Id { get; set; }

		[Required, MaxLength(20)]
		public string FirstName { get; set; }

		[Required,MaxLength(20)]
		public string LastName { get; set; }
	}
}