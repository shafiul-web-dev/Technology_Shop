using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Technology_Shop.Data;
using Technology_Shop.DTO.Password;
using Technology_Shop.Interfaces.EmailInterface;
using Technology_Shop.Models;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
	private readonly ApplicationDbContext _db;
	private readonly IPasswordHasher _hasher;
	private readonly JwtService _jwt;
	private readonly IEmailService _emailService;

	public AuthController(
		ApplicationDbContext db,
		IEmailService emailService,
		IPasswordHasher hasher,
		JwtService jwt)
	{
		_db = db;
		_hasher = hasher;
		_jwt = jwt;
		_emailService = emailService;
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

	[HttpPost("forget-password")]
	public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestDto forgetPasswordRequestDto)
	{
		var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == forgetPasswordRequestDto.Email);
		if (user == null)
		{
			return BadRequest("User Not Found");
		}
		var token = Guid.NewGuid().ToString();
		var resetEntry = new PasswordResetToken
		{
			UserId = user.Id,
			Token = token,
			ExpiredAt = DateTime.UtcNow.AddMinutes(10)

		};
		_db.PasswordResetTokens.Add(resetEntry);
		await _db.SaveChangesAsync();

		var resetLink = $"https://yourapp.com/reset-password?token={token}";
		var body = $"Click here to reset your password:<br/><a href=\"{resetLink}\">{resetLink}</a>";

		await _emailService.SendAsync(user.Email, "Reset Your Password", body);
		return Ok("If that email exists, a reset link has been sent.");
	}

	[HttpPost("reset-password")]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
	{
		var entry = await _db.PasswordResetTokens.Include(e => e.User)
			.SingleOrDefaultAsync(e => e.Token == resetPasswordDto.Token);
		if (entry == null || entry.IsUsed || entry.ExpiredAt < DateTime.UtcNow)
		{
			return BadRequest("Invalid or expired token.");
		}

		entry.User.PasswordHash = _hasher.Hash(resetPasswordDto.NewPassword);
		entry.IsUsed = true;
		await _db.SaveChangesAsync();
		return Ok("Password Change Successfully");
	}
}