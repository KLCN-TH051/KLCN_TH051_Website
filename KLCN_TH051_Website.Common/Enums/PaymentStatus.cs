using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Enums
{
    public enum PaymentStatus
    {
        Failed,     // Thanh toán thất bại
        Completed,  // Thanh toán thành công
        Pending,    // Đang chờ xử lý
        Cancelled,  // Hủy giao dịch
        Refunded    // Hoàn tiền
    }
}
