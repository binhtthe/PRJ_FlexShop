using PRJ_MKS_BTT.Data;
using PRJ_MKS_BTT.Model;

namespace PRJ_MKS_BTT.IRepository
{
    public interface IUserRepository
    {
     Task<User> GetUserByIdAsync(int userId);
     Task<User> GetUserByEmailAsync(string email);
     Task<User> CreateUserAsync(User user);
     Task<UserAddress> AddUserAddressAsync(UserAddress address);
     Task<User> UpdateUserAsync(User user);
     Task<bool> DeleteUserAsync(int userId);
     Task<bool> CheckEmailExistAsync(string email);
    }
}
