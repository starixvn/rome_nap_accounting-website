// -----------------------------------------------------------------------
// <copyright file="BingoMonthlyWLAccess.cs" company="">
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
    public class BingoMonthlyWLAccess: IBingoMonthlyWLAccess
    {

        public DataTable GetBingoReportWLByMonth(string month, string year)
        {
            DataTable dtb = null;
            using (ObjectDb objDB = new ObjectDb("BingoWinlossByMonth"))
            {
                objDB.AddInParameter("month", DbType.Int16, CastAide.AsInt16(month));
                objDB.AddInParameter("year", DbType.Int32, CastAide.AsInt32(year));
                var ds = objDB.ExecuteDataSet();
                dtb = ds.Tables[0];
            }
            return dtb;
        }
    }
}
