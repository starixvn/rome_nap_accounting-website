using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Accounting.Entities.Report.MatchWL;
using Sunplus.Accounting.Entities.Report.MemberStatistics;
using Sunplus.Accounting.Entities.Report.Mixparlay;
using Sunplus.Accounting.Entities.ViewModel;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReportService : IReportService
    {

        public IEnumerable<Entities.Report.MenuReport> GetMenuList(string system)
        {
            var reportMenu = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.IReportService>();
            return reportMenu.GetMenuList(system);
        }

        public MatchWL2aDTO SelMatchWL2a(string wldate, int currpage)
        {
            var report2a = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.MatchWL.IMatchWL2aService>();
            var rs = report2a.SelMatchWL2a(Convert.ToDateTime(wldate), currpage);
            return rs;
        }

        public MixparlayDataGridBO GetMixparlayReport(bool isSoccer, string date, int pageIndex, int pageSize, bool fromcache, int sortType)
        {
            var mixparlay = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Report.Mixparlay.IMixparlayWon>();
            var rs = mixparlay.GetList_MixparlayWon(isSoccer, date, pageIndex, pageSize, fromcache, sortType);
            return rs;
        }

        public MixparlayDataGridBO GetCombinationMixparlayReport(bool isSoccer, string date, int pageIndex, int pageSize, bool fromcache)
        {
            var mixparlay = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Report.CombinationMixparlay.ICombinationMixparlayWon>();
            var rs = mixparlay.GetList_CombinationMixparlayWon(isSoccer, date, pageIndex, pageSize, fromcache);
            return rs;
        }


        public void MixparlayWonExportToExcel(bool isSoccer, string date, int sortType)
        {
            var mixparlay = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Report.Mixparlay.IMixparlayWon>();
            mixparlay.MixparlayWonExportToExcel(isSoccer, date, sortType);
        }

        public void CombinationMixparlayWonExportToExcel(bool isSoccer, string date)
        {
            var mixparlay = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Report.CombinationMixparlay.ICombinationMixparlayWon>();
            mixparlay.CombinationMixparlayWonExportToExcel(isSoccer, date);
        }

        public Entities.Report.CheckBet.CheckBet GetCheckBet(string fromDate)
        {
            var report = CommonBootStrapper.Locator.GetInstance<Business.Report.IReportService>();
            return report.GetCheckBet(fromDate);
        }
    
        public List<GamesActiveMembers> GetNumberOfActiveMembers(string fromDate, string toDate)
        {
            var gamesActiveMembers = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.MemberStatistics.IGamesActiveMembersManager>();
            var result = gamesActiveMembers.GetNumberOfActiveMembers(fromDate, toDate);
            return result;
        }


        #region IReportService Members


        public WLbyBettypeDTO SelWLbyBettype(string wldate)
        {
            var report2a = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.MatchWL.IMatchWL2aServiceNEW>();
            return report2a.SelWLbyBettype( CastAide.AsDateTime(wldate) );
        }

        #endregion

        #region IReportService Members


        public MatchWLBySportDTO SelMatchWLBySport(string wldate, int currIndex, int currIndexBtype)
        {
            var report2a = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.MatchWL.IMatchWL2aServiceNEW>();
            return report2a.SelMatchWLBySport(CastAide.AsDateTime(wldate), currIndex, currIndexBtype);
        }

        #endregion

        #region IReportService Members


        public MatchWLFooterDTO SelMatchWl3RdFooter(string wldate)
        {
            var report2a = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.MatchWL.IMatchWL2aServiceNEW>();
            return report2a.SelMatchWl3RdFooter(CastAide.AsDateTime(wldate));
        }

        #endregion

        #region IReportService Members


        public MixParlayView GetMixParlayDetailDailyWl(string wldate, int bettype)
        {
            var reportDailyWL =
                CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.DailyWL.IReportDailyWL>();
            return reportDailyWL.DailyWLDirect_MixParlayAdvance(wldate, bettype);
        }

        #endregion
    }
}
