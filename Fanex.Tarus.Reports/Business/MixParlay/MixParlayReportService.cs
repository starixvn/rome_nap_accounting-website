using System;
using Fanex.Tarus.Reports.DataAccess.MixParlay;
using Fanex.Tarus.Reports.Entities.MixParlay;
using Sunplus.Accounting.Core.Common;

namespace Fanex.Tarus.Reports.Business.MixParlay
{
    public partial class MixParlayReportService : IMixParlayReportService
    {
        #region IFields

        //
        // GET: /MixParlayReportAccess/
        private readonly IMixParlayReportAccess _mixParlayReportAccess;
        private readonly string _notAvailableString = Fanex.Tarus.Reports.Resource.Language.NotAvailableString;
        private string _advanceMixParlayReport = string.Empty;
        private string _normalMixParlayReport = string.Empty;

        #endregion IFields
        #region IConstructions

        public MixParlayReportService()
            : this(null)
        {
        }

        public MixParlayReportService(IMixParlayReportAccess mixParlayReportAccess)
        {
            _mixParlayReportAccess = mixParlayReportAccess ?? new MixParlayReportAccess();
            _advanceMixParlayReport = "AdvanceMixParlayReport" + AppSession.Username;
            _normalMixParlayReport = "NormalMixParlayReport" + AppSession.Username;
        }

        #endregion IConstructions
        #region IMethods

        private decimal CalculateMargin(ReportDetail reportDetail)
        {
            decimal result = 0;
            if (reportDetail != null && reportDetail.Turnover != 0)
            {
                result = (reportDetail.LineTotal / reportDetail.Turnover) * 100;
            }
            return result;
        }


        #endregion IMethods


        public DailyParlayWinLoss SelDailyParlayWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW)
        {
            DailyParlayWinLossService parlay = new DailyParlayWinLossService();
            return parlay.SelDailyParlayWinLoss(fromDate, toDate, isViewDW);
        }

        public DailyParlayWinLoss SelAdvanceParlayWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW)
        {
            DailyParlayWinLossService parlay = new DailyParlayWinLossService();
            return parlay.SelAdvanceParalyWinLoss(fromDate, toDate, isViewDW); 
        }


        public void ExportExcelDailyParlayWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW)
        {
            DailyParlayWinLossService parlay = new DailyParlayWinLossService();
            parlay.ExportExcelDailyParlayWinLoss(fromDate, toDate, isViewDW); 
        }


        public void ExportExcelAdvanceParlayWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW)
        {
            DailyParlayWinLossService parlay = new DailyParlayWinLossService();
            parlay.ExportExcelAdvanceParlayWinLoss(fromDate, toDate, isViewDW); 
        }
    }
}
