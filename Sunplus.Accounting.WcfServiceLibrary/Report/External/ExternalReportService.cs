using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report.External;
using Sunplus.Accounting.Entities.Report.External;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.External
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExternalReportService:IExternalReportService
    {
        public ExternalWLDailyBO SelWinLossDailyByCom(string winlostdate)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<IExternalReport>();
            return ex.SelWinLossDailyByCom(winlostdate);
        }


        public ExternalWLDailyBO SelWinLossDailyBySuper(string winlostdate, int custid)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<IExternalReport>();
            return ex.SelWinLossDailyBySuper(winlostdate, custid);
        }


        public ExternalWLDailyBO SelWinLossDailyByMaster(string winlostdate, int custid)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<IExternalReport>();
            return ex.SelWinLossDailyByMaster(winlostdate, custid);
        }


        public ExternalWLDailyBO SelWinLossDailyByAgent(string winlostdate, int custid)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<IExternalReport>();
            return ex.SelWinLossDailyByAgent(winlostdate, custid);
        }


        public ExternalWLDetail SelWinLossDetailByCom(string param, Sorting sorting)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<IExternalReport>();
            return ex.SelWinLossDetailByCom(param, sorting);
        }


        public ExternalWLDetail SelWinLossDetailBySuper(int custid, string param, Sorting sorting, int pageIndex, int pageSize)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<IExternalReport>();
            return ex.SelWinLossDetailBySuper(custid, param, sorting, pageIndex, pageSize);
        }


        public ExternalWLDetail SelWinLossDetailByMaster(int custid, string param, Sorting sorting, int pageIndex, int pageSize)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<IExternalReport>();
            return ex.SelWinLossDetailByMaster(custid, param, sorting, pageIndex, pageSize);
        }

        public ExternalWLDetail SelWinLossDetailByAgent(int custid, string param, Sorting sorting, int pageIndex, int pageSize)
        {
            var ex = CommonBootStrapper.Locator.GetInstance<IExternalReport>();
            return ex.SelWinLossDetailByAgent(custid, param, sorting, pageIndex, pageSize);
        }
    }
}
