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

            // === 1. TẠO ROLES ===
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

            // === 2. TẠO ADMIN USER ===
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

            // === 3. THÊM PHÂN QUYỀN (PERMISSIONS) CHO ADMIN ===
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                var adminPermissions = new[]
                {
                    "Subject.Create", "Subject.Edit", "Subject.Delete", "Subject.View",
                    "Course.Create", "Course.Edit", "Course.Delete", "Course.View",
                    "User.Create", "User.Edit", "User.Delete", "User.View",
                    "Role.View", "Role.Manage", "Permission.Manage",
                    "Dashboard.View", "Report.View"
                };

                var existingClaims = await roleManager.GetClaimsAsync(adminRole); // ← LẤY TẤT CẢ CLAIM HIỆN TẠI

                foreach (var perm in adminPermissions)
                {
                    if (!existingClaims.Any(c => c.Type == "Permission" && c.Value == perm))
                    {
                        await roleManager.AddClaimAsync(adminRole, new Claim("Permission", perm));
                    }
                }
            }

            // === 4. GÁN QUYỀN CHO TEACHER ===
            var teacherRole = await roleManager.FindByNameAsync("Teacher");
            if (teacherRole != null)
            {
                var teacherPerms = new[] { "Subject.View", "Course.View", "Dashboard.View" };
                var existing = await roleManager.GetClaimsAsync(teacherRole);

                foreach (var perm in teacherPerms)
                {
                    if (!existing.Any(c => c.Type == "Permission" && c.Value == perm))
                    {
                        await roleManager.AddClaimAsync(teacherRole, new Claim("Permission", perm));
                    }
                }
            }

            // === 5. GÁN QUYỀN CHO STUDENT ===
            var studentRole = await roleManager.FindByNameAsync("Student");
            if (studentRole != null)
            {
                var existing = await roleManager.GetClaimsAsync(studentRole);
                if (!existing.Any(c => c.Type == "Permission" && c.Value == "Course.View"))
                {
                    await roleManager.AddClaimAsync(studentRole, new Claim("Permission", "Course.View"));
                }
            }
        }
    }
}
