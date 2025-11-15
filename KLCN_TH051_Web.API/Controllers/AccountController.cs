using KLCN_TH051_Website.Common.DTO;
using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Extensions;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountController(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
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

            // 🔥 Decode chuẩn
            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Token không hợp lệ hoặc bị hỏng."
                });
            }

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

            if (user == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Không tìm thấy người dùng." });

            return Ok(new ApiResponse<UserResponse>
            {
                Success = true,
                Data = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    DateOfBirth = user.DateOfBirth
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

            // Luôn trả về cùng một message để tránh lộ thông tin tồn tại user
            var responseMessage = "Nếu email tồn tại, bạn sẽ nhận được hướng dẫn reset password.";

            if (user == null)
            {
                return Ok(new { Success = true, Message = responseMessage });
            }

            // Tạo token reset password
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode token và email để nhét vào URL an toàn
            var encodedToken = WebUtility.UrlEncode(token);
            var encodedEmail = WebUtility.UrlEncode(user.Email);

            // Tạo URL callback (dẫn đến trang reset password)
            var callbackUrl = $"{_configuration["AppUrl"]}/Account/ResetPasswordForm?email={encodedEmail}&token={encodedToken}";

            // Gửi email chứa link reset
            await _emailService.SendConfirmationEmailAsync(user.Email, callbackUrl);

            return Ok(new { Success = true, Message = responseMessage });
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
                return BadRequest(new { Success = false, Message = "Email hoặc token không hợp lệ." });
            }

            // KHÔNG DECODE LẠI - token đã được decode bởi browser
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

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
        /// <summary>
        /// 
        /// [POST] Admin tạo tài khoản giáo viên
        /// 
        /// </summary>
        [HttpPost("create-teacher")]
        [Authorize(Roles = "Admin")]  // chỉ admin mới được tạo
        public async Task<IActionResult> CreateTeacher([FromBody] RegisterTeacherRequest model)
        {
            // Lấy creatorId từ token
            var creatorId = User.FindFirst("id")?.Value
                            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (creatorId == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Không xác định được tài khoản tạo."
                });
            }

            var result = await _accountService.CreateTeacherAsync(model, creatorId);

            // Trả ra kết quả chuẩn REST
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}