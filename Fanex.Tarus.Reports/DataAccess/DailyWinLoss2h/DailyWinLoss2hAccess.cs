
using System;
using System.Collections;
using System.Data;
using DNAide.Core;
using Fanex.Data;
using Fanex.Tarus.Reports.DataAccess.Constants;
using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.Report;

namespace Fanex.Tarus.Reports.DataAccess.DailyWinLoss2h
{
    public class DailyWinLoss2hAccess : Sunplus.Accounting.DataAccess.DataAccess, IDailyWinLoss2hAccess
    {
        public DataSet DailyWLDirect_Sporttype(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet rs = null;
            try
            {
                string storeSuffix = GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
                string strMapping = "DailyWLDirect_Sporttype" + storeSuffix;

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

        public DataTable DailyWLDirect_MixParlay(DailyWinLoss2hParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                string strMapping = "DailyWLDirect_MixParlay";
                if (param.ReportDate <= Fanex.Tarus.Reports.DataAccess.BaseDataAccess.BalanceUp.MoveBetDay)
                    strMapping += "_WH";
                using (ObjectDb objDB = new ObjectDb(strMapping))
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                    objDB.AddInParameter("bettype", DbType.Int32, param.BetType);

                    var ds = objDB.ExecuteDataSet();
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return dt;
        }

        public DataTable DailyWLDirectRacing(DailyWinLoss2hParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                string strMapping = "DailyWLDirectRacing" + GetStoreMappingSubfix(param.ReportDate);
                using (ObjectDb objDB = new ObjectDb(strMapping))
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                    var ds = objDB.ExecuteDataSet();
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return dt;
        }

        public DataTable DailyWLDirectRacingBySport(DailyWinLoss2hParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                string strMapping = "DailyWLDirectRacingBySport" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
                using (ObjectDb objDB = new ObjectDb(strMapping))
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                    objDB.AddInParameter("sporttype", DbType.Int32, param.SportType);
                    var ds = objDB.ExecuteDataSet();
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return dt;
        }

        public DataTable DailyWLDirectRacingBySportDetails(DailyWinLoss2hParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                string strMapping = string.Empty;
                if (param.BetType == 1231 || param.BetType == 1232 || param.BetType == 1233)
                {
                    strMapping = "DailyWLDirect_VirtualHorseRacing_Detail" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
                }
                else
                {
                    strMapping = "DailyWLDirectRacingBySportDetails" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
                }
                
                using (ObjectDb objDB = new ObjectDb(strMapping))
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                    objDB.AddInParameter("sporttype", DbType.Int32, param.SportType);
                    objDB.AddInParameter("bettype", DbType.Int32, param.BetType);
                    var ds = objDB.ExecuteDataSet();
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return dt;
        }

        public DataSet SelSoccerReportMain(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = "DailyWLDirect_soccer_bettype" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
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

        public DataSet SelDailyWLDirectDetail(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }

            return result;
        }

        public DataSet DailyWLDirect_NumberGame(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = "DailyWLDirect_NumberGame" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
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

        public DataSet SelCasinoReport(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = "DailyWLDirect_Casino_Detail" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
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

        public DataSet SelBingoReport(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = "DailyWLDirect_Bingo" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
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

        public DataSet SelFinanceReport(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = "DailyWLDirect_Finance" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
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

        public DataTable SelLiveCasinoReport(DailyWinLoss2hParam param)
        {
            DataTable datatable = null;
            try
            {
                string spalias = "DailyWLDirect_LiveCasino" + GetStoreMappingSubfix(param.ReportDate);
                using (ObjectDb objDB = new ObjectDb(spalias))
                {
                    objDB.AddInParameter("winlostdate", DbType.DateTime, param.ReportDate);
                    var ds = objDB.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        datatable = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return datatable;
        }

        public DataTable SelAdvanceMixParlayReport(DailyWinLoss2hParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                string strMapping = "DailyWLDirect_MixParlayAdvance";
                if (param.ReportDate <= Fanex.Tarus.Reports.DataAccess.BaseDataAccess.BalanceUp.MoveBetDay)
                    strMapping += "_WH";
                using (ObjectDb objDB = new ObjectDb(strMapping))
                {
                    objDB.AddInParameter("fromdate", DbType.DateTime, param.ReportDate);
                    objDB.AddInParameter("todate", DbType.DateTime, param.ReportDate);
                    objDB.AddInParameter("bettype", DbType.Int32, param.BetType);
                    var ds = objDB.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public DataSet SelOtherSportsReport(DailyWinLoss2hParam param, ref Hashtable outputParam)
        {
            DataSet result = null;
            try
            {
                string strMapping = "DailyWLDirect_othersport" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
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
                string strMapping = "DailyWLDirect_othersport_bettype" + GetStoreMappingSubfix(CastAide.AsDateTime(param.ReportDate));
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
