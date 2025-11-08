using KLCN_TH051_Website.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Web.Repositories.Data
{
    public class AppDbContext : IdentityDbContext<
        ApplicationUser,         // User
        ApplicationRole,         // Role
        int,                     // Key type
        ApplicationUserClaim,    // UserClaim
        ApplicationUserRole,     // UserRole
        ApplicationUserLogin,    // UserLogin
        ApplicationRoleClaim,    // RoleClaim
        ApplicationUserToken     // UserToken
    >
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<IBaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedDate = DateTime.Now;
                if (entry.State == EntityState.Modified)
                    entry.Entity.LastUpdatedDate = DateTime.Now;
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedTime = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tùy chỉnh tên bảng
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            builder.Entity<ApplicationUserClaim>().ToTable("UserClaims");
            builder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims");
            builder.Entity<ApplicationUserLogin>().ToTable("UserLogins");
            builder.Entity<ApplicationUserToken>().ToTable("UserTokens");

        }
    }
}
