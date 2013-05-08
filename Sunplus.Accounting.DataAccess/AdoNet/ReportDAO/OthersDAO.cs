using System;
using System.Collections;
using System.Data;
using DNAide.Data;
using Fanex.Data;
using Sunplus.Accounting.DataAccess.Report;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class OthersDAO: DataAccess, IReportDAO.IOthersDAO
    {
        public DbResult GetMenuList(string system)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("custid", Sunplus.Accounting.Core.Common.AppSession.AccId);
            hashtable.Add("system", system);
            return DbHelper.Execute("Acc_MenuSelByCustID", hashtable, DBType.DB03, 600);
        }

        public DbResult GetMixParlayWonList(bool isSoccer, DateTime date)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others, "MixparlayWon");

            Hashtable hashtable = new Hashtable();
            hashtable.Add("issoccer", isSoccer);
            hashtable.Add("winlostdate", date);
            hashtable.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
            hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
        }

        public DbResult CombinationGetMixParlayWonList(bool isSoccer, DateTime date)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others, "CombinationMixparlayWon");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("issoccer", isSoccer);
            hashtable.Add("winlostdate", date);
            hashtable.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
            hashtable.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
            hashtable.Add("bettype", 29);
            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
        }


        public DataSet GetCheckBet(DateTime winlostDate)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_checkhack2", 180))
            {
                var p = new Hashtable();
                p.Add("winlostdate", winlostDate);
                objDb.AssignParamValues(p);

                return objDb.ExecuteDataSet();
            }
        }

    }
}
