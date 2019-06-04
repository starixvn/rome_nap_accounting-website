using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.Customer.Entities.CN88
{
    public class CreditRangeModel
    {
        public CreditRangeModel()
        {
        }

        public CreditRangeModel(
            decimal givenCredit,
            decimal usedCredit,
            decimal minCredit,
            decimal maxCredit)
        {
            GivenCredit = givenCredit;
            UsedCredit = usedCredit;
            MinCredit = minCredit;
            MaxCredit = maxCredit;
        }

        public decimal GivenCredit { get; }

        public decimal UsedCredit { get; }

        public decimal MinCredit { get; }

        public decimal MaxCredit { get; }
    }
}
