using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.Customer.Requests.CN88
{
    public class AddNewHeadlineRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public int Currency { get; set; }

        public decimal SportbooksPossess { get; set; }

        public decimal NonSportsPossess { get; set; }

        public string Code { get; set; }
    }
}
