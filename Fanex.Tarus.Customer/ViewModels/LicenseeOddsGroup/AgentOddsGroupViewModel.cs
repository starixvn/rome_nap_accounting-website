using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeOddsGroup
{
    public class AgentOddsGroupEntity
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string OperatorId { get; set; }

        public string OddsGroup { get; set; }

        public bool IsActive { get; set; }
    }
}
