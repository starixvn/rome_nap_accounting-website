using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.OTP.Requests
{
    public class VerifyRequest
    {
        public string UserName { get; set; }

        public int CustId { get; set; }
    }
}
