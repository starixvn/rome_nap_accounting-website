using System.Collections;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.DataAccess.Report;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class PokerReportDAO : DataAccessReport, IPokerReportDAO
    {
        public DbResult SelWinlostP2P(string FromDate, string ToDate)
        {
            var sp = P2PSession.Instance.P2PReports["WinlostP2P"];

            Hashtable hashtable = new Hashtable();
            hashtable.Add("fromdate", FromDate);
            hashtable.Add("todate", ToDate);
            hashtable.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
            DbResult db = DbHelper.Execute(sp.Name, hashtable, sp.DbName, 200);
            return db;
        }

        public DbResult SelWinlostP2PDetail(int currency, string date)
        {
            var spName = CastAide.AsDateTime(date) > ReportUtils.BalanceUp.MoveBetsDay ? "WinlostP2P_Detail" : "WinlostP2P_Details_14";
            var sp = P2PSession.Instance.P2PReports[spName];

            Hashtable hashtable = new Hashtable();
            hashtable.Add("currency", currency);
            hashtable.Add("winlostdate", date);
            DbResult db = DbHelper.Execute(sp.Name, hashtable, sp.DbName, 200);
            return db;
        }
    }
}