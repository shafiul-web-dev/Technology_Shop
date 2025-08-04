using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technology_Shop.Data;
using Technology_Shop.Models;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
	private readonly ApplicationDbContext _db;
	private readonly IPasswordHasher _hasher;
	private readonly JwtService _jwt;

	public AuthController(
		ApplicationDbContext db,
		IPasswordHasher hasher,
		JwtService jwt)
	{
		_db = db;
		_hasher = hasher;
		_jwt = jwt;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterRequest dto)
	{
		if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
			return Conflict("Email already in use.");

		var user = new User
		{
			FirstName = dto.FirstName,
			LastName = dto.LastName,
			Email = dto.Email,
			PasswordHash = _hasher.Hash(dto.Password)
		};

		_db.Users.Add(user);
		await _db.SaveChangesAsync();

		return Ok("Registration successful.");
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginRequest dto)
	{
		// Check Admins first
		var admin = await _db.Admins
			.SingleOrDefaultAsync(a => a.Email == dto.Email);

		if (admin != null && _hasher.Verify(
			admin.PasswordHash, dto.Password))
		{
			return Ok(_jwt.GenerateToken(admin.Email, "Admin"));
		}

		// Then Users
		var user = await _db.Users
			.SingleOrDefaultAsync(u => u.Email == dto.Email);

		if (user == null || !_hasher.Verify(
			user.PasswordHash, dto.Password))
		{
			return Unauthorized("Invalid credentials.");
		}

		return Ok(_jwt.GenerateToken(user.Email, "User"));
	}
}