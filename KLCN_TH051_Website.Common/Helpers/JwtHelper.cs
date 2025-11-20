using KLCN_TH051_Website.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(ApplicationUser user, IList<string> roles, RoleManager<ApplicationRole> roleManager = null)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FullName ?? user.Email!)
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                // Chỉ thêm Permission khi có RoleManager (token nội bộ)
                if (roleManager != null)
                {
                    var roleObj = roleManager.FindByNameAsync(role).Result;
                    if (roleObj != null)
                    {
                        var roleClaims = roleManager.GetClaimsAsync(roleObj).Result;
                        foreach (var rc in roleClaims.Where(c => c.Type == "Permission"))
                        {
                            claims.Add(rc);
                        }
                    }
                }
            }

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
