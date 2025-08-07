namespace Technology_Shop.Models
{
	public class BcryptPasswordHasher : IPasswordHasher
	{
		public string Hash(string password)
		{
			// Generates with default safe settings
			return BCrypt.Net.BCrypt.HashPassword(password);
		}
		public bool Verify(string password, string hash)
		{
			if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
				return false;

			return BCrypt.Net.BCrypt.Verify(password, hash);
		}
	}



}
