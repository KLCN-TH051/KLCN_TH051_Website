using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateQuizAttemptRequest
    {
        public int Score { get; set; }
        public bool Passed { get; set; }
        public string Answers { get; set; } // Cập nhật câu trả lời nếu cần
    }
}
