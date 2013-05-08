

namespace Fanex.Tarus.Customer.DataAccess
{
    using System.Data;
    using Fanex.Data;
    public class BaseDataAccess
    {
        public DataTable GetCurrency()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GameStatistics_SelCurrency"))
            {
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }
    }
}
