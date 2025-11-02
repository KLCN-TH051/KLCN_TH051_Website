using KLCN_TH051_Website.Common.DTO;
using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Extensions;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountController(
            IAccountService accountService,
            UserManager<User> userManager,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _accountService = accountService;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        /// <summary>
        /// [POST] Đăng ký tài khoản
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<UserResponse>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _accountService.RegisterAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        /// <summary>
        /// [POST] Đăng nhập → trả JWT Token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _accountService.LoginAsync(model);
            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        /// <summary>
        /// [GET] Xác thực email từ link trong Gmail
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <param name="token">Token xác thực (đã encode)</param>
        /// <summary>
        /// [GET] Xác thực email từ link trong Gmail
        /// </summary>
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Thiếu thông tin xác thực (userId hoặc token)."
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng."
                });
            }

            var decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                if (!user.IsActive)
                {
                    user.IsActive = true;
                    await _userManager.UpdateAsync(user);
                }

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Xác thực email thành công! Bạn có thể đăng nhập ngay."
                });
            }

            return BadRequest(new ApiResponse<string>
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
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng với email này."
                });
            }

            return Ok(new ApiResponse<UserResponse>
            {
                Success = true,
                Data = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth
                }
            });
        }

        /// <summary>
        /// [GET] Lấy thông tin người dùng hiện tại (cần JWT)
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return NotFound(new ApiResponse<string> { Success = false, Message = "Không tìm thấy người dùng." });

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "None";

            return Ok(new ApiResponse<UserResponse>
            {
                Success = true,
                Data = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                }
            });
        }

        /// <summary>
        /// [POST] Quên mật khẩu → gửi email reset password
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ." });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Không tiết lộ user có tồn tại hay không
                return Ok(new { Success = true, Message = "Nếu email tồn tại, bạn sẽ nhận được hướng dẫn reset password." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);
            var callbackUrl = $"{_configuration["AppUrl"]}/reset-password?email={user.Email}&token={encodedToken}";

            await _emailService.SendConfirmationEmailAsync(user.Email, callbackUrl); // gửi email token

            return Ok(new { Success = true, Message = "Nếu email tồn tại, bạn sẽ nhận được hướng dẫn reset password." });
        }

        /// <summary>
        /// [POST] Reset mật khẩu mới
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ." });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Không tiết lộ user có tồn tại hay không
                return BadRequest(new { Success = false, Message = "Email hoặc token không hợp lệ." });
            }

            var decodedToken = System.Web.HttpUtility.UrlDecode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { Success = true, Message = "Đặt lại mật khẩu thành công!" });
            }

            return BadRequest(new
            {
                Success = false,
                Message = "Token không hợp lệ hoặc đã hết hạn.",
                Errors = result.Errors.Select(e => e.Description)
            });
        }


    }
}