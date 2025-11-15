using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Payment: BaseEntity
    {
        public int Id { get; set; }

        // Học viên thanh toán
        public int StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        // Khóa học được thanh toán
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Số tiền thanh toán
        public decimal Amount { get; set; }

        // Phương thức thanh toán: Card, Paypal, VNPay...
        public string PaymentMethod { get; set; }

        // Trạng thái thanh toán
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // Ngày thanh toán
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        // Mã giao dịch từ cổng thanh toán
        public string? TransactionId { get; set; }
    }
}
