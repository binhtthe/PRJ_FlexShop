using Microsoft.EntityFrameworkCore;
using PRJ_MKS_BTT.Data;
using PRJ_MKS_BTT.IRepository;
using PRJ_MKS_BTT.Model;
using System.Security.Cryptography;

namespace PRJ_MKS_BTT.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            Console.WriteLine("🔍 CONNECTED DB = " + _context.Database.GetDbConnection().Database);
        }
        public async Task<UserAddress> AddUserAddressAsync(UserAddress address)
        {
            throw new NotImplementedException();    
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<User> CreateUserAsync(User user)
        {
          
            user.CreatedAt = DateTime.UtcNow;
            user.EmailVerifyToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.EmailVerifyTokenExpiry = DateTime.UtcNow.AddHours(1);
            _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (result == null)
                return null;

            result.FullName = user.FullName;
            result.Phone = user.Phone;
            result.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return result;
        }

    }
}
