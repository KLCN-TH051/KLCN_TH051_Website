using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdatePaymentStatusRequest
    {
        public PaymentStatus Status { get; set; } // Success, Failed, Pending
        public string? TransactionId { get; set; }
    }
}
