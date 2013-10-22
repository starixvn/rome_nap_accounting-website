// -----------------------------------------------------------------------
// <copyright file="GamingDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Gaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Fanex.Data;
    using DNAide.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GamingDataAccess:IGamingDataAccess
    {

        public DataTable SelNumberGameWinlossByDate(string fromdate, string todate)
        {
            DataTable dtb = new DataTable();
            using (ObjectDb objDB = new ObjectDb("Acc_Rpt_NumberGameFinanceWinlossByMonth", 1000))
            {
                objDB.AddInParameter("@datestart", DbType.DateTime, CastAide.AsDateTime(fromdate));
                objDB.AddInParameter("@dateend", DbType.DateTime, CastAide.AsDateTime(todate));
                
                var ds = objDB.ExecuteDataSet();
                if(ds != null && ds.Tables.Count>0)
                    dtb = ds.Tables[0];
            }
            return dtb;
        }
    }
}
