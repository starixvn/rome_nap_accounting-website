using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.OTP.Requests
{
    public class VerifyCommand
    {
        public string UserName { get; set; }

        public int CustId { get; set; }
        
        public string OTP { get; set; }
    }
}
