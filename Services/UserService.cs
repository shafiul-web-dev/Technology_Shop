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
	}
}
