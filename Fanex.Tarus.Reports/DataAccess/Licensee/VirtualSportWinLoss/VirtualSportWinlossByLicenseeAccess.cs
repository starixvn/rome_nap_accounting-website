

namespace Fanex.Tarus.Reports.DataAccess.Licensee.VirtualSportWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Fanex.Data;
    using System.Collections;

    public class VirtualSportWinlossByLicenseeAccess: IVirtualSportsWinlossByLicenseeAccess
    {
        public DataTable GetReseller(string product, bool isExcludeBA, bool isExcludeTest)
        {
            
            DataTable dtbResult = null;
            using (ObjectDb objDB = new ObjectDb("GetLicenseesByResellers"))
            {
                var hash = new Hashtable();
                hash.Add("productcode", product);
                hash.Add("excludeba", isExcludeBA);
                hash.Add("excludetest", isExcludeTest);
                objDB.AssignParamValues(hash);
                DataSet ds = objDB.ExecuteDataSet();
                if (ds != null)
                    dtbResult = ds.Tables[0];
            }

            return dtbResult;
        }

        public DataTable GetSportType()
        {
            DataTable dtbResult = null;
            using (ObjectDb objDB = new ObjectDb("GetSportTypeVirtualSport"))
            {
                DataSet ds = objDB.ExecuteDataSet();
                if (ds != null)
                    dtbResult = ds.Tables[0];
            }

            return dtbResult;
        }

        public DataTable GetVirtualSportWinloss(string normalLicensee, string operators, DateTime fromDate, DateTime toDate, string sportList)
        {

            DataTable dtbResult = null;
            using (ObjectDb objDB = new ObjectDb("VirtualSportWinlossByLicensee"))
            {
                var hash = new Hashtable();
                hash.Add("normallicensee", normalLicensee);
                hash.Add("operator", operators);
                hash.Add("fromdate", fromDate);
                hash.Add("todate", toDate);
                hash.Add("sportlist", sportList);
                objDB.AssignParamValues(hash);
                DataSet ds = objDB.ExecuteDataSet();
                if (ds != null)
                {
                    dtbResult = ds.Tables[0];
                }
            }
            return dtbResult;
        }
    }
}
