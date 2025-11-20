using KLCN_TH051_Website.Common.Configurations;
using KLCN_TH051_Website.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Helpers
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Tạo Roles nếu chưa có
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

            // 2. Tạo Admin user nếu chưa có
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Super Admin",
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System"
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 3. Reset quyền cho Admin
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                // Xóa hết claim cũ
                var existingClaims = await roleManager.GetClaimsAsync(adminRole);
                foreach (var claim in existingClaims.Where(c => c.Type == "Permission"))
                {
                    await roleManager.RemoveClaimAsync(adminRole, claim);
                }

                // Gán toàn bộ quyền mới từ Permissions.All
                foreach (var perm in Permissions.All)
                {
                    await roleManager.AddClaimAsync(adminRole, new Claim("Permission", perm));
                }
            }

            // chi them khong xoa 
            //var adminRole = await roleManager.FindByNameAsync("Admin");
            //if (adminRole != null)
            //{
            //    var existingClaims = await roleManager.GetClaimsAsync(adminRole);

            //    foreach (var perm in Permissions.All)
            //    {
            //        // Chỉ thêm những quyền chưa có
            //        if (!existingClaims.Any(c => c.Type == "Permission" && c.Value == perm))
            //        {
            //            await roleManager.AddClaimAsync(adminRole, new Claim("Permission", perm));
            //        }
            //    }
            //}
        }
    }
}
