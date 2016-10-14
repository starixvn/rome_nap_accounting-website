using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Web.Models.Customer.Balance
{

    public class CustomerBalanceViewModel
    {
        public int TotalRows { get; set; }

        public List<CustomerBalance> CustomerBalance { get; set; }

        public CustomerBalanceViewModel()
        {
            this.CustomerBalance = new List<Balance.CustomerBalance>();
        }
    }


    public class CustomerBalance
    {
        public int RowNum { get; set; }

        public string UserName { get; set; }

        public int CustId { get; set; }

        public bool Closed { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Outstanding { get; set; }

        public double Balance { get; set; }

        public double YesterdayBalance { get; set; }

        public double Cash { get; set; }

        public double YesterdayCash { get; set; }

        public double GivenCredit { get; set; }

        public double AvailableCredit { get; set; }

        public double Recommend { get; set; }

        public double MRecommend { get; set; }

        public bool SyncCasino { get; set; }
    }
}
