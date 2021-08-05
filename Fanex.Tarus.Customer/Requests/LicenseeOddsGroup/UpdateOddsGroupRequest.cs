using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.Customer.Requests.LicenseeOddsGroup
{
    public class UpdateOddsGroupRequest
    {
        public int CustId { get; set; }

        public string OddsGroup { get; set; }

        public bool IsActive { get; set; }

        public bool ApplyExistingMember { get; set; }
    }
}
