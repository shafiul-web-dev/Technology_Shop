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
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		if (!PasswordValidator.IsStrong(dto.Password))
		{
			return BadRequest("Password must be at least 6 characters long," +
				" contain an uppercase letter, a digit, and a special character.");
			//Password123!@
		}
		if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
			return Conflict("Email already in use.");

		var user = new User
		{
			FirstName = dto.FirstName.Trim(),
			LastName = dto.LastName.Trim(),
			Email = dto.Email.Trim().ToLower(),
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
			dto.Password, admin.PasswordHash))
		{
			return Ok(_jwt.GenerateToken(admin.Id, admin.Email, "Admin"));
		}

		// Then Users
		var user = await _db.Users
			.SingleOrDefaultAsync(u => u.Email == dto.Email);

		if (user == null || !_hasher.Verify(
			dto.Password,user.PasswordHash))
		{
			return Unauthorized("Invalid credentials.");
		}

		return Ok(_jwt.GenerateToken(user.Id, user.Email, "User"));
	}
}