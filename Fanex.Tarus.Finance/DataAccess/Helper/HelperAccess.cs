using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Finance.DataAccess.Helper
{
    using System.Data;

    using Fanex.Data;

    public class HelperAccess: IHelperAccess
    {
        public DataTable GetForexAccounts()
        {
            using (var objDb = new ObjectDb("Acc_CustomerSelForexAccount"))
            {
                var dataSet = objDb.ExecuteDataSet();
                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    return null;
                }
               
                return dataSet.Tables[0];
            }
        }

        public DataTable BalanceDateInfo()
        {
            DataTable datatable;
            var balmoveday = new Dictionary<string, DateTime>();
            using (var objDb = new ObjectDb("SelBalanceUpDay"))
            {
                var ds = objDb.ExecuteDataSet();
                datatable = ds.Tables[0];

            }
            return datatable;
        }

        public bool CheckIsInternal(string userName)
        {
            using (ObjectDb db = new ObjectDb("Acc_FinanceAccountByCustId"))
            {
                db.AddInParameter("Username", DbType.String, userName);
                db.AddOutParameter("isInternal", DbType.Boolean, sizeof(bool));
                db.ExecuteDataSet();
                var result = DNAide.Core.CastAide.AsBoolean(db.GetParameterValue("isInternal"));

                return result;
            }
        }

        public void Countries()
        {

        }
    }
}
