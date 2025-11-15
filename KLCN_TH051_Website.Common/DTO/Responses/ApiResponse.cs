using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        // Helper: Trả về thất bại
        public ApiResponse<T> Failed(string message)
        {
            Success = false;
            Message = message;
            return this;
        }

        // Helper: Trả về thành công
        public ApiResponse<T> Successed(string message)
        {
            Success = true;
            Message = message;
            return this;
        }
    }
}
