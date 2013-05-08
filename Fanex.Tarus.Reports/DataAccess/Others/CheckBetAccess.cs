// -----------------------------------------------------------------------
// <copyright file="CheckBetAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Others
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
    public class CheckBetAccess: ICheckBetAccess
    {

        public DataTable GetDataCheckBet(string Date, string alias)
        {
            DataTable datatable = null;
            using (ObjectDb objDB = new ObjectDb(alias))
            {
                objDB.AddInParameter("winlostdate", DbType.DateTime, CastAide.AsDateTime(Date));
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
            }
            return datatable;
        }

    }
}
