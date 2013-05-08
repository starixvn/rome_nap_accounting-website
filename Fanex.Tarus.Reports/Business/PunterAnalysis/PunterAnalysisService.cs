using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DNAide.Core;
using DNAide.Utils;
using Fanex.Tarus.Reports.DataAccess.PunterAnalysis;
using Fanex.Tarus.Reports.Entities.PunterAnalysis;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;


namespace Fanex.Tarus.Reports.Business.PunterAnalysis
{
    public partial class PunterAnalysisService : IPunterAnalysisService
    {
        #region IFields

        private const int MaxRetryNumber = 10;
        IPunterAnalysicAccess _punterAnalysisAccess = null;
        private string _topPunterSummary = "TopPunterSummary";
        private string _topPunterSummaryByLeague = "TopPunterSummaryByLeague";
        private string _topPunterByCustId = "TopPunterByCustId";
        private string _topPunterByLeagueCustId = "TopPunterByLeagueCustId";
        private string _topPunterBySporttype = "TopPunterBySporttype";
        private string _topPunterByBettype = "TopPunterByBettype";
        private string _topPunterBySporttypeSum = "TopPunterBySporttypeSum";
        private string _ipByCustId = "IPByCustId";
        private string _betCountByCustId = "TopPunterBetCountByUser";
        private string _notAvailableStr = "N/A";
        private string[] _arrMonths = new string[] 
        { 
            "January",  "February", "March", "April",
            "May",  "June", "July", "August",
            "September",  "October", "November", "December"
        };

        #endregion IFields
        public PunterAnalysisService() : this(null) { }
        public PunterAnalysisService(IPunterAnalysicAccess punterAnalysisAccess)
        {
            _punterAnalysisAccess = punterAnalysisAccess ?? new PunterAnalysisAccess();
        }


        public PunterAnalysisGroupSportsDTO SelSportsGroup()
        {
            PunterAnalysisGroupSportsDTO sports = new PunterAnalysisGroupSportsDTO();
            sports.GroupName = new List<PunterAnalysisGroupSports>();
            try
            {
                DataTable dt = _punterAnalysisAccess.SelProductSport(false);

                List<int> inds = dt.AsEnumerable().Select(s => CastAide.AsInt32(s["ind"])).Distinct().ToList();

                //Soccer;
                sports.GroupName.Add(new PunterAnalysisGroupSports()
                {
                    Id = 1,
                    Name = CacheRef.GetSportName(1),
                    GroupSports = new List<Sporttype>()
                });

                var bettypesOfSoccer = dt.AsEnumerable().Where(s => CastAide.AsString(s["product"]).ToLower() == "sportsbook"
                    && CastAide.AsInt32(s["ind"]) == 0).ToList();
                foreach (DataRow row in bettypesOfSoccer)
                {
                    sports.GroupName[0].GroupSports.Add(new Sporttype()
                    {
                        Id = CastAide.AsInt32(row["bettype"]),
                        Name = CastAide.AsString(row["bettypename"])
                    });
                }

                //Others sport
                sports.GroupName.Add(new PunterAnalysisGroupSports()
                {
                    Id = 999,
                    Name = "Others",
                    GroupSports = new List<Sporttype>()

                });

                var othersSport = dt.AsEnumerable().Where(s => CastAide.AsString(s["product"]).ToLower() == "sportsbook"
                    && CastAide.AsInt32(s["ind"]) == 1).ToList();

                var sportIds = othersSport.Select(s => CastAide.AsInt32(s["sporttype"])).Distinct().ToList();
                foreach (int sportId in sportIds)
                {
                    var bettypesBySportId = othersSport.Where(s => CastAide.AsInt32(s["sporttype"]) == sportId);
                    Sporttype sporttype = new Sporttype();

                    sports.GroupName[1].GroupSports.Add(sporttype);

                    sporttype.Id = sportId;
                    sporttype.Name = bettypesBySportId.Select(s => CastAide.AsString(s["sportname"])).FirstOrDefault();

                    foreach (DataRow row in bettypesBySportId)
                    {
                        sporttype.Bettypes.Add(new Model()
                        {
                            Id = CastAide.AsInt32(row["bettype"]),
                            Name = CastAide.AsString(row["BettypeName"])
                        });

                    }
                }

                //Parlay
                sports.GroupName.Add(new PunterAnalysisGroupSports()
                {
                    Id = 0,
                    Name = "Parlay",
                    GroupSports = new List<Sporttype>()
                });

                var bettypesOfParlay = dt.AsEnumerable().Where(s => CastAide.AsString(s["product"]).ToLower() == "sportsbook"
                    && CastAide.AsInt32(s["ind"]) == 2).ToList();
                foreach (DataRow row in bettypesOfParlay)
                {
                    sports.GroupName[2].GroupSports.Add(new Sporttype()
                    {
                        Id = CastAide.AsInt32(row["bettype"]),
                        Name = CastAide.AsString(row["bettypename"])
                    });
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return sports;
        }

        public List<Model> SelLeagueBySportId(PunterAnalysisParam param)
        {
            List<Model> leagues = new List<Model>();
            try
            {
                DataRow[] rows = _punterAnalysisAccess.SelLeagueBySportId(param).Select();
                foreach (DataRow row in rows)
                {
                    leagues.Add(new Model()
                    {
                        Id = CastAide.AsInt32(row["leagueId"]),
                        Name = CastAide.AsString(row["leagueName"])

                    });

                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return leagues;
        }

        public PunterAnalysisTotalDTO SelTopPunterSummary(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO punter = new PunterAnalysisTotalDTO();
            punter.Punters = new List<PunterAnalysisBO>();
            punter.Total = new PunterAnalysisBO();
            try
            {
                DataRow[] rows = _punterAnalysisAccess.SelTopPunterSummary(param).Select();
                foreach (DataRow row in rows)
                {
                    var punterDetail = new PunterAnalysisBO()
                    {
                        Id = CastAide.AsString(row["custid"]),
                        Name = CastAide.AsString(row["username"]),
                        Winlost = NumberFormat.DecFormat(row["winloss"], 2),
                        Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                        Betcount = NumberFormat.DecFormat(row["betcount"], 0),

                        //SportType = new Sporttype()
                        //{
                        //    Id = CastAide.AsInt32(row["sporttype"]),
                        //    Name = CacheRef.GetSportName(row["sporttype"]),
                        //}
                    };
                    punterDetail.Margin = CalculateMargin(punterDetail.Winlost, punterDetail.Turnover);
                    punter.Punters.Add(punterDetail);
                }

                punter.Total = new PunterAnalysisBO()
                {
                    Winlost = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Winlost)), 2),
                    Turnover = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                    Betcount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Betcount)), 0),
                    Name = "Total",
                    HasData = punter.Punters.Count > 0 ? "1" : "0"
                };

                punter.Total.Margin = CalculateMargin(punter.Total.Winlost, punter.Total.Turnover);
                SessionExtension.AddSession(HttpContext.Current.Session, _topPunterSummary, punter);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return punter;
        }

        public PunterAnalysisTotalDTO SelTopPunterSummaryByLeague(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO punter = new PunterAnalysisTotalDTO();
            punter.Punters = new List<PunterAnalysisBO>();
            punter.Total = new PunterAnalysisBO();
            try
            {
                DataRow[] rows = _punterAnalysisAccess.SelTopPunterSummaryByLeague(param).Select();
                foreach (DataRow row in rows)
                {
                    var punterDetail = new PunterAnalysisBO()
                    {
                        Id = CastAide.AsString(row["custid"]),
                        Name = CastAide.AsString(row["username"]),
                        Winlost = NumberFormat.DecFormat(row["winloss"], 2),
                        Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                        Betcount = NumberFormat.DecFormat(row["betcount"], 0),

                        SportType = new Sporttype() { }
                    };
                    punterDetail.Margin = CalculateMargin(punterDetail.Winlost, punterDetail.Turnover);
                    punter.Punters.Add(punterDetail);
                }

                punter.Total = new PunterAnalysisBO()
                {
                    Winlost = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Winlost)), 2),
                    Turnover = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                    Betcount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Betcount)), 0),
                    Name = "Total",
                    HasData = punter.Punters.Count > 0 ? "1" : "0"
                };
                punter.Total.Margin = CalculateMargin(punter.Total.Winlost, punter.Total.Turnover);
                SessionExtension.AddSession(HttpContext.Current.Session, _topPunterSummaryByLeague, punter);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return punter;
        }

        public PunterAnalysisTotalDTO SelTopPunterByCustID(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO punter = new PunterAnalysisTotalDTO();
            punter.Punters = new List<PunterAnalysisBO>();
            punter.Total = new PunterAnalysisBO();
            try
            {
                DataRow[] rows = _punterAnalysisAccess.SelTopPunterByCustID(param).Select();
                foreach (DataRow row in rows)
                {
                    var punterDetail = new PunterAnalysisBO()
                    {
                        Id = CastAide.AsString(row["sporttype"]),
                        Name = CastAide.AsString(row["sportname"]),
                        Winlost = NumberFormat.DecFormat(row["winloss"], 2),
                        Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                        Betcount = NumberFormat.DecFormat(row["betcount"], 0),
                        CustId = CastAide.AsString(row["custid"])
                    };
                    punterDetail.Margin = CalculateMargin(punterDetail.Winlost, punterDetail.Turnover);
                    punter.Punters.Add(punterDetail);
                }

                punter.Total = new PunterAnalysisBO()
                {
                    Winlost = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Winlost)), 2),
                    Turnover = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                    Betcount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Betcount)), 0),
                    Name = "Total",
                    HasData = punter.Punters.Count > 0 ? "1" : "0"
                };
                punter.Total.Margin = CalculateMargin(punter.Total.Winlost, punter.Total.Turnover);

                SessionExtension.AddSession(HttpContext.Current.Session, _topPunterByCustId, punter);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return punter;
        }

        private List<DataTable> SelTopPunterBySporttype_ProcessData(PunterAnalysisParam param)
        {
            List<DataTable> lstDataTable = new List<DataTable>();
            try
            {
                DataTable dt = _punterAnalysisAccess.SelTopPunterBySporttype(param);
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("HasData", typeof(int));

                DataTable dtSum = dt.Clone();
                List<int> bettypes = dt.AsEnumerable().Select(s => CastAide.AsInt32(s["bettype"])).Distinct().ToList();
                foreach (int bettype in bettypes)
                {
                    List<DataRow> rows = dt.AsEnumerable().Where(s => CastAide.AsInt32(s["bettype"]) == bettype).Select(s => s).ToList();
                    if (rows.Count > 0)
                    {
                        DataRow drNewRow = dtSum.NewRow();
                        drNewRow["sporttype"] = rows[0]["sporttype"];
                        drNewRow["bettype"] = rows[0]["bettype"];
                        drNewRow["winloss"] = rows.Sum(s => CastAide.AsDecimal(s["winloss"]));
                        drNewRow["turnover"] = rows.Sum(s => CastAide.AsDecimal(s["turnover"]));
                        drNewRow["margin"] = CastAide.AsDecimal(drNewRow["turnover"]) == 0
                            ? "0"
                            : CastAide.AsString(CastAide.AsDecimal(drNewRow["winloss"]) / CastAide.AsDecimal(drNewRow["turnover"]));
                        drNewRow["betcount"] = rows.Sum(s => CastAide.AsDecimal(s["betcount"]));
                        drNewRow["islive"] = 2;

                        dtSum.Rows.Add(drNewRow);
                    }
                }


                int i = 0;
                foreach (DataRow row in dtSum.Select())
                {
                    i++;
                    row["Id"] = i;
                    row["HasData"] = 1;
                }

                DataTable dtTemplate = dt.Clone();
                foreach (DataRow row in dtSum.Select())
                {
                    DataRow drNewRow = dtTemplate.NewRow();
                    drNewRow["sporttype"] = row["sporttype"];
                    drNewRow["bettype"] = row["bettype"];
                    drNewRow["Id"] = row["Id"];

                    dtTemplate.Rows.Add(drNewRow);
                }


                int[] liveindicators = { 0, 1 };

                foreach (int liveindicator in liveindicators)
                {
                    DataTable dtData = dtTemplate.Copy();
                    List<DataRow> lstRows = dt.AsEnumerable().Select(s => s).Where(s => CastAide.AsInt32(s["islive"]) == liveindicator).ToList();
                    foreach (DataRow row in dtData.Select())
                    {
                        int bettype = CastAide.AsInt32(row["bettype"]);
                        var rs = lstRows.Select(s => s).Where(s => CastAide.AsInt32(s["bettype"]) == bettype).ToList();
                        if (rs.Count == 0)
                        {
                            row["HasData"] = 0;
                        }
                        else
                        {
                            row["winloss"] = rs.Sum(s => CastAide.AsDecimal(s["winloss"]));
                            row["turnover"] = rs.Sum(s => CastAide.AsDecimal(s["turnover"]));
                            row["margin"] = CastAide.AsDecimal(row["turnover"]) == 0
                           ? "0"
                           : CastAide.AsString(CastAide.AsDecimal(row["winloss"]) / CastAide.AsDecimal(row["turnover"]));
                            row["betcount"] = rs.Sum(s => CastAide.AsInt32(s["betcount"]));

                            row["HasData"] = 1;
                        }
                    }

                    lstDataTable.Add(dtData);
                }

                lstDataTable.Add(dtSum);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return lstDataTable;
        }

        public PunterAnalysisGrandTotalDTO SelTopPunterBySporttype(PunterAnalysisParam param)
        {
            PunterAnalysisGrandTotalDTO p = new PunterAnalysisGrandTotalDTO();
            p.Punters = new List<PunterAnalysisRowBO>();
            p.Total = new PunterAnalysisRowBO();


            try
            {
                string sporttype_bettype = param.ProductIds + "," + param.BettypeId;
                if (param.SearchBy == "0")
                {
                    param.BettypeId = Helper.GetBettypeBySportId(sporttype_bettype, param.SportId);
                }

                List<DataTable> lstDataTables = SelTopPunterBySporttype_ProcessData(param);

                int tblCount = lstDataTables.Count;
                int rowCount = tblCount > 0 ? lstDataTables[0].Rows.Count : 0;

                for (int i = 0; i < rowCount; i++)
                {
                    PunterAnalysisRowBO row = new PunterAnalysisRowBO();
                    row.Id = CastAide.AsString(lstDataTables[0].Rows[i]["bettype"]);
                    row.Name = (row.Id == "18999" ? "Kick Off" : CacheRef.GetBettype(row.Id));
                    row.SportType = new Sporttype()
                    {
                        Id = CastAide.AsInt32(lstDataTables[0].Rows[i]["sporttype"]),
                        Name = CacheRef.GetSportName(lstDataTables[0].Rows[i]["sporttype"]),
                    };

                    string[] type = { "None Live", "Live", "None Live & Live" };
                    row.Columns = new List<PunterAnalysisRowByType>();
                    for (int j = 0; j < tblCount; j++)
                    {

                        DataRow dr = lstDataTables[j].Rows[i];
                        var punterDetail = new PunterAnalysisRowByType()
                        {
                            Winlost = NumberFormat.DecFormat(dr["winloss"], 2),
                            Turnover = NumberFormat.DecFormat(dr["turnover"], 2),
                            Betcount = NumberFormat.DecFormat(dr["betcount"], 0),
                            HasData = CastAide.AsString(dr["HasData"]),
                            Type = type[j]// 0: None Live, 1: Live, 2:  None Live & Live

                        };
                        punterDetail.Margin = CalculateMargin(punterDetail.Winlost, punterDetail.Turnover);
                        row.Columns.Add(punterDetail);
                    }

                    p.Punters.Add(row);

                }

                //Total

                PunterAnalysisRowBO total = new PunterAnalysisRowBO();
                total.Name = "Total";
                total.Columns = new List<PunterAnalysisRowByType>();

                for (int j = 0; j < tblCount; j++)
                {

                    var Betcount = p.Punters.Sum(s => CastAide.AsDecimal(s.Columns[j].Betcount));
                    var Winlost = p.Punters.Sum(s => CastAide.AsDecimal(s.Columns[j].Winlost));
                    var Turnover = p.Punters.Sum(s => CastAide.AsDecimal(s.Columns[j].Turnover));
                    var totalDetail = new PunterAnalysisRowByType()
                    {
                        Betcount = NumberFormat.DecFormat(Betcount, 0),
                        Winlost = NumberFormat.DecFormat(Winlost, 2),
                        Turnover = NumberFormat.DecFormat(Turnover, 2),
                        HasData = p.Punters.Count > 0 ? "1" : "0"
                    };
                    totalDetail.Margin = CalculateMargin(totalDetail.Winlost, totalDetail.Turnover);

                    total.Columns.Add(totalDetail);
                }

                p.Total = total;

                SessionExtension.AddSession(HttpContext.Current.Session, _topPunterBySporttype, p);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return p;
        }

        public PunterAnalysisTotalDTO SelTopPunterByLeagueCustID(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO punter = new PunterAnalysisTotalDTO();
            punter.Punters = new List<PunterAnalysisBO>();
            punter.Total = new PunterAnalysisBO();
            try
            {
                DataRow[] rows = _punterAnalysisAccess.SelTopPunterByLeagueCustID(param).Select();
                foreach (DataRow row in rows)
                {
                    var punterDetail = new PunterAnalysisBO()
                    {
                        Id = CastAide.AsString(row["bettype"]),
                        Name = CacheRef.GetBettype(row["bettype"]),
                        Winlost = NumberFormat.DecFormat(row["winloss"], 2),
                        Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                        Betcount = NumberFormat.DecFormat(row["betcount"], 0),

                        SportType = new Sporttype()
                        {
                            Id = CastAide.AsInt32(row["sporttype"]),
                            Name = CacheRef.GetSportName(row["sporttype"]),
                        }
                    };
                    punterDetail.Margin = CalculateMargin(punterDetail.Winlost, punterDetail.Turnover);
                    punter.Punters.Add(punterDetail);
                }

                punter.Total = new PunterAnalysisBO()
                {
                    Winlost = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Winlost)), 2),
                    Turnover = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                    Betcount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Betcount)), 0),
                    Name = "Total"
                };
                punter.Total.Margin = CalculateMargin(punter.Total.Winlost, punter.Total.Turnover);

                SessionExtension.AddSession(HttpContext.Current.Session, _topPunterByLeagueCustId, punter);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return punter;
        }

        private PunterAnalysisGrandTotalDTO SelTopPunterByBettype_GenData(List<DataTable> lstDataTables, string PeriodType)
        {
            PunterAnalysisGrandTotalDTO p = new PunterAnalysisGrandTotalDTO();
            p.Punters = new List<PunterAnalysisRowBO>();
            p.Total = new PunterAnalysisRowBO();

            SelTopPunterByBettypeProcessData bettype = new SelTopPunterByBettypeProcessData();

            try
            {
                int tblCount = lstDataTables.Count;
                int rowCount = tblCount > 0 ? lstDataTables[0].Rows.Count : 0;

                for (int i = 0; i < rowCount; i++)
                {
                    PunterAnalysisRowBO row = new PunterAnalysisRowBO();
                    row.Id = CastAide.AsString(lstDataTables[0].Rows[i]["leagueid"]);
                    row.Name = ExecuteCacheRefWithRetry(CastAide.AsString(row.Id), CacheRef.GetLeagueName);
                    row.SportType = new Sporttype() { };

                    row.Columns = new List<PunterAnalysisRowByType>();
                    for (int j = 0; j < tblCount; j++)
                    {
                        DataRow dr = lstDataTables[j].Rows[i];
                        var punterDetail = new PunterAnalysisRowByType()
                        {
                            Winlost = NumberFormat.DecFormat(dr["winloss"], 2),
                            Turnover = NumberFormat.DecFormat(dr["turnover"], 2),
                            Betcount = NumberFormat.DecFormat(dr["betcount"], 0),

                            HasData = CastAide.AsString(dr["HasData"]),
                            Type = CastAide.AsString(dr["rptYear"]) + "|" + CastAide.AsString(dr["rptMonth"])
                        };

                        if (PeriodType == "1")//weekly
                        {
                            punterDetail.Type = CastAide.AsString(dr["rptYear"]) + "|" + CastAide.AsString(dr["week"]) + "_" + bettype.FirstDateOfWeek(CastAide.AsInt32(dr["rptYear"]), CastAide.AsInt32(dr["week"]), System.Globalization.CalendarWeekRule.FirstFourDayWeek);

                        }

                        punterDetail.Margin = CalculateMargin(punterDetail.Winlost, punterDetail.Turnover);
                        row.Columns.Add(punterDetail);
                    }

                    p.Punters.Add(row);

                }

                //Total

                PunterAnalysisRowBO total = new PunterAnalysisRowBO();
                total.Name = "Total";
                total.Columns = new List<PunterAnalysisRowByType>();

                for (int j = 0; j < tblCount; j++)
                {

                    var Betcount = p.Punters.Sum(s => CastAide.AsDecimal(s.Columns[j].Betcount));
                    var Winlost = p.Punters.Sum(s => CastAide.AsDecimal(s.Columns[j].Winlost));
                    var Turnover = p.Punters.Sum(s => CastAide.AsDecimal(s.Columns[j].Turnover));

                    var totalDetail = new PunterAnalysisRowByType()
                    {
                        Betcount = NumberFormat.DecFormat(Betcount, 0),
                        Winlost = NumberFormat.DecFormat(Winlost, 2),
                        Turnover = NumberFormat.DecFormat(Turnover, 2),
                        HasData = p.Punters.Count > 0 ? "1" : "0",

                    };
                    totalDetail.Margin = CalculateMargin(totalDetail.Winlost, totalDetail.Turnover);
                    total.Columns.Add(totalDetail);
                }

                p.Total = total;

                SessionExtension.AddSession(HttpContext.Current.Session, _topPunterByBettype, p);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return p;
        }

        private List<PunterAnalysisBO> SelTopPunterByBettype_SumLongrange(DataTable dtData, int liveindicator)
        {
            List<PunterAnalysisBO> punter = new List<PunterAnalysisBO>();
            try
            {
                List<DataRow> filterRows = new List<DataRow>();
                if (liveindicator == 2)
                {
                    filterRows = dtData.AsEnumerable().Select(s => s).ToList();
                }
                else
                {
                    filterRows = dtData.AsEnumerable().Where(s => CastAide.AsInt32(s["islive"]) == liveindicator).Select(s => s).ToList();
                }

                List<int> lstYears = filterRows.Select(s => CastAide.AsInt32(s["rptYear"])).Distinct().ToList();
                var lstYearsSorted = lstYears.OrderBy(s => s);


                foreach (int year in lstYearsSorted)
                {
                    var rowsByYear = filterRows.Where(s => CastAide.AsInt32(s["rptYear"]) == year).Select(s => s).ToList();
                    decimal wl = CastAide.AsDecimal(rowsByYear.Sum(s => CastAide.AsDecimal(s["winloss"])));
                    decimal to = CastAide.AsDecimal(rowsByYear.Sum(s => CastAide.AsDecimal(s["Turnover"])));
                    var totalDetail = new PunterAnalysisBO()
                    {
                        Id = CastAide.AsString(year),
                        Name = CastAide.AsString(year),
                        Winlost = NumberFormat.DecFormat(wl, 2),
                        Turnover = NumberFormat.DecFormat(to, 2),
                        Betcount = NumberFormat.DecFormat(rowsByYear.Sum(s => CastAide.AsDecimal(s["Betcount"])), 0),
                    };
                    totalDetail.Margin = CalculateMargin(totalDetail.Winlost, totalDetail.Turnover);
                    punter.Add(totalDetail);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return punter;
        }

        public PunterAnalysisGrandTotalDTO SelTopPunterByBettype(PunterAnalysisParam param)
        {
            PunterAnalysisGrandTotalDTO punter = new PunterAnalysisGrandTotalDTO();
            punter.Punters = new List<PunterAnalysisRowBO>();
            punter.GrandTotal = new List<PunterAnalysisBO>();


            try
            {

                DataTable dt = _punterAnalysisAccess.SelTopPunterByBettype(param);
                int liveIndicator = CastAide.AsInt32(param.LiveIndicator);
                List<DataTable> lstDataTable = new List<DataTable>();

                SelTopPunterByBettypeProcessData dataByBettype = new SelTopPunterByBettypeProcessData();

                switch (param.PeriodType)
                {
                    case "2"://Monthly 
                        lstDataTable = dataByBettype.SelTopPunterByBettype_ProcessDataMonthly(dt, liveIndicator);
                        break;
                    case "3"://Annual  
                        lstDataTable = dataByBettype.SelTopPunterByBettype_ProcessDataAnnual(dt, liveIndicator);
                        break;
                    case "4"://Long Range  
                        lstDataTable = dataByBettype.SelTopPunterByBettype_ProcessDataLongRange(dt, liveIndicator);
                        break;
                    case "0"://Daily
                        lstDataTable = dataByBettype.SelTopPunterByBettype_ProcessDataDateRange(dt, liveIndicator);
                        break;
                    case "1"://weekly
                        lstDataTable = dataByBettype.SelTopPunterByBettype_ProcessDataWeekRange(dt, liveIndicator);
                        break;
                }


                punter = SelTopPunterByBettype_GenData(lstDataTable, param.PeriodType);
                if (param.PeriodType == "4")
                {
                    punter.GrandTotal = SelTopPunterByBettype_SumLongrange(dt, liveIndicator);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return punter;
        }

        public PunterAnalysisTotalDTO SelTopPunterBySporttypeSum(PunterAnalysisParam param)
        {
            PunterAnalysisTotalDTO punter = new PunterAnalysisTotalDTO();
            punter.Punters = new List<PunterAnalysisBO>();
            punter.Total = new PunterAnalysisBO();
            try
            {
                DataRow[] rows = _punterAnalysisAccess.SelTopPunterBySporttypeSum(param).Select();
                foreach (DataRow row in rows)
                {
                    var punterDetail = new PunterAnalysisBO()
                    {
                        Id = CastAide.AsString(row["sporttype"]),
                        Name = CacheRef.GetSportName(row["sporttype"]),
                        Winlost = NumberFormat.DecFormat(row["winloss"], 2),
                        Turnover = NumberFormat.DecFormat(row["turnover"], 2),
                        Betcount = NumberFormat.DecFormat(row["betcount"], 0),

                        CustId = param.CustId,
                    };
                    punterDetail.Margin = CalculateMargin(punterDetail.Winlost, punterDetail.Turnover);
                    punter.Punters.Add(punterDetail);
                }

                punter.Total = new PunterAnalysisBO()
                {
                    Winlost = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Winlost)), 2),
                    Turnover = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Turnover)), 2),
                    Betcount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Betcount)), 0),
                    Name = "Total",
                    HasData = punter.Punters.Count > 0 ? "1" : "0"
                };
                punter.Total.Margin = CalculateMargin(punter.Total.Winlost, punter.Total.Turnover);

                SessionExtension.AddSession(HttpContext.Current.Session, _topPunterBySporttypeSum, punter);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return punter;
        }

        public List<PunterAnalysisIpBO> SelIPByCustomer(PunterAnalysisParam param)
        {
            List<PunterAnalysisIpBO> punter = new List<PunterAnalysisIpBO>();
            try
            {
                DataTable dt = _punterAnalysisAccess.SelIPByCustomer(param);
                DataRow[] rows = dt.Select();
                foreach (DataRow row in rows)
                {
                    punter.Add(new PunterAnalysisIpBO()
                    {
                        Month = CastAide.AsString(row["rptmonth"]),
                        Year = CastAide.AsString(row["rptyear"]),
                        IP = CastAide.AsString(row["IP"]),
                        Country = CastAide.AsString(row["country"]),
                    });
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _ipByCustId, punter);

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return punter;
        }

        public PunterAnalysisBetCountDTO SelBetCountByCustomer(PunterAnalysisParam param)
        {
            PunterAnalysisBetCountDTO punter = new PunterAnalysisBetCountDTO();
            punter.Punters = new List<PunterAnalysisBetCountBO>();
            punter.Total = new PunterAnalysisBetCountBO();

            try
            {
                DataTable dt = _punterAnalysisAccess.SelBetCountByCustomer(param);
                DataRow[] rows = dt.Select();
                foreach (DataRow row in rows)
                {
                    punter.Punters.Add(new PunterAnalysisBetCountBO()
                    {
                        Date = CastAide.AsString(row["rptDate"]),
                        BetCount = NumberFormat.DecFormat(row["BetCount"], 0),
                        WinCount = NumberFormat.DecFormat(row["WinCount"], 0),
                        DrawCount = NumberFormat.DecFormat(row["DrawCount"], 0),
                        Winlost = NumberFormat.DecFormat(row["winloss"], 2),
                    });
                }

                punter.Total = new PunterAnalysisBetCountBO()
                {

                    BetCount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.BetCount)), 0),
                    WinCount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.WinCount)), 0),
                    DrawCount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.DrawCount)), 0),
                    Winlost = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Winlost)), 2),

                    HasData = punter.Punters.Count > 0 ? "1" : "0"
                };

                SessionExtension.AddSession(HttpContext.Current.Session, _betCountByCustId, punter);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return punter;
        }

        public PunterAnalysisBetCountDTO SelBetCountByCustomer_Detail(PunterAnalysisParam param)
        {
            PunterAnalysisBetCountDTO punter = new PunterAnalysisBetCountDTO();
            punter.Punters = new List<PunterAnalysisBetCountBO>();
            punter.Total = new PunterAnalysisBetCountBO();

            try
            {

                DataTable dt = _punterAnalysisAccess.SelBetCountByCustomer_Detail(param);
                DataRow[] rows = dt.Select();
                foreach (DataRow row in rows)
                {
                    punter.Punters.Add(new PunterAnalysisBetCountBO()
                    {
                        Date = CastAide.AsString(row["rptDate"]),
                        BetCount = NumberFormat.DecFormat(row["BetCount"], 0),
                        WinCount = NumberFormat.DecFormat(row["WinCount"], 0),
                        DrawCount = NumberFormat.DecFormat(row["DrawCount"], 0),
                        Winlost = NumberFormat.DecFormat(row["winloss"], 2),
                    });
                }

                punter.Total = new PunterAnalysisBetCountBO()
                {
                    BetCount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.BetCount)), 0),
                    WinCount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.WinCount)), 0),
                    DrawCount = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.DrawCount)), 0),
                    Winlost = NumberFormat.DecFormat(punter.Punters.Sum(s => CastAide.AsDecimal(s.Winlost)), 2),

                    HasData = punter.Punters.Count > 0 ? "1" : "0"
                };

                SessionExtension.AddSession(HttpContext.Current.Session, _betCountByCustId, punter);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return punter;
        }

        private static string ExecuteCacheRefWithRetry(object param, Func<object, string> op)
        {
            int retry = MaxRetryNumber;
            string result = "";
            try
            {
                while (retry > 0)
                {
                    result = op(param);
                    if (!string.IsNullOrEmpty(result))
                    {
                        retry = -1;
                        break;
                    }
                    retry--;
                }
            }
            catch (Exception e)
            {
                LogExtensions.ExceptionHandler(e);
            }
            return result;
        }

        private string CalculateMargin(string strWinlost, string strTurnover)
        {
            string resultMargin = "N/A";
            if (strWinlost != null && strTurnover != null)
            {
                var winlost = Math.Round(CastAide.AsDecimal(strWinlost), 2);
                var turnover = Math.Round(CastAide.AsDecimal(strTurnover), 2);
                if (turnover != 0)
                {
                    resultMargin = NumberFormat.DecFormat((winlost / turnover) * 100, 2) + "%";
                }
            }
            return resultMargin;
        }

        public List<Product> SelProduct()
        {
            List<Product> products = new List<Product>();
            try
            {
                DataTable dt = _punterAnalysisAccess.SelProductSport(false);
                if (dt.AsEnumerable().Where(s => s["Product"].ToString().ToLower() == "sportsbook").Count() > 0)
                {
                    products.Add(new Product()
                    {
                        Id = 1,
                        Name = "Sportsbook"
                    });
                }

                var _otherProducts = dt.AsEnumerable().Where(s => s["Product"].ToString().ToLower() != "sportsbook")
                    .Select(s => CastAide.AsInt32(s["sporttype"])).Distinct();

                foreach (var _product in _otherProducts)
                {
                    var sportsByProduct = dt.AsEnumerable().Where(s => CastAide.AsInt32(s["sporttype"]) == _product);
                    int c = sportsByProduct.Count();

                    Product product = new Product();
                    products.Add(product);

                    product.Id = sportsByProduct.Select(s => CastAide.AsInt32(s["sporttype"])).FirstOrDefault();
                    product.Name = sportsByProduct.Select(s => CastAide.AsString(s["Product"])).FirstOrDefault();
                    product.Sports = new List<Model>();
                    foreach (var rowSport in sportsByProduct)
                    {
                        product.Sports.Add(new Model()
                        {
                            Id = CastAide.AsInt32(rowSport["bettype"]),
                            Name = CastAide.AsString(rowSport["BettypeName"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return products;
        }

        public List<Model> SelBettypeBySportId(int SportId)
        {
            List<Model> Bettypes = new List<Model>();
            try
            {
                var dt = _punterAnalysisAccess.SelProductSport(true);
                var dataBettypes = dt.AsEnumerable().Where(s => CastAide.AsInt32(s["Sporttype"]) == SportId);
                foreach (DataRow row in dataBettypes)
                {
                    Bettypes.Add(new Model()
                    {
                        Id = CastAide.AsInt32(row["Bettype"]),
                        Name = CastAide.AsString(row["BettypeName"])
                    });
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return Bettypes;
        }
    }
}
