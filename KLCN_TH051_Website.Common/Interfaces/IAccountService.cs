using KLCN_TH051_Website.Common.DTO;
using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<UserResponse>> RegisterAsync(RegisterRequest model);
        Task<ApiResponse<string>> LoginAsync(LoginRequest model);
        // Mới: Admin tạo giáo viên
        Task<ApiResponse<UserResponse>> CreateTeacherAsync(RegisterTeacherRequest model);
    }
}
