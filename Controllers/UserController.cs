using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IActionResult> GetById(int id)
		{
			var user = await _service.GetUserByIdAsync(id);
			if(user == null) return NotFound($"User with id {id} not found.");
			return Ok(user);
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
		{
			if( id != dto.Id ) return BadRequest("User ID mismatch.");

			var userUpdated = await _service.UpdateUserAsync(dto);
			return Ok(userUpdated);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var userDeleted = await _service.DeleteUserAsync(id);
			if (!userDeleted) return NotFound($"User with id {id} not found.");
			return Ok($"User with id {id} deleted successfully.");
		}
	}
}
