using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report.Poker;
using Sunplus.Accounting.Entities.Report.Poker;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.Poker
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PokerService:IPokerService
    {
        public PokerMainBO SelWinlostP2P(string FromDate, string ToDate)
        {
            var poker = CommonBootStrapper.Locator.GetInstance<IPokerReport>();
            return poker.SelWinlostP2P(FromDate, ToDate);
        }
        public PokerMainBO SelWinlostP2PDetail(int currency, string wldate)
        {
            var poker = CommonBootStrapper.Locator.GetInstance<IPokerReport>();
            return poker.SelWinlostP2PDetail(currency, wldate);
        }

    }

}
