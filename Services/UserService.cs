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

		public Task<bool> UpdateUserAsync(UserUpdateDto dto)
		{
			var user = new User
			{
				Id = dto.Id,
				FirstName = dto.FirstName,
				LastName = dto.LastName
			};
			return _repo.UpdateAsync(user);
		}
		public Task<bool> DeleteUserAsync(int id)
		{
			return _repo.DeleteAsync(id);
		}
	}
}
