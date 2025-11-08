using Google.Apis.Auth;
using KLCN_TH051_Web.Repositories.Data;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Helpers;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Web.Services.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly string _googleClientId;

        public GoogleAuthService(UserManager<ApplicationUser> userManager, AppDbContext context, JwtHelper jwtHelper, IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _jwtHelper = jwtHelper;
            // Lấy Google ClientId từ biến môi trường nếu có, nếu không fallback sang config
            _googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")
                              ?? config["Authentication:Google:ClientId"]!;
        }

        public async Task<string?> LoginWithGoogleAsync(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _googleClientId }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            // 1️⃣ Tìm user theo Google login trong bảng UserLogins
            var login = await _context.UserLogins
                .FirstOrDefaultAsync(l => l.LoginProvider == "Google" && l.ProviderKey == payload.Subject);

            ApplicationUser user = null;

            if (login != null)
            {
                user = await _userManager.FindByIdAsync(login.UserId.ToString());
            }
            else
            {
                // 2️⃣ Nếu chưa có user với Google login, kiểm tra theo email
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    // 3️⃣ Nếu email cũng chưa có, tạo user mới
                    user = new ApplicationUser
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        FullName = payload.Name,
                        EmailConfirmed = true,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };
                    await _userManager.CreateAsync(user);

                    // 4️⃣ Phân role mặc định
                    await _userManager.AddToRoleAsync(user, "Student");
                }

                // 5️⃣ Thêm Google login vào UserLogins với IBaseEntity
                var userLogin = new ApplicationUserLogin
                {
                    UserId = user.Id,
                    LoginProvider = "Google",
                    ProviderKey = payload.Subject,
                    ProviderDisplayName = "Google",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };
                _context.UserLogins.Add(userLogin);
                await _context.SaveChangesAsync();
            }

            // 6️⃣ Lấy role và sinh JWT
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHelper.GenerateToken(user, roles);
            return token;
        }


        //public async Task<string?> LoginWithGoogleAsync(string idToken)
        //{
        //    var settings = new GoogleJsonWebSignature.ValidationSettings()
        //    {
        //        Audience = new List<string> { _googleClientId }
        //    };
        //    var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

        //    var user = await _userManager.FindByEmailAsync(payload.Email);
        //    if (user == null)
        //    {
        //        user = new ApplicationUser
        //        {
        //            Email = payload.Email,
        //            UserName = payload.Email,
        //            FullName = payload.Name,
        //            EmailConfirmed = true,
        //            IsActive = true,
        //            CreatedDate = DateTime.Now, 
        //            IsDeleted = false        
        //        };
        //        await _userManager.CreateAsync(user);

        //        // PHÂN ROLE "Student" CHO USER MỚI
        //        await _userManager.AddToRoleAsync(user, "Student");
        //    }

        //    var roles = await _userManager.GetRolesAsync(user);
        //    var token = _jwtHelper.GenerateToken(user, roles);
        //    return token;
        //}
    }
}
