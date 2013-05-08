using System;
using System.Data;
using DNAide.Core;
using Fanex.Data;
using Fanex.Tarus.Reports.Business.PunterAnalysis;
using Fanex.Tarus.Reports.Entities.PunterAnalysis;
using Sunplus.Accounting.Core.Extensions;

namespace Fanex.Tarus.Reports.DataAccess.PunterAnalysis
{
    public class PunterAnalysisAccess : IPunterAnalysicAccess
    {
        int timeout = 600;//second
        public DataTable SelProductSport(bool IsSearchByLeague)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_TopPunterSelSports", timeout))
                {
                    db.AddInParameter("issb", DbType.Boolean, IsSearchByLeague);

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

        public DataTable SelLeagueBySportId(PunterAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_TopPunterSelLeagues", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("sporttype", DbType.String, param.SportId);

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

        private PunterAnalysisParam setParamDefault(PunterAnalysisParam param)
        {
            int byleague = 1;
            string products = param.ProductIds;
            if (param.SearchBy == "0") //search by user
            {
                byleague = 0;
            }

            return param;
        }

        public DataTable SelTopPunterSummary(PunterAnalysisParam param)
        {
            param = setParamDefault(param);
            DataTable dt = new DataTable();
            try
            {
                var sportbettype = param.ProductIds + "," + param.SportId;
                using (ObjectDb db = new ObjectDb("Punter_SelTopPunterSummary", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("sportbettype", DbType.String, sportbettype);
                    db.AddInParameter("username", DbType.String, param.UserName);
                    db.AddInParameter("toprow", DbType.Int32, CastAide.AsInt32(param.TopRow));
                    db.AddInParameter("orderby", DbType.Int32, CastAide.AsInt32(param.OrderBy));
                    db.AddInParameter("ordertype", DbType.Boolean, param.OrderType == "1");
                    db.AddInParameter("reporttype", DbType.Int32, CastAide.AsInt32(param.PeriodType));

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

        public DataTable SelTopPunterSummaryByLeague(PunterAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelTopPunterSummaryByLeague", timeout))
                {
                    db.AddInParameter("sporttype", DbType.String, param.SportId);
                    db.AddInParameter("bettype", DbType.String, param.BettypeId);
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("leagueid", DbType.Int32, CastAide.AsInt32(param.LeagueId));
                    db.AddInParameter("toprow", DbType.Int32, CastAide.AsInt32(param.TopRow));
                    db.AddInParameter("orderby", DbType.Int32, CastAide.AsInt32(param.OrderBy));
                    db.AddInParameter("ordertype", DbType.Boolean, param.OrderType == "1");
                    db.AddInParameter("reporttype", DbType.Int32, CastAide.AsInt32(param.PeriodType));

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

        public DataTable SelTopPunterByCustID(PunterAnalysisParam param)
        {
            param = setParamDefault(param);
            var sportbettype = param.ProductIds + "," + param.SportId;

            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelTopPunterByCustID", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("sportbettype", DbType.String, sportbettype);

                    if (CastAide.AsInt32(param.CustId) > 0)
                        db.AddInParameter("custid", DbType.Int32, CastAide.AsInt32(param.CustId));
                    else
                        db.AddInParameter("username", DbType.String, CastAide.AsString(param.UserName));

                    db.AddInParameter("orderby", DbType.Int32, CastAide.AsInt32(param.OrderBy));
                    db.AddInParameter("ordertype", DbType.Boolean, param.OrderType == "1");
                    db.AddInParameter("reporttype", DbType.Int32, CastAide.AsInt32(param.PeriodType));

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

        public DataTable SelTopPunterBySporttype(PunterAnalysisParam param)
        {
            param = setParamDefault(param);
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelTopPunterBySporttype", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("custid", DbType.Int32, CastAide.AsInt32(param.CustId));
                    db.AddInParameter("sporttype", DbType.String, param.SportId);
                    db.AddInParameter("bettype", DbType.String, param.BettypeId);

                    if (param.SearchBy == "1")//League
                    {
                        db.AddInParameter("leagueid", DbType.Int32, CastAide.AsInt32(param.LeagueId));
                    }

                    db.AddInParameter("orderby", DbType.Int32, CastAide.AsInt32(param.OrderBy));
                    db.AddInParameter("ordertype", DbType.Boolean, param.OrderType == "1");
                    db.AddInParameter("reporttype", DbType.Int32, CastAide.AsInt32(param.PeriodType));

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

        public DataTable SelTopPunterByLeagueCustID(PunterAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelTopPunterByLeagueCustID", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("custid", DbType.Int32, CastAide.AsInt32(param.CustId));
                    db.AddInParameter("leagueid", DbType.Int32, CastAide.AsInt32(param.LeagueId));
                    db.AddInParameter("orderby", DbType.Int32, CastAide.AsInt32(param.OrderBy));
                    db.AddInParameter("ordertype", DbType.Boolean, param.OrderType == "1");
                    db.AddInParameter("reporttype", DbType.Int32, CastAide.AsInt32(param.PeriodType));

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

        public DataTable SelTopPunterByBettype(PunterAnalysisParam param)
        {
            param = setParamDefault(param);
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelTopPunterByBettype", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("custid", DbType.Int32, CastAide.AsInt32(param.CustId));
                    db.AddInParameter("sporttype", DbType.String, CastAide.AsInt32(param.SportId));
                    db.AddInParameter("bettype", DbType.Int32, CastAide.AsInt32(param.BettypeId));
                    db.AddInParameter("orderby", DbType.Int32, CastAide.AsInt32(param.OrderBy));
                    db.AddInParameter("ordertype", DbType.Boolean, param.OrderType == "1");
                    db.AddInParameter("reporttype", DbType.Int32, param.PeriodType);

                    if (param.SearchBy == "1")//League
                    {
                        db.AddInParameter("leagueId", DbType.Int32, CastAide.AsInt32(param.LeagueId));
                    }

                    if (param.LiveIndicator != null && CastAide.AsInt32(param.LiveIndicator) != 2)
                    {
                        db.AddInParameter("status", DbType.Int32, CastAide.AsInt32(param.LiveIndicator));
                    }

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

        public DataTable SelTopPunterBySporttypeSum(PunterAnalysisParam param)
        {
            param = setParamDefault(param);
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelTopPunterBySporttypeSum", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("custid", DbType.Int32, CastAide.AsInt32(param.CustId));
                    db.AddInParameter("sporttype", DbType.Int32, CastAide.AsInt32(param.SportId));
                    db.AddInParameter("bettype", DbType.String, param.BettypeId);
                    db.AddInParameter("leagueid", DbType.Int32, CastAide.AsInt32(param.LeagueId));
                    db.AddInParameter("reporttype", DbType.Int32, CastAide.AsInt32(param.PeriodType));

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

        public DataTable SelIPByCustomer(PunterAnalysisParam param)
        {

            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelIPByCustomer", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("custid", DbType.Int32, CastAide.AsInt32(param.CustId));

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

        public DataTable SelBetCountByCustomer(PunterAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelBetCountByCustomer", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("custid", DbType.Int32, CastAide.AsInt32(param.CustId));
                    db.AddInParameter("reporttype", DbType.Int32, param.PeriodType);

                    if (param.SearchBy == "1")//League
                    {
                        db.AddInParameter("LeagueID", DbType.Int32, CastAide.AsInt32(param.LeagueId));
                        db.AddInParameter("sporttype", DbType.Int32, CastAide.AsInt32(param.SportId));
                        db.AddInParameter("Bettype", DbType.String, param.BettypeId);
                        db.AddInParameter("byLeague", DbType.Boolean, true);
                    }
                    else
                    {
                        var sportbettype = "";
                        if (param.SportId != null)
                        {
                            sportbettype = param.ProductIds + "," + (param.SportId.Split('_').Length == 1 ? param.SportId + "_0" : param.SportId);
                            if (param.SportId.Split('_').Length == 1)
                            {
                                sportbettype = Helper.GetSporttypeBettypeBySportId(param.ProductIds, param.SportId);
                            }
                        }
                        else
                        {
                            sportbettype = param.ProductIds;
                        }

                        db.AddInParameter("sportbettype", DbType.String, sportbettype);
                    }

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

        public DataTable SelBetCountByCustomer_Detail(PunterAnalysisParam param)
        {
            DataTable dt = new DataTable();

            try
            {
                using (ObjectDb db = new ObjectDb("Punter_SelBetCountByCustomer_Detail", timeout))
                {
                    db.AddInParameter("fromdate", DbType.DateTime, CastAide.AsDateTime(param.FromDate));
                    db.AddInParameter("todate", DbType.DateTime, CastAide.AsDateTime(param.ToDate));
                    db.AddInParameter("custid", DbType.Int32, CastAide.AsInt32(param.CustId));
                    db.AddInParameter("bettype", DbType.Int32, CastAide.AsInt32(param.BettypeId));
                    db.AddInParameter("reporttype", DbType.Int32, param.PeriodType);

                    if (param.SearchBy == "1")
                    {
                        db.AddInParameter("leagueid", DbType.Int32, CastAide.AsInt32(param.LeagueId));
                    }


                    if (param.LiveIndicator != null && CastAide.AsInt32(param.LiveIndicator) != 2)
                        db.AddInParameter("status", DbType.Int32, CastAide.AsInt32(param.LiveIndicator));

                    if (param.SportId != null)
                        db.AddInParameter("Sporttype", DbType.String, param.SportId);

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


        public DataTable SelProduct()
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Punter_BetCountSelSports", timeout))
                {
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
