using System;
using Fanex.Tarus.BetList.Business;

namespace Fanex.Tarus.BetList.ViewModel
{
    public class BetListByRefNoViewModel
    {
        public BetListByRefNoViewModel()
        {            
        }

        public DateTime BalanceUpDay => ReportService.BalanceUp.BalanceUpDay;
    }
}
