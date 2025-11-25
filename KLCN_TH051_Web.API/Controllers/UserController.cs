using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IAccountService _accountService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IAccountService accountService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _accountService = accountService;
        }

        /// <summary>
        /// Tạo tài khoản mới
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "User.Create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email đã tồn tại" });

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Avatar = model.Avatar,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Gán role nếu có
            if (!string.IsNullOrEmpty(model.Role))
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = model.Role });
                }
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var response = new UserWithRoleResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "",
                Avatar = user.Avatar,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };

            return Ok(response);
        }

        /// <summary>
        /// Lấy tất cả tài khoản
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "User.ViewAll")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserWithRoleResponse>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(new UserWithRoleResponse
                {
                    Id = u.Id,
                    FullName = u.FullName ?? "",
                    Email = u.Email ?? "",
                    Role = roles.FirstOrDefault() ?? "",
                    Avatar = u.Avatar,
                    IsActive = u.IsActive,
                    CreatedDate = u.CreatedDate
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Lấy chi tiết tài khoản theo Id
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Policy = "User.View")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Tài khoản không tồn tại" });

            var roles = await _userManager.GetRolesAsync(user);
            var response = new UserWithRoleResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "",
                Avatar = user.Avatar,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };

            return Ok(response);
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Policy = "User.Edit")]
        public async Task<IActionResult> UpdateAccount(string id, [FromBody] UpdateAccountRequest model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Tài khoản không tồn tại" });

            // -----------------------
            // CẬP NHẬT THÔNG TIN CƠ BẢN
            // -----------------------
            user.FullName = model.FullName ?? user.FullName;
            user.Avatar = model.Avatar ?? user.Avatar;
            user.IsActive = model.IsActive ?? user.IsActive;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return BadRequest(updateResult.Errors);

            // -----------------------
            // 👉 CẬP NHẬT MẬT KHẨU NẾU CÓ
            // -----------------------
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                // Xóa mật khẩu cũ
                var removePass = await _userManager.RemovePasswordAsync(user);
                if (!removePass.Succeeded)
                    return BadRequest(new { message = "Không thể xóa mật khẩu cũ" });

                // Thêm mật khẩu mới
                var addPass = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!addPass.Succeeded)
                    return BadRequest(addPass.Errors);
            }

            // -----------------------
            // CẬP NHẬT ROLE
            // -----------------------
            if (!string.IsNullOrEmpty(model.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Nếu role mới khác role cũ → thay thế
                if (!currentRoles.Contains(model.Role))
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new ApplicationRole { Name = model.Role });
                    }

                    await _userManager.AddToRoleAsync(user, model.Role);
                }
            }

            // Lấy role mới
            var updatedRoles = await _userManager.GetRolesAsync(user);

            var response = new UserWithRoleResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = updatedRoles.FirstOrDefault() ?? "",
                Avatar = user.Avatar,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };

            return Ok(response);
        }


        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "User.Delete")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Tài khoản không tồn tại" });

            // Lấy roles của user
            var roles = await _userManager.GetRolesAsync(user);

            // Nếu có role Admin → không cho xóa
            if (roles.Contains("Admin"))
            {
                return BadRequest(new { message = "Không thể xóa tài khoản Admin" });
            }

            await _userManager.DeleteAsync(user);
            return Ok(new { message = "Tài khoản đã xóa" });
        }

        /// <summary>
        /// Lấy danh sách giáo viên
        /// </summary>
        [HttpGet("teachers")]
        [Authorize(Policy = "User.View")]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = await _accountService.GetTeachersAsync();
            return Ok(teachers);
        }
    }
}
