﻿namespace Technology_Shop.Models
{
	public class AuthResponse
	{
		public string Token { get; set; } = string.Empty;
		public DateTime ExpiresAt { get; set; }
	}

}
