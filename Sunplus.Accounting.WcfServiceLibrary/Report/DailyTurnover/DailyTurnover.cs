using System.Collections.Generic;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities.Report.DailyTurnover;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.DailyTurnover
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DailyTurnover : IDailyTurnover
    {
        public List<DailyTurnoverDTO> SelDailyTurnover(string date)
        {
            var dailyTurnover = CommonBootStrapper.Locator.GetInstance<Business.Report.DailyTurnover.DailyTurnoverManager>();
            return dailyTurnover.SelDailyTurnover(date);
        }

        public void DailyTurnoverExportExcel(string date) {
            var dailyTurnover = CommonBootStrapper.Locator.GetInstance<Business.Report.DailyTurnover.DailyTurnoverManager>();
            dailyTurnover.DailyTurnoverExportExcel(date);
        }


        #region IDailyTurnover Members


        public List<ActionTurnover> SelDetailTurnover(string date)
        {
            var detailTurnover = CommonBootStrapper.Locator.GetInstance<Business.Report.DailyTurnover.DailyTurnoverManager>();
            return detailTurnover.SelDetailTurnover(date);
        }

        public List<ActionDetail> SelDetailActionTurnOver(string date, string action, string username)
        {
            var detailActionTurnOver = CommonBootStrapper.Locator.GetInstance<Business.Report.DailyTurnover.DailyTurnoverManager>();
            return detailActionTurnOver.SelDetailActionTurnover(date, action, username);
        }

        public void DetailTurnoverExportExcel(string date)
        {
            var dailyTurnover = CommonBootStrapper.Locator.GetInstance<Business.Report.DailyTurnover.DailyTurnoverManager>();
            dailyTurnover.DetailTurnoverExportExcel(date);
        }


        public void DetailActionTurnoverExportExcel(string datetime, string action, string username)
        {
            var dailyTurnover = CommonBootStrapper.Locator.GetInstance<Business.Report.DailyTurnover.DailyTurnoverManager>();
            dailyTurnover.DetailActionTurnoverExportExcel(datetime, action, username);
        }
        #endregion
    }
}
