using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }

        public PaymentResponse(Payment payment)
        {
            Id = payment.Id;
            StudentId = payment.StudentId;
            CourseId = payment.CourseId;
            Amount = payment.Amount;
            PaymentMethod = payment.PaymentMethod;
            Status = payment.Status;
            PaymentDate = payment.PaymentDate;
            TransactionId = payment.TransactionId;
        }
    }
}
