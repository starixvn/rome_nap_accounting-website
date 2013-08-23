using System.ServiceModel.Activation;
using BLL.Report.NonePT;
using ExcelUtil.Spreadsheet;

namespace Sunplus.Accounting.Business.Report.NonePT
{
    /// <summary>
    /// Summary description for DailyTransManager
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceNonePT : IServiceNonePT
    {
        #region IServiceNonePT Members

        public string LoadHeader()
        {
            var s = new NonePTMemSummaryManager();
            return s.LoadHeader();
        }

        public string getNonePTMembers(string username, string currpage)
        {
            var s = new NonePTMemSummaryManager();
            return s.getNonePTMembers(username, currpage);
        }

        public ExcelFile ExportToExcel_NonePTMemSummary(string username, string currpage)
        {
            var s = new NonePTExportExcelManager();
            return s.ExportToExcel_NonePTMemSummary(username, currpage);
        }

        public string getNonePTLastestBet(string username, string currpage, string bettype, string fromstake, string tostake,
                                          string pageindex, string pagesize)
        {
            var s = new NonePTLastestBetManager();
            return s.GenData(username, currpage, bettype, fromstake, tostake, pageindex, pagesize);
        }

        public string getAllBettypes()
        {
            var s = new NonePTLastestBetManager();
            return s.GenBettype();
        }

        public string getAllCurrencies()
        {
            var s = new NonePTLastestBetManager();
            return s.GenCurrency();
        }

        #endregion IServiceNonePT Members
    }
}