

using PRJ_MKS_BTT.Model;
using PRJ_MKS_BTT.Response;

namespace PRJ_MKS_BTT.IService
{
    public interface IUserService
    {
       Task<LoginResponse> LoginAsync(string email, string password);
       Task<RegisterResponse> RegisterAsync(string username, string email, string password);     
       Task<bool> VerifyEmailAsync(string token);

        string GenerateJwtToken(User user);
        string HashPassword(string password);
       

    }
}
