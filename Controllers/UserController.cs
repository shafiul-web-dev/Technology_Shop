using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Technology_Shop.DTO;
using Technology_Shop.DTO.User;
using Technology_Shop.Models;
using Technology_Shop.Services;

namespace Technology_Shop.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _service;
		public UserController(IUserService service)
		{
			_service = service;
		}
		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll()
		{
			var users = await _service.GetAllUsersAsync();
			return Ok(users);
		}
		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetById(int id)
		{
			var user = await _service.GetUserByIdAsync(id);
			if (user == null) return NotFound($"User with id {id} not found.");
			return Ok(user);
		}
		[HttpPut("admin/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
		{
			if (id != dto.Id) return BadRequest("User ID mismatch.");

			var userUpdated = await _service.UpdateUserAsync(dto);
			if (!userUpdated) return NotFound("User not found or Update failed!");
			return Ok("User Update Successfully");
		}
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int id)
		{
			var userDeleted = await _service.DeleteUserAsync(id);
			if (!userDeleted) return NotFound($"User with id {id} not found.");
			return Ok($"User with id {id} deleted successfully.");
		}
		[HttpGet("me")]
		[Authorize]
		public async Task<IActionResult> GetMyProfile()
		{
			var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
				return Unauthorized("Invalid or missing user ID in token.");

			var userInfo = await _service.GetUserProfileAsync(userId);
			if (userInfo == null)
				return NotFound("User not found.");

			return Ok(userInfo);
		}
		[Authorize(Policy = "CanUpdateOwnProfile")]
		[HttpPut("me")]
		public async Task<IActionResult> UpdateeOwnProfile([FromBody] UserOwnUpdateDto dto)
		{
			var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!int.TryParse(userIdStr, out int userId))
				return Unauthorized("Invalid token.");

			var success = await _service.UpdateOwnProfileAsync(userId, dto);
			if (!success)
				return NotFound("User not found or update failed.");

			return Ok("Profile updated successfully.");
		}
		[Authorize]
		[HttpPut("me/change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
		{
			var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!int.TryParse(userIdStr, out int userId))
				return Unauthorized("Invalid token.");

			var success = await _service.ChangePasswordAsync(userId, dto);
			if (!success)
				return BadRequest("Current password is incorrect or update failed.");

			return Ok("Password changed successfully.");
		}


	}
}