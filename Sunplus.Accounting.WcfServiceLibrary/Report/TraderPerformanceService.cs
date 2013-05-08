using System.Collections;
using System.Linq;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.Report.Other;
using Sunplus.Accounting.Entities.Report;
using System.Collections.Generic;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TraderPerformanceService : ITraderPerformanceService
    {
        public TraderPerformanceDTO GetTraderPerformance(string fromDate, string toDate, int isSoccer)
        {
            var traderPerformance = new TraderPerfomanceManager();
            var result = traderPerformance.GetTraderPerformance(fromDate, toDate, isSoccer);
            return result;
        }

        public void TraderPerformanceExport2Excel(string fromDate, string toDate, int isSoccer)
        {
            var traderPerformance = new TraderPerfomanceManager();
            traderPerformance.TraderPerformanceExport2Excel(fromDate, toDate, isSoccer);
        }

        public TraderPerformanceDetailDTO GetTraderPerformanceReportDetail(string fromDate, string toDate, int isSoccer, int pageIndex, int pageSize, bool getNewData)
        {
            #region get data
            var traderPerformanceDetail = new TraderPerfomanceManager();
            var traderResult = new TraderPerformanceDetailDTO();
            traderResult = traderPerformanceDetail.GetTraderPerformanceDetail(fromDate, toDate, isSoccer, getNewData);
            #endregion

            #region assign and return
            if (traderResult != null && traderResult.rows != null)
            {
                var traderPaging = new TraderPerformanceDetailDTO();
                traderPaging.pageindex = pageIndex;
                traderPaging.total = traderResult.rows != null ? traderResult.rows.Count() : 0;               
                var query = (from trader in traderResult.rows select trader).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                traderPaging.rows = query.ToList();
                traderPaging.footer = traderResult.footer;
                return traderPaging;
            }
            else
                return traderResult;
            #endregion
        }

        public void TraderPerformanceDetailExport2Excel(string fromDate, string toDate, int isSoccer)
        {
            var traderPerformance = new TraderPerfomanceManager();
            traderPerformance.TraderPerformanceDetailExport2Excel(fromDate, toDate, isSoccer);
        }
    }
}