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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly JwtHelper _jwtHelper; 

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IEmailService emailService,
            JwtHelper jwtHelper) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            var token = _jwtHelper.GenerateToken(user, roles, _roleManager);


            response.Success = true;
            response.Message = "Đăng nhập thành công.";
            response.Data = token;

            return response;
        }

        public async Task<ApiResponse<UserResponse>> CreateTeacherAsync(RegisterTeacherRequest model, string creatorId)
        {
            var response = new ApiResponse<UserResponse>();

            // 1. Kiểm tra email tồn tại
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return response.Failed("Email đã được sử dụng.");
            }

            // 2. Khởi tạo user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                CreatedBy = creatorId,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            // 3. Tạo user
            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                response.Errors = createResult.Errors.Select(e => e.Description);
                return response.Failed("Tạo giáo viên thất bại.");
            }

            // 4. Gán role Teacher
            var roleResult = await _userManager.AddToRoleAsync(user, "Teacher");
            if (!roleResult.Succeeded)
            {
                response.Errors = roleResult.Errors.Select(e => e.Description);
                return response.Failed("Gán vai trò thất bại.");
            }

            // 5. Trả về UserResponse
            response.Data = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };

            return response.Successed("Tạo giáo viên thành công!");
        }

        public async Task<List<UserWithRoleResponse>> GetAllAccountsAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserWithRoleResponse>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(new UserWithRoleResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = roles.FirstOrDefault() ?? "",
                    Avatar = u.Avatar,
                    IsActive = u.LockoutEnd == null || u.LockoutEnd <= DateTime.Now,
                    CreatedDate = u.CreatedDate
                });
            }

            return result;
        }
        public async Task<UserWithRoleResponse?> GetAccountByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserWithRoleResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "User",
                Avatar = user.Avatar,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };
        }

        public async Task<ApiResponse<UserWithRoleResponse>> UpdateAccountAsync(int id, UpdateAccountRequest model)
        {
            // Lấy user theo id
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiResponse<UserWithRoleResponse> { Success = false, Message = "Tài khoản không tồn tại" };

            // Cập nhật thông tin cơ bản
            if (!string.IsNullOrEmpty(model.FullName)) user.FullName = model.FullName;
            if (!string.IsNullOrEmpty(model.Email)) user.Email = model.Email;
            if (!string.IsNullOrEmpty(model.Avatar)) user.Avatar = model.Avatar;

            // Cập nhật role
            if (!string.IsNullOrEmpty(model.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles); // Xóa role cũ
                await _userManager.AddToRoleAsync(user, model.Role);          // Thêm role mới
            }

            // Cập nhật trạng thái hoạt động
            if (model.IsActive.HasValue)
            {
                if (model.IsActive.Value)
                    await _userManager.SetLockoutEndDateAsync(user, null); // Mở khóa
                else
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue); // Khóa
            }

            // Lưu các thay đổi
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return new ApiResponse<UserWithRoleResponse>
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };

            // Trả về thông tin đã cập nhật
            return new ApiResponse<UserWithRoleResponse>
            {
                Success = true,
                Data = new UserWithRoleResponse
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User",
                    IsActive = user.LockoutEnd == null || user.LockoutEnd <= DateTime.Now,
                    Avatar = user.Avatar
                }
            };
        }
        // -------------------------------
        // Lấy danh sách giáo viên
        // -------------------------------
        public async Task<List<UserWithRoleResponse>> GetTeachersAsync()
        {
            // Lấy tất cả user
            var users = await _userManager.Users.ToListAsync();

            var teachers = new List<UserWithRoleResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Teacher"))
                {
                    teachers.Add(new UserWithRoleResponse
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        Role = "Teacher",
                        IsActive = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.Now,
                        Avatar = user.Avatar
                    });
                }
            }

            return teachers;
        }

    }
}
