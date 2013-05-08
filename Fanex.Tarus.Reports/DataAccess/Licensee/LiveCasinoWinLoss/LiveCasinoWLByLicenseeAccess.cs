// -----------------------------------------------------------------------
// <copyright file="LiveCasinoWLByLicenseeAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Licensee.LiveCasinoWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.Entities.Licensee;
    using System.Data;
    using Fanex.Data;
    using DNAide.Core;

   
    public class LiveCasinoWLByLicenseeAccess: ILiveCasinoWLByLicenseeAccess
    {
        public DataTable SelAllLicensees(string productCode)
        {
            DataTable dtb = null;
            using (ObjectDb objDB = new ObjectDb("GetLicenseesByResellers"))
            {
                objDB.AddInParameter("productcode", DbType.String, productCode);
                var ds = objDB.ExecuteDataSet();
                if (ds != null & ds.Tables != null)
                {
                    dtb = ds.Tables[0];
                }
            }
            return dtb;
        }

        public DataTable SelLiveCasinoWLByLicensees(string normalLicensee, string Operator , DateTime fromDate, DateTime toDate)
        {
            DataTable datatable = null;
            using (ObjectDb objDB = new ObjectDb("GetLiveCasinoWLByLicensee"))
            {
                objDB.AddInParameter("normallicensee", DbType.String, normalLicensee);
                objDB.AddInParameter("operator", DbType.String, Operator);
                objDB.AddInParameter("fromdate", DbType.DateTime, fromDate);
                objDB.AddInParameter("todate", DbType.DateTime, toDate);
                var ds = objDB.ExecuteDataSet();
                if (ds != null & ds.Tables != null)
                {
                    datatable = ds.Tables[0];
                }

            }
            return datatable;
        }

    }
}
