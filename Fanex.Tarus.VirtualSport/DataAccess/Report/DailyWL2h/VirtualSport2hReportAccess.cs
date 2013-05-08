
using System;
using System.Collections;
using System.Data;
using DNAide.Core;
using Fanex.Data;
using Fanex.Tarus.DataAccess.Implement.Constants;
using Fanex.Tarus.VirtualSport.Entities.Report.DailyWL2h;

namespace Fanex.Tarus.VirtualSport.DataAccess.Report.DailyWL2h
{
    public class VirtualSport2hReportAccess : IVirtualSport2hReportAccess
    {
        public DataSet SelVirtualSportsReport(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            string strMapping = "DailyWLDirect_VirtualSports" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
            using (ObjectDb objDB = new ObjectDb(strMapping))
            {
                objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                result = objDB.ExecuteDataSet();
            }
            return result;
        }

        public DataSet SelVirtualSportsReportBySport(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            string strMapping = "DailyWLDirect_VirtualSports_Bettype" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
            using (ObjectDb objDB = new ObjectDb(strMapping))
            {
                objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                objDB.AddInParameter("sporttype", DbType.Int32, param.SportType);
                result = objDB.ExecuteDataSet();
            }

            return result;
        }

        public DataSet SelDailyWLDirectDetail(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;

            string strMapping = "DailyWLDirect_Detail" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
            using (ObjectDb objDB = new ObjectDb(strMapping))
            {
                objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                objDB.AddInParameter("sporttype", DbType.Int32, param.SportType);
                objDB.AddInParameter("liveindicator", DbType.Int32, param.IsLive);
                objDB.AddInParameter("bettype", DbType.Int32, param.BetType);
                objDB.AddInParameter("ishedge", DbType.Boolean, param.IsHedge);
                objDB.AddOutParameter("winlost_8rm", DbType.Double, DbTypeSize.DoubleType);
                objDB.AddOutParameter("winlost_barm", DbType.Double, DbTypeSize.DoubleType);
                result = objDB.ExecuteDataSet();
                outputParam = objDB.GetOuputParams();
            }


            return result;
        }

        private string GetStoreMappingSubfix(DateTime wldate)
        {
            if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
            {
                return string.Empty;
            }
            else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
            {
                return "_14";
            }
            else
            {
                return "_WH";
            }
        }

    }
}
