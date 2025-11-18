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
        // Chuẩn: admin tạo giáo viên, cần biết ai tạo
        Task<ApiResponse<UserResponse>> CreateTeacherAsync(RegisterTeacherRequest model, string creatorId);
        Task<List<UserWithRoleResponse>> GetAllAccountsAsync();
        // Lấy chi tiết user theo id
        Task<UserWithRoleResponse?> GetAccountByIdAsync(int id);
        Task<ApiResponse<UserWithRoleResponse>> UpdateAccountAsync(int id, UpdateAccountRequest model);
        // Lấy danh sách tất cả giáo viên
        Task<List<UserWithRoleResponse>> GetTeachersAsync();
    }
}
