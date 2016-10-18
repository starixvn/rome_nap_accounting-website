using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Web.Models.Customer.Balance
{
    [Serializable]
    public class CustomerBalancePageStatus
    {
        public int SuperId { get; set; }

        public string SuperName { get; set; }

        public int MasterId { get; set; }

        public string MasterName { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public int CurrentRoleId { get; set; }
    }
}
