


using System;
using System.Data;
using Fanex.Data;
using Fanex.Tarus.Reports.Helpers;

namespace Fanex.Tarus.Reports.DataAccess.MatchWinLoss
{
    public class WLSummaryByBetTypeAccess : IWLSummaryByBetTypeAccess
    {
        #region IMethods

        public DataSet SelSummaryReportByBetType(DateTime reportDate)
        {
            DataSet result = null;
            ReportHelpers rpHelper = new ReportHelpers();
            string subfix = rpHelper.GetStoreMappingSubfix(reportDate);
            string spName = "WLSummaryReportByBettype" + subfix;

            using (ObjectDb objDB = new ObjectDb(spName))
            {
                if (subfix.Contains(rpHelper.WareHouseSPSuffix))
                {
                    objDB.AddInParameter("fromdate", DbType.DateTime, reportDate);
                    objDB.AddInParameter("todate", DbType.DateTime, reportDate);
                }
                else
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, reportDate);
                }
                result = objDB.ExecuteDataSet();
            }

            return result;
        }

        #endregion IMethods
    }
}