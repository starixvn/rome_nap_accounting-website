using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.SpecialDelete.TicketsListing;
using Sunplus.Accounting.Entities.SpecialDelete.TicketsListing;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.Business.SpecialDelete.TicketsListing
{
    public class TicketsListing : ITicketsListing
    {
        private int parentBettype = -1;

        private int MaxRows = 5000;

        private ITicketsListingDAO _ticketsListingDao;
        private DataAccess.IBetListDAO _betlistDAO;

        public TicketsListing(ITicketsListingDAO iTicketsListingDao)
        {
            _ticketsListingDao = iTicketsListingDao;
        }

        #region SelAllLicensee

        public LicenseeDTO SelAllLicensee()
        {
            LicenseeDTO licensee = new LicenseeDTO();
            licensee.Items = new List<Licensee>();

            try
            {
                DataTable dataTable = new DataTable();

                DbResult dbResult = _ticketsListingDao.SelAllLicensee();
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

                DataRow[] rows = dataTable.Select();

                foreach (var row in rows)
                {
                    licensee.Items.Add(new Licensee()
                                           {
                                               Id = CastAide.AsInt32(row["LicenseeID"]),
                                               Name = CastAide.AsString(row["LicenseeName"])
                                           });
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licensee;
        }

        #endregion SelAllLicensee

        #region SelBettype

        public BettypeDTO SelBettype()
        {
            BettypeDTO bettype = new BettypeDTO();
            bettype.ItemsSB = new List<Bettype>();
            bettype.ItemsNGLive = new List<Bettype>();
            bettype.ItemsNGNoneLive = new List<Bettype>();
            bettype.ItemsVS = new List<Bettype>();

            try
            {
                DataTable dataTable = new DataTable();

                DbResult dbResult = _ticketsListingDao.SelBettype();
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

                DataRow[] rows = dataTable.Select();

                foreach (var row in rows)
                {
                    int type = CastAide.AsInt32(row["Type"]);
                    switch (type)
                    {
                        case 0:
                            bettype.ItemsSB.Add(new Bettype()
                            {
                                Id = CastAide.AsInt32(row["typeid"]),
                                Name = CastAide.AsString(row["bettypename"]),
                                Type = type,
                                LiveIndicator = 0
                            });
                            break;

                        case 1:
                            int bt = CastAide.AsInt32(row["typeid"]);
                            int liveIndicator = CastAide.AsInt32(row["liveindicator"]);
                            if (liveIndicator == 1)
                            {
                                bettype.ItemsNGLive.Add(new Bettype
                                {
                                    Id = bt,
                                    Name = GetBettypeName(bt, liveIndicator == 1),
                                    Type = type,
                                    LiveIndicator = liveIndicator
                                });
                            }
                            else
                            {
                                bettype.ItemsNGNoneLive.Add(new Bettype
                                {
                                    Id = bt,
                                    Name = GetBettypeName(bt, liveIndicator == 1),
                                    Type = type,
                                    LiveIndicator = liveIndicator
                                });
                            }
                            break;

                        case 2:
                            bettype.ItemsVS.Add(new Bettype()
                            {
                                Id = CastAide.AsInt32(row["typeid"]),
                                Name = CastAide.AsString(row["bettypename"]),
                                Type = type,
                                LiveIndicator = 0
                            });
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return bettype;
        }

        #endregion SelBettype

        #region SelTicketsListing

        public ProductListingDTO SelTicketsListing(int matchID, int licenseeID, string betTypes, string betTypesLive, int Type, int GroupBy, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            ProductListingDTO product = new ProductListingDTO();
            product.Items = new List<ProductListing>();

            try
            {
                DbResult rs = _ticketsListingDao.SelTicketsListing(matchID, licenseeID, betTypes, betTypesLive, Type, LiveType, HomeScore, AwayScore, FromTransDate, ToTransDate, SAFilter);
                product.ErrCode = CastAide.AsInt16(rs.Outputs["errcode"]);

                if (product.ErrCode > 0)
                {
                    product.ErrMsg = CastAide.AsString(rs.Outputs["errmsg"]);
                    return product;
                }
                else
                {
                    if (rs.ResultSet.Tables.Count == 0) return product;

                    DataTable DtMatchInfo = rs.ResultSet.Tables[0];
                    int isNeutral = CastAide.AsInt16(DtMatchInfo.Rows[0]["isneutral"]);
                    product.MatchInfo = new MatchInfo();
                    product.MatchInfo.home = CacheRef.GetTeamName(DtMatchInfo.Rows[0]["homeid"]) + (isNeutral == 1 ? "(N)" : "");
                    product.MatchInfo.away = CacheRef.GetTeamName(DtMatchInfo.Rows[0]["awayid"]);
                    product.MatchInfo.matchcode = CastAide.AsString(DtMatchInfo.Rows[0]["matchcode"]);
                    product.MatchInfo.eventdate = CastAide.AsDateTime(DtMatchInfo.Rows[0]["eventdate"]).ToShortDateString();

                    DataTable DtBetCount = rs.ResultSet.Tables[1];
                    if (GroupBy == 0)
                    {
                        product.Items = assignTicketsListingGroupByLicensee(DtBetCount);
                    }
                    else
                    {
                        product.Items = assignTicketsListingGroupByBettype(DtBetCount);
                    }
                    return product;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                product.ErrMsg = ex.Message;
            }

            return product;
        }

        private List<ProductListing> assignTicketsListingGroupByLicensee(DataTable tb)
        {
            var mainItems = getDistinc(0, tb);
            var product = new List<ProductListing>();
            foreach (string s in mainItems)//s:licenseeid
            {
                var betcout = new ProductListing
                {
                    Name = s,
                    TotalTrans = CastAide.AsInt32(tb.Compute("SUM(TotalTrans)", "LicenseeName='" + s + "'")),
                    Id = s,
                    LiveIndicator = 0,
                    Items = new List<ProductListing>()
                };

                List<DataRow> rows = tb.AsEnumerable().Where(r => CastAide.AsString(r["LicenseeName"]) == s).Select(r => r).ToList();
                foreach (DataRow row in rows)
                {
                    int liveIndicator = CastAide.AsInt32(row["liveindicator"]);

                    var betcountdetail = new ProductListing
                    {
                        Name = GetBettypeName(CastAide.AsInt32(row["bettype"]), liveIndicator == 1),
                        TotalTrans = CastAide.AsInt32(row["TotalTrans"]),
                        LiveIndicator = liveIndicator,
                        Id = row["bettype"].ToString()
                    };
                    betcout.Items.Add(betcountdetail);
                }
                product.Add(betcout);
            }
            return product;
        }

        private List<ProductListing> assignTicketsListingGroupByBettype(DataTable tb)
        {
            var mainItems = getDistinc(tb);
            var product = new List<ProductListing>();
            foreach (NGBettype s in mainItems)//s:NGBettype
            {
                var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(CastAide.AsInt32(s.ID));
                if (s.ID == "86")
                {
                    bettypeName = s.LiveIndicator == 1 ? "Next Odd/Even" : "FT. Odd/Even";
                }
                else if (s.ID == "85")
                {
                    bettypeName = "Next Over/Under";
                }
                else if (s.ID == "90")
                {
                    bettypeName += s.LiveIndicator == 1 ? " (Live Game)" : " (Pre Game)";
                }

                var betcount = new ProductListing
                {
                    Name = bettypeName,
                    TotalTrans = CastAide.AsInt32(tb.Compute("SUM(TotalTrans)", "bettype='" + s.ID + "' and liveindicator=" + s.LiveIndicator)),
                    Id = s.ID,
                    LiveIndicator = s.LiveIndicator,
                    Items = new List<ProductListing>()
                };

                List<DataRow> rows = tb.AsEnumerable().Where(r => CastAide.AsString(r["bettype"]) == s.ID && CastAide.AsInt16(r["liveindicator"]) == betcount.LiveIndicator).Select(r => r).ToList();
                foreach (DataRow row in rows)
                {
                    var betcountdetail = new ProductListing
                    {
                        Name = row["licenseename"].ToString(),
                        TotalTrans = CastAide.AsInt32(row["totaltrans"]),
                        LiveIndicator = CastAide.AsInt16(row["liveindicator"]),
                        Id = row["licenseename"].ToString()
                    };
                    betcount.Items.Add(betcountdetail);
                }
                product.Add(betcount);
            }
            return product;
        }

        public TicketsListingBetlistDTO GetTicketsListingBetlist(bool fromCache, int pageSize, int pageIndex, bool isMain, int Type, int matchID, string Licensees, string bettypes, string bettypesLive, int groupBy, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            _betlistDAO = new Sunplus.Accounting.DataAccess.AdoNet.BetListDAO();
            TicketsListingBetlistDTO ticketListing = _betlistDAO.GetTicketsListingBetlist(fromCache, isMain, Type, matchID, Licensees, bettypes, bettypesLive, groupBy, 0, LiveType, HomeScore, AwayScore, FromTransDate, ToTransDate, SAFilter);
            TicketsListingBetlistDTO ticketListingPaging = new TicketsListingBetlistDTO();
            ticketListingPaging.Items = ticketListing.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            ticketListingPaging.totalItem = ticketListing.totalItem;
            return ticketListingPaging;
        }

        #endregion SelTicketsListing

        private string GetBettypeName(int bettype, bool liveIndicator)
        {
            var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bettype);
            if (bettype == 86)
            {
                bettypeName = liveIndicator ? "Next Odd/Even" : "FT. Odd/Even";
            }
            else if (bettype == 85)
            {
                bettypeName = "Next Over/Under";
            }
            else if (bettype == 90)
            {
                bettypeName += liveIndicator ? " (Live Game)" : " (Pre Game)";
            }
            return bettypeName;
        }

        private List<string> getDistinc(int type, DataTable tb)
        {
            if (type == 0)
            {
                return tb.AsEnumerable().Select(s => CastAide.AsString(s["LicenseeName"])).Distinct().ToList();
            }
            else
            {
                return tb.AsEnumerable().Select(s => CastAide.AsString(s["bettype"])).Distinct().ToList();
            }
        }

        private List<NGBettype> getDistinc(DataTable tb)
        {
            var list = new List<NGBettype>();
            foreach (DataRow row in tb.Rows)
            {
                if (!list.Exists(s => s.ID == CastAide.AsString(row["bettype"]) && s.LiveIndicator == CastAide.AsInt16(row["liveindicator"])))
                {
                    list.Add(new NGBettype { ID = row["bettype"].ToString(), LiveIndicator = CastAide.AsInt16(row["liveindicator"]) });
                }
            }
            return list;
        }

        public class NGBettype
        {
            public string ID { get; set; }

            public int LiveIndicator { get; set; }
        }

        public void ExportExcelTicketListing(bool isMain, int _type, int groupBy, int matchID, string Licensees, string bettypes,
            int Language, string bettypesLive, string matchCode, string eventDate, string title, string home, string away, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            var response = HttpContext.Current.Response;

            #region getdata

            var betListManager = new Sunplus.Accounting.Business.BetList.BetListManager();
            var ticketsListingBetlistDTO = betListManager.GetTicketsListingBetlist(isMain, _type, matchID, Licensees, bettypes, bettypesLive, groupBy, Language, LiveType, HomeScore, AwayScore, FromTransDate, ToTransDate, SAFilter);

            #endregion getdata

            response.Clear();
            response.AddHeader("content-disposition", "attachment;filename=ticketlist_" + Licensees + ".xls");
            response.Charset = "utf-8";
            response.ContentType = "application/vnd.xls";

            #region excel

            var excelFile = new ExcelUtil.Spreadsheet.ExcelFile();

            int currentSheetIndex = 1;

            var wsReport = excelFile.Worksheets.Add("Sheet" + currentSheetIndex.ToString());

            int currentIndexColumn = 0;
            int currentIndexRow = 0;

            #region title

            currentIndexColumn = 5;
            wsReport.Rows[currentIndexRow].Height = 255 * 2;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Style.Font.Size = 288;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Value = title;

            wsReport.Columns[0].Width = 255 * 16;
            wsReport.Columns[1].Width = 255 * 18;
            wsReport.Columns[2].Width = 255 * 18;
            wsReport.Columns[3].Width = 255 * 32;
            wsReport.Columns[8].Width = 255 * 16;
            if (_type == 2)
            {
                wsReport.Columns[8].Width += 255 * 4;
            }

            #endregion title

            #region section 1

            #region header

            currentIndexRow += 2;
            currentIndexColumn = 0;

            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "CODE";

            ++currentIndexColumn;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Style.Font.Size = 164;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Value = "MATCH EVENT";

            currentIndexColumn += 2;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "EVENT DATE";

            #endregion header

            #region body

            ++currentIndexRow;
            currentIndexColumn = 0;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                    MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                    , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = matchCode;

            ++currentIndexColumn;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Style.Font.Size = 164;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, currentIndexColumn, currentIndexRow, currentIndexColumn + 1).Value = home + "   -vs-   " + away;

            currentIndexColumn += 2;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                    MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                    , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = eventDate;

            #endregion body

            #endregion section 1

            #region section 2

            #region header

            currentIndexRow += 2;
            currentIndexColumn = 0;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Ref.no";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Trans.Time";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Username";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Choice";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Odds";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Stake";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Winlost";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Currency";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Bettype";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Status";

            if (_type != 2)
            {
                ++currentIndexColumn;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                    , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "S.A";
            }

            #endregion header

            #region body

            if (ticketsListingBetlistDTO == null || ticketsListingBetlistDTO.Items.Count <= 0)
            {
                ++currentIndexRow;
                if (_type == 2)
                {
                    currentIndexColumn = 8;
                }
                else
                {
                    currentIndexColumn = 9;
                }
                wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Style.Font.Size = 164;
                wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(currentIndexRow, 0, currentIndexRow, currentIndexColumn).Value =
                    "There is no information";
                goto end;
            }
            int count = 0;
            for (int i = 0; i < ticketsListingBetlistDTO.Items.Count; ++i)
            {
                ++count;
                if (count <= MaxRows)
                {
                    var ticket = ticketsListingBetlistDTO.Items[i];
                    BuildRow(ref wsReport, ref currentIndexColumn, ref currentIndexRow, ticket, _type);
                }
                else
                {
                    NewSheet(ticketsListingBetlistDTO, currentSheetIndex, i, excelFile, _type);
                    break;
                }
            }

            #endregion body

            #endregion section 2

        end:
            excelFile.SaveXls(response.OutputStream);
            response.End();

            #endregion excel
        }

        #region export excel ticket listing

        private void BuildRow(ref ExcelWorksheet wsReport, ref int currentIndexColumn, ref int currentIndexRow, Sunplus.Data.DataAbstract.Entity.TicketsListingBetlist ticket, int _type)
        {
            #region collapse

            Sunplus.Data.DataAbstract.Entity.Bettran bettran = ticket.BettranDetail;
            ++currentIndexRow;
            currentIndexColumn = 0;

            if (ticket.BettranDetail.Bettype == 9 || ticket.BettranDetail.Bettype == 29)
            {
                wsReport.Rows[currentIndexRow].Height = 255 * 2;
            }

            //----------------------------------
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = bettran.Transid;
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            DateTime transDate = CastAide.AsDateTime(bettran.Transdate);
            string strDate = transDate.ToShortDateString() + " " + transDate.ToShortTimeString();
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = strDate;
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = bettran.UserName;
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = GenEvent(ticket, false);
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            var _odds = ticket.Odds + ((ticket.OddName == null || string.IsNullOrEmpty(ticket.OddName.Trim())) ? string.Empty : " (" + ticket.OddName + ")");
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = _odds;
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = CastAide.AsDecimal(ticket.Stake);
            if (DNAide.Core.CastAide.AsDecimal(ticket.Stake) < 0)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Color = System.Drawing.Color.Red;
            }
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);

            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            if (bettran.Status.ToLower() == "running" || bettran.Status.ToLower() == "waiting")
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "";
            else
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = CastAide.AsDecimal(ticket.AgentWinloss);
            if (DNAide.Core.CastAide.AsDecimal(ticket.AgentWinloss) < 0)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Color = System.Drawing.Color.Red;
            }
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = bettran.CurrencyName;
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            if (bettran.Bettype == 29)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = GetBettypeNameNumberGame(bettran.Bettype, bettran.Liveindicator, bettran.SystemParlaySelection_Bettran);
            else
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = GetBettypeNameNumberGame(bettran.Bettype, bettran.Liveindicator);

            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //----------------------------------
            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = bettran.Status;
            if (bettran.Liveindicator)
            {
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
            }
            if (bettran.SATicket == 1)
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
            //------------------------------------
            if (_type != 2)
            {
                ++currentIndexColumn;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(
                    MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Bottom
                    , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                // wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].
                //wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = bettran.SATicket;

                if (bettran.Liveindicator)
                {
                    wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.Yellow);
                }
                if (bettran.SATicket == 0)
                    wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = string.Empty;
                else
                {
                    wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "X";
                    wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.LightPink);
                }
            }

            #endregion collapse
        }

        private void NewSheet(Sunplus.Data.DataAbstract.Entity.TicketsListingBetlistDTO ticketsListingBetlistDTO, int currentSheetIndex, int currentRowIndex, ExcelFile excelFile, int _type)
        {
            ++currentSheetIndex;
            var wsReport = excelFile.Worksheets.Add("Sheet" + currentSheetIndex.ToString());

            int currentIndexColumn = 0;
            int currentIndexRow = 0;

            wsReport.Columns[0].Width = 255 * 16;
            wsReport.Columns[1].Width = 255 * 18;
            wsReport.Columns[2].Width = 255 * 18;
            wsReport.Columns[3].Width = 255 * 32;
            wsReport.Columns[8].Width = 255 * 16;
            if (_type == 2)
            {
                wsReport.Columns[8].Width += 255 * 4;
            }

            #region header

            currentIndexColumn = 0;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Ref.no";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Trans.Time";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Username";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Choice";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Odds";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Stake";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Winlost";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Currency";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Bettype";

            ++currentIndexColumn;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
            wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "Status";

            if (_type != 2)
            {
                ++currentIndexColumn;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].SetBorders(MultipleBorders.Top | MultipleBorders.Left | MultipleBorders.Right
                    , System.Drawing.Color.FromArgb(124, 152, 164), LineStyle.Thin);
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.Font.Size = 164;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Style.FillPattern.SetSolid(System.Drawing.Color.FromArgb(220, 230, 241));
                wsReport.Rows[currentIndexRow].Cells[currentIndexColumn].Value = "S.A";
            }

            #endregion header

            #region body

            int count = 0;
            for (int i = currentRowIndex; i < ticketsListingBetlistDTO.Items.Count; ++i)
            {
                ++count;
                if (count <= MaxRows)
                {
                    var ticket = ticketsListingBetlistDTO.Items[i];
                    BuildRow(ref wsReport, ref currentIndexColumn, ref currentIndexRow, ticket, _type);
                }
                else
                {
                    NewSheet(ticketsListingBetlistDTO, currentSheetIndex, i, excelFile, _type);
                    break;
                }
            }

            #endregion body
        }

        private bool IsHRBettype(int bettype)
        {
            switch (bettype)
            {
                case 31:
                case 32:
                case 33:
                case 41:
                case 42:
                case 43:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsHRWinPlaceBettype(int bettype)
        {
            switch (bettype)
            {
                case 33:
                case 43:
                    return true;
            }
            return false;
        }

        private bool IsVoidRejectRefundLose(string status)
        {
            return (status.Equals(Status.VOID) || status.Equals(Status.REJECT) || status.Equals(Status.LOSE) || status.Equals(Status.Refund.ToUpper())); //Because it is "Refund" text.
        }

        private bool IsRunningWaiting(string status)
        {
            return (status.Equals(Status.RUNNING) || status.Equals(Status.WAITING));
        }

        private string GetOdds(Sunplus.Data.DataAbstract.Entity.Bettran bettran)
        {
            var _bettype = bettran.Bettype;
            var _odds = DNAide.Core.CastAide.AsDouble(bettran.Odds);
            var _modds = DNAide.Core.CastAide.AsDouble(bettran.Modds);
            var _OddsTypeName = CacheRef.GetOddsTypeName(bettran.OddsType);
            var _finalodds = 0;
            var _finalodds2 = 0;

            if (_bettype == 9)
            {
                return FormatNumber4(_odds) + _OddsTypeName;
            }
            else if (_bettype == 71 || _bettype == 72 || _bettype == 73 || _bettype == 113 || _bettype == 114 || _bettype == 112)
            {
                return string.Empty;
            }
            else if (_bettype == 29)
            {
                if (bettran.Status.ToLower().Contains("running"))
                {
                    return "-";
                }
                else
                {
                    return string.Format("{0}({1})", FormatNumber4(_modds), _OddsTypeName);
                }
            }
            else
            {
                #region HR bettype

                if (IsHRBettype(_bettype))
                {
                    if (IsHRWinPlaceBettype(_bettype)) // is 33, 43
                    {
                        if (!bettran.Status.Equals(Status.WIN) && !bettran.Status.Equals(Status.LOSE))
                        {
                            if (_bettype == 33)
                            {
                                return "-";
                            }
                            else // is 43
                            {
                                string HTMLwin = "{lblWinOddsSet}: {winOddsSet}";
                                string HTMLplace = "{lblPlaceOddsSet}: {placeOddsSet}";
                                TemplateHandler.Assign(ref HTMLwin, "{lblWinOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin"));
                                TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace"));
                                if (IsRunningWaiting(bettran.Status.ToLower()))
                                {
                                    TemplateHandler.Assign(ref HTMLwin, "{winOddsSet}", FormatNumber4(_finalodds));
                                    TemplateHandler.Assign(ref HTMLplace, "{placeOddsSet}", FormatNumber4(_finalodds2));
                                }
                                else // Reject, Void, Refund..
                                {
                                    TemplateHandler.Assign(ref HTMLwin, "{winOddsSet}", "0");
                                    TemplateHandler.Assign(ref HTMLplace, "{placeOddsSet}", "0");
                                }
                                return HTMLwin + HTMLplace;
                            }
                        }
                        else // Settled : is WIN || LOSE
                        {
                            string windOdds = "";
                            string placeOdds = "";
                            switch (bettran.Livehomescore)
                            {
                                case "2":
                                case "0":
                                    windOdds = FormatNumber4(bettran.Hdp1);
                                    break;

                                case "4":
                                    if (_bettype == 33)
                                        windOdds = "0.00";
                                    else
                                        windOdds = FormatNumber4(bettran.Hdp1);
                                    break;
                            }
                            switch (bettran.Liveawayscore)
                            {
                                case "2":
                                    placeOdds = FormatNumber4(bettran.Hdp2);
                                    break;

                                case "0":
                                case "4":
                                    if (_bettype == 33)
                                        placeOdds = "0.00";
                                    else
                                        placeOdds = FormatNumber4(bettran.Hdp2);
                                    break;
                            }
                            if (_bettype == 33)
                            {
                                string HTMLwin = "{lblWinOddsSet}: {winOddsSet}";
                                string HTMLplace = "{lblPlaceOddsSet}: {placeOddsSet}";

                                TemplateHandler.Assign(ref HTMLwin, "{lblWinOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin"));
                                TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace"));
                                TemplateHandler.Assign(ref HTMLwin, "{winOddsSet}", windOdds);
                                TemplateHandler.Assign(ref HTMLplace, "{placeOddsSet}", placeOdds);
                                return HTMLwin + HTMLplace;
                            }
                            else //is 43
                            {
                                string HTMLwin = "<!--winOdds43-->{lblWinOdds}: {winOdds}<!--/winOdds43-->{lblWinOddsSet}: {winOddsSet}";
                                string HTMLplace = "<!--PlaceOdds43-->{lblPlaceOdds}: {placeOdds}<!--/PlaceOdds43-->{lblPlaceOddsSet}: {placeOddsSet}";

                                // WIN Odds: liveHomeScore=2: WON
                                TemplateHandler.Assign(ref HTMLwin, "{winOddsSet}", (bettran.Livehomescore != "2") ? "0.00" : windOdds);
                                if (_finalodds != CastAide.AsDecimal(windOdds) && bettran.Livehomescore == "2")
                                {
                                    TemplateHandler.Assign(ref HTMLwin, "{lblWinOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WSettle"));
                                    TemplateHandler.Assign(ref HTMLwin, "{lblWinOdds}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "wBetAt"));
                                    TemplateHandler.Assign(ref HTMLwin, "{winOdds}", FormatNumber2(_finalodds));
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref HTMLwin, "{lblWinOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin"));
                                    TemplateHandler.RemoveBlock(ref HTMLwin, "winOdds43");
                                }

                                // PLACE Odds: liveAwayScore=2: WON
                                TemplateHandler.Assign(ref HTMLplace, "{placeOddsSet}", (bettran.Liveawayscore != "2") ? "0.00" : placeOdds);
                                if (_finalodds2 != CastAide.AsDecimal(placeOdds) && bettran.Liveawayscore == "2")
                                {
                                    TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "pSettle"));
                                    TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOdds}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "pBetAt"));
                                    TemplateHandler.Assign(ref HTMLplace, "{placeOdds}", FormatNumber2(_finalodds2));
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref HTMLplace, "{lblPlaceOddsSet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace"));
                                    TemplateHandler.RemoveBlock(ref HTMLplace, "PlaceOdds43");
                                }
                                return HTMLwin + HTMLplace;
                            }
                        }
                    }
                    else // 31, 32, 41, 42
                    {
                        if (bettran.Bettype < 33 && !bettran.Status.Equals(Status.WIN))
                        {
                            return "-";
                        }
                        else
                        {
                            // 31, 32, 41, 42
                            if (IsVoidRejectRefundLose(bettran.Status.ToUpper()))
                            {
                                return "-";
                            }
                            else
                            {
                                if (_bettype == 41 || _bettype == 42)
                                {
                                    if (IsRunningWaiting(bettran.Status.ToLower()))
                                        return FormatNumber4(_modds);
                                    else // Settled: WON, LOSE
                                    {
                                        if (_modds != _odds)
                                        {
                                            var _strOdds = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "betAt") + ": " + FormatNumber4(_modds)
                                                    + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "settle") + ":" + FormatNumber4(_odds);
                                            return _strOdds;
                                        }
                                        else
                                            return FormatNumber4(_odds);
                                    }
                                }
                                else // 31, 32
                                    return string.Format("{0}({1})", FormatNumber4(_odds), _OddsTypeName);
                            }
                        }
                    }
                }

                #endregion HR bettype

                else
                {
                    return string.Format("{0}({1})", FormatNumber4(_odds), _OddsTypeName);
                }
            }
        }

        private string FormatNumber2(object value)
        {
            var s = IBCUtils.FormatHandler.FormatNumber2(value);
            return FormatDecimal(s);
        }

        private string FormatNumber4(object value)
        {
            var s = IBCUtils.FormatHandler.FormatNumber4(value);
            return FormatDecimal(s);
        }

        private string FormatDecimal(string s)
        {
            while (s.Length - 1 >= 0 && s.Substring(s.Length - 1, 1) == "0")
            {
                s = s.Substring(0, s.Length - 1);
            }

            while (s.Length - 1 >= 0 && s.Substring(s.Length - 1, 1) == ".")
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }

        private string oddMP(object _odds)
        {
            return "@" + FormatNumber2(_odds);
        }

        private string GenHdpEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                    string.Format("   [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            var choice = string.Empty;

            if (_bettype == 20 || _bettype == 21) //Money line
            {
                if (_betteam == "h") choice = home;
                else choice = away;
            }
            else
            {
                var _hdp = "";
                if (_betteam == "h")
                {
                    _hdp = (_hdp2 - _hdp1).Equals(0) ? "0" : FormatNumber2(_hdp2 - _hdp1);
                }
                else
                {
                    _hdp = (_hdp2 - _hdp1).Equals(0) ? "0" : FormatNumber2(_hdp1 - _hdp2);
                }
                if (_betteam == "h") choice = home + " " + _hdp + liveScore;
                else choice = away + " " + _hdp + liveScore;
            }

            if (isMP) choice += oddMP(_odds);
            return choice.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), "") + GetBettypeName(_oddsM, _bettype);
        }

        private string GetBettypeName(double _oddsM, int _bettype)
        {
            string _bettypeName = string.Empty;
            if (parentBettype == 9 || parentBettype == 29)
            {
                _bettypeName = "    @" + _oddsM.ToString() + "\n" + Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype);
            }
            return _bettypeName;
        }

        private string GenOEEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var choice = string.Empty;
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                    string.Format("   [{0}-{1}]", _liveHomeScore, _liveAwayScore);

            if (_betteam == "h")
                choice = "ODD " + liveScore;
            else
                choice = "EVEN " + liveScore;
            //if (_betteam == "h") choice = "ODD";
            //else choice = "EVEN";

            if (isMP) choice += oddMP(_odds);

            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenOUEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                    string.Format("   [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            string choice = string.Empty;

            if (_betteam == "h") choice = "OVER " + Math.Abs(_hdp1).ToString() + liveScore;
            else choice = "UNDER " + Math.Abs(_hdp1).ToString() + liveScore;
            if (isMP) choice += oddMP(_odds);
            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenCSEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            string choice = "";
            choice = (_betteam == "5:0" || _betteam == "0:5") ? _betteam + " UP" : _betteam;
            if (_bettype == 30)
            {
                choice = (_betteam == "4:0" || _betteam == "0:4") ? _betteam + " UP" : _betteam;
            }
            if (isMP) choice += oddMP(_odds);

            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string Gen1x2Event(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP, bool isFT)
        {
            string choice = isFT ? "FT." + _betteam : "HT." + _betteam;
            if (listType == 1) { if (isMP) choice += oddMP(_odds); }
            if ((listType == 2 || listType == 3) && isMP) choice += "  @ " + FormatNumber2(_odds);

            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                    string.Format("    [{0}-{1}]", _liveHomeScore, _liveAwayScore);

            choice += liveScore;
            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenTGEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            string choice = _betteam;
            if (listType == 1) { if (isMP) choice += oddMP(_odds); }
            else if ((listType == 2 || listType == 3) && isMP) choice += "  @ " + FormatNumber2(_odds);
            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenOutrightRow(double _oddsM, int _bettype, string home, string away, int listType)
        {
            string choice = home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), "");
            if (listType == 1 || listType == 3) choice = away;
            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenHREvent(double _oddsM, int _bettype, string home)
        {
            return home + GetBettypeName(_oddsM, _bettype);
        }

        private string GenMixEvent(Sunplus.Data.DataAbstract.Entity.TicketsListingBetlist ticket)
        {
            parentBettype = 9;
            string strResult = GenEvent(ticket, true);
            parentBettype = -1;
            return strResult;
        }

        private bool IsFIOU(int bettype)
        {
            switch (bettype)
            {
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsFIOE(int bettype)
        {
            switch (bettype)
            {
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                    return true;
                default:
                    return false;
            }
        }

        private string GetFIBettypeName(double _oddsM, string betteam, int _bettype)
        {
            if (IsFIOU(_bettype))
            {
                return betteam == "a" ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUnder") + GetBettypeName(_oddsM, _bettype) : ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOver") + GetBettypeName(_oddsM, _bettype);
            }
            else if (IsFIOE(_bettype))
            {
                return betteam == "a" ? "EVEN" + GetBettypeName(_oddsM, _bettype) : "ODD" + GetBettypeName(_oddsM, _bettype);
            }
            return "";
        }

        private string GenFinancialRow(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            return GetFIBettypeName(_oddsM, _betteam, _bettype) + " " + FormatNumber2(_hdp1) + GetBettypeName(_oddsM, _bettype);
        }

        private string GenCasino(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            string choice = "";
            if (_bettype == 71 || _bettype == 113)
                choice = "Casino Game";
            else if (_bettype == 72 || _bettype == 114)
                choice = "P2P Game";
            else if (_bettype == 112)
                choice = "Casino Game - Transfer";
            else if (_bettype == 73)
                choice = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblIBCbetBingo");
            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenEvent_NumberGame(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP, string _betId)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                        string.Format("   [{0}]", _betId);
            var choice = string.Empty;

            #region check bettype & apply betty name

            if (_bettype == 81 || _bettype == 82 || _bettype == 85)
            {
                var maxBall = (double.Parse(Sunplus.Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);

                choice = _betteam.ToLower() == "h" ? " OVER " : " UNDER ";
                choice += string.Format("{0}", maxBall);
                choice += liveScore;
            }
            else if (_bettype == 83 || _bettype == 84 || _bettype == 86)
            {
                choice = _betteam.ToLower() == "h" ? "ODD " : "EVEN ";
                choice += liveScore;
            }
            else if (_bettype == 87)//High-Low
            {
                choice = _betteam.ToLower() == "h" ? "HIGH " : "LOW ";
                choice += liveScore;
            }
            else if (_bettype == Constants.BetType.Warrior)//warrior
                choice = _betteam.ToLower() == "h" ? "2nd ball to win " : "3rd ball to win ";
            else if (_bettype == Constants.BetType.NextCombo)//next combo
            {
                #region number group - bettypes

                switch (_betteam.ToLower())
                {
                    case "1:1":
                        choice = "Over/Odd ";
                        break;

                    case "1:2":
                        choice = "Over/Even ";
                        break;

                    case "2:1":
                        choice = "Under/Odd ";
                        break;

                    case "2:2":
                        choice = "Under/Even ";
                        break;

                    default:
                        choice = "Over/Odd ";
                        break;
                }

                #endregion number group - bettypes

                var maxBall = (double.Parse(Sunplus.Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);
                choice += string.IsNullOrEmpty(_liveStyle) ? string.Empty : string.Format("{0}", maxBall);
                choice += liveScore;
            }
            else if (_bettype == 90)
            {
                choice = "Group " + _hdp1 + "-" + _betteam + " ";
                choice += liveScore;
            }

            #endregion check bettype & apply betty name

            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GetBettypeNameNumberGame(int _bettype, bool _liveStyle)
        {
            string _bettypeName = string.Empty;
            if (_bettype == 85)
            {
                _bettypeName = "Next Over/Under";
            }
            else if (_bettype == 86)
            {
                if (_liveStyle)
                {
                    _bettypeName = "Next Odd/Even";
                }
                else
                {
                    _bettypeName = "FT. Odd/Even";
                }
            }
            else
            {
                _bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype);
            }

            return _bettypeName;
        }

        private string GetBettypeNameNumberGame(int _bettype, bool _liveStyle, string _betteam)
        {
            string _bettypeName = string.Empty;
            if (_bettype == 85)
            {
                _bettypeName = "Next Over/Under";
            }
            else if (_bettype == 86)
            {
                if (_liveStyle)
                {
                    _bettypeName = "Next Odd/Even";
                }
                else
                {
                    _bettypeName = "FT. Odd/Even";
                }
            }
            else
            {
                _bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(_bettype);
            }
            //add systemparlay sub- bettype
            if (_bettype == 29)
            {
                string selection = "";
                switch (_betteam)
                {
                    case "110":
                        selection = "Trixie";
                        break;

                    case "1110":
                        selection = "Yankee";
                        break;

                    case "11110":
                        selection = "Canadian";
                        break;

                    case "111110":
                        selection = "Heinz";
                        break;

                    case "1111110":
                        selection = "Super Heinz";
                        break;

                    case "11111110":
                        selection = "Goliath";
                        break;

                    default:
                        selection = "Trixie";
                        break;
                }
                _bettypeName += " - \n" + selection;// CacheRef.GetCombinationName(_betteam);//trixie or golias...
            }
            return _bettypeName;
        }

        private string GenDoubleChanceEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                        string.Format("   [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            var choice = string.Empty;

            #region assign value for choice parameter

            if (_betteam.ToLower().Equals("1x"))
                choice = string.Format("{0} or Draw", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (_betteam.ToLower().Equals("2x"))
                choice = string.Format("{0} or Draw", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (_betteam.ToLower().Equals("12"))
                choice = string.Format("{0} or {1}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) choice += oddMP(_odds);

            #endregion assign value for choice parameter

            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenDrawNoBetEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                        string.Format("    [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            var choice = string.Empty;

            #region assign value for choice parameter

            if (_betteam.ToLower().Equals("h"))
                choice = string.Format("{0}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (_betteam.ToLower().Equals("a"))
                choice = string.Format("{0}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) choice += oddMP(_odds);

            #endregion assign value for choice parameter

            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenBothOneNeitherEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                        string.Format("  [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            var choice = string.Empty;

            #region assign value for choice parameter

            if (_betteam.ToLower().Equals("b"))
                choice = "Both";
            else if (_betteam.ToLower().Equals("o"))
                choice = "One";
            else if (_betteam.ToLower().Equals("n"))
                choice = "No Goal";

            if (isMP) choice += oddMP(_odds);

            #endregion assign value for choice parameter

            return choice + liveScore + GetBettypeName(_oddsM, _bettype);
        }

        private string GenToWinToNilEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                        string.Format("    [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            var choice = string.Empty;

            #region assign value for choice parameter

            if (_betteam.ToLower().Equals("h"))
                choice = string.Format("{0}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (_betteam.ToLower().Equals("a"))
                choice = string.Format("{0}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) choice += oddMP(_odds);

            #endregion assign value for choice parameter

            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenCleanSheetEvent(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                    string.Format("    [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            var choice = string.Empty;

            #region assign value for choice parameter

            if (_betteam.ToLower().Equals("hy"))
                choice = string.Format("{0} {1}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "YES");
            else if (_betteam.ToLower().Equals("hn"))
                choice = string.Format("{0} {1}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "NO");
            else if (_betteam.ToLower().Equals("ay"))
                choice = string.Format("{0} {1}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "YES");
            else if (_betteam.ToLower().Equals("an"))
                choice = string.Format("{0} {1}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "NO");

            if (isMP) choice += oddMP(_odds);

            #endregion assign value for choice parameter

            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenThreeWayHandicap(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                           string.Format("    [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            var choice = string.Empty;
            var hdpResult = 0;

            #region assign value for choice parameter

            if (_betteam.ToLower().Equals("1"))
            {
                hdpResult = (int)(_hdp1 - _hdp2 > 0 ? -(_hdp1 - _hdp2) : Math.Abs(_hdp1 - _hdp2));

                choice = string.Format("{0} ({1}{2})",
                    home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    (hdpResult > 0 ? "+" : string.Empty), hdpResult);
            }
            else if (_betteam.ToLower().Equals("2"))
            {
                hdpResult = (int)(_hdp2 - _hdp1 > 0 ? -(_hdp2 - _hdp1) : Math.Abs(_hdp2 - _hdp1));
                choice = string.Format("{0} ({1}{2})",
                    away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    (hdpResult > 0 ? "+" : string.Empty), hdpResult);
            }
            else if (_betteam.ToLower().Equals("x"))
                choice = string.Format("Draw ({0} +{1})", _hdp1 - _hdp2 > 0 ? "A" : "H", (int)(Math.Abs(_hdp1 - _hdp2)));

            if (isMP) choice += oddMP(_odds);

            #endregion assign value for choice parameter

            return choice + GetBettypeName(_oddsM, _bettype);
        }

        private string GenEventCombination(Sunplus.Data.DataAbstract.Entity.TicketsListingBetlist ticket)
        {
            parentBettype = 29;
            string strRelust = GenEvent(ticket, true);
            parentBettype = -1;
            return strRelust;
        }

        private string GenEvent(Sunplus.Data.DataAbstract.Entity.TicketsListingBetlist ticket, bool isMP)
        {
            Sunplus.Data.DataAbstract.Entity.Bettran bettran = ticket.BettranDetail;
            Sunplus.Data.DataAbstract.Entity.Bettran bettranm = ticket.MBettranDetail;

            if (!isMP)
            {
                bettran = ticket.BettranDetail;
                bettranm = ticket.MBettranDetail;
            }
            else
            {
                bettran = ticket.MBettranDetail;
                bettranm = ticket.BettranDetail;
            }

            if (bettran == null) return string.Empty;
            var _liveStyle = bettran.Liveindicator ? "event-live" : "";
            var home = CacheRef.GetTeamName(bettran.HomeId) + (bettran.IsNeutral ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
            string away = CacheRef.GetTeamName(bettran.AwayId);
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? "   " :
                    string.Format("   [{0}-{1}]   ", bettran.Livehomescore, bettran.Liveawayscore);
            var _liveHomeScore = bettran.Livehomescore;
            var _liveAwayScore = bettran.Liveawayscore;
            var betteam = bettran.Betteam;
            var listType = 1;
            var bettype = bettran.Bettype;
            var _hdp1 = DNAide.Core.CastAide.AsDouble(bettran.Hdp1);
            var _hdp2 = DNAide.Core.CastAide.AsDouble(bettran.Hdp2);
            var _odds = DNAide.Core.CastAide.AsDouble(bettran.Odds);
            var _oddsM = DNAide.Core.CastAide.AsDouble(ticket.oddsM);

            switch (bettran.Bettype)
            {
                case 1:
                case 7:
                case 20:
                case 22: //Next Goal bet type
                case 21: //Next Goal bet type
                case 124:
                case 125:
                case 1201:
                    return GenHdpEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 2:
                case 12:
                    return GenOEEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 3:
                case 8:
                case 1203:
                    return GenOUEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 4:
                case 30:
                case 1204:
                    return GenCSEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 5:
                case 1205:
                    return Gen1x2Event(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false, true);
                case 15:
                    return Gen1x2Event(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false, false);
                case 6:
                case 14:
                case 16:
                case 1206:
                    return GenTGEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 10:
                    return GenOutrightRow(_oddsM, bettype, home, away, listType);
                case 121:
                case 122:
                case 123:
                    return GenHomeAwayDrawNoBet(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                //Phuong.Nguyen, HR
                case 31:
                case 32:
                case 33:
                case 41:
                case 42:
                case 43:
                    return GenHREvent(_oddsM, bettype, home);
                case 9:
                    return GenMixEvent(ticket);
                case 51: //Betting of Finance
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                    //bettran.home = CacheRef.GetFncIndexName(row["homeid"]);
                    return GenFinancialRow(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 71:
                case 72:
                case 73:
                case 112:
                case 113:
                case 114:
                    //GenCasino(ref eventItem, bettype, CastAide.AsString(row["betid"]));
                    return GenCasino(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 81://Number Game
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                    return GenEvent_NumberGame(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false, bettran.Betid);
                case 24:
                case 1224:
                    return GenDoubleChanceEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 25:
                    return GenDrawNoBetEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 26:
                    return GenBothOneNeitherEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 27:
                    return GenToWinToNilEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 13:
                    return GenCleanSheetEvent(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 28:
                    return GenThreeWayHandicap(_oddsM, _odds, bettype, _hdp1, _hdp2, _liveStyle, _liveHomeScore, _liveAwayScore, home, away, betteam, listType, false);
                case 29://Combination
                    return GenEventCombination(ticket);
            }
            return string.Empty;
        }

        private string GenHomeAwayDrawNoBet(double _oddsM, double _odds, int _bettype, double _hdp1, double _hdp2, string _liveStyle, string _liveHomeScore, string _liveAwayScore, string home, string away, string _betteam, int listType, bool isMP)
        {
            var liveScore = string.IsNullOrEmpty(_liveStyle) ? string.Empty :
                   string.Format("   [{0}-{1}]", _liveHomeScore, _liveAwayScore);
            var choice = string.Empty;

            if (_bettype == 121)
            {
                if (_betteam == "x") choice = "Draw";
                else if (_betteam == "a") choice = away;
            }
            else if (_bettype == 122)
            {
                if (_betteam == "h") choice = home;
                else if (_betteam == "x") choice = "Draw";
            }
            else if (_bettype == 123)
            {
                if (_betteam == "h") choice = "Draw";
                else if (_betteam == "a") choice = "No Draw";
            }

            if (isMP) choice += oddMP(_odds);
            return choice.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), "") + GetBettypeName(_oddsM, _bettype);
        }

        #endregion export excel ticket listing
    }

}