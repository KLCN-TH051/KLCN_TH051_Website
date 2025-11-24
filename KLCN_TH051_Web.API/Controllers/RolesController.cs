using KLCN_TH051_Website.Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KLCN_TH051_Web.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager; 
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        // 1. LẤY TẤT CẢ ROLE + PERMISSIONS (để hiển thị checkbox)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.Roles
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();

            var result = new List<object>();

            foreach (var r in roles)
            {
                var roleEntity = await _roleManager.FindByIdAsync(r.Id.ToString());
                var claims = await _roleManager.GetClaimsAsync(roleEntity);

                // CHỈ LẤY PERMISSION
                var permissions = claims
                    .Where(c => c.Type == "Permission")
                    .Select(c => c.Value)
                    .ToList();

                result.Add(new
                {
                    Id = r.Id,
                    Name = r.Name,
                    Permissions = permissions
                });
            }

            return Ok(result);
        }

        // 2. CẬP NHẬT TOÀN BỘ QUYỀN CHO ROLE (checkbox list) ← QUAN TRỌNG NHẤT!!!
        [HttpPut("{roleName}/permissions")]
        public async Task<IActionResult> UpdatePermissions(string roleName, [FromBody] List<string> permissions)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return NotFound("Không tìm thấy role");

            // Lấy danh sách claim hiện tại
            var existingClaims = await _roleManager.GetClaimsAsync(role);
            var currentPermClaims = existingClaims.Where(c => c.Type == "Permission").ToList();

            // XÓA những quyền không còn trong danh sách mới
            foreach (var claim in currentPermClaims)
            {
                if (!permissions.Contains(claim.Value))
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
            }

            // THÊM những quyền mới
            foreach (var perm in permissions)
            {
                if (!currentPermClaims.Any(c => c.Value == perm))
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", perm));
                }
            }

            return Ok("Cập nhật quyền thành công");
        }

        // 3. (Tùy chọn) XÓA 1 QUYỀN RIÊNG LẺ
        [HttpDelete("{roleName}/permissions/{permissionValue}")]
        public async Task<IActionResult> RemovePermission(string roleName, string permissionValue)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return NotFound();

            var claim = new Claim("Permission", permissionValue);
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            return result.Succeeded ? Ok("Xóa quyền thành công") : BadRequest("Lỗi");
        }

        [HttpGet("claim-values")]
        public async Task<IActionResult> GetClaimValues()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var allClaims = new HashSet<string>();

            foreach (var role in roles)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    if (claim.Type == "Permission")
                    {
                        allClaims.Add(claim.Value);
                    }
                }
            }

            return Ok(allClaims);
        }


        // lý do khong dùng phương thức này vì đã có put rồi không dung post nữa

        //[HttpPost("{roleName}/assign-permissions")]
        //public async Task<IActionResult> AssignPermissions(string roleName, [FromBody] List<string> permissions)
        //{
        //    var role = await _roleManager.FindByNameAsync(roleName);
        //    if (role == null) return NotFound("Role không tồn tại");

        //    var existingClaims = await _roleManager.GetClaimsAsync(role);
        //    var currentPermClaims = existingClaims.Where(c => c.Type == "Permission").ToList();

        //    foreach (var perm in permissions)
        //    {
        //        if (!currentPermClaims.Any(c => c.Value == perm))
        //        {
        //            await _roleManager.AddClaimAsync(role, new Claim("Permission", perm));
        //        }
        //    }

        //    return Ok("Gán quyền thành công");
        //}
    }
}
