// -----------------------------------------------------------------------
// <copyright file="SMAManagementAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Data;
using Fanex.Data;
using System.Collections;
namespace Fanex.Tarus.Admin.DataAccess.SMAManagement
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SMAManagementAccess: ISMAManagementAccess
    {

        public DataTable SelAllSites()
        {
            DataTable datatable = null;
            using (var dbContext = new ObjectDb("Acc_SiteSel_Credit"))
            {
                var ds = dbContext.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable SelAllCurrencies()
        {
            DataTable datatable = null;
            using (var dbContext = new ObjectDb("CustomerSetting_ExchangeSel"))
            {
                var ds = dbContext.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }


        public DataTable SelSMAListByGoupHeadline(string siteId, int currency, bool isOpen, bool isSusupended,
            bool isClosed, bool isDisabled, int roleId, string userName, string groupHeadline)
        {
            DataTable datatable = null;
            var param = new Hashtable();
            param.Add("siteid", siteId);
            param.Add("currency", currency);
            param.Add("open", isOpen);
            param.Add("suspended", isSusupended);
            param.Add("closed", isClosed);
            param.Add("disabled", isDisabled);
            param.Add("roleid", roleId);
            param.Add("username", userName);
            param.Add("groupheadline", groupHeadline);

            using (var dbContext = new ObjectDb("Acc_CustomerSelByGroupHeadline"))
            {
                dbContext.AssignParamValues(param);
                var ds = dbContext.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }
    }
}
