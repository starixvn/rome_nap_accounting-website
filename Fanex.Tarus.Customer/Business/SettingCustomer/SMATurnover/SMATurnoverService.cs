// --------------------------------------------------------------------------
// <copyright file="SMATurnoverService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Customer.Criteria.SMATurnover;
    using Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Accounting.Core.Cache;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Helper;
    using System.Drawing;

    public class SMATurnoverService : ISMATurnoverService
    {
        public ISMATurnoverAccess _SMATurnoverAccess;
        private Dictionary<object, string> _teamNameCache;

        public SMATurnoverService()
            : this(null)
        { }

        public SMATurnoverService(ISMATurnoverAccess SMATurnoverAccess)
        {
            _SMATurnoverAccess = SMATurnoverAccess ?? new SMATurnoverAccess();
        }

        public List<Sport> GetSportType(int mode)
        {
            var sportList = new List<Sport>();
            DataTable dtb = _SMATurnoverAccess.GetSportType(mode);
            foreach (DataRow row in dtb.Rows)
            {
                var sport = new Sport()
                {
                    Id = CastAide.AsInt32(row["sporttype"]),
                    Name = CastAide.AsString(row["sportname"])
                };
                sportList.Add(sport);
            }
            return sportList;
        }

        public List<League> GetLeague(int sportType, DateTime fromDate, DateTime toDate)
        {
            var leagueList = new List<League>();
            var dtb = _SMATurnoverAccess.GetLeague(sportType, fromDate, toDate);
            foreach (DataRow row in dtb.Rows)
            {
                var league = new League()
                {
                    Id = CastAide.AsInt32(row["leagueid"]),
                    Name = CastAide.AsString(row["leaguename"])
                };
                leagueList.Add(league);
            }
            return leagueList;
        }

        public MatchInfo GetInfoByMatchId(int matchId)
        {
            var matchInfo = _SMATurnoverAccess.GetInfoByMatchId(matchId);
            return matchInfo;
        }

        public List<CustomerTurnoverDetail> GetSMATurnoverList(int sportId, int leagueId, DateTime fromDate, DateTime toDate)
        {
            var leagueList = new List<CustomerTurnoverDetail>();
            try
            {
                var dtb = _SMATurnoverAccess.GetSMATurnoverList(sportId, leagueId, fromDate, toDate);
                var query = from league in dtb.AsEnumerable()
                            group league by league.Field<string>("leaguename") into dtbLeague
                            from match in
                                (from league in dtbLeague.AsEnumerable()
                                 group league by league.Field<int>("matchid"))
                            group match by dtbLeague.Key;

                foreach (var leagueGroup in query)
                {
                    var League = new CustomerTurnoverDetail();
                    League.LeagueName = leagueGroup.Key;
                    var matchList = new List<CustomerTurnoverDetail>();
                    foreach (var matchGroup in leagueGroup)
                    {
                        var Match = new CustomerTurnoverDetail();
                        Match.MatchId = matchGroup.Key.ToString();
                        var customerList = new List<CustomerTurnoverDetail>();
                        var i = 0;
                        foreach (var detail in matchGroup)
                        {
                            if (i == 0)
                            {
                                Match.EventDate = string.Format("{0:MM/dd/yyyy}", detail["eventdate"]);
                                Match.HomeTeam = CacheRef.GetTeamName(detail["homeid"]);
                                Match.AwayTeam = CacheRef.GetTeamName(detail["awayid"]);
                                Match.EventStatus = CastAide.AsString(detail["eventstatus"]);
                            }
                            var Customer = new CustomerTurnoverDetail()
                            {
                                Username = detail["username"].ToString(),
                                CustId = detail["custid"].ToString(),
                                Currency = detail["currency"].ToString(),
                                SMATurnover = NumberFormat.DecFormat(detail["TotalTurnover"], 2),
                                MaxTurnover = NumberFormat.DecFormat(detail["MaxTurnover"], 0),
                                PercentageUsed = NumberFormat.DecFormat(detail["PercentageUsed"], 0)
                            };
                            i++;
                            customerList.Add(Customer);
                        }
                        Match.CustomerList = customerList;
                        matchList.Add(Match);
                    }
                    League.CustomerList = matchList;
                    leagueList.Add(League);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return leagueList;
        }

        public Result AddSMATurnover(int matchId, string custName, decimal maxTurnover)
        {
            return _SMATurnoverAccess.AddSMATurnover(matchId, custName, maxTurnover, AppSession.AccId, HttpContext.Current.Request.UserHostAddress);
        }

        public Result UpdateSMATurnover(int matchId, int custId, decimal maxTurnover)
        {
            return _SMATurnoverAccess.UpdateSMATurnover(matchId, custId, maxTurnover, AppSession.AccId, HttpContext.Current.Request.UserHostAddress);
        }

        public Result CheckMaxTurnoverValue(int matchId,
            string custName,
            decimal maxTurnover,
            int mode)
        {
            return _SMATurnoverAccess.CheckMaxTurnoverValue(matchId, custName, maxTurnover, mode);
        }

        public AddSMAListResult UploadSMAList(HttpPostedFileBase fileUploaded,
            AddSMAListCriteria criteria)
        {
            try
            {
                var addResult = new AddSMAListResult
                {
                    FailedInfoList = new List<AddSMAListResult>(),
                    SucceededInfoList = new List<AddSMAListResult>()
                };

                var smaList = this.ReadSMAList(fileUploaded);
                var matchIdList = criteria.MatchIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var smaListXMLString = this.GetSMAListXMLString(smaList, matchIdList);

                Hashtable outputParams = new Hashtable();
                var rawData = this._SMATurnoverAccess.CheckSMAList(new CheckSMAListCriteria
                {
                    ListSMA = smaListXMLString
                }, ref outputParams);

                var succeededTable = rawData[0];
                var smaListLength = smaList.Count;
                var matchIdListLength = matchIdList.Length;

                var matchGroups = rawData.GroupBy(data => data.MatchID);
                foreach (var matchGroup in matchGroups)
                {
                    var succeededMatchInfo = new AddSMAListResult
                    {
                        SucceededInfoList = new List<AddSMAListResult>(),
                        MatchId = CastAide.AsInt32(matchGroup.Key)
                    };

                    var matchGroupListInfo = matchGroup.ToList();
                    var matchGroupsLength = matchGroupListInfo.Count;
                    for (var j = 0; j < matchGroupsLength; j++)
                    {
                        var smaInfo = matchGroupListInfo[j];
                        succeededMatchInfo.SucceededInfoList.Add(new AddSMAListResult
                        {
                            Username = CastAide.AsString(smaInfo.SMAName),
                            MaxTurnover = CastAide.AsDecimal(smaInfo.MaxTurnover),
                            MatchId = CastAide.AsString(smaInfo.MatchID)
                        });
                    }

                    addResult.SucceededInfoList.Add(succeededMatchInfo);
                }

                var failedTable = rawData[1];
                foreach (var data in failedTable)
                {
                    addResult.FailedInfoList.Add(new AddSMAListResult
                    {
                        Username = CastAide.AsString(data.SMAName),
                        MaxTurnover = CastAide.AsDecimal(data.MaxTurnover),
                        MatchId = CastAide.AsString(data.MatchID),
                        AddResultMessage = CastAide.AsString(data.Reason)
                    });
                }

                return addResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        private List<AddSMAListResult> ReadSMAList(HttpPostedFileBase fileUploaded)
        {
            var smaList = new List<AddSMAListResult>();

            var dtSMAs = new DataTable();
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(fileUploaded.InputStream);
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();
            dtSMAs.Columns.Add("SMA Name");
            dtSMAs.Columns.Add("Max Turnover");

            #region ReadFile

            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dtSMAs.NewRow();
                var count = 0;
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);
                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.NUMERIC:
                                dr[i] = cell.NumericCellValue;
                                break;
                            default:
                                try
                                {
                                    if (string.IsNullOrEmpty(cell.StringCellValue))
                                    {
                                        count++;
                                        break;
                                    }

                                    dr[i] = cell.StringCellValue;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    LogExtensions.ExceptionHandler(ex);
                                    throw new Exception("Input invalid at row " + (row.RowNum + 1) + " cell " + (i + 1) + "!");
                                }
                        }
                    }
                    else
                    {
                        count++;
                    }
                }

                if (count != row.LastCellNum && row.RowNum > 0)
                {
                    dtSMAs.Rows.Add(dr);
                    smaList.Add(new AddSMAListResult
                    {
                        Username = CastAide.AsString(dr[0]),
                        MaxTurnover = CastAide.AsDecimal(dr[1])
                    });
                }
            }

            #endregion ReadFile

            return smaList;
        }

        public MatchInfo GetMatchInfo_AddSMAList(GetMatchInfo_AddSMAListCriteria criteria)
        {
            try
            {
                var result = new MatchInfo
                {
                    ChildsInfo = new List<MatchInfo>()
                };

                Hashtable outputParams = new Hashtable();

                var rawData = this._SMATurnoverAccess.GetMatchInfo_AddSMAList(criteria,
                    ref outputParams);

                var homeIdList = rawData.Select(data => CastAide.AsInt32(data.homeid))
                                   .ToList();
                this.SaveCacheRefReponse(homeIdList, CacheRef.GetTeamName, ref this._teamNameCache);
                var awayIdList = rawData.Select(data => CastAide.AsInt32(data.awayid))
                                        .ToList();
                this.SaveCacheRefReponse(awayIdList, CacheRef.GetTeamName, ref this._teamNameCache);

                var dateGroups = rawData.GroupBy(data => data.eventdate);
                foreach (var dateGroup in dateGroups)
                {
                    var dateInfo = new MatchInfo
                    {
                        EventDate = string.Format("{0:M/d/yyyy}", CastAide.AsDateTime(dateGroup.Key)),
                        ChildsInfo = new List<MatchInfo>()
                    };

                    dateInfo.Id = dateInfo.EventDate.Replace("/", "_");
                    foreach (var data in dateGroup)
                    {
                        var matchInfo = new MatchInfo
                        {
                            Id = CastAide.AsString(data.matchid),
                            EventDate = dateInfo.EventDate,
                            LeagueId = CastAide.AsString(data.leagueid),
                            HomeTeamId = CastAide.AsInt32(data.homeid),
                            AwayTeamId = CastAide.AsInt32(data.awayid)
                        };

                        matchInfo.HomeTeam = this._teamNameCache[matchInfo.HomeTeamId];
                        matchInfo.AwayTeam = this._teamNameCache[matchInfo.AwayTeamId];
                        dateInfo.ChildsInfo.Add(matchInfo);
                    }

                    result.ChildsInfo.Add(dateInfo);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        public Dictionary<object, string> SaveCacheRefReponse(List<object> idList,
           Func<object, string> cacheRefFunction,
           ref Dictionary<object, string> currentStorage)
        {
            currentStorage = currentStorage ?? new Dictionary<object, string>();
            foreach (var idInfo in idList)
            {
                if (!currentStorage.ContainsKey(idInfo))
                {
                    currentStorage.Add(idInfo, cacheRefFunction(idInfo).ToString());
                }
            }

            return currentStorage;
        }

        private string GetSMAListXMLString(List<AddSMAListResult> smaList,
            string[] matchIdList)
        {
            var smaListXMLString = string.Empty;
            smaListXMLString += "<ListSMA>";
            var smaListLength = smaList.Count;
            var matchIdListLength = matchIdList.Length;
            var smaIndex = 1;
            for (var i = 0; i < matchIdListLength; i++)
            {
                var matchId = matchIdList[i];
                for (var j = 0; j < smaListLength; j++)
                {
                    var smaInfo = smaList[j];
                    var smaName_Encode = HttpUtility.HtmlEncode(
                       string.IsNullOrEmpty(smaInfo.Username) ?
                       string.Empty :
                       smaInfo.Username.Trim());
                    var matchId_Encode = HttpUtility.HtmlEncode(
                       string.IsNullOrEmpty(matchId) ?
                       string.Empty :
                       matchId.Trim());
                    var maxTurnover_Encode = HttpUtility.HtmlEncode(
                       string.IsNullOrEmpty(smaInfo.MaxTurnover.ToString()) ?
                       string.Empty :
                       smaInfo.MaxTurnover.ToString().Trim());

                    smaListXMLString += "<SMA ";
                    smaListXMLString += "Seq=\"" + smaIndex + "\" ";
                    smaListXMLString += "SMAName=\"" + smaName_Encode + "\" ";
                    smaListXMLString += "MatchID=\"" + matchId_Encode + "\" ";
                    smaListXMLString += "MaxTurnover=\"" + maxTurnover_Encode + "\" ";
                    smaListXMLString += "></SMA>";
                    smaIndex++;
                }
            }

            smaListXMLString += "</ListSMA>";
            return smaListXMLString;
        }

        public void DownloadTemplate_AddSMAList()
        {
            try
            {
                #region ExcelFormat
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add("SMA List");
                var reportName = "Template_Add_SMA_List";

                var excelGemboxUtils = new ExcelGemboxUtils();
                var bgHeader = excelGemboxUtils.GetStyleTitle(Color.FromArgb(205, 222, 255));
                var commonStyle = excelGemboxUtils.GetStyleText(255, 255, 255);
                var numberDefaultFormat = excelGemboxUtils.GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false);

                //var maxColIndex = 1;
                var startCol = 0;
                var startRow = 0;
                var currentCol = startCol;
                var currentRow = startRow;
                ExcelCell currentCell;

                #endregion ExcelFormat
                #region Header

                currentCol = startCol;
                wsReport.Columns[currentCol++].Width = 4000;
                wsReport.Columns[currentCol++].Width = 4000;

                currentCol = startCol;
                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgHeader;
                currentCell.Value = "SMA Name";
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = bgHeader;
                currentCell.Value = "Max Turnover";
                currentCol++;
                currentRow++;

                #endregion Header
                #region Content

                // Sample Row 1
                currentCol = startCol;
                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = commonStyle;
                currentCell.Value = "super01";
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Value = 1000;
                currentCol++;
                currentRow++;

                // Sample Row 2
                currentCol = startCol;
                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = commonStyle;
                currentCell.Value = "super02";
                currentCol++;

                currentCell = wsReport.Cells[currentRow, currentCol];
                currentCell.Style = numberDefaultFormat;
                currentCell.Value = 5000;
                currentCol++;
                currentRow++;

                #endregion Content

                ReportService.RespondStream(excelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}
