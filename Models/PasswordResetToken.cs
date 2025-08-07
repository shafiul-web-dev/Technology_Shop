namespace Technology_Shop.Models
{
	public class PasswordResetToken
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Token { get; set; }
		public DateTime ExpiredAt { get; set; }
		public bool IsUsed { get; set; } = false;
		public User User { get; set; }

	}
}
