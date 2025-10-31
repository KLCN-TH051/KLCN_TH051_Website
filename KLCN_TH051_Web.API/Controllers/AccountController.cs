using KLCN_TH051_Website.Common.DTO;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;

        public AccountController(
            IAccountService accountService,
            UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        /// <summary>
        /// [POST] Đăng ký tài khoản
        /// </summary>
        /// <param name="model">Thông tin đăng ký</param>
        /// <returns>Thông báo thành công/thất bại</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            var result = await _accountService.RegisterAsync(model);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Đăng ký thành công! Vui lòng kiểm tra email để xác thực tài khoản."
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = "Đăng ký thất bại.",
                Errors = result.Errors.Select(e => e.Description)
            });
        }

        /// <summary>
        /// [POST] Đăng nhập → trả JWT Token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var token = await _accountService.LoginAsync(model);

                return Ok(new
                {
                    Success = true,
                    Message = "Đăng nhập thành công!",
                    Token = token,
                    TokenType = "Bearer"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Lỗi hệ thống: " + ex.Message
                });
            }
        }

        /// <summary>
        /// [GET] Xác thực email từ link trong Gmail
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <param name="token">Token xác thực (đã encode)</param>
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Thiếu thông tin xác thực (userId hoặc token)."
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng."
                });
            }

            // Giải mã token
            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                // Kích hoạt tài khoản
                if (!user.IsActive)
                {
                    user.IsActive = true;
                    await _userManager.UpdateAsync(user);
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Xác thực email thành công! Bạn có thể đăng nhập ngay."
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = "Token không hợp lệ hoặc đã hết hạn.",
                Errors = result.Errors.Select(e => e.Description)
            });
        }

        /// <summary>
        /// [GET] Kiểm tra trạng thái tài khoản (test)
        /// </summary>
        [HttpGet("status/{email}")]
        public async Task<IActionResult> GetUserStatus(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng với email này."
                });
            }

            return Ok(new
            {
                Success = true,
                Email = user.Email,
                FullName = user.FullName,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth?.ToString("yyyy-MM-dd")
            });
        }

        /// <summary>
        /// [GET] Lấy thông tin người dùng hiện tại (cần JWT)
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                Success = true,
                Data = new
                {
                    user.Id,
                    user.Email,
                    user.FullName,
                    user.PhoneNumber,
                    user.DateOfBirth,
                    user.IsActive
                }
            });
        }
    }
}