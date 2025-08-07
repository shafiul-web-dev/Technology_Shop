using Technology_Shop.Models;

namespace Technology_Shop.Repositories
{
	public interface IUserRepository
	{
		Task<IEnumerable<User>> GetAllAsync();
		Task<User?> GetByIdAsync(int id);
		Task<bool> UpdateAsync(User user);
		Task<bool> DeleteAsync(int id);
		Task<User?> GettByIdAsync(int id);
		Task<bool> UpdateProfileAsync(User user);

		Task<PagedResult<User>> GetPagedUserAsync(int pageNumber, int pageSize);



	}
}
