
using System;
using System.Collections;
using System.Data;
using DNAide.Core;
using Fanex.Data;
using Fanex.Tarus.Reports.DataAccess.Constants;
using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.Report;

namespace Fanex.Tarus.Reports.DataAccess.DailyWinLoss2hOtherSports
{
    public class DailyWinLoss2hOtherSportsAccess : Sunplus.Accounting.DataAccess.DataAccess, IDailyWinLoss2hOtherSportsAccess
    {
        private enum SpAlias
        {
            DailyWLDirect_Sporttype_OtherSports,
            DailyWLDirect_Detail,
            DailyWLDirect_othersport,
            DailyWLDirect_othersport_bettype
        }

        public DataSet DailyWLDirect_Sporttype(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet rs = null;
            try
            {
                string storeSuffix = GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
                string strMapping = SpAlias.DailyWLDirect_Sporttype_OtherSports.ToString() + storeSuffix;

                using (ObjectDb objDB = new ObjectDb(strMapping))
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                    objDB.AddOutParameter("linetotal", DbType.Double, DbTypeSize.DoubleType);

                    switch (storeSuffix)
                    {
                        case "":
                            break;
                        case "_WH":
                            objDB.AddInParameter("fi_3rdshare", DbType.Double, 0);
                            break;
                        case "_14":
                            break;

                    }

                    rs = objDB.ExecuteDataSet();
                    outputParam = objDB.GetOuputParams();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        public DataSet SelDailyWLDirectDetail(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = SpAlias.DailyWLDirect_Detail.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }

            return result;
        }

        public DataSet SelOtherSportsReport(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = SpAlias.DailyWLDirect_othersport.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
                using (ObjectDb objDB = new ObjectDb(strMapping))
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                    result = objDB.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public DataSet SelOtherSportsReportBySport(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = SpAlias.DailyWLDirect_othersport_bettype.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
                using (ObjectDb objDB = new ObjectDb(strMapping))
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                    objDB.AddInParameter("sporttype", DbType.Int32, param.SportType);
                    //objDB.AddInParameter("_details", DbType.Int32, 0);
                    result = objDB.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

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
