using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleAuthController(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.IdToken))
            {
                return BadRequest(new { success = false, message = "IdToken is required." });
            }

            var token = await _googleAuthService.LoginWithGoogleAsync(request.IdToken);
            if (token == null)
            {
                return Unauthorized(new { success = false, message = "Google login failed." });
            }

            return Ok(new { success = true, token = token });
        }
    }
}
