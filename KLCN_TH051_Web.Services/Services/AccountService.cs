using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Helpers;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KLCN_TH051_Web.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly JwtHelper _jwtHelper; 

        public AccountService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IEmailService emailService,
            JwtHelper jwtHelper) 
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _jwtHelper = jwtHelper;
        }

        public async Task<ApiResponse<UserResponse>> RegisterAsync(RegisterRequest model)
        {
            var response = new ApiResponse<UserResponse>();

            // Kiểm tra email tồn tại
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                response.Success = false;
                response.Message = "Email đã được sử dụng.";
                return response;
            }

            // Không cần kiểm tra số điện thoại nữa

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                CreatedDate = DateTime.Now,
                CreatedBy = "self",
                IsActive = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = "Đăng ký thất bại.";
                response.Errors = result.Errors.Select(e => e.Description);
                return response;
            }

            // Gán vai trò "Student"
            var addRoleResult = await _userManager.AddToRoleAsync(user, "Student");
            if (!addRoleResult.Succeeded)
            {
                response.Success = false;
                response.Message = "Gán vai trò thất bại.";
                response.Errors = addRoleResult.Errors.Select(e => e.Description);
                return response;
            }

            // Gửi email xác thực
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var clientUrl = _configuration["AppUrl"];
            var callbackUrl = $"{clientUrl}/confirm-email?userId={user.Id}&token={encodedToken}";

            await _emailService.SendConfirmationEmailAsync(user.Email, callbackUrl);

            response.Success = true;
            response.Message = "Đăng ký thành công! Vui lòng kiểm tra email để xác thực tài khoản.";
            response.Data = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth
            };

            return response;
        }
        public async Task<ApiResponse<string>> LoginAsync(LoginRequest model)
        {
            var response = new ApiResponse<string>();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                response.Success = false;
                response.Message = "Sai email hoặc mật khẩu.";
                return response;
            }

            if (!user.IsActive)
            {
                response.Success = false;
                response.Message = "Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email.";
                return response;
            }

            // Cập nhật lần đăng nhập cuối
            user.LastUpdatedDate = DateTime.Now;
            user.LastUpdatedBy = user.Email;
            await _userManager.UpdateAsync(user);

            // Lấy roles
            var roles = await _userManager.GetRolesAsync(user);

            // ✅ Gọi JwtHelper để sinh token
            var token = _jwtHelper.GenerateToken(user, roles);

            response.Success = true;
            response.Message = "Đăng nhập thành công.";
            response.Data = token;

            return response;
        }
    }
}
