// -----------------------------------------------------------------------
// <copyright file="RacingDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Racing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Data;
    using DNAide.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RacingDataAccess: IRacingDataAccess
    {
        
        int timeout = 600;
        public DataTable SelNumberActiveMember(string fromdate, string todate, string balanceUpDay)
        {
            
            DataTable dt = new DataTable();
            try
            { 
                using (ObjectDb db = new ObjectDb("Acc_Rpt_RacingActiveMember", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(fromdate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(todate));
                    db.AddInParameter("balanceupday", DbType.DateTime, CastAide.AsDateTime(balanceUpDay)); 

                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                    {
                        dt = result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }
    }
}
