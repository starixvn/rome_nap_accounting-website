using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Data;
using DNAide.Core;

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover
{
    public class SMATurnoverAccess : ISMATurnoverAccess
    {
        public static Dictionary<string, DateTime> SelBalanceUpMovebetDays()
        {
            var datatable = new DataTable();
            var balmoveday = new Dictionary<string, DateTime>();
            using (ObjectDb objDB = new ObjectDb("SelBalanceUpDay"))
            {
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
                balmoveday.Add("BalanceUpDay", CastAide.AsDateTime(datatable.Rows[0]["BalanceUpDay"]));
                balmoveday.Add("MovebetsDay", CastAide.AsDateTime(datatable.Rows[0]["MovebetsDay"]));
            }
            return balmoveday;
        }

        public static DateTime BalanceUpDay()
        {
            Dictionary<string, DateTime> balmoveDays = SelBalanceUpMovebetDays();
            return balmoveDays["BalanceUpDay"];
        }

        public static DateTime MoveBetDay()
        {
            Dictionary<string, DateTime> balmoveDays = SelBalanceUpMovebetDays();
            return balmoveDays["MovebetsDay"];
        }

        public DataTable GetSportType(int mode)
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("SMASportType"))
            {
                objDB.AddInParameter("mode", DbType.Int32, mode);
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable GetLeague(int sport, DateTime fromDate, DateTime toDate)
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GetLeague"))
            {
                objDB.AddInParameter("sporttype", DbType.Int32, sport);
                objDB.AddInParameter("fromdate", DbType.DateTime, fromDate);
                objDB.AddInParameter("todate", DbType.DateTime, toDate);
                objDB.AddInParameter("movebetsday", DbType.DateTime, MoveBetDay());
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
            }
            return datatable;
        }
    }
}
