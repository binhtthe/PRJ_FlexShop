using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRJ_MKS_BTT.IService;
using PRJ_MKS_BTT.Request;

namespace PRJ_MKS_BTT.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, ILogger<UserController> logger, IConfiguration configuration, IEmailService emailService)
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _userService.LoginAsync(request.Email, request.Password);
                if (response == null)
                {
                    return Unauthorized(new { Message = "Invalid email or password." });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred during login.");
                return StatusCode(500, new { Message = "An error occurred while processing your request.",
                    Error = ex.Message,  // Thêm dòng này để xem lỗi cụ thể
                    InnerError = ex.InnerException?.Message // và dòng này

                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _userService.RegisterAsync(request.FullName, request.Email, request.Password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("REGISTER ERROR: " + ex.ToString());
                _logger.LogError(ex, "Error occurred during registration.");

                // Trả về chi tiết lỗi khi development
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Error = ex.Message,  // Thêm dòng này để xem lỗi cụ thể
                    InnerError = ex.InnerException?.Message // và dòng này
                });
            }
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            try
            {
                var result = await _userService.VerifyEmailAsync(token);
                if (result)
                {
                    return Ok(new { Message = "Email verified successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "Invalid or expired token." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during email verification.");
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

    }
}
