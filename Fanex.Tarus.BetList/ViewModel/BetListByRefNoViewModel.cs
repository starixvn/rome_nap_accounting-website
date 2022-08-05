using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Tarus.BetList.Business;
using Fanex.Tarus.BetList.Models;

namespace Fanex.Tarus.BetList.ViewModel
{
    public class BetListByRefNoViewModel
    {
        public BetListByRefNoViewModel(IEnumerable<SiteModel> allSites)
        {
            DepositSites = allSites.Where(i => i.IsDeposit).OrderBy(i => i.Name);
            CreditSites = allSites.Where(i => !i.IsDeposit).OrderBy(i => i.Name);
        }

        public DateTime BalanceUpDay => ReportService.BalanceUp.BalanceUpDay;

        public IEnumerable<SiteModel> DepositSites { get; }

        public IEnumerable<SiteModel> CreditSites { get; }
    }
}
