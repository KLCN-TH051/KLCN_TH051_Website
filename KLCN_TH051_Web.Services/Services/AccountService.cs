using KLCN_TH051_Website.Common.DTO;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Interfaces;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
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

namespace KLCN_TH051_Web.Services.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AccountService(
            UserManager<User> userManager,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            // Kiểm tra email đã tồn tại
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email đã được sử dụng." });
            }

            // Kiểm tra số điện thoại đã tồn tại
            if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Số điện thoại đã được sử dụng." });
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.Now,
                CreatedBy = "self",
                IsActive = false // Chưa active cho đến khi xác thực email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Tạo token xác thực email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(token);
                var callbackUrl = $"{_configuration["AppUrl"]}/api/account/confirm-email?userId={user.Id}&token={encodedToken}";

                // Gửi email xác nhận
                await _emailService.SendConfirmationEmailAsync(user.Email, callbackUrl);
            }

            return result;
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new UnauthorizedAccessException("Sai email hoặc mật khẩu.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Tài khoản chưa được kích hoạt. Vui lòng kiểm tra email.");
            }

            // Cập nhật lần đăng nhập cuối
            user.LastUpdatedDate = DateTime.Now;
            user.LastUpdatedBy = user.Email;
            await _userManager.UpdateAsync(user);

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? user.Email!)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
