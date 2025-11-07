using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
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
            if (!ModelState.IsValid || string.IsNullOrEmpty(request?.IdToken))
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "IdToken is required."
                });
            }

            try
            {
                var token = await _googleAuthService.LoginWithGoogleAsync(request.IdToken);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Google login failed."
                    });
                }

                // TRẢ VỀ ĐÚNG ĐỊNH DẠNG NHƯ /api/Account/login
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Data = token
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Google token không hợp lệ."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Lỗi server: " + ex.Message
                });
            }
        }
    }
}
