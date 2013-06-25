

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Business.Implement.Report.Common;
using Fanex.Tarus.Entities.Sport;
using Fanex.Tarus.Infrastructure.Caching;
using Fanex.Tarus.Infrastructure.Constants;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Infrastructure.Resource;
using Fanex.Tarus.VirtualSport.Business.Report.DailyWL2h;
using Fanex.Tarus.VirtualSport.DataAccess.Report.DailyWL2h;
using Fanex.Tarus.VirtualSport.Entities.Report.DailyWL2h;

namespace Fanex.Tarus.Reports.Business.DailyWinLoss2h
{
    public class VirtualSport2hReportService : IVirtualSport2hReportService
    {
        #region IFields

        //
        // GET: /VirtualSport2hReportAccess/
        private readonly IVirtualSport2hReportAccess _virtualSport2hReportAccess;

        private RMReportUtils _barmUtils = new RMReportUtils(RMReportUtils.ReportName.Report2h, RMReportUtils.RMType.BARM);
        private RMReportUtils _8rmUtils = new RMReportUtils(RMReportUtils.ReportName.Report2h, RMReportUtils.RMType.EightRM);
        private string _notAvailableString = Fanex.Tarus.VirtualSport.Resources.Report.NotAvailableString;
        private string _headgeGameSuffix = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.HedgeGame;


        #endregion IFields
        #region IConstructions

        public VirtualSport2hReportService()
            : this(null)
        {
        }

        public VirtualSport2hReportService(IVirtualSport2hReportAccess virtualSport2hReportAccess)
        {
            _virtualSport2hReportAccess = virtualSport2hReportAccess ?? new VirtualSport2hReportAccess();
        }

        #endregion IConstructions
        #region IMethods

        public LiveAndNoneLiveReport SelVirtualSportsReport(DailyWinLoss2hParam param)
        {
            var allTotalSummary = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.AllTotalSummary;

            var result = new LiveAndNoneLiveReport
            {
                NoneLiveReport = new Report
                {
                    ReportDetails = new List<ReportDetail>(),
                    Total = new ReportTotal
                    {
                        NormalTotal = new ReportDetail
                        {
                            Name = allTotalSummary
                        },
                        EightRMTotal = new ReportDetail(),
                        BARMTotal = new ReportDetail(),
                        AllTotal = new ReportDetail(),
                        GrandTotal = new ReportGrandTotal()
                    },
                    IsBARMActive = _barmUtils.IsActive
                },
                LiveReport = new Report
                {
                    ReportDetails = new List<ReportDetail>(),
                    Total = new ReportTotal
                    {
                        NormalTotal = new ReportDetail
                        {
                            Name = allTotalSummary
                        },
                        EightRMTotal = new ReportDetail(),
                        BARMTotal = new ReportDetail(),
                        AllTotal = new ReportDetail(),
                        GrandTotal = new ReportGrandTotal()
                    },
                    IsBARMActive = _barmUtils.IsActive
                }
            };

            try
            {
                Hashtable outParam = new Hashtable();
                DataSet dbResult = _virtualSport2hReportAccess.SelVirtualSportsReport(param, ref outParam);
                if (dbResult != null && dbResult.Tables.Count > 0)
                {
                    #region Declaration

                    DataTable dtReport = dbResult.Tables[0];
                    bool isHavingHedgeColumn = false;
                    if (dtReport.Columns.Contains("IsHedge"))
                    {
                        isHavingHedgeColumn = true;
                    }

                    var emptyRow = new ReportDetail
                    {
                        RowType = new Entities.Sport.BaseModel
                        {
                            Id = RowType.EmptyRow
                        }
                    };

                    ReportDetail sportTotal = new ReportDetail
                    {
                        RowType = new Entities.Sport.BaseModel
                        {
                            Id = RowType.SportTotal
                        }
                    };

                    var dailyWinLoss2hItemX = new ReportDetail();

                    #endregion Declaration
                    #region ForEach

                    foreach (DataRow row in dtReport.Rows)
                    {
                        dailyWinLoss2hItemX = new ReportDetail
                        {
                            Id = CastAide.AsInt32(row["SportType"]),
                            Name = CacheRef.GetSportName(CastAide.AsInt32(row["SportType"])),
                            WinLoss = CastAide.AsDecimal(row["WinLost"]),
                            Position = CastAide.AsDecimal(row["Position"]),
                            Commission = CastAide.AsDecimal(row["Commission"]),
                            LineTotal = CastAide.AsDecimal(row["LineTotal"]),
                            Turnover = CastAide.AsDecimal(row["Turnover"]),
                            IsLive = false,

                            RowType = new Entities.Sport.BaseModel
                            {
                                Id = RowType.Normal
                            }
                        };

                        if (isHavingHedgeColumn)
                        {
                            dailyWinLoss2hItemX.Hedge = CastAide.AsString(row["IsHedge"]);
                        }
                        dailyWinLoss2hItemX.AliasName = dailyWinLoss2hItemX.Name;
                        dailyWinLoss2hItemX.Margin = CalculateMargin(dailyWinLoss2hItemX);
                        if (dailyWinLoss2hItemX.IsLive)
                        {
                            result.LiveReport.Total.NormalTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.LiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                        }
                        else
                        {
                            result.NoneLiveReport.Total.NormalTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.NoneLiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                        }
                    }

                    #endregion ForEach

                    result.NoneLiveReport.Total.NormalTotal.Margin = CalculateMargin(result.NoneLiveReport.Total.NormalTotal);
                    result.LiveReport.Total.NormalTotal.Margin = CalculateMargin(result.LiveReport.Total.NormalTotal);
                    if (dbResult.Tables.Count > 1)
                    {
                        DataTable dt8RM = dbResult.Tables[1];
                        result.LiveReport.Total.EightRMTotal.WinLoss = CastAide.AsDecimal(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=1"));
                        result.NoneLiveReport.Total.EightRMTotal.WinLoss = CastAide.AsDecimal(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=0"));
                    }

                    if (dbResult.Tables.Count > 2)
                    {
                        DataTable dtBARM = dbResult.Tables[2];
                        result.LiveReport.Total.BARMTotal.WinLoss = CastAide.AsDecimal(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=1"));
                        result.NoneLiveReport.Total.BARMTotal.WinLoss = CastAide.AsDecimal(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=0"));
                    }

                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public LiveAndNoneLiveReport SelVirtualSportsReportBySport(DailyWinLoss2hParam param)
        {
            var allTotalSummary = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.AllTotalSummary;

            var result = new LiveAndNoneLiveReport
            {
                NoneLiveReport = new Report
                {
                    ReportDetails = new List<ReportDetail>(),
                    Total = new ReportTotal
                    {
                        NormalTotal = new ReportDetail
                        {
                            Name = allTotalSummary
                        },
                        EightRMTotal = new ReportDetail(),
                        BARMTotal = new ReportDetail(),
                        AllTotal = new ReportDetail(),
                        GrandTotal = new ReportGrandTotal()
                    },
                    IsBARMActive = _barmUtils.IsActive
                },
                LiveReport = new Report
                {
                    ReportDetails = new List<ReportDetail>(),
                    Total = new ReportTotal
                    {
                        NormalTotal = new ReportDetail
                        {
                            Name = allTotalSummary
                        },
                        EightRMTotal = new ReportDetail(),
                        BARMTotal = new ReportDetail(),
                        AllTotal = new ReportDetail(),
                        GrandTotal = new ReportGrandTotal()
                    },
                    IsBARMActive = _barmUtils.IsActive
                }
            };


            try
            {
                Hashtable outParam = new Hashtable();
                DataSet dbResult = _virtualSport2hReportAccess.SelVirtualSportsReportBySport(param, ref outParam);

                if (dbResult != null && dbResult.Tables.Count > 0)
                {
                    #region Declaration

                    DataTable dtReport = dbResult.Tables[0];
                    bool isHavingHedgeColumn = false;
                    if (dtReport.Columns.Contains("IsHedge"))
                    {
                        isHavingHedgeColumn = true;
                    }

                    var emptyRow = new ReportDetail
                    {
                        RowType = new Entities.Sport.BaseModel
                        {
                            Id = RowType.EmptyRow
                        }
                    };

                    ReportDetail sportTotal = new ReportDetail
                    {
                        RowType = new Entities.Sport.BaseModel
                        {
                            Id = RowType.SportTotal
                        }
                    };

                    var dailyWinLoss2hItemX = new ReportDetail();

                    #endregion Declaration
                    #region ForEach

                    foreach (DataRow row in dtReport.Rows)
                    {
                        dailyWinLoss2hItemX = new ReportDetail
                        {
                            Id = CastAide.AsInt32(row["BetType"]),
                            Name = CacheRef.GetBettype(CastAide.AsInt32(row["BetType"])),
                            WinLoss = CastAide.AsDecimal(row["WinLost"]),
                            Position = CastAide.AsDecimal(row["Position"]),
                            Commission = CastAide.AsDecimal(row["Commission"]),
                            LineTotal = CastAide.AsDecimal(row["LineTotal"]),
                            Turnover = CastAide.AsDecimal(row["Turnover"]),
                            IsLive = false,

                            RowType = new Entities.Sport.BaseModel
                            {
                                Id = RowType.Normal
                            }
                        };

                        if (isHavingHedgeColumn)
                        {
                            dailyWinLoss2hItemX.Hedge = CastAide.AsString(row["IsHedge"]);
                        }
                        dailyWinLoss2hItemX.BetType = new BaseModel
                        {
                            Id = dailyWinLoss2hItemX.Id,
                            Name = dailyWinLoss2hItemX.Name
                        };
                        dailyWinLoss2hItemX.AliasName = GetBetTypeName(dailyWinLoss2hItemX);
                        dailyWinLoss2hItemX.Margin = CalculateMargin(dailyWinLoss2hItemX);
                        if (dailyWinLoss2hItemX.IsLive)
                        {
                            result.LiveReport.Total.NormalTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.LiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                        }
                        else
                        {
                            result.NoneLiveReport.Total.NormalTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.NoneLiveReport.ReportDetails.Add(dailyWinLoss2hItemX);
                        }
                    }

                    #endregion ForEach

                    result.NoneLiveReport.Total.NormalTotal.Margin = CalculateMargin(result.NoneLiveReport.Total.NormalTotal);
                    result.LiveReport.Total.NormalTotal.Margin = CalculateMargin(result.LiveReport.Total.NormalTotal);
                    if (dbResult.Tables.Count > 1)
                    {
                        DataTable dt8RM = dbResult.Tables[1];
                        result.LiveReport.Total.EightRMTotal.WinLoss = CastAide.AsDecimal(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=1"));
                        result.NoneLiveReport.Total.EightRMTotal.WinLoss = CastAide.AsDecimal(dt8RM.Compute("sum(WinLost8RM)", "liveindicator=0"));
                    }

                    if (dbResult.Tables.Count > 2)
                    {
                        DataTable dtBARM = dbResult.Tables[2];
                        result.LiveReport.Total.BARMTotal.WinLoss = CastAide.AsDecimal(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=1"));
                        result.NoneLiveReport.Total.BARMTotal.WinLoss = CastAide.AsDecimal(dtBARM.Compute("sum(WinLostBARM)", "liveindicator=0"));
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public Report SelDailyWLDirectDetails(DailyWinLoss2hParam param)
        {
            var totalSummary = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.TotalSummary;
            var allTotalSummary = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.AllTotalSummary;

            var result = new Report
            {
                ReportDetails = new List<ReportDetail>(),
                Total = new ReportTotal
                {
                    NormalTotal = new ReportDetail
                    {
                        Name = CacheRef.GetBettype(param.BetType) + " " + totalSummary
                    },
                    EightRMTotal = new ReportDetail(),
                    BARMTotal = new ReportDetail(),
                    AllTotal = new ReportDetail(),
                    GrandTotal = new ReportGrandTotal()
                },
                IsBARMActive = _barmUtils.IsActive
            };

            try
            {
                #region Declaration

                Hashtable outParam = new Hashtable();
                var dbResult = _virtualSport2hReportAccess.SelDailyWLDirectDetail(param, ref outParam);

                var emptyRow = new ReportDetail
                {
                    RowType = new Entities.Sport.BaseModel
                    {
                        Id = RowType.EmptyRow
                    }
                };

                #endregion Declaration

                if (dbResult != null && dbResult.Tables.Count > 0)
                {
                    DataTable dataTable = dbResult.Tables[0];
                    var dailyWinLoss2hItemX = new ReportDetail();
                    List<int> leagueIds = dataTable.AsEnumerable().Select(s => CastAide.AsInt32(s["LeagueId"])).Distinct().ToList();

                    foreach (var leagueId in leagueIds)
                    {
                        var leagueHeadLine = new ReportDetail
                        {
                            RowType = new Entities.Sport.BaseModel
                            {
                                Id = RowType.LeagueHeadLine
                            },
                            Id = leagueId,
                            Name = CacheRef.GetLeagueName(leagueId),
                        };
                        leagueHeadLine.GroupName = leagueHeadLine.Name;

                        ReportDetail sportTotal = new ReportDetail
                        {
                            RowType = new Entities.Sport.BaseModel
                            {
                                Id = RowType.SportTotal
                            },
                            Name = totalSummary,
                            GroupName = leagueHeadLine.Name
                        };

                        var rowIndex = 0;

                        result.ReportDetails.Add(leagueHeadLine);
                        DataRow[] rows = dataTable.Select("LeagueId='" + leagueId + "'");

                        #region ForEach

                        foreach (DataRow row in rows)
                        {
                            dailyWinLoss2hItemX = new ReportDetail
                            {
                                MatchInfo = new Match
                                {
                                    Name = CastAide.AsString(row["MatchCode"]),
                                    HomeTeam = new BaseModel
                                    {
                                        Id = CastAide.AsInt32(row["HomeId"]),
                                        Name = CacheRef.GetTeamName(CastAide.AsInt32(row["HomeId"]))
                                    },
                                    AwayTeam = new BaseModel
                                    {
                                        Id = CastAide.AsInt32(row["AwayId"]),
                                        Name = CacheRef.GetTeamName(CastAide.AsInt32(row["AwayId"]))
                                    },
                                    IsNeutral = CastAide.AsBoolean(row["IsNeutral"])
                                },

                                GroupName = leagueHeadLine.Name,
                                WinLoss = CastAide.AsDecimal(row["WinLost"]),
                                Position = CastAide.AsDecimal(row["Position"]),
                                Commission = CastAide.AsDecimal(row["Commission"]),
                                Turnover = CastAide.AsDecimal(row["RealTurnover"]),
                                RowType = new Entities.Sport.BaseModel
                                {
                                    Id = RowType.Normal
                                },
                                RowIndex = rowIndex
                            };

                            dailyWinLoss2hItemX.LineTotal = dailyWinLoss2hItemX.WinLoss + dailyWinLoss2hItemX.Position + dailyWinLoss2hItemX.Commission;
                            dailyWinLoss2hItemX.Margin = CalculateMargin(dailyWinLoss2hItemX);
                            sportTotal.Add(dailyWinLoss2hItemX);
                            dailyWinLoss2hItemX.Round();
                            result.ReportDetails.Add(dailyWinLoss2hItemX);
                            rowIndex++;
                        }

                        #endregion ForEach
                        result.Total.NormalTotal.Add(sportTotal);
                        if (rowIndex > 1)
                        {
                            sportTotal.Margin = CalculateMargin(sportTotal);
                            sportTotal.Round();
                            result.ReportDetails.Add(sportTotal);
                        }
                    }

                    result.ReportDetails.Sort(delegate(ReportDetail dwlA, ReportDetail dwlB)
                    {
                        return SortReportDetail(dwlA, dwlB);
                    });

                    result.Total.NormalTotal.Margin = CalculateMargin(result.Total.NormalTotal);
                    result.Total.EightRMTotal.WinLoss = CastAide.AsDecimal(outParam["winlost_8rm"], 0);
                    result.Total.BARMTotal.WinLoss = CastAide.AsDecimal(outParam["winlost_barm"], 0);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private int SortReportDetail(ReportDetail dwlA, ReportDetail dwlB)
        {
            if (dwlA.GroupName.StartsWith("*") && !dwlB.GroupName.StartsWith("*"))
            {
                return -1;
            }
            if (!dwlA.GroupName.StartsWith("*") && dwlB.GroupName.StartsWith("*"))
            {
                return 1;
            }
            if (dwlA.GroupName == dwlB.GroupName)
            {
                if (dwlA.RowType.Id == dwlB.RowType.Id)
                {
                    if (dwlA.MatchInfo != null && dwlB.MatchInfo != null)
                    {
                        return string.Compare(dwlA.MatchInfo.Name, dwlB.MatchInfo.Name);
                    }
                    return 0;
                }

                if (dwlA.RowType.Id == RowType.LeagueHeadLine)
                {
                    return -1;
                }
                if (dwlB.RowType.Id == RowType.LeagueHeadLine)
                {
                    return 1;
                }
                return dwlA.RowType.Id.CompareTo(dwlB.RowType.Id);
            }
            return dwlA.GroupName.CompareTo(dwlB.GroupName);
        }

        public void ExportToExcel_VirtualSportsMain(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelVirtualSportsReport(param);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);

            if (dml2hReport != null)
            {
                ExportReport_VirtualSportsMain(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
            }

            (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
        }

        public void ExportToExcel_VirtualSportsMain_NoneLive(DailyWinLoss2hParam param)
        {

            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelVirtualSportsReport(param);

            if (dml2hReport != null)
            {
                ExportReport_VirtualSportsMain(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
            }

            (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
        }

        private void ExportReport_VirtualSportsMain(ref ExcelWorksheet wsReport,
            Report report,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
        {
            var reportHeaderTitle = string.Empty;
            if (isLive)
            {
                reportHeaderTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.MainReportHeader;
            }
            else
            {
                reportHeaderTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.MainReportHeader;
            }

            ExportHeader_VirtualSportsReportMain(ref wsReport, startCol, ref currentRow, reportHeaderTitle);
            ExportBody_VirtualSportsReportMain(ref wsReport, report, startCol, ref currentRow, param, isLive);
        }

        public void ExportToExcel_VirtualSportsBySport(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelVirtualSportsReportBySport(param);

            var excelGemboxUtils = new ExcelGemboxUtils();
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);

            if (dml2hReport != null)
            {
                ExportReport_VirtualSportsBySport(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
            }

            (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
        }

        public void ExportToExcel_VirtualSportsBySport_NoneLive(DailyWinLoss2hParam param)
        {
            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelVirtualSportsReportBySport(param);

            if (dml2hReport != null)
            {
                ExportReport_VirtualSportsBySport(ref wsReport, dml2hReport.NoneLiveReport, startCol, ref currentRow, param, false);
            }

            (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
        }

        private void ExportReport_VirtualSportsBySport(ref ExcelWorksheet wsReport,
            Report report,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
        {
            var reportHeaderTitle = string.Empty;
            if (isLive)
            {
                reportHeaderTitle = string.Format(Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.ReportBySportHeader, param.SportTypeName.ToUpper());
            }
            else
            {
                reportHeaderTitle = string.Format(Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.ReportBySportHeader, param.SportTypeName.ToUpper());
            }

            ExportHeader_VirtualSportsReportMain(ref wsReport, startCol, ref currentRow, reportHeaderTitle);
            ExportBody_VirtualSportsReportMain(ref wsReport, report, startCol, ref currentRow, param, isLive);
        }

        private void ExportHeader_VirtualSportsReportMain(ref ExcelWorksheet wsReport,
            int startCol,
            ref int currentRow,
            string reportHeaderTitle)
        {
            string typeTilte = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Bettype;
            string winLossTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.WinLoss;
            string positionTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Position;
            string commTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Commission;
            string lineTotalTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.LineTotal;
            string turnoverTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Turnover;
            string marginTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Margin;

            var currentCol = startCol;
            ExcelCell currentCell;
            CellRange currentCellRange;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var commStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, startCol + 6);
            currentCellRange.Merged = true;
            currentCellRange.Style = commStyle;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentCellRange.Value = reportHeaderTitle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow++;

            currentCol = startCol;
            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = typeTilte;
            wsReport.Columns[currentCol].Width = 6000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = positionTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = commTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = lineTotalTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;
            currentRow++;
        }

        private void ExportBody_VirtualSportsReportMain(ref ExcelWorksheet wsReport,
            Report report,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
        {
            var endCol = startCol + 6;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, false);
            var bgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);
            var currentCol = startCol;
            ExcelCell currentCell;

            var evenRowColor = Color.FromArgb(205, 222, 243);
            var evenRowcommonStyle = excelGemboxUtils.GetStyleText(evenRowColor.R, evenRowColor.G, evenRowColor.B);
            var evenRowbgFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, false, true);
            var evenRowbgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", evenRowColor, false, true);
            var evenRowbgTurnoverFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, true, true);
            var evenRownumberDefaultFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, false, false, false);

            if (report != null && report.ReportDetails.Count > 0)
            {
                var isEvenRow = true;
                foreach (var detail in report.ReportDetails)
                {
                    #region MainRow

                    currentCol = startCol;
                    currentCell = wsReport.Cells[currentRow, currentCol];
                    currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                    currentCell.Value = detail.AliasName;
                    currentCol++;

                    currentCell = wsReport.Cells[currentRow, currentCol];
                    currentCell.Style = (isEvenRow ? evenRowbgFormat : bgFormat);
                    currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    currentCell.Value = CastAide.AsDecimal(detail.WinLoss);
                    currentCol++;

                    currentCell = wsReport.Cells[currentRow, currentCol];
                    currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                    currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    currentCell.Value = CastAide.AsDecimal(detail.Position);
                    currentCol++;

                    currentCell = wsReport.Cells[currentRow, currentCol];
                    currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                    currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    currentCell.Value = CastAide.AsDecimal(detail.Commission);
                    currentCol++;

                    currentCell = wsReport.Cells[currentRow, currentCol];
                    currentCell.Style = (isEvenRow ? evenRowbgFormat : bgFormat);
                    currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    currentCell.Value = CastAide.AsDecimal(detail.LineTotal);
                    currentCol++;

                    currentCell = wsReport.Cells[currentRow, currentCol];
                    currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                    currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    currentCell.Value = CastAide.AsDecimal(detail.Turnover);
                    currentCol++;

                    currentCell = wsReport.Cells[currentRow, currentCol];
                    currentCell.Style = (isEvenRow ? evenRowbgFormatPercent : bgFormatPercent);
                    currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    if (Math.Round(detail.Turnover, 2) != 0)
                    {
                        currentCell.Value = CastAide.AsDouble(detail.Margin) / 100;
                    }
                    else
                    {
                        currentCell.Value = _notAvailableString;
                    }

                    currentCol++;
                    currentRow++;
                    isEvenRow = !isEvenRow;

                    #endregion MainRow
                }

                this.ExportEmptyRow(ref wsReport, ref currentRow, startCol, startCol + 6);

                #region TotalRow

                var normalTotal = report.Total.NormalTotal;
                currentCol = startCol;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = commonStyle;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Value = normalTotal.Name;
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Value = CastAide.AsDecimal(normalTotal.WinLoss);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Value = CastAide.AsDecimal(normalTotal.Position);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Value = CastAide.AsDecimal(normalTotal.Commission);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Style.FillPattern.SetSolid(Color.FromArgb(181, 196, 208));
                currentCell.Value = CastAide.AsDecimal(normalTotal.LineTotal);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
                currentCell.Value = CastAide.AsDecimal(normalTotal.Turnover);
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgFormatPercent;
                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                if (Math.Round(normalTotal.Turnover, 2) != 0)
                {
                    currentCell.Value = CastAide.AsDouble(normalTotal.Margin) / 100;
                }
                else
                {
                    currentCell.Value = _notAvailableString;
                }

                currentCol++;
                currentRow++;

                #endregion TotalRow

                ExportIBCTotalRow_VirtualSportsMain(ref wsReport, report.Total, startCol, ref currentRow, param, isLive);
            }
            else
            {
                this.ExportEmptyReport(ref wsReport, ref currentRow, startCol, startCol + 8);
            }
        }

        private void ExportIBCTotalRow_VirtualSportsMain(ref ExcelWorksheet wsReport,
            ReportTotal detail,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param,
            bool isLive)
        {
            decimal allIBCTotal = 0;
            if (_8rmUtils.IsActive)
            {
                allIBCTotal += detail.EightRMTotal.WinLoss;
            }

            if (_barmUtils.IsActive)
            {
                allIBCTotal += detail.BARMTotal.WinLoss;
            }

            if (allIBCTotal == 0)
            {
                return;
            }

            var currentCol = startCol;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            ExcelCell currentCell;
            CellRange currentCellRange;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = commonStyle;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
            var rmTotalText = string.Empty;
            if (_8rmUtils.IsActive && _barmUtils.IsActive)
            {
                rmTotalText = "IBC(" + _8rmUtils.DisplayName + " + " + _barmUtils.DisplayName + ") Total:";
            }
            else if (_8rmUtils.IsActive)
            {
                rmTotalText = "IBC(" + _8rmUtils.DisplayName + ") Total:";
            }
            else if (_barmUtils.IsActive)
            {
                rmTotalText = "IBC(" + _barmUtils.DisplayName + ") Total:";
            }

            currentCell.Value = rmTotalText;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCell.Value = CastAide.AsDecimal(allIBCTotal);
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 4);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Value = string.Empty;
            currentCol += 5;
            currentRow++;
        }

        public void ExportToExcel_DailyWLDirectDetails(DailyWinLoss2hParam param)
        {
            #region ExcelFormat

            const int startCol = 0;
            var currentRow = 0;
            var reportName = param.ReportFileName;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(param.DefaultSheetName.Replace("/", "_"));
            var dml2hReport = SelDailyWLDirectDetails(param);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, false);
            var bgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);

            var evenRowColor = Color.FromArgb(205, 222, 243);
            var evenRowcommonStyle = excelGemboxUtils.GetStyleText(evenRowColor.R, evenRowColor.G, evenRowColor.B);
            var evenRowbgFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, false, true);
            var evenRowbgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", evenRowColor, false, true);
            var evenRownumberDefaultFormat = excelGemboxUtils.GetStyleIntNumber(".00", evenRowColor, false, false, false);

            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;
            var isEvenRow = true;

            #endregion ExcelFormat

            var reportHeaderTitle = param.ReportTitle;
            ExportHeader_DailyWLDirectDetails(ref wsReport, startCol, ref currentRow, reportHeaderTitle, param);

            #region Body

            if (dml2hReport != null && dml2hReport.ReportDetails.Count > 0)
            {
                foreach (var detail in dml2hReport.ReportDetails)
                {
                    switch (detail.RowType.Id)
                    {
                        case RowType.Normal:
                            #region NormalRow

                            currentCol = startCol;
                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                            currentCell.Value = detail.MatchInfo.Name;
                            currentCol++;

                            var eventDetail = string.Empty;
                            if (param.BetType == Fanex.Tarus.Infrastructure.Constants.BetType.Outright ||
                                param.SportType == Fanex.Tarus.Infrastructure.Constants.Sports.Financials)
                            {
                                eventDetail = detail.MatchInfo.HomeTeam.Name + (detail.MatchInfo.IsNeutral ? " (N)" : string.Empty);
                                currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
                                currentCellRange.Merged = true;
                                currentCellRange.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                                currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                                currentCellRange.Value = eventDetail;
                                currentCol += 3;
                            }
                            else if (param.SportType != Fanex.Tarus.Infrastructure.Constants.Sports.NumberGame)
                            {
                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = detail.MatchInfo.HomeTeam.Name;
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                                currentCell.Value = "-vs-";
                                currentCol++;

                                currentCell = wsReport.Cells[currentRow, currentCol];
                                currentCell.Style = (isEvenRow ? evenRowcommonStyle : commonStyle);
                                currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                                currentCell.Value = detail.MatchInfo.AwayTeam.Name;
                                currentCol++;
                            }

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormat : bgFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.WinLoss);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Position);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Commission);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormat : bgFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.LineTotal);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRownumberDefaultFormat : numberDefaultFormat);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Turnover);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = (isEvenRow ? evenRowbgFormatPercent : bgFormatPercent);
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            if (Math.Round(detail.Turnover, 2) != 0)
                            {
                                currentCell.Value = CastAide.AsDouble(detail.Margin) / 100;
                            }
                            else
                            {
                                currentCell.Value = _notAvailableString;
                            }

                            currentCol++;
                            currentRow++;
                            isEvenRow = !isEvenRow;
                            break;

                            #endregion NormalRow
                        case RowType.SportTotal:
                            #region TotalRow

                            currentCol = startCol;
                            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 3);
                            currentCellRange.Merged = true;
                            currentCellRange.Style = commonStyle;
                            currentCellRange.Style.FillPattern.SetSolid(Color.FromArgb(124, 152, 164));
                            currentCellRange.Value = string.Empty;
                            currentCol += 4;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.WinLoss);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = numberDefaultFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Position);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = numberDefaultFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Commission);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.LineTotal);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = numberDefaultFormat;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            currentCell.Value = CastAide.AsDecimal(detail.Turnover);
                            currentCol++;

                            currentCell = wsReport.Cells[currentRow, currentCol];
                            currentCell.Style = bgFormatPercent;
                            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            if (Math.Round(detail.Turnover, 2) != 0)
                            {
                                currentCell.Value = CastAide.AsDouble(detail.Margin) / 100;
                            }
                            else
                            {
                                currentCell.Value = _notAvailableString;
                            }

                            currentCol++;
                            currentRow++;
                            isEvenRow = true;
                            break;

                            #endregion TotalRow
                        case RowType.EmptyRow:
                            isEvenRow = true;
                            break;
                        case RowType.LeagueHeadLine:

                            currentCol = startCol;
                            currentCol++;
                            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 8);
                            currentCellRange.Merged = true;
                            currentCellRange.Style = commonStyle;
                            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
                            currentCellRange.Value = detail.Name;
                            currentCol += 9;
                            currentRow++;
                            isEvenRow = true;
                            break;
                    }
                }

                ExportNormalTotal_DailyWLDirectDetails(ref wsReport, dml2hReport.Total.NormalTotal, startCol, ref currentRow, param);
                ExportIBCTotal_DailyWLDirectDetails(ref wsReport, dml2hReport.Total, startCol, ref currentRow, param);
            }
            else
            {
                this.ExportEmptyReport(ref wsReport, ref currentRow, startCol, startCol + 9);
            }

            #endregion Body

            (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
        }

        private void ExportHeader_DailyWLDirectDetails(ref ExcelWorksheet wsReport,
            int startCol,
            ref int currentRow,
            string reportHeaderTitle,
            DailyWinLoss2hParam param)
        {
            string typeTilte = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Event;
            string winLossTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.WinLoss;
            string positionTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Position;
            string commTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Commission;
            string lineTotalTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.LineTotal;
            string turnoverTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Turnover;
            string marginTitle = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Margin;

            var currentCol = startCol;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);

            var currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startCol, currentRow, startCol + 9);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentCellRange.Value = reportHeaderTitle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentRow++;

            currentCol = startCol;
            var currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Code;
            wsReport.Columns[currentCol].Width = 5000;
            currentCol++;

            var teamNameColWidth = 4000;
            if (param.SportType != Fanex.Tarus.Infrastructure.Constants.Sports.NumberGame)
            {
                currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
                currentCellRange.Merged = true;
                currentCellRange.Value = typeTilte;
                currentCellRange.Style = bgHeader;

                wsReport.Columns[currentCol].Width = teamNameColWidth;
                currentCol++;
                wsReport.Columns[currentCol].Width = 1000;
                currentCol++;
                wsReport.Columns[currentCol].Width = teamNameColWidth;
                currentCol++;
            }

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = winLossTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = positionTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = commTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = lineTotalTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = turnoverTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgHeader;
            currentCell.Value = marginTitle;
            wsReport.Columns[currentCol].Width = 4000;
            currentCol++;
            currentRow++;
        }

        private void ExportNormalTotal_DailyWLDirectDetails(ref ExcelWorksheet wsReport,
            ReportDetail total,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param)
        {
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, false);
            var bgFormatPercent = excelGemboxUtils.GetStyleIntNumber(".00%", bgColor, false, true);

            var totalSummary = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.TotalSummary;
            ExcelCell currentCell;
            CellRange currentCellRange;
            var currentCol = startCol;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 3);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[currentRow, currentCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCellRange.Value =
                (param.IsLive != 0 ? "Live! " : string.Empty) + CacheRef.GetBettype(param.BetType) +
                (param.IsHedge ? " (Hegde)" : string.Empty) + " " + totalSummary;
            currentCol += 4;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.WinLoss);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.Position);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.Commission);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.LineTotal);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = numberDefaultFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Value = CastAide.AsDecimal(total.Turnover);
            currentCol++;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormatPercent;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            if (Math.Round(total.Turnover, 2) != 0)
            {
                currentCell.Value = CastAide.AsDouble(total.Margin) / 100;
            }
            else
            {
                currentCell.Value = _notAvailableString;
            }

            currentCol++;
            currentRow++;
        }

        private void ExportIBCTotal_DailyWLDirectDetails(ref ExcelWorksheet wsReport,
            ReportTotal detail,
            int startCol,
            ref int currentRow,
            DailyWinLoss2hParam param)
        {
            decimal allIBCTotal = 0;
            if (_8rmUtils.IsActive)
            {
                allIBCTotal += detail.EightRMTotal.WinLoss;
            }

            if (_barmUtils.IsActive)
            {
                allIBCTotal += detail.BARMTotal.WinLoss;
            }

            if (allIBCTotal == 0)
            {
                return;
            }

            var currentCol = startCol;
            var excelGemboxUtils = new ExcelGemboxUtils();
            var bgColor = Color.FromArgb(255, 255, 255);
            var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            var bgFormat = excelGemboxUtils.GetStyleIntNumber(".00", bgColor, false, true);
            ExcelCell currentCell;
            CellRange currentCellRange;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 3);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            var rmTotalText = string.Empty;
            if (_8rmUtils.IsActive && _barmUtils.IsActive)
            {
                rmTotalText = "IBC(" + _8rmUtils.DisplayName + " + " + _barmUtils.DisplayName + ") Total:";
            }
            else if (_8rmUtils.IsActive)
            {
                rmTotalText = "IBC(" + _8rmUtils.DisplayName + ") Total:";
            }
            else if (_barmUtils.IsActive)
            {
                rmTotalText = "IBC(" + _barmUtils.DisplayName + ") Total:";
            }

            currentCellRange.Value = rmTotalText;
            currentCol += 4;

            currentCell = wsReport.Cells[currentRow, currentCol];
            currentCell.Style = bgFormat;
            currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            currentCell.Style.Font.Weight = ExcelFont.BoldWeight;
            currentCell.Value = CastAide.AsDecimal(allIBCTotal);
            currentCol++;

            currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 4);
            currentCellRange.Merged = true;
            currentCellRange.Style = commonStyle;
            currentCellRange.Value = string.Empty;
            currentCol += 5;
        }

        private void ExportEmptyReport(ref ExcelWorksheet wsReport,
            ref int currentRow,
            int startColumn,
            int endColumn)
        {
            var currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startColumn, currentRow, endColumn);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var textStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            currentCellRange.Merged = true;
            currentCellRange.Value = Labels.EmptyReportMessage;
            currentCellRange.Style = textStyle;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentRow++;
        }

        private void ExportEmptyRow(ref ExcelWorksheet wsReport,
            ref int currentRow,
            int startColumn,
            int endColumn)
        {
            var currentCellRange = wsReport.Cells.GetSubrangeAbsolute(currentRow, startColumn, currentRow, endColumn);
            var excelGemboxUtils = new ExcelGemboxUtils();
            var textStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
            currentCellRange.Merged = true;
            currentCellRange.Style = textStyle;
            currentCellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            currentRow++;
        }

        #region Utils

        private decimal CalculateMargin(ReportDetail item)
        {
            decimal result = 0;
            if (item != null && item.Turnover != 0)
            {
                result = (item.LineTotal / item.Turnover) * 100;
            }
            return result;
        }

        private string GetBetTypeName(ReportDetail dwl2hItem)
        {
            string bettypeName = dwl2hItem.BetType.Name;

            if (string.IsNullOrEmpty(dwl2hItem.BetType.Name))
            {
                switch (dwl2hItem.BetType.Id)
                {
                    case BetType.CasinoGames:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Casino;
                        break;
                    case BetType.P2P:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.P2P;
                        break;
                    case RowType.IBCBingo:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.IBCBingo;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(dwl2hItem.Hedge) && dwl2hItem.Hedge.Contains(_headgeGameSuffix))
            {
                switch (dwl2hItem.BetType.Id)
                {
                    case BetType.Handicap:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Handicap + " " + _headgeGameSuffix;
                        break;
                    case BetType.OverUnder:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.OverUnder + " " + _headgeGameSuffix;
                        break;
                    case BetType.FirstHandicap:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.FirstHandicap + " " + _headgeGameSuffix;
                        break;
                    case BetType.FirstOverUnder:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.FirstOverUnder + " " + _headgeGameSuffix;
                        break;
                }
            }
            else
            {
                switch (dwl2hItem.BetType.Id)
                {
                    case BetType.Handicap:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Handicap;
                        break;
                    case BetType.OverUnder:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.OverUnder;
                        break;
                    case BetType.CorrectScore:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.Casino;
                        break;
                    case BetType.FirstCorrectScore:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.FirstCorrectScore;
                        break;
                    case BetType.TotalGoal:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.TotalGoal;
                        break;
                    case BetType.FirstHandicap:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.FirstHandicap;
                        break;
                    case BetType.FirstOverUnder:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.FirstOverUnder;
                        break;
                    case BetType.FirstGoal_LastGoal:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.FirstGoal_LastGoal;
                        break;
                    case RowType.KickOff:
                        bettypeName = Fanex.Tarus.VirtualSport.Resources.DailyWinLoss2h.KickOff;
                        break;
                    default:
                        break;
                }
            }

            return bettypeName;
        }

        #endregion Utils
        #endregion IMethods
    }
}
