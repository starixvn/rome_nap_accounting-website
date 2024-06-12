namespace Fanex.Tarus.Customer.DataAccess
{
    using CustomerSetting.Module.Core.Models;
    using Fanex.Data;
    using NAP.Framework.Data;
    using Sunplus.Accounting.Core.Common;
    using System.Data;

    public class BaseDataAccess
    {
        public DataTable GetCurrency()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("Acc_ExchangeSel"))
            {
                objDB.AddInParameter("userid", DbType.Int32, AppSession.AccId);
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }
        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objDB.AssignParamValues(criteria.GetSpParams());
                objDB.ExecuteNonQuery();
                var outputs = objDB.GetOuputParams();
                return new Result()
                {
                    ErrorCode = (int)outputs["errcode"],
                    ErrorMessage = outputs["errmsg"]?.ToString()
                };
            }
        }
    }
}
