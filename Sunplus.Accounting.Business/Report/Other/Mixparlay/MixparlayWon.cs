using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using DNAide.Utils;

using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Core.LoggingLib;
using ExcelUtil.Spreadsheet;
using System.Drawing;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Report.Mixparlay;



/// <summary>
/// Summary description for Winloss12Bet
/// </summary>
namespace Sunplus.Accounting.Report.Mixparlay
{
    public class MixparlayWon : BaseManager, IRequiresSessionState, IMixparlayWon
    {
        #region private varialbes
        private static string MixparlayWon_SessionKey = "MixparlayWon";
        private readonly IOthersDAO _reportMenuDao;
        #endregion
        public MixparlayWon(IOthersDAO reportDAO)
        {
            _reportMenuDao = reportDAO;
        }
        public MixparlayDataGridBO GetList_MixparlayWon(bool isSoccer, string date, int pageIndex, int pageSize, bool fromcache, int sortType)
        {
            MixparlayDataGridBO mixparlaywongrid = new MixparlayDataGridBO();
            var mixparlaydatagrid_session = SessionExtension.GetSession(HttpContext.Current.Session, MixparlayWon_SessionKey);
            if (mixparlaydatagrid_session == null || fromcache == false)
            {

                #region get data
                DataTable dt = null;
                DateTime _date = CastAide.AsDateTime(date, DateTime.Today);
                dt = _reportMenuDao.GetMixParlayWonList(isSoccer, _date).ResultSet.Tables[0];
                var lsrow = new List<MixparlayWonBO>();
                decimal stake = 0;
                decimal winloss = 0;
                decimal basetake = 0;
                decimal basewinloss = 0;

                decimal sumbasestake = 0;
                decimal sumbasewinloss = 0;
              
                int i = 1;
                mixparlaywongrid.total = dt.Rows.Count;
                DataTable dtSort = new DataTable();
                DataView dataView = new DataView();
                dataView = dt.DefaultView;

                if (sortType > 0)
                {
                    dataView.Sort = sortType == 1 ? "Winloss_RM DESC" : "Winloss_RM ASC";
                }
                

                dtSort = dataView.ToTable();

                foreach (DataRow row in dtSort.Rows)
                {
                    var item = new MixparlayWonBO();

                    stake = CastAide.AsDecimal(row["stake"]);
                    winloss = CastAide.AsDecimal(row["Winloss"]);
                    basetake = CastAide.AsDecimal(row["stake_RM"]);
                    basewinloss = CastAide.AsDecimal(row["Winloss_RM"]);

                    sumbasestake += basetake;
                    sumbasewinloss += basewinloss;                   

                    item.Stake = NumberFormat.DecFormat(stake, 2);
                    item.Winloss = NumberFormat.DecFormat(winloss, 2);
                    item.BaseStake = NumberFormat.DecFormat(basetake, 2);
                    item.BaseWinloss = NumberFormat.DecFormat(basewinloss, 2);
                    item.TransID = row["transid"].ToString();
                    item.UserName = row["username"].ToString();
                    item.Currency = row["Currency"].ToString();
                    item.Exchange = row["exchange"].ToString();
                    item.Selection = row["selection"].ToString();
                    item.WinOdds = row["modds"].ToString();
                    i++;
                    lsrow.Add(item);
                }

                MixparlayWonBO totalitem = new MixparlayWonBO();
                totalitem.BaseStake = NumberFormat.DecFormat(sumbasestake, 2);
                totalitem.BaseWinloss = NumberFormat.DecFormat(sumbasewinloss, 2);
                totalitem.UserName ="Total";

                mixparlaywongrid.rows = lsrow;
                mixparlaywongrid.footer = new List<MixparlayWonBO>();
                mixparlaywongrid.footer.Add(totalitem);

                SessionExtension.AddSession(HttpContext.Current.Session, MixparlayWon_SessionKey, mixparlaywongrid);

                #endregion
            }
            else
            {
                mixparlaywongrid = (MixparlayDataGridBO)mixparlaydatagrid_session;
            }
            
            #region paging
        
            MixparlayDataGridBO mixparlaydatagridpaging = new MixparlayDataGridBO();
            mixparlaydatagridpaging.rows =  mixparlaywongrid.rows.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            mixparlaydatagridpaging.total = mixparlaywongrid.total;
            mixparlaydatagridpaging.footer = mixparlaywongrid.footer;

            #endregion
            return mixparlaydatagridpaging;
        }


        #region Export 2 excel file
        public void MixparlayWonExportToExcel(bool isSoccer, string date, int sortType)
        {
            var mixparlaydatagridSession = SessionExtension.GetSession(HttpContext.Current.Session, MixparlayWon_SessionKey);
            var mixparlayData = new MixparlayDataGridBO();
            #region get data
            if (mixparlaydatagridSession != null)
                mixparlayData = (MixparlayDataGridBO) mixparlaydatagridSession;
            else
                mixparlayData = GetList_MixparlayWon(isSoccer, date, 1, 50, false, sortType);
            #endregion

            #region export excel

            var reportName = string.Format("MixparlayWon-{0}", isSoccer ? "SoccerOnly" : "OtherSports");
            var excelFile = new ExcelFile();
            const int startRowIndex = 0;
            const int startColumnIndex = 0;
            int nextRow = 0, nextColumn = 0;
            var wsMixparlayWon = excelFile.Worksheets.Add(reportName);
            var bgFormat = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Center,
                                       ExcelFont.BoldWeight, false);
            #region Header

            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex, startRowIndex + (nextRow+=1),
                                                     startColumnIndex).Merged = true;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex, startRowIndex + (nextRow),
                                                     startColumnIndex).Style = bgFormat;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex, startRowIndex + (nextRow),
                                                     startColumnIndex).Value = "TransId";

            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn+=1), startRowIndex + (nextRow),
                                                     startColumnIndex+ nextColumn).Merged = true;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Style = bgFormat;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Value = "UserName";

            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn+=1), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Merged = true;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Style = bgFormat;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Value = "Currency";

            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn += 1), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Merged = true;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Style = bgFormat;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Value = "Exchange";

            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn+=1), startRowIndex,
                                                     startColumnIndex + (nextColumn+=1)).Merged = true;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex,
                                                     startColumnIndex + nextColumn).Style = bgFormat;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex,
                                                     startColumnIndex + nextColumn).Value = "User Currency";
            wsMixparlayWon.Cells[startRowIndex + 1, nextColumn - 1].Style = bgFormat;
            wsMixparlayWon.Cells[startRowIndex + 1, nextColumn - 1].Value = "Stake";
            wsMixparlayWon.Cells[startRowIndex + 1, nextColumn].Style = bgFormat;
            wsMixparlayWon.Cells[startRowIndex + 1, nextColumn].Value = "WinLost";

            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn += 1), startRowIndex,
                                                     startColumnIndex + (nextColumn+=1)).Merged = true;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex,
                                                     startColumnIndex + nextColumn).Style = bgFormat;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex,
                                                     startColumnIndex + nextColumn).Value = "Base Currency";
            wsMixparlayWon.Cells[startRowIndex + 1, nextColumn - 1].Style = bgFormat;
            wsMixparlayWon.Cells[startRowIndex + 1, nextColumn - 1].Value = "Stake";
            wsMixparlayWon.Cells[startRowIndex + 1, nextColumn].Style = bgFormat;
            wsMixparlayWon.Cells[startRowIndex + 1, nextColumn].Value = "WinLost";

            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn += 1), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Merged = true;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Style = bgFormat;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Value = "Selection";

            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn += 1), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Merged = true;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Style = bgFormat;
            wsMixparlayWon.Cells.GetSubrangeAbsolute(startRowIndex, startColumnIndex + (nextColumn), startRowIndex + (nextRow),
                                                     startColumnIndex + nextColumn).Value = "Win Odds";

            #endregion

            #region Body
            if(mixparlayData.rows != null && mixparlayData.rows.Count > 0)
            {
                var rowColor = Color.FromArgb(248, 248, 248);
                var rowColorOdd = Color.FromArgb(223, 234, 255);

                var detailNumberFormat = ExcelFormat(rowColor, HorizontalAlignmentStyle.Right,
                                                     ExcelFont.NormalWeight, true);
                var normalFormat =  ExcelFormat(rowColor, HorizontalAlignmentStyle.Left,
                                                     ExcelFont.NormalWeight, false);

                var baseCurrencyFormat = ExcelFormat(Color.FromArgb(204, 255, 204), HorizontalAlignmentStyle.Right,
                                                     ExcelFont.NormalWeight, false);

                var selectionFormat = ExcelFormat(rowColor, HorizontalAlignmentStyle.Right,
                                                     ExcelFont.NormalWeight, false );
                foreach (var row in mixparlayData.rows)
                {
                    nextColumn = 1;
                    nextRow++;
                    #region assign colort for each row
                    var currentColor = nextRow % 2 == 0 ? rowColor : rowColorOdd;
                    detailNumberFormat = ExcelFormat(currentColor, HorizontalAlignmentStyle.Right,
                                                     ExcelFont.NormalWeight, true);
                    normalFormat = ExcelFormat(currentColor, HorizontalAlignmentStyle.Left,
                                                     ExcelFont.NormalWeight, false);
                    selectionFormat = ExcelFormat(currentColor, HorizontalAlignmentStyle.Right,
                                                     ExcelFont.NormalWeight, false);
                    #endregion
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex].Value = row.TransID;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex].Style = normalFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = row.UserName;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = normalFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = row.Currency;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = normalFormat;
                    
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsDecimal(row.Exchange);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = detailNumberFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsDecimal(row.Stake);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = detailNumberFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsDecimal(row.Winloss);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = detailNumberFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsDecimal(row.BaseStake);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = baseCurrencyFormat;
                    
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsDecimal(row.BaseWinloss);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = baseCurrencyFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsInt32(row.Selection);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = selectionFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsDecimal(row.WinOdds);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn-1)].Style = detailNumberFormat;
                    

                    
                }
            }

            #endregion

            #region Footer
            nextRow += 1;
            if (mixparlayData.footer != null && mixparlayData.footer.Count > 0)
            {
                foreach (var footer in mixparlayData.footer)
                {
                    nextColumn = 1;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex].Value = string.Empty;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = "Total";
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn-1)].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = string.Empty;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = string.Empty;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = string.Empty;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = string.Empty;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsDecimal(footer.BaseStake);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = CastAide.AsDecimal(footer.BaseWinloss);
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = string.Empty;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = bgFormat;

                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn++].Value = string.Empty;
                    wsMixparlayWon.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn - 1)].Style = bgFormat;
                }
            }
            #endregion

            #endregion

            #region response
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
            #endregion
        }

        private static CellStyle ExcelFormat(Color backgroundColor, HorizontalAlignmentStyle alignmentStyle, int font, bool isDecFormat)
        {
            var excelFormat = new CellStyle();

            excelFormat.FillPattern.SetSolid(backgroundColor);
            excelFormat.Borders.SetBorders(MultipleBorders.Outside, Color.FromArgb(65, 116, 177), LineStyle.Thin);
            excelFormat.Font.Weight = font;
            excelFormat.HorizontalAlignment = alignmentStyle;
            excelFormat.WrapText = true;
            excelFormat.Font.Name = "Tahoma";
            excelFormat.Font.Size = 160;

            if (isDecFormat)
                excelFormat.NumberFormat = "#,##0.00_);[RED](-#,##0.00);";
            return excelFormat;
        }
        #endregion
    }
}
