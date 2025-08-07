using Technology_Shop.DTO;
using Technology_Shop.DTO.User;
using Technology_Shop.Models;

namespace Technology_Shop.Services
{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User?> GetUserByIdAsync(int id);
		Task<bool> UpdateUserAsync(UserUpdateDto dto);
		Task<bool> DeleteUserAsync(int id);
		Task<User?> GetUserProfileAsync(int userId);
		Task<bool> UpdateOwnProfileAsync( int userId, UserOwnUpdateDto userOwnUpdateDto);
		Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
	}
}
