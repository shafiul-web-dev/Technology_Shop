using Technology_Shop.DTO;
using Technology_Shop.DTO.User;
using Technology_Shop.Models;
using Technology_Shop.Repositories;

namespace Technology_Shop.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repo;
		public UserService(IUserRepository repo)
		{
			_repo = repo;
		}

		public Task<IEnumerable<User>> GetAllUsersAsync()
			=> _repo.GetAllAsync();
		public Task<User?> GetUserByIdAsync(int id)
			=> _repo.GetByIdAsync(id);

		public async Task<bool> UpdateUserAsync(UserUpdateDto dto)
		{
			var user = await _repo.GetByIdAsync(dto.Id);
			if (user == null) return false;

			if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
				return false;

			user.FirstName = dto.FirstName?.Trim();
			user.LastName = dto.LastName?.Trim();

			return await _repo.UpdateAsync(user);
		}
		public Task<bool> DeleteUserAsync(int id)
		{
			return _repo.DeleteAsync(id);
		}
		public async Task<User?> GetUserProfileAsync(int userId)
		{
			return await _repo.GetByIdAsync(userId);
		}

		public async Task<bool> UpdateOwnProfileAsync(int userId, UserOwnUpdateDto dto)
		{
			var user = await _repo.GetByIdAsync(userId);
			if (user == null)
				return false;

			user.FirstName = dto.FirstName;
			user.LastName = dto.LastName;

			return await _repo.UpdateProfileAsync(user);
		}

		public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
		{
			var user = await _repo.GetByIdAsync(userId);
			if (user == null)
				return false;

			// Validate current password
			if (string.IsNullOrWhiteSpace(dto.CurrentPassword) ||
				!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
				return false;

			// Validate new password
			if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword.Length < 6)
				return false;

			// Hash and update
			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
			return await _repo.UpdateAsync(user);
		}

		public Task<PagedResult<User>> GetPagedResultAsync(int pageNumber, int pageSize)
		{
			return _repo.GetPagedUserAsync(pageNumber, pageSize);
		}

	}
}
