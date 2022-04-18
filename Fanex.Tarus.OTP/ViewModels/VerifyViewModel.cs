using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.OTP.ViewModels
{
    public class VerifyViewModel
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string ErrorMessage { get; set; }
    }
}
