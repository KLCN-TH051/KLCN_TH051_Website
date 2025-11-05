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
        private readonly string _googleClientId;

        public GoogleAuthService(UserManager<User> userManager, JwtHelper jwtHelper, IConfiguration config)
        {
            _userManager = userManager;
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

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    FullName = payload.Name,
                    EmailConfirmed = true,
                    IsActive = true
                };
                await _userManager.CreateAsync(user);

                // PHÂN ROLE "Student" CHO USER MỚI
                await _userManager.AddToRoleAsync(user, "Student");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHelper.GenerateToken(user, roles);
            return token;
        }
    }
}
