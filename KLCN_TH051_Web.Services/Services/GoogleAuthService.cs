using Google.Apis.Auth;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Helpers;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public GoogleAuthService(UserManager<User> userManager, JwtHelper jwtHelper, IConfiguration configuration)
        {
            _userManager = userManager;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        public async Task<string> LoginWithGoogleAsync(string idToken)
        {
            // ✅ Xác thực token Google
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            });

            // 🔍 Tìm user trong DB
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    FullName = payload.Name,
                    EmailConfirmed = true,
                    IsActive = true
                };
                await _userManager.CreateAsync(user);
            }

            // 🔑 Sinh JWT token
            var roles = await _userManager.GetRolesAsync(user);
            return _jwtHelper.GenerateToken(user, roles);
        }
    }
}
