using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRJ_MKS_BTT.Data;
using PRJ_MKS_BTT.IRepository;
using PRJ_MKS_BTT.IService;
using PRJ_MKS_BTT.Model;
using PRJ_MKS_BTT.Response;

namespace PRJ_MKS_BTT.Service
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public UserService(ApplicationDbContext context, IConfiguration configuration, IUserRepository userRepository, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _userRepository = userRepository;
            _emailService = emailService;
        }
    

        public  string GenerateJwtToken(User user)
        {
          var jwtSetting = _configuration.GetSection("JwtSettings");
          var secretKey = jwtSetting.GetValue<string>("SecretKey");
          var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
          var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
          var claims = new[]
          {
              new System.Security.Claims.Claim("UserId", user.Id.ToString()),
              new System.Security.Claims.Claim("Email", user.Email),
              new System.Security.Claims.Claim("FullName", user.FullName),
              new System.Security.Claims.Claim("Role", user.Role ?? "User")
          };
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: jwtSetting.GetValue<string>("Issuer"),
                audience: jwtSetting.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSetting.GetValue<int>("ExpiryMinutes")),
                signingCredentials: credentials
            );
            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);


                if (user == null)
                    return null;

                // Kiểm tra password trước
                if (!VerifyPassword(password, user.PasswordHash))
                    return null;

                // Nếu email chưa verify, gửi lại email xác thực
                if (user.IsEmailVerified == false)
                {
                    // Tạo token mới nếu token cũ đã hết hạn hoặc không tồn tại
                    if (string.IsNullOrEmpty(user.EmailVerifyToken) ||
                        user.EmailVerifyTokenExpiry == null ||
                        user.EmailVerifyTokenExpiry.Value < DateTime.UtcNow)
                    {
                        user.EmailVerifyToken = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(64));
                        user.EmailVerifyTokenExpiry = DateTime.UtcNow.AddHours(1);

                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();
                    }

                    // Gửi email xác thực
                    var verifyUrl = $"http://localhost:7291/api/auth/verify-email?token={user.EmailVerifyToken}";
                    var body = $@"
                        <h2>Hello {user.FullName}</h2>
                        <p>Your email is not verified yet. Please click the link below to verify:</p>
                        <a href='{verifyUrl}' target='_blank'>Verify Email</a>
                        <p>This link will expire in 1 hour.</p>
                    ";

                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Verify your email - Login Attempt",
                        body
                    );

                    throw new Exception("Email not verified. A verification email has been sent to your inbox.");
                }

                // Tạo JWT token
                var token = GenerateJwtToken(user);

                // Trả về response
                return new LoginResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Token = token
                };
            }
            catch
            {
                throw;
            }
        }


        public async Task<RegisterResponse> RegisterAsync(string fullName, string email, string password)
        {
            // 1. Kiểm tra email tồn tại
            if (await _userRepository.CheckEmailExistAsync(email))
                throw new Exception("Email already in use");

            // 2. Hash password
            var hashedPassword = HashPassword(password);

            // 3. Tạo user (không set token ở đây vì CreateUserAsync đã lo)
            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = hashedPassword,
                IsEmailVerified = false,
                Role = "Buyer"
            };

            // 4. Lưu DB (CreateUserAsync sẽ tự tạo token + time)
            var createdUser = await _userRepository.CreateUserAsync(user);

            var verifyUrl = $"http://localhost:7291/api/auth/verify-email?token={createdUser.EmailVerifyToken}";
            var body = $@"
        <h2>Welcome {createdUser.FullName}</h2>
        <p>Click the link below to verify your email:</p>
        <a href='{verifyUrl}' target='_blank'>Verify Email</a>
    ";
            await _emailService.SendEmailAsync(
       createdUser.Email,
       "Verify your email",
       body
   );



            // 5. Tạo response
            return new RegisterResponse
            {
                UserId = createdUser.Id,
                Username = createdUser.FullName,
                Email = createdUser.Email,
                Message = "Registration successful. Please verify your email.",
                EmailSent = true
            };
        }


        public async Task<bool> VerifyEmailAsync(string token)
        {
            try
            {
               
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.EmailVerifyToken == token);

                if (user == null)
                    return false;

               
                if (user.IsEmailVerified)
                    return true;

              
                if (user.EmailVerifyTokenExpiry < DateTime.UtcNow)
                    return false;

             
                user.IsEmailVerified = true;
                user.EmailVerifyToken = null;
                user.EmailVerifyTokenExpiry = null;

                
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw;
            }
        }


        public bool VerifyPassword(string password, string hash)
        {
           return BCrypt.Net.BCrypt.Verify(password, hash);
        }

    

      
    }
}
