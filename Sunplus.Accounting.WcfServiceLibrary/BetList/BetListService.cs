using System.Collections.Generic;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.BetList;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.WcfServiceLibrary.BetList
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BetListService : IBetListService
    {
        public List<BetMonitor> GetLatestBABetList(int totalRow, int amount, System.Int64 lastTransId)
        {
            var betlistService = CommonBootStrapper.Locator.GetInstance<IBetListManager>();
            var betList = betlistService.GetLatestBABetList(totalRow, amount, lastTransId);
            return betList;
        }

        public BABetList GetBABetList(int pagesize, int pageindex, string fromdate, string todate, string refno, string username, int currency, int bettype, int fromStake, int toStake, int excludeTest)
        {
            var betlistService = CommonBootStrapper.Locator.GetInstance<IBetListManager>();
            var betList = betlistService.GetBABetList(pagesize, pageindex, fromdate, todate, refno, username, currency, bettype, fromStake, toStake, excludeTest);
            return betList;
        }

        public List<BetListDetail> GetMixParlayTickets(string refNo)
        {
            var betlistService = CommonBootStrapper.Locator.GetInstance<IBetListManager>();
            var betList = betlistService.GetMixParlayTickets(refNo);
            return betList;
        }

        public LicenseeDTO GetDatetimeOption(string dateOption)
        {
            var betlistService = CommonBootStrapper.Locator.GetInstance<IBetListManager>();
            var dateTime = betlistService.GetDatetimeOption(dateOption);
            return dateTime;
        }

        public List<Bettype> GetBettype()
        {
            var betlistService = CommonBootStrapper.Locator.GetInstance<IBetListManager>();
            return betlistService.GetBettype();
        }

        public List<CustDetail> GetCurrency()
        {
            var listCurrency = new List<CustDetail>();
            var betlistService = CommonBootStrapper.Locator.GetInstance<IBetListManager>();
            listCurrency = betlistService.GetCurrency();
            return listCurrency;
        }

        #region IBetListService Members

        public CombinationDetailMP GetCombinationDetailMPTickets(string refNo)
        {
            var betlistDetail = new CombinationDetailMP();
            var betlistService = CommonBootStrapper.Locator.GetInstance<IBetListManager>();
            betlistDetail = betlistService.GetCombinationDetailMP(refNo);
            return betlistDetail;
        }

        #endregion IBetListService Members

        #region IBetListService Members

        public SystemParlayDetail GetBetListSystemParlayDetailByTransId(string transId, int custId)
        {
            var betlistService = CommonBootStrapper.Locator.GetInstance<IBetListManager>();
            return betlistService.GetBetListSystemParlayDetailByTransId(transId, custId);
        }

        #endregion IBetListService Members
    }
}