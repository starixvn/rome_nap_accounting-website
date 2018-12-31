namespace Fanex.Tarus.Customer.DataAccess
{
    using System.Data;
    using Fanex.Data;
    using NAP.Framework.Data;
    using CustomerSetting.Module.Core.Models;

    public class BaseDataAccess
    {
        public DataTable GetCurrency()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("Acc_ExchangeSel"))
            {
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
                    ErrorCode = (int)outputs[""],
                    ErrorMessage = outputs[""].ToString()
                };
            }
        }
    }
}
