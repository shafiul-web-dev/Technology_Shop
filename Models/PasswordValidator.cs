namespace Technology_Shop.Models
{
	public static class PasswordValidator
	{
		public static bool IsStrong(string password)
		{
			if (string.IsNullOrWhiteSpace(password)) return false;

			bool hasUpper = password.Any(char.IsUpper);
			bool hasDigit = password.Any(char.IsDigit);
			bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

			return hasUpper && hasDigit && hasSpecial;
		}


	}
}
