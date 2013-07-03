using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover
{
    public class CustomerTurnoverDetail
    {
        public string CustId { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public string SMATurnover { get; set; }

        public string MaxTurnover { get; set; }

        public string EventDate { get; set; }

        public string LeagueName { get; set; }

        public string MatchId { get; set; }

        public string EventStatus { get; set; }

        public string PercentageUsed { get; set; }


        public string SportName { get; set; }

        public List<CustomerTurnoverDetail> CustomerList { get; set; }
    }
}
