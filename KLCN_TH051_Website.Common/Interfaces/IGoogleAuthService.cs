using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<string> LoginWithGoogleAsync(string idToken);
    }
}
