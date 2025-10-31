using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string email, string callbackUrl);
    }
}
