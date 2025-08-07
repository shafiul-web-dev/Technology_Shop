using Microsoft.EntityFrameworkCore;
using Technology_Shop.Data;
using Technology_Shop.Models;
using Technology_Shop.Repositories;

namespace Technology_Shop.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _db;
		public UserRepository(ApplicationDbContext db)
			=> _db = db;

		public async Task<IEnumerable<User>> GetAllAsync()
			=> await _db.Users.AsNoTracking().ToListAsync();

		public async Task<User?> GetByIdAsync(int id)
			=> await _db.Users.FindAsync(id);

		public async Task<bool> UpdateAsync(User user)
		{
			_db.Users.Update(user);
			return await _db.SaveChangesAsync() > 0;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var user = await _db.Users.FindAsync(id);
			if(user == null)
			{
				return false;
			}
			_db.Users.Remove(user);
			return await _db.SaveChangesAsync() > 0;
		}
		public async Task<User?> GettByIdAsync(int id)
		{
			return await _db.Users.FindAsync(id);
		}
		public async Task<bool> UpdateProfileAsync(User user)
		{
			_db.Users.Update(user);
			return await _db.SaveChangesAsync() > 0;
		}

		public async Task<PagedResult<User>> GetPagedUserAsync(int pageNumber, int pageSize)
		{
			var query = _db.Users.AsNoTracking();
			var totalRecords = await query.CountAsync();
			var items = await query
				.Skip((pageNumber-1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return new PagedResult<User>
			{
				TotalRecords = totalRecords,
				PageSize = pageSize,
				PageNumbers = pageNumber,
				Items = items
			};
		}
	}
}
