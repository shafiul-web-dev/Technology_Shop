using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Technology_Shop.Configurations;

namespace Technology_Shop.Models
{
	public class JwtService
	{
		private readonly JwtSettings _settings;

		public JwtService(IOptions<JwtSettings> opts)
		{
			_settings = opts.Value;
		}

		public AuthResponse GenerateToken(string email, string role)
		{
			var claims = new[]
			{
			new Claim(JwtRegisteredClaimNames.Sub, email),
			new Claim(ClaimTypes.Role, role)
		};

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_settings.Key));

			var creds = new SigningCredentials(
				key, SecurityAlgorithms.HmacSha256);

			var expires = DateTime.UtcNow.AddMinutes(
				_settings.DurationInMinutes);

			var token = new JwtSecurityToken(
				issuer: _settings.Issuer,
				audience: _settings.Audience,
				claims: claims,
				expires: expires,
				signingCredentials: creds
			);

			return new AuthResponse
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				ExpiresAt = expires
			};
		}
	}
}
