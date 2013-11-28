using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Report
{
    public sealed class DailyCreatedCustomer
    {
        /// <summary>
        /// @Author: Simon20091210
        /// Content: Get Daily Created Customers List
        /// </summary>
        public static DbResult GetDailyCreatedCustomer(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_DailyCreatedCustomer", 600))
                {
                    db.AssignParamValues(hash);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    rs = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// @Author: Simon20091210
        /// Content: View downline of Daily Created Customers List
        /// </summary>
        public static DbResult GetDailyCreatedCustomerDetails(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_DailyCreatedCustomer_Details",600))
                {
                    db.AssignParamValues(hash);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    rs = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }
    }
}