using System;
using System.Collections;
using System.Data;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class OthersReportDAO : DataAccess, IOthersReportDAO
    {
        public DataTable SelectNumberOfBetsForCreditSites(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("CreditSiteBetCount"))
                {
                    var param = new Hashtable();
                    param.Add("datestart", fromDate);
                    param.Add("dateend", toDate);

                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }                    
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        protected StoreProcedureElement GetStoreProcedure(string site)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others, site);
        }

        public DbResult SelDuplicateTickets(string wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("date", wldate);
                using (ObjectDb db = new ObjectDb("SelDuplicateTickets"))
                {
                    db.AssignParamValues(hashtable);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
