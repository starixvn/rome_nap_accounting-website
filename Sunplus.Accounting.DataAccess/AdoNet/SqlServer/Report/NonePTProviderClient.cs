using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Report;
using Fanex.Data;

/// <summary>
/// Summary description for PunterAnalysisProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class NonePTProviderClient : NonePTProvider
    {
        public override DbResult NonePTMemSummary(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_NonePTMemSummary", param, DBType.REPORT, 1000);
        }

        public override DbResult NonePTLastestBet(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_NonPTLatestBets", param, DBType.REPORT, 600);
        }

        public override DbResult GetBettypeList()
        {
            var spElement = GetStoreProcedure(Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "BetTypeSel");
            return DbHelper.Execute(spElement.Name, spElement.DbName);
        }

        public override DbResult GetCurrencyList()
        {
            using (ObjectDb objDB = new ObjectDb("Acc_ExchangeSel", 600))
            {
                var dataset = objDB.ExecuteDataSet();
                DbResult dbResult = new DbResult(objDB.GetOuputParams(), dataset);
                return dbResult;
            }            
        }
    }
}