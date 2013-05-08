using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.DataAccess.Implement
{
    using System.Data;

    using Fanex.Data;
    using Fanex.Tarus.DataAccess.Interface;

    public class HelperAccess:IHelperAccess
    {
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
