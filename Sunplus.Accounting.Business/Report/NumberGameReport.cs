using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.DataAccess.IBCDLL.Report;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Accounting.Entities.Report.NumberGame;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.Report
{
    public class NumberGameReport : BaseManager, IRequiresSessionState, INumberGameReport
    {
        #region private varialbes
        private static string NumberGameWinlossByMatch_SessionKey = "NumberGameWinlossByMatch";
        private static string NumgerGameWinLossByMatchDetail_SessionKey = "NumberGameWinlossByMatchDetail";
        private static string NumberGameWinlossByDate_SessionKey = "NumberGameWinlossByDate";
        private static string NumberGameMonthlyWinloss_SessionKey = "NumberGameMonthlyWinloss";
        private static string _NumberGameWinLossByBettypeSessionKey = "NumberGameWinLossByBettypeSessionKey";
        private static string _NumberGameWinLossDetailByBettypeSessionKey = "NumberGameWinLossDetailByBettypeSessionKey";
        #endregion

        private INumberGameReportDAO _numberGameDAO;

        public NumberGameReport()
        {

        }

        public NumberGameReport(INumberGameReportDAO numberGameDao)
        {
            this._numberGameDAO = numberGameDao;
        }

        #region Winloss by date
        public NumberGameByDateTreeGridBO WinLoss_ByDate_NumberGame(string fromdate, string todate)
        {

            NumberGameByDateTreeGridBO numbergametreegrid = new NumberGameByDateTreeGridBO();
            var lsrow = new List<NumberGameWinLostByDateBO>();
            NumberGameWinLostByDateBO granditem = new NumberGameWinLostByDateBO();
            DataTable dt = null;
            try
            {
                DateTime _fdate = CastAide.AsDateTime(fromdate, DateTime.Today);
                DateTime _todate = CastAide.AsDateTime(todate, DateTime.Today);
                dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_Rpt_NumberGameWinlossByDate(_fdate, _todate).ResultSet.Tables[0];
                decimal grandwinloss = 0;
                decimal grandposition = 0;
                decimal grandcommission = 0;
                decimal grandlinetotal = 0;
                decimal grandvirtuallinetotal = 0;
                decimal grandturnover = 0;

                var listDate = GetDistinctDate(dt);
                if (listDate.Count > 0)
                {
                    foreach (DateTime _date in listDate)
                    {
                        var itemdate = new NumberGameWinLostByDateBO();
                        itemdate.children = new List<NumberGameWinLostByDateBO>();
                        decimal totalwinlossbymatch = 0;
                        decimal totalwinlossbylive = 0;

                        decimal totalpositionbymatch = 0;
                        decimal totalpositionbylive = 0;


                        decimal totalcommissionbymatch = 0;
                        decimal totalcommissionbylive = 0;

                        decimal totallinetotalbymatch = 0;
                        decimal totallinetotalbylive = 0;

                        decimal totalvirtuallinetotalbymatch = 0;
                        decimal totalvirtuallinetotalbylive = 0;

                        decimal totalturnoverbymatch = 0;
                        decimal totalturnoverbylive = 0;



                        decimal winloss = 0;
                        decimal position = 0;
                        decimal commission = 0;
                        decimal linetotal = 0;
                        decimal virtuallinetotal = 0;
                        decimal turnover = 0;
                        decimal margin = 0;



                        #region get by matchid, islive
                        DataRow[] rows = dt.Select("date='" + _date.ToString() + "' and islive=1");
                        if (rows.Length > 0)
                        {

                            var itemlive = new NumberGameWinLostByDateBO();
                            itemlive.children = new List<NumberGameWinLostByDateBO>();
                            itemlive.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "") + "_" + "Live";
                            itemlive.Name = "Live";

                            itemdate.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "");
                            itemdate.Name = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString();

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByDateBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = true;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }
                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;


                            }

                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = totalturnoverbylive != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemdate.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;


                        }
                        #endregion

                        #region get by matchid, islive=false
                        totalwinlossbylive = 0;
                        totalpositionbylive = 0;
                        totalcommissionbylive = 0;
                        totallinetotalbylive = 0;
                        totalvirtuallinetotalbylive = 0;
                        totalturnoverbylive = 0;

                        rows = dt.Select("date='" + _date.ToString() + "' and islive=0");
                        if (rows.Length > 0)
                        {
                            var itemlive = new NumberGameWinLostByDateBO();
                            itemlive.children = new List<NumberGameWinLostByDateBO>();
                            itemlive.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "") + "_" + "NoneLive";
                            itemlive.Name = "None Live";

                            itemdate.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "");
                            itemdate.Name = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString();

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByDateBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = false;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = Math.Round(turnover, 2) != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;

                            }


                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2);
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemdate.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        itemdate.Winlost = NumberFormat.DecFormat(totalwinlossbymatch, 2);
                        itemdate.Position = NumberFormat.DecFormat(totalpositionbymatch, 2);
                        itemdate.Commission = NumberFormat.DecFormat(totalcommissionbymatch, 2);
                        itemdate.LineTotal = NumberFormat.DecFormat(totallinetotalbymatch, 2);
                        itemdate.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbymatch, 2);
                        itemdate.TurnOver = NumberFormat.DecFormat(totalturnoverbymatch, 2);
                        itemdate.Margin = CastAide.AsDecimal(NumberFormat.DecFormat(totalturnoverbymatch, 2)) != 0 ? NumberFormat.DecFormat(totallinetotalbymatch * 100 / totalturnoverbymatch, 2) + "%" : "n/a";
                        itemdate.Variance = NumberFormat.DecFormat(totallinetotalbymatch - totalvirtuallinetotalbymatch, 2);

                        grandwinloss += totalwinlossbymatch;
                        grandposition += totalpositionbymatch;
                        grandcommission += totalcommissionbymatch;
                        grandlinetotal += totallinetotalbymatch;
                        grandvirtuallinetotal += totalvirtuallinetotalbymatch;
                        grandturnover += totalturnoverbymatch;



                        lsrow.Add(itemdate);
                    }
                    granditem.Name = "Grand Total";
                    granditem.Winlost = NumberFormat.DecFormat(grandwinloss, 2);
                    granditem.Position = NumberFormat.DecFormat(grandposition, 2);
                    granditem.Commission = NumberFormat.DecFormat(grandcommission, 2);
                    granditem.LineTotal = NumberFormat.DecFormat(grandlinetotal, 2);
                    granditem.VirtualLineTotal = NumberFormat.DecFormat(grandvirtuallinetotal, 2);
                    granditem.TurnOver = NumberFormat.DecFormat(grandturnover, 2);
                    granditem.Margin = grandturnover != 0 ? NumberFormat.DecFormat(grandlinetotal * 100 / grandturnover, 2) + "%" : "n/a";
                    granditem.Variance = NumberFormat.DecFormat(grandlinetotal - grandvirtuallinetotal, 2);

                }
                numbergametreegrid.rows = lsrow;
                numbergametreegrid.totalrow = granditem;

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinlossByDate_SessionKey, numbergametreegrid);
            return numbergametreegrid;
            // return dt;
        }

        public NumberGameByDateTreeGridBO WinLoss_ByDate_NumberGame(string fromdate, string todate, int SiteID, int Currency)
        {

            NumberGameByDateTreeGridBO numbergametreegrid = new NumberGameByDateTreeGridBO();
            var lsrow = new List<NumberGameWinLostByDateBO>();
            NumberGameWinLostByDateBO granditem = new NumberGameWinLostByDateBO();
            DataTable dt = null;
            try
            {
                DateTime _fdate = CastAide.AsDateTime(fromdate, DateTime.Today);
                DateTime _todate = CastAide.AsDateTime(todate, DateTime.Today);
                dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_Rpt_NumberGameWinlossByDate(_fdate, _todate, SiteID, Currency).ResultSet.Tables[0];
                decimal grandwinloss = 0;
                decimal grandposition = 0;
                decimal grandcommission = 0;
                decimal grandlinetotal = 0;
                decimal grandvirtuallinetotal = 0;
                decimal grandturnover = 0;

                var listDate = GetDistinctDate(dt);
                if (listDate.Count > 0)
                {
                    foreach (DateTime _date in listDate)
                    {
                        var itemdate = new NumberGameWinLostByDateBO();
                        itemdate.children = new List<NumberGameWinLostByDateBO>();
                        decimal totalwinlossbymatch = 0;
                        decimal totalwinlossbylive = 0;

                        decimal totalpositionbymatch = 0;
                        decimal totalpositionbylive = 0;


                        decimal totalcommissionbymatch = 0;
                        decimal totalcommissionbylive = 0;

                        decimal totallinetotalbymatch = 0;
                        decimal totallinetotalbylive = 0;

                        decimal totalvirtuallinetotalbymatch = 0;
                        decimal totalvirtuallinetotalbylive = 0;

                        decimal totalturnoverbymatch = 0;
                        decimal totalturnoverbylive = 0;



                        decimal winloss = 0;
                        decimal position = 0;
                        decimal commission = 0;
                        decimal linetotal = 0;
                        decimal virtuallinetotal = 0;
                        decimal turnover = 0;
                        decimal margin = 0;



                        #region get by matchid, islive
                        DataRow[] rows = dt.Select("date='" + _date.ToString() + "' and islive=1");
                        if (rows.Length > 0)
                        {

                            var itemlive = new NumberGameWinLostByDateBO();
                            itemlive.children = new List<NumberGameWinLostByDateBO>();
                            itemlive.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "") + "_" + "Live";
                            itemlive.Name = "Live";

                            itemdate.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "");
                            itemdate.Name = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString();

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByDateBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = true;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }
                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;


                            }

                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemdate.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;


                        }
                        #endregion

                        #region get by matchid, islive=false
                        totalwinlossbylive = 0;
                        totalpositionbylive = 0;
                        totalcommissionbylive = 0;
                        totallinetotalbylive = 0;
                        totalvirtuallinetotalbylive = 0;
                        totalturnoverbylive = 0;

                        rows = dt.Select("date='" + _date.ToString() + "' and islive=0");
                        if (rows.Length > 0)
                        {
                            var itemlive = new NumberGameWinLostByDateBO();
                            itemlive.children = new List<NumberGameWinLostByDateBO>();
                            itemlive.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "") + "_" + "NoneLive";
                            itemlive.Name = "None Live";

                            itemdate.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "");
                            itemdate.Name = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString();

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByDateBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = false;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = Math.Round(turnover, 2) != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;

                            }


                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2);
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemdate.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        itemdate.Winlost = NumberFormat.DecFormat(totalwinlossbymatch, 2);
                        itemdate.Position = NumberFormat.DecFormat(totalpositionbymatch, 2);
                        itemdate.Commission = NumberFormat.DecFormat(totalcommissionbymatch, 2);
                        itemdate.LineTotal = NumberFormat.DecFormat(totallinetotalbymatch, 2);
                        itemdate.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbymatch, 2);
                        itemdate.TurnOver = NumberFormat.DecFormat(totalturnoverbymatch, 2);
                        itemdate.Margin = Math.Round(totalturnoverbymatch, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbymatch * 100 / totalturnoverbymatch, 2) + "%" : "n/a";
                        itemdate.Variance = NumberFormat.DecFormat(totallinetotalbymatch - totalvirtuallinetotalbymatch, 2);

                        grandwinloss += totalwinlossbymatch;
                        grandposition += totalpositionbymatch;
                        grandcommission += totalcommissionbymatch;
                        grandlinetotal += totallinetotalbymatch;
                        grandvirtuallinetotal += totalvirtuallinetotalbymatch;
                        grandturnover += totalturnoverbymatch;



                        lsrow.Add(itemdate);
                    }
                    granditem.Name = "Grand Total";
                    granditem.Winlost = NumberFormat.DecFormat(grandwinloss, 2);
                    granditem.Position = NumberFormat.DecFormat(grandposition, 2);
                    granditem.Commission = NumberFormat.DecFormat(grandcommission, 2);
                    granditem.LineTotal = NumberFormat.DecFormat(grandlinetotal, 2);
                    granditem.VirtualLineTotal = NumberFormat.DecFormat(grandvirtuallinetotal, 2);
                    granditem.TurnOver = NumberFormat.DecFormat(grandturnover, 2);
                    granditem.Margin = Math.Round(grandturnover, 2) != 0 ? NumberFormat.DecFormat(grandlinetotal * 100 / grandturnover, 2) + "%" : "n/a";
                    granditem.Variance = NumberFormat.DecFormat(grandlinetotal - grandvirtuallinetotal, 2);

                }
                numbergametreegrid.rows = lsrow;
                numbergametreegrid.totalrow = granditem;

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinlossByDate_SessionKey, numbergametreegrid);
            return numbergametreegrid;
            // return dt;
        }

        public NumberGameByDateTreeGridBO NumberGameWinLossByDate(string companies, string currencies, string bettypes, string fromdate, string todate)
        {
            var numbergametreegrid = new NumberGameByDateTreeGridBO();
            var lsrow = new List<NumberGameWinLostByDateBO>();
            var granditem = new NumberGameWinLostByDateBO();
            DataTable dt = null;
            try
            {
                currencies = currencies.Replace(";", ",");
                companies = companies.Replace(";", ",");
                bettypes = bettypes.Replace(";", ",");
                var _fdate = CastAide.AsDateTime(fromdate, DateTime.Today);
                var _todate = CastAide.AsDateTime(todate, DateTime.Today);
                dt = DataAccess.IBCDLL.Accounting.AccountReport.NumberGameWinlossByDate(_fdate, _todate, companies, currencies, bettypes).ResultSet.Tables[0];
                decimal grandwinloss = 0;
                decimal grandposition = 0;
                decimal grandcommission = 0;
                decimal grandlinetotal = 0;
                decimal grandvirtuallinetotal = 0;
                decimal grandturnover = 0;

                var listDate = GetDistinctDate(dt);
                if (listDate.Count > 0)
                {
                    foreach (DateTime _date in listDate)
                    {
                        var itemdate = new NumberGameWinLostByDateBO();
                        itemdate.children = new List<NumberGameWinLostByDateBO>();
                        decimal totalwinlossbymatch = 0;
                        decimal totalwinlossbylive = 0;

                        decimal totalpositionbymatch = 0;
                        decimal totalpositionbylive = 0;


                        decimal totalcommissionbymatch = 0;
                        decimal totalcommissionbylive = 0;

                        decimal totallinetotalbymatch = 0;
                        decimal totallinetotalbylive = 0;

                        decimal totalvirtuallinetotalbymatch = 0;
                        decimal totalvirtuallinetotalbylive = 0;

                        decimal totalturnoverbymatch = 0;
                        decimal totalturnoverbylive = 0;

                        decimal winloss = 0;
                        decimal position = 0;
                        decimal commission = 0;
                        decimal linetotal = 0;
                        decimal virtuallinetotal = 0;
                        decimal turnover = 0;
                        decimal margin = 0;

                        #region get by matchid, islive
                        DataRow[] rows = dt.Select(string.Format("date='{0}' and islive=1", _date));
                        if (rows.Length > 0)
                        {

                            var itemlive = new NumberGameWinLostByDateBO();
                            itemlive.children = new List<NumberGameWinLostByDateBO>();
                            itemlive.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "") + "_" + "Live";
                            itemlive.Name = "Live";

                            itemdate.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "");
                            itemdate.Name = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString();

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByDateBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = true;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }
                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;


                            }

                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemdate.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;


                        }
                        #endregion

                        #region get by matchid, islive=false
                        totalwinlossbylive = 0;
                        totalpositionbylive = 0;
                        totalcommissionbylive = 0;
                        totallinetotalbylive = 0;
                        totalvirtuallinetotalbylive = 0;
                        totalturnoverbylive = 0;

                        rows = dt.Select("date='" + _date + "' and islive=0");
                        if (rows.Length > 0)
                        {
                            var itemlive = new NumberGameWinLostByDateBO();
                            itemlive.children = new List<NumberGameWinLostByDateBO>();
                            itemlive.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "") + "_" + "NoneLive";
                            itemlive.Name = "None Live";

                            itemdate.Id = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString().Replace("/", "");
                            itemdate.Name = CastAide.AsDateTime(rows[0]["date"]).ToShortDateString();

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByDateBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = false;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = Math.Round(turnover, 2) != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;

                            }


                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2);
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemdate.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        itemdate.Winlost = NumberFormat.DecFormat(totalwinlossbymatch, 2);
                        itemdate.Position = NumberFormat.DecFormat(totalpositionbymatch, 2);
                        itemdate.Commission = NumberFormat.DecFormat(totalcommissionbymatch, 2);
                        itemdate.LineTotal = NumberFormat.DecFormat(totallinetotalbymatch, 2);
                        itemdate.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbymatch, 2);
                        itemdate.TurnOver = NumberFormat.DecFormat(totalturnoverbymatch, 2);
                        itemdate.Margin = Math.Round(totalturnoverbymatch, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbymatch * 100 / totalturnoverbymatch, 2) + "%" : "n/a";
                        itemdate.Variance = NumberFormat.DecFormat(totallinetotalbymatch - totalvirtuallinetotalbymatch, 2);

                        grandwinloss += totalwinlossbymatch;
                        grandposition += totalpositionbymatch;
                        grandcommission += totalcommissionbymatch;
                        grandlinetotal += totallinetotalbymatch;
                        grandvirtuallinetotal += totalvirtuallinetotalbymatch;
                        grandturnover += totalturnoverbymatch;



                        lsrow.Add(itemdate);
                    }
                    granditem.Name = "Grand Total";
                    granditem.Winlost = NumberFormat.DecFormat(grandwinloss, 2);
                    granditem.Position = NumberFormat.DecFormat(grandposition, 2);
                    granditem.Commission = NumberFormat.DecFormat(grandcommission, 2);
                    granditem.LineTotal = NumberFormat.DecFormat(grandlinetotal, 2);
                    granditem.VirtualLineTotal = NumberFormat.DecFormat(grandvirtuallinetotal, 2);
                    granditem.TurnOver = NumberFormat.DecFormat(grandturnover, 2);
                    granditem.Margin = Math.Round(grandturnover, 2) != 0 ? NumberFormat.DecFormat(grandlinetotal * 100 / grandturnover, 2) + "%" : "n/a";
                    granditem.Variance = NumberFormat.DecFormat(grandlinetotal - grandvirtuallinetotal, 2);

                }
                numbergametreegrid.rows = lsrow;
                numbergametreegrid.totalrow = granditem;

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinlossByDate_SessionKey, numbergametreegrid);
            return numbergametreegrid;
            // return dt;
        }
        #endregion

        #region Match winloss

        public NumberGameTreeGridBO WinLoss_ByMatch_NumberGame(string date)
        {

            NumberGameTreeGridBO numbergametreegrid = new NumberGameTreeGridBO();
            var lsrow = new List<NumberGameWinLostByMatchBO>();
            NumberGameWinLostByMatchBO granditem = new NumberGameWinLostByMatchBO();
            DataTable dt = null;
            try
            {
                DateTime _date = CastAide.AsDateTime(date, DateTime.Today);
                int SiteID = string.IsNullOrEmpty(HttpContext.Current.Request.Form["hiddenCompnayFilter"]) ? 0
                    : HttpContext.Current.Request.Form["hiddenCompnayFilter"].Contains("All") ? 0
                    : HttpContext.Current.Request.Form["hiddenCompnayFilter"].Contains("IBCGroup") ? -1
                    : CastAide.AsInt32(HttpContext.Current.Request.Form["hiddenCompnayFilter"]);

                int Currency = string.IsNullOrEmpty(HttpContext.Current.Request.Form["hiddenCurrencyFilter"]) ? 0
                : CastAide.AsInt32(HttpContext.Current.Request.Form["hiddenCurrencyFilter"]);

                dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_Rpt_NumberGameMemberWinlossByMatch(_date, SiteID, Currency).ResultSet.Tables[0];
                var listMatch = GetDistinctMatch(dt);
                decimal grandwinloss = 0;
                decimal grandposition = 0;
                decimal grandcommission = 0;
                decimal grandlinetotal = 0;
                decimal grandvirtuallinetotal = 0;
                decimal grandturnover = 0;

                if (listMatch.Count > 0)
                {
                    foreach (int matchid in listMatch)
                    {
                        var itemmatch = new NumberGameWinLostByMatchBO();
                        itemmatch.children = new List<NumberGameWinLostByMatchBO>();
                        decimal totalwinlossbymatch = 0;
                        decimal totalwinlossbylive = 0;

                        decimal totalpositionbymatch = 0;
                        decimal totalpositionbylive = 0;


                        decimal totalcommissionbymatch = 0;
                        decimal totalcommissionbylive = 0;

                        decimal totallinetotalbymatch = 0;
                        decimal totallinetotalbylive = 0;

                        decimal totalvirtuallinetotalbymatch = 0;
                        decimal totalvirtuallinetotalbylive = 0;

                        decimal totalturnoverbymatch = 0;
                        decimal totalturnoverbylive = 0;




                        decimal winloss = 0;
                        decimal position = 0;
                        decimal commission = 0;
                        decimal linetotal = 0;
                        decimal virtuallinetotal = 0;
                        decimal turnover = 0;
                        decimal margin = 0;



                        #region get by matchid, islive
                        DataRow[] rows = dt.Select("matchid=" + matchid + " and islive=1");
                        if (rows.Length > 0)
                        {

                            var itemlive = new NumberGameWinLostByMatchBO();
                            itemlive.children = new List<NumberGameWinLostByMatchBO>();
                            itemlive.Id = CastAide.AsInt32(rows[0]["matchcode"]) + "_" + "Live";
                            itemlive.Name = "Live";

                            itemmatch.Id = rows[0]["matchcode"].ToString();
                            itemmatch.Name = itemmatch.Id;

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByMatchBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = true;
                                itembettype.MatchId = CastAide.AsInt32(row["matchid"]);

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;
                            }

                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemmatch.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        #region get by matchid, islive=false
                        totalwinlossbylive = 0;
                        totalpositionbylive = 0;
                        totalcommissionbylive = 0;
                        totallinetotalbylive = 0;
                        totalvirtuallinetotalbylive = 0;
                        totalturnoverbylive = 0;

                        rows = dt.Select("matchid=" + matchid + " and islive=0");
                        if (rows.Length > 0)
                        {
                            var itemlive = new NumberGameWinLostByMatchBO();
                            itemlive.children = new List<NumberGameWinLostByMatchBO>();
                            itemlive.Id = CastAide.AsInt32(rows[0]["matchcode"]) + "_" + "NoneLive";
                            itemlive.Name = "None Live";

                            itemmatch.Id = rows[0]["matchcode"].ToString();
                            itemmatch.Name = itemmatch.Id;

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByMatchBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.MatchId = CastAide.AsInt32(row["matchid"]);
                                itembettype.IsLive = false;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;

                            }


                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemmatch.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        itemmatch.Winlost = NumberFormat.DecFormat(totalwinlossbymatch, 2);
                        itemmatch.Position = NumberFormat.DecFormat(totalpositionbymatch, 2);
                        itemmatch.Commission = NumberFormat.DecFormat(totalcommissionbymatch, 2);
                        itemmatch.LineTotal = NumberFormat.DecFormat(totallinetotalbymatch, 2);
                        itemmatch.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbymatch, 2);
                        itemmatch.TurnOver = NumberFormat.DecFormat(totalturnoverbymatch, 2);
                        itemmatch.Margin = Math.Round(totalturnoverbymatch, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbymatch * 100 / totalturnoverbymatch, 2) + "%" : "n/a";
                        itemmatch.Variance = NumberFormat.DecFormat(totallinetotalbymatch - totalvirtuallinetotalbymatch, 2);

                        grandwinloss += totalwinlossbymatch;
                        grandposition += totalpositionbymatch;
                        grandcommission += totalcommissionbymatch;
                        grandlinetotal += totallinetotalbymatch;
                        grandvirtuallinetotal += totalvirtuallinetotalbymatch;
                        grandturnover += totalturnoverbymatch;

                        lsrow.Add(itemmatch);
                    }
                    granditem.Name = "Grand Total";
                    granditem.Winlost = NumberFormat.DecFormat(grandwinloss, 2);
                    granditem.Position = NumberFormat.DecFormat(grandposition, 2);
                    granditem.Commission = NumberFormat.DecFormat(grandcommission, 2);
                    granditem.LineTotal = NumberFormat.DecFormat(grandlinetotal, 2);
                    granditem.VirtualLineTotal = NumberFormat.DecFormat(grandvirtuallinetotal, 2);
                    granditem.TurnOver = NumberFormat.DecFormat(grandturnover, 2);
                    granditem.Margin = Math.Round(grandturnover, 2) != 0 ? NumberFormat.DecFormat(grandlinetotal * 100 / grandturnover, 2) + "%" : "n/a";
                    granditem.Variance = NumberFormat.DecFormat(grandlinetotal - grandvirtuallinetotal, 2);
                }
                numbergametreegrid.rows = lsrow;
                numbergametreegrid.totalrow = granditem;

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinlossByMatch_SessionKey, numbergametreegrid);
            return numbergametreegrid;
            // return dt;
        }

        public NumberGameTreeGridBO WinLoss_ByMatch_NumberGame(string date, int SiteID, int Currency)
        {

            NumberGameTreeGridBO numbergametreegrid = new NumberGameTreeGridBO();
            var lsrow = new List<NumberGameWinLostByMatchBO>();
            NumberGameWinLostByMatchBO granditem = new NumberGameWinLostByMatchBO();
            DataTable dt = null;
            try
            {
                DateTime _date = CastAide.AsDateTime(date, DateTime.Today);
                dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_Rpt_NumberGameMemberWinlossByMatch(_date, SiteID, Currency).ResultSet.Tables[0];
                var listMatch = GetDistinctMatch(dt);
                decimal grandwinloss = 0;
                decimal grandposition = 0;
                decimal grandcommission = 0;
                decimal grandlinetotal = 0;
                decimal grandvirtuallinetotal = 0;
                decimal grandturnover = 0;

                if (listMatch.Count > 0)
                {
                    foreach (int matchid in listMatch)
                    {
                        var itemmatch = new NumberGameWinLostByMatchBO();
                        itemmatch.children = new List<NumberGameWinLostByMatchBO>();
                        decimal totalwinlossbymatch = 0;
                        decimal totalwinlossbylive = 0;

                        decimal totalpositionbymatch = 0;
                        decimal totalpositionbylive = 0;


                        decimal totalcommissionbymatch = 0;
                        decimal totalcommissionbylive = 0;

                        decimal totallinetotalbymatch = 0;
                        decimal totallinetotalbylive = 0;

                        decimal totalvirtuallinetotalbymatch = 0;
                        decimal totalvirtuallinetotalbylive = 0;

                        decimal totalturnoverbymatch = 0;
                        decimal totalturnoverbylive = 0;




                        decimal winloss = 0;
                        decimal position = 0;
                        decimal commission = 0;
                        decimal linetotal = 0;
                        decimal virtuallinetotal = 0;
                        decimal turnover = 0;
                        decimal margin = 0;



                        #region get by matchid, islive
                        DataRow[] rows = dt.Select("matchid=" + matchid + " and islive=1");
                        if (rows.Length > 0)
                        {

                            var itemlive = new NumberGameWinLostByMatchBO();
                            itemlive.children = new List<NumberGameWinLostByMatchBO>();
                            itemlive.Id = CastAide.AsInt32(rows[0]["matchcode"]) + "_" + "Live";
                            itemlive.Name = "Live";

                            itemmatch.Id = rows[0]["matchcode"].ToString();
                            itemmatch.Name = itemmatch.Id;

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByMatchBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = true;
                                itembettype.MatchId = CastAide.AsInt32(row["matchid"]);

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;
                            }

                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemmatch.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        #region get by matchid, islive=false
                        totalwinlossbylive = 0;
                        totalpositionbylive = 0;
                        totalcommissionbylive = 0;
                        totallinetotalbylive = 0;
                        totalvirtuallinetotalbylive = 0;
                        totalturnoverbylive = 0;

                        rows = dt.Select("matchid=" + matchid + " and islive=0");
                        if (rows.Length > 0)
                        {
                            var itemlive = new NumberGameWinLostByMatchBO();
                            itemlive.children = new List<NumberGameWinLostByMatchBO>();
                            itemlive.Id = CastAide.AsInt32(rows[0]["matchcode"]) + "_" + "NoneLive";
                            itemlive.Name = "None Live";

                            itemmatch.Id = rows[0]["matchcode"].ToString();
                            itemmatch.Name = itemmatch.Id;

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByMatchBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.MatchId = CastAide.AsInt32(row["matchid"]);
                                itembettype.IsLive = false;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;

                            }


                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemmatch.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        itemmatch.Winlost = NumberFormat.DecFormat(totalwinlossbymatch, 2);
                        itemmatch.Position = NumberFormat.DecFormat(totalpositionbymatch, 2);
                        itemmatch.Commission = NumberFormat.DecFormat(totalcommissionbymatch, 2);
                        itemmatch.LineTotal = NumberFormat.DecFormat(totallinetotalbymatch, 2);
                        itemmatch.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbymatch, 2);
                        itemmatch.TurnOver = NumberFormat.DecFormat(totalturnoverbymatch, 2);
                        itemmatch.Margin = Math.Round(totalturnoverbymatch, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbymatch * 100 / totalturnoverbymatch, 2) + "%" : "n/a";
                        itemmatch.Variance = NumberFormat.DecFormat(totallinetotalbymatch - totalvirtuallinetotalbymatch, 2);

                        grandwinloss += totalwinlossbymatch;
                        grandposition += totalpositionbymatch;
                        grandcommission += totalcommissionbymatch;
                        grandlinetotal += totallinetotalbymatch;
                        grandvirtuallinetotal += totalvirtuallinetotalbymatch;
                        grandturnover += totalturnoverbymatch;

                        lsrow.Add(itemmatch);
                    }
                    granditem.Name = "Grand Total";
                    granditem.Winlost = NumberFormat.DecFormat(grandwinloss, 2);
                    granditem.Position = NumberFormat.DecFormat(grandposition, 2);
                    granditem.Commission = NumberFormat.DecFormat(grandcommission, 2);
                    granditem.LineTotal = NumberFormat.DecFormat(grandlinetotal, 2);
                    granditem.VirtualLineTotal = NumberFormat.DecFormat(grandvirtuallinetotal, 2);
                    granditem.TurnOver = NumberFormat.DecFormat(grandturnover, 2);
                    granditem.Margin = Math.Round(grandturnover, 2) != 0 ? NumberFormat.DecFormat(grandlinetotal * 100 / grandturnover, 2) + "%" : "n/a";
                    granditem.Variance = NumberFormat.DecFormat(grandlinetotal - grandvirtuallinetotal, 2);
                }
                numbergametreegrid.rows = lsrow;
                numbergametreegrid.totalrow = granditem;

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinlossByMatch_SessionKey, numbergametreegrid);
            return numbergametreegrid;
            // return dt;
        }

        public NumberGameDataGridBO Acc_Rpt_NumberGameMemberWinlossByMatch_details(string date, bool islive, int bettype, int matchid)
        {
            NumberGameDataGridBO numbergamedatagrid = new NumberGameDataGridBO();
            DataTable dt = null;
            DateTime _date = CastAide.AsDateTime(date, DateTime.Today);
            dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_Rpt_NumberGameMemberWinlossByMatch_details(_date, islive, bettype, matchid).ResultSet.Tables[0];
            var lsrow = new List<NumberGameWinLostDetailBO>();
            decimal turnover = 0;
            decimal commission = 0;
            decimal memberwinloss = 0;
            decimal membercomm = 0;
            decimal membertotal = 0;
            decimal agentwinloss = 0;
            decimal agentcomm = 0;
            decimal agenttotal = 0;
            decimal masterwinloss = 0;
            decimal mastercomm = 0;
            decimal mastertotal = 0;
            decimal superwinloss = 0;
            decimal supercomm = 0;
            decimal supertotal = 0;
            decimal companytotal = 0;

            decimal sumturnover = 0;
            decimal sumcommission = 0;
            decimal summemberwinloss = 0;
            decimal summembercomm = 0;
            decimal summembertotal = 0;
            decimal sumagentwinloss = 0;
            decimal sumagentcomm = 0;
            decimal sumagenttotal = 0;
            decimal summasterwinloss = 0;
            decimal summastercomm = 0;
            decimal summastertotal = 0;
            decimal sumsuperwinloss = 0;
            decimal sumsupercomm = 0;
            decimal sumsupertotal = 0;
            decimal sumcompanytotal = 0;
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                var item = new NumberGameWinLostDetailBO();

                turnover = CastAide.AsDecimal(row["turnover"]);
                commission = CastAide.AsDecimal(row["grosscomm"]);
                memberwinloss = CastAide.AsDecimal(row["memberwinlost"]);
                membercomm = CastAide.AsDecimal(row["membercomm"]);
                membertotal = CastAide.AsDecimal(row["membertotal"]);
                agentwinloss = CastAide.AsDecimal(row["agwinlost"]);
                agentcomm = CastAide.AsDecimal(row["agentcomm"]);
                agenttotal = CastAide.AsDecimal(row["agenttotal"]);
                masterwinloss = CastAide.AsDecimal(row["masterwinlost"]);
                mastercomm = CastAide.AsDecimal(row["mastercomm"]);
                mastertotal = CastAide.AsDecimal(row["mastertotal"]);
                superwinloss = CastAide.AsDecimal(row["superwinlost"]);
                supercomm = CastAide.AsDecimal(row["supercomm"]);
                supertotal = CastAide.AsDecimal(row["supertotal"]);
                companytotal = CastAide.AsDecimal(row["company"]);



                sumturnover += turnover;
                sumcommission += commission;
                summemberwinloss += memberwinloss;
                summembercomm += membercomm;
                summembertotal += membertotal;
                sumagentwinloss += agentwinloss;
                sumagentcomm += agentcomm;
                sumagenttotal += agenttotal;
                summasterwinloss += masterwinloss;
                summastercomm += mastercomm;
                summastertotal += mastertotal;
                sumsuperwinloss += superwinloss;
                sumsupercomm += supercomm;
                sumsupertotal += supertotal;
                sumcompanytotal += companytotal;

                item.TurnOver = NumberFormat.DecFormat(turnover, 2);
                item.Commission = NumberFormat.DecFormat(commission, 2);
                item.MemberWinlost = NumberFormat.DecFormat(memberwinloss, 2);
                item.MemberComm = NumberFormat.DecFormat(membercomm, 2);
                item.MemberTotal = NumberFormat.DecFormat(membertotal, 2);
                item.AgentWinlost = NumberFormat.DecFormat(agentwinloss, 2);
                item.AgentComm = NumberFormat.DecFormat(agentcomm, 2);
                item.AgentTotal = NumberFormat.DecFormat(agenttotal, 2);
                item.MasterWinlost = NumberFormat.DecFormat(masterwinloss, 2);
                item.MasterComm = NumberFormat.DecFormat(mastercomm, 2);
                item.MasterTotal = NumberFormat.DecFormat(mastertotal, 2);
                item.SuperWinlost = NumberFormat.DecFormat(superwinloss, 2);
                item.SuperComm = NumberFormat.DecFormat(supercomm, 2);
                item.SuperTotal = NumberFormat.DecFormat(supertotal, 2);
                item.CompanyTotal = NumberFormat.DecFormat(companytotal, 2);
                item.Id = i.ToString();//row["custid"].ToString();
                item.Username = row["Username"].ToString();
                i++;
                lsrow.Add(item);
            }

            //for (var n = 0; n < 1000; n++)
            //{
            //    var item = new NumberGameWinLostDetailBO();

            //    turnover = 1000000 + n;
            //    commission = 1000000 + n;
            //    memberwinloss = 1000000 + n;
            //    membercomm = 1000000 + n;
            //    membertotal = 1000000 + n;
            //    agentwinloss = 1000000 + n;
            //    agentcomm = 1000000 + n;
            //    agenttotal = 1000000 + n;
            //    masterwinloss = 1000000 + n;
            //    mastercomm = 1000000 + n;
            //    mastertotal = 1000000 + n;
            //    superwinloss = 1000000 + n;
            //    supercomm = 1000000 + n;
            //    supertotal = 1000000 + n;
            //    companytotal = 1000000 + n;



            //    sumturnover += turnover;
            //    sumcommission += commission;
            //    summemberwinloss += memberwinloss;
            //    summembercomm += membercomm;
            //    summembertotal += membertotal;
            //    sumagentwinloss += agentwinloss;
            //    sumagentcomm += agentcomm;
            //    sumagenttotal += agenttotal;
            //    summasterwinloss += masterwinloss;
            //    summastercomm += mastercomm;
            //    summastertotal += mastertotal;
            //    sumsuperwinloss += superwinloss;
            //    sumsupercomm += supercomm;
            //    sumsupertotal += supertotal;
            //    sumcompanytotal += companytotal;

            //    item.TurnOver = NumberFormat.DecFormat(turnover, 2);
            //    item.Commission = NumberFormat.DecFormat(commission, 2);
            //    item.MemberWinlost = NumberFormat.DecFormat(memberwinloss, 2);
            //    item.MemberComm = NumberFormat.DecFormat(membercomm, 2);
            //    item.MemberTotal = NumberFormat.DecFormat(membertotal, 2);
            //    item.AgentWinlost = NumberFormat.DecFormat(agentwinloss, 2);
            //    item.AgentComm = NumberFormat.DecFormat(agentcomm, 2);
            //    item.AgentTotal = NumberFormat.DecFormat(agenttotal, 2);
            //    item.MasterWinlost = NumberFormat.DecFormat(masterwinloss, 2);
            //    item.MasterComm = NumberFormat.DecFormat(mastercomm, 2);
            //    item.MasterTotal = NumberFormat.DecFormat(mastertotal, 2);
            //    item.SuperWinlost = NumberFormat.DecFormat(superwinloss, 2);
            //    item.SuperComm = NumberFormat.DecFormat(supercomm, 2);
            //    item.SuperTotal = NumberFormat.DecFormat(supertotal, 2);
            //    item.CompanyTotal = NumberFormat.DecFormat(companytotal, 2);
            //    item.Id = i.ToString();//row["custid"].ToString();
            //    item.Username = "test";
            //    i++;
            //    lsrow.Add(item);
            //}

            NumberGameWinLostDetailBO totalitem = new NumberGameWinLostDetailBO();
            totalitem.Id = "&nbsp;";
            totalitem.Username = "Total";
            totalitem.TurnOver = NumberFormat.DecFormat(sumturnover, 2);
            totalitem.Commission = NumberFormat.DecFormat(sumcommission, 2);
            totalitem.MemberWinlost = NumberFormat.DecFormat(summemberwinloss, 2);
            totalitem.MemberComm = NumberFormat.DecFormat(summembercomm, 2);
            totalitem.MemberTotal = NumberFormat.DecFormat(summembertotal, 2);
            totalitem.AgentWinlost = NumberFormat.DecFormat(sumagentwinloss, 2);
            totalitem.AgentComm = NumberFormat.DecFormat(sumagentcomm, 2);
            totalitem.AgentTotal = NumberFormat.DecFormat(sumagenttotal, 2);
            totalitem.MasterWinlost = NumberFormat.DecFormat(summasterwinloss, 2);
            totalitem.MasterComm = NumberFormat.DecFormat(summastercomm, 2);
            totalitem.MasterTotal = NumberFormat.DecFormat(summastertotal, 2);
            totalitem.SuperWinlost = NumberFormat.DecFormat(sumsuperwinloss, 2);
            totalitem.SuperComm = NumberFormat.DecFormat(sumsupercomm, 2);
            totalitem.SuperTotal = NumberFormat.DecFormat(sumsupertotal, 2);
            totalitem.CompanyTotal = NumberFormat.DecFormat(sumcompanytotal, 2);


            numbergamedatagrid.rows = lsrow;
            numbergamedatagrid.footer = new List<NumberGameWinLostDetailBO>();
            numbergamedatagrid.footer.Add(totalitem);

            SessionExtension.AddSession(HttpContext.Current.Session, NumgerGameWinLossByMatchDetail_SessionKey, numbergamedatagrid);
            return numbergamedatagrid;

        }

        public NumberGameDataGridBO Acc_Rpt_NumberGameMemberWinlossByMatch_details(string date, bool islive, int bettype, int matchid, int SiteID, int Currency)
        {
            NumberGameDataGridBO numbergamedatagrid = new NumberGameDataGridBO();
            DataTable dt = null;
            DateTime _date = CastAide.AsDateTime(date, DateTime.Today);
            dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_Rpt_NumberGameMemberWinlossByMatch_details(_date, islive, bettype, matchid, SiteID, Currency).ResultSet.Tables[0];
            var lsrow = new List<NumberGameWinLostDetailBO>();
            decimal turnover = 0;
            decimal commission = 0;
            decimal memberwinloss = 0;
            decimal membercomm = 0;
            decimal membertotal = 0;
            decimal agentwinloss = 0;
            decimal agentcomm = 0;
            decimal agenttotal = 0;
            decimal masterwinloss = 0;
            decimal mastercomm = 0;
            decimal mastertotal = 0;
            decimal superwinloss = 0;
            decimal supercomm = 0;
            decimal supertotal = 0;
            decimal companytotal = 0;

            decimal sumturnover = 0;
            decimal sumcommission = 0;
            decimal summemberwinloss = 0;
            decimal summembercomm = 0;
            decimal summembertotal = 0;
            decimal sumagentwinloss = 0;
            decimal sumagentcomm = 0;
            decimal sumagenttotal = 0;
            decimal summasterwinloss = 0;
            decimal summastercomm = 0;
            decimal summastertotal = 0;
            decimal sumsuperwinloss = 0;
            decimal sumsupercomm = 0;
            decimal sumsupertotal = 0;
            decimal sumcompanytotal = 0;
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                var item = new NumberGameWinLostDetailBO();

                turnover = CastAide.AsDecimal(row["turnover"]);
                commission = CastAide.AsDecimal(row["grosscomm"]);
                memberwinloss = CastAide.AsDecimal(row["memberwinlost"]);
                membercomm = CastAide.AsDecimal(row["membercomm"]);
                membertotal = CastAide.AsDecimal(row["membertotal"]);
                agentwinloss = CastAide.AsDecimal(row["agwinlost"]);
                agentcomm = CastAide.AsDecimal(row["agentcomm"]);
                agenttotal = CastAide.AsDecimal(row["agenttotal"]);
                masterwinloss = CastAide.AsDecimal(row["masterwinlost"]);
                mastercomm = CastAide.AsDecimal(row["mastercomm"]);
                mastertotal = CastAide.AsDecimal(row["mastertotal"]);
                superwinloss = CastAide.AsDecimal(row["superwinlost"]);
                supercomm = CastAide.AsDecimal(row["supercomm"]);
                supertotal = CastAide.AsDecimal(row["supertotal"]);
                companytotal = CastAide.AsDecimal(row["company"]);



                sumturnover += turnover;
                sumcommission += commission;
                summemberwinloss += memberwinloss;
                summembercomm += membercomm;
                summembertotal += membertotal;
                sumagentwinloss += agentwinloss;
                sumagentcomm += agentcomm;
                sumagenttotal += agenttotal;
                summasterwinloss += masterwinloss;
                summastercomm += mastercomm;
                summastertotal += mastertotal;
                sumsuperwinloss += superwinloss;
                sumsupercomm += supercomm;
                sumsupertotal += supertotal;
                sumcompanytotal += companytotal;

                item.TurnOver = NumberFormat.DecFormat(turnover, 2);
                item.Commission = NumberFormat.DecFormat(commission, 2);
                item.MemberWinlost = NumberFormat.DecFormat(memberwinloss, 2);
                item.MemberComm = NumberFormat.DecFormat(membercomm, 2);
                item.MemberTotal = NumberFormat.DecFormat(membertotal, 2);
                item.AgentWinlost = NumberFormat.DecFormat(agentwinloss, 2);
                item.AgentComm = NumberFormat.DecFormat(agentcomm, 2);
                item.AgentTotal = NumberFormat.DecFormat(agenttotal, 2);
                item.MasterWinlost = NumberFormat.DecFormat(masterwinloss, 2);
                item.MasterComm = NumberFormat.DecFormat(mastercomm, 2);
                item.MasterTotal = NumberFormat.DecFormat(mastertotal, 2);
                item.SuperWinlost = NumberFormat.DecFormat(superwinloss, 2);
                item.SuperComm = NumberFormat.DecFormat(supercomm, 2);
                item.SuperTotal = NumberFormat.DecFormat(supertotal, 2);
                item.CompanyTotal = NumberFormat.DecFormat(companytotal, 2);
                item.Id = i.ToString();//row["custid"].ToString();
                item.Username = row["Username"].ToString();
                i++;
                lsrow.Add(item);
            }

            //for (var n = 0; n < 1000; n++)
            //{
            //    var item = new NumberGameWinLostDetailBO();

            //    turnover = 1000000 + n;
            //    commission = 1000000 + n;
            //    memberwinloss = 1000000 + n;
            //    membercomm = 1000000 + n;
            //    membertotal = 1000000 + n;
            //    agentwinloss = 1000000 + n;
            //    agentcomm = 1000000 + n;
            //    agenttotal = 1000000 + n;
            //    masterwinloss = 1000000 + n;
            //    mastercomm = 1000000 + n;
            //    mastertotal = 1000000 + n;
            //    superwinloss = 1000000 + n;
            //    supercomm = 1000000 + n;
            //    supertotal = 1000000 + n;
            //    companytotal = 1000000 + n;



            //    sumturnover += turnover;
            //    sumcommission += commission;
            //    summemberwinloss += memberwinloss;
            //    summembercomm += membercomm;
            //    summembertotal += membertotal;
            //    sumagentwinloss += agentwinloss;
            //    sumagentcomm += agentcomm;
            //    sumagenttotal += agenttotal;
            //    summasterwinloss += masterwinloss;
            //    summastercomm += mastercomm;
            //    summastertotal += mastertotal;
            //    sumsuperwinloss += superwinloss;
            //    sumsupercomm += supercomm;
            //    sumsupertotal += supertotal;
            //    sumcompanytotal += companytotal;

            //    item.TurnOver = NumberFormat.DecFormat(turnover, 2);
            //    item.Commission = NumberFormat.DecFormat(commission, 2);
            //    item.MemberWinlost = NumberFormat.DecFormat(memberwinloss, 2);
            //    item.MemberComm = NumberFormat.DecFormat(membercomm, 2);
            //    item.MemberTotal = NumberFormat.DecFormat(membertotal, 2);
            //    item.AgentWinlost = NumberFormat.DecFormat(agentwinloss, 2);
            //    item.AgentComm = NumberFormat.DecFormat(agentcomm, 2);
            //    item.AgentTotal = NumberFormat.DecFormat(agenttotal, 2);
            //    item.MasterWinlost = NumberFormat.DecFormat(masterwinloss, 2);
            //    item.MasterComm = NumberFormat.DecFormat(mastercomm, 2);
            //    item.MasterTotal = NumberFormat.DecFormat(mastertotal, 2);
            //    item.SuperWinlost = NumberFormat.DecFormat(superwinloss, 2);
            //    item.SuperComm = NumberFormat.DecFormat(supercomm, 2);
            //    item.SuperTotal = NumberFormat.DecFormat(supertotal, 2);
            //    item.CompanyTotal = NumberFormat.DecFormat(companytotal, 2);
            //    item.Id = i.ToString();//row["custid"].ToString();
            //    item.Username = "test";
            //    i++;
            //    lsrow.Add(item);
            //}

            NumberGameWinLostDetailBO totalitem = new NumberGameWinLostDetailBO();
            totalitem.Id = "&nbsp;";
            totalitem.Username = "Total";
            totalitem.TurnOver = NumberFormat.DecFormat(sumturnover, 2);
            totalitem.Commission = NumberFormat.DecFormat(sumcommission, 2);
            totalitem.MemberWinlost = NumberFormat.DecFormat(summemberwinloss, 2);
            totalitem.MemberComm = NumberFormat.DecFormat(summembercomm, 2);
            totalitem.MemberTotal = NumberFormat.DecFormat(summembertotal, 2);
            totalitem.AgentWinlost = NumberFormat.DecFormat(sumagentwinloss, 2);
            totalitem.AgentComm = NumberFormat.DecFormat(sumagentcomm, 2);
            totalitem.AgentTotal = NumberFormat.DecFormat(sumagenttotal, 2);
            totalitem.MasterWinlost = NumberFormat.DecFormat(summasterwinloss, 2);
            totalitem.MasterComm = NumberFormat.DecFormat(summastercomm, 2);
            totalitem.MasterTotal = NumberFormat.DecFormat(summastertotal, 2);
            totalitem.SuperWinlost = NumberFormat.DecFormat(sumsuperwinloss, 2);
            totalitem.SuperComm = NumberFormat.DecFormat(sumsupercomm, 2);
            totalitem.SuperTotal = NumberFormat.DecFormat(sumsupertotal, 2);
            totalitem.CompanyTotal = NumberFormat.DecFormat(sumcompanytotal, 2);


            numbergamedatagrid.rows = lsrow;
            numbergamedatagrid.footer = new List<NumberGameWinLostDetailBO>();
            numbergamedatagrid.footer.Add(totalitem);

            SessionExtension.AddSession(HttpContext.Current.Session, NumgerGameWinLossByMatchDetail_SessionKey, numbergamedatagrid);
            return numbergamedatagrid;

        }

        public NumberGameDataGridBO Acc_Rpt_NumberGameMemberWinlossByMatch_Details_Paging(string date, bool islive, int bettype, int matchid, int pageIndex, int pageSize, bool fromcache)
        {
            NumberGameDataGridBO numbergamedatagrid = new NumberGameDataGridBO();
            var numbergamedatagrid_session = SessionExtension.GetSession(HttpContext.Current.Session, NumgerGameWinLossByMatchDetail_SessionKey);
            if (numbergamedatagrid_session == null || fromcache == false)
            {

                #region get data
                DataTable dt = null;
                DateTime _date = CastAide.AsDateTime(date, DateTime.Today);
                dt = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_Rpt_NumberGameMemberWinlossByMatch_details(_date, islive, bettype, matchid).ResultSet.Tables[0];
                var lsrow = new List<NumberGameWinLostDetailBO>();
                decimal turnover = 0;
                decimal commission = 0;
                decimal memberwinloss = 0;
                decimal membercomm = 0;
                decimal membertotal = 0;
                decimal agentwinloss = 0;
                decimal agentcomm = 0;
                decimal agenttotal = 0;
                decimal masterwinloss = 0;
                decimal mastercomm = 0;
                decimal mastertotal = 0;
                decimal superwinloss = 0;
                decimal supercomm = 0;
                decimal supertotal = 0;
                decimal companytotal = 0;

                decimal sumturnover = 0;
                decimal sumcommission = 0;
                decimal summemberwinloss = 0;
                decimal summembercomm = 0;
                decimal summembertotal = 0;
                decimal sumagentwinloss = 0;
                decimal sumagentcomm = 0;
                decimal sumagenttotal = 0;
                decimal summasterwinloss = 0;
                decimal summastercomm = 0;
                decimal summastertotal = 0;
                decimal sumsuperwinloss = 0;
                decimal sumsupercomm = 0;
                decimal sumsupertotal = 0;
                decimal sumcompanytotal = 0;
                int i = 1;
                numbergamedatagrid.total = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    var item = new NumberGameWinLostDetailBO();

                    turnover = CastAide.AsDecimal(row["turnover"]);
                    commission = CastAide.AsDecimal(row["grosscomm"]);
                    memberwinloss = CastAide.AsDecimal(row["memberwinlost"]);
                    membercomm = CastAide.AsDecimal(row["membercomm"]);
                    membertotal = CastAide.AsDecimal(row["membertotal"]);
                    agentwinloss = CastAide.AsDecimal(row["agwinlost"]);
                    agentcomm = CastAide.AsDecimal(row["agentcomm"]);
                    agenttotal = CastAide.AsDecimal(row["agenttotal"]);
                    masterwinloss = CastAide.AsDecimal(row["masterwinlost"]);
                    mastercomm = CastAide.AsDecimal(row["mastercomm"]);
                    mastertotal = CastAide.AsDecimal(row["mastertotal"]);
                    superwinloss = CastAide.AsDecimal(row["superwinlost"]);
                    supercomm = CastAide.AsDecimal(row["supercomm"]);
                    supertotal = CastAide.AsDecimal(row["supertotal"]);
                    companytotal = CastAide.AsDecimal(row["company"]);



                    sumturnover += turnover;
                    sumcommission += commission;
                    summemberwinloss += memberwinloss;
                    summembercomm += membercomm;
                    summembertotal += membertotal;
                    sumagentwinloss += agentwinloss;
                    sumagentcomm += agentcomm;
                    sumagenttotal += agenttotal;
                    summasterwinloss += masterwinloss;
                    summastercomm += mastercomm;
                    summastertotal += mastertotal;
                    sumsuperwinloss += superwinloss;
                    sumsupercomm += supercomm;
                    sumsupertotal += supertotal;
                    sumcompanytotal += companytotal;

                    item.TurnOver = NumberFormat.DecFormat(turnover, 2);
                    item.Commission = NumberFormat.DecFormat(commission, 2);
                    item.MemberWinlost = NumberFormat.DecFormat(memberwinloss, 2);
                    item.MemberComm = NumberFormat.DecFormat(membercomm, 2);
                    item.MemberTotal = NumberFormat.DecFormat(membertotal, 2);
                    item.AgentWinlost = NumberFormat.DecFormat(agentwinloss, 2);
                    item.AgentComm = NumberFormat.DecFormat(agentcomm, 2);
                    item.AgentTotal = NumberFormat.DecFormat(agenttotal, 2);
                    item.MasterWinlost = NumberFormat.DecFormat(masterwinloss, 2);
                    item.MasterComm = NumberFormat.DecFormat(mastercomm, 2);
                    item.MasterTotal = NumberFormat.DecFormat(mastertotal, 2);
                    item.SuperWinlost = NumberFormat.DecFormat(superwinloss, 2);
                    item.SuperComm = NumberFormat.DecFormat(supercomm, 2);
                    item.SuperTotal = NumberFormat.DecFormat(supertotal, 2);
                    item.CompanyTotal = NumberFormat.DecFormat(companytotal, 2);
                    item.Id = i.ToString();//row["custid"].ToString();
                    item.Username = row["Username"].ToString();
                    i++;
                    lsrow.Add(item);
                }

                NumberGameWinLostDetailBO totalitem = new NumberGameWinLostDetailBO();
                totalitem.Id = "&nbsp;";
                totalitem.Username = "Total";
                totalitem.TurnOver = NumberFormat.DecFormat(sumturnover, 2);
                totalitem.Commission = NumberFormat.DecFormat(sumcommission, 2);
                totalitem.MemberWinlost = NumberFormat.DecFormat(summemberwinloss, 2);
                totalitem.MemberComm = NumberFormat.DecFormat(summembercomm, 2);
                totalitem.MemberTotal = NumberFormat.DecFormat(summembertotal, 2);
                totalitem.AgentWinlost = NumberFormat.DecFormat(sumagentwinloss, 2);
                totalitem.AgentComm = NumberFormat.DecFormat(sumagentcomm, 2);
                totalitem.AgentTotal = NumberFormat.DecFormat(sumagenttotal, 2);
                totalitem.MasterWinlost = NumberFormat.DecFormat(summasterwinloss, 2);
                totalitem.MasterComm = NumberFormat.DecFormat(summastercomm, 2);
                totalitem.MasterTotal = NumberFormat.DecFormat(summastertotal, 2);
                totalitem.SuperWinlost = NumberFormat.DecFormat(sumsuperwinloss, 2);
                totalitem.SuperComm = NumberFormat.DecFormat(sumsupercomm, 2);
                totalitem.SuperTotal = NumberFormat.DecFormat(sumsupertotal, 2);
                totalitem.CompanyTotal = NumberFormat.DecFormat(sumcompanytotal, 2);


                numbergamedatagrid.rows = lsrow;
                numbergamedatagrid.footer = new List<NumberGameWinLostDetailBO>();
                numbergamedatagrid.footer.Add(totalitem);

                SessionExtension.AddSession(HttpContext.Current.Session, NumgerGameWinLossByMatchDetail_SessionKey, numbergamedatagrid);


                #endregion
            }
            else
            {
                numbergamedatagrid = (NumberGameDataGridBO)numbergamedatagrid_session;
            }
            #region paging
            int from = pageSize * (pageIndex - 1) + 1;
            int to = pageSize * pageIndex;

            var pagingrows = new List<NumberGameWinLostDetailBO>();

            int n = 1;
            //var test = SessionExtension.GetSession(HttpContext.Current.Session, NumgerGameWinLossByMatchDetail_SessionKey);
            foreach (NumberGameWinLostDetailBO row in numbergamedatagrid.rows)
            {
                if (n <= to && n >= from)
                {
                    pagingrows.Add(row);
                }
                n++;
            }
            NumberGameDataGridBO numbergamedatagridpaging = new NumberGameDataGridBO();
            numbergamedatagridpaging.rows = pagingrows;
            numbergamedatagridpaging.total = numbergamedatagrid.total;
            numbergamedatagridpaging.footer = numbergamedatagrid.footer;

            #endregion
            return numbergamedatagridpaging;
        }

        public NumberGameTreeGridBO NumberGameWinLossByMatch(string date, string companies, string currencies, string bettypes)
        {
            var numbergametreegrid = new NumberGameTreeGridBO();
            var lsrow = new List<NumberGameWinLostByMatchBO>();
            var granditem = new NumberGameWinLostByMatchBO();
            DataTable dt = null;
            try
            {
                currencies = currencies.Replace(";", ",");
                companies = companies.Replace(";", ",");
                bettypes = bettypes.Replace(";", ",");

                var _date = CastAide.AsDateTime(date, DateTime.Today);
                dt = DataAccess.IBCDLL.Accounting.AccountReport.NumberGameWinlossByMatch(_date, companies, currencies, bettypes).ResultSet.Tables[0];
                var listMatch = GetDistinctMatch(dt);
                decimal grandwinloss = 0;
                decimal grandposition = 0;
                decimal grandcommission = 0;
                decimal grandlinetotal = 0;
                decimal grandvirtuallinetotal = 0;
                decimal grandturnover = 0;

                if (listMatch.Count > 0)
                {
                    foreach (int matchid in listMatch)
                    {
                        var itemmatch = new NumberGameWinLostByMatchBO();
                        itemmatch.children = new List<NumberGameWinLostByMatchBO>();
                        decimal totalwinlossbymatch = 0;
                        decimal totalwinlossbylive = 0;

                        decimal totalpositionbymatch = 0;
                        decimal totalpositionbylive = 0;


                        decimal totalcommissionbymatch = 0;
                        decimal totalcommissionbylive = 0;

                        decimal totallinetotalbymatch = 0;
                        decimal totallinetotalbylive = 0;

                        decimal totalvirtuallinetotalbymatch = 0;
                        decimal totalvirtuallinetotalbylive = 0;

                        decimal totalturnoverbymatch = 0;
                        decimal totalturnoverbylive = 0;

                        decimal winloss = 0;
                        decimal position = 0;
                        decimal commission = 0;
                        decimal linetotal = 0;
                        decimal virtuallinetotal = 0;
                        decimal turnover = 0;
                        decimal margin = 0;



                        #region get by matchid, islive
                        DataRow[] rows = dt.Select("matchid=" + matchid + " and islive=1");
                        if (rows.Length > 0)
                        {

                            var itemlive = new NumberGameWinLostByMatchBO();
                            itemlive.children = new List<NumberGameWinLostByMatchBO>();
                            itemlive.Id = CastAide.AsInt32(rows[0]["matchcode"]) + "_" + "Live";
                            itemlive.Name = "Live";

                            itemmatch.Id = rows[0]["matchcode"].ToString();
                            itemmatch.Name = itemmatch.Id;

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByMatchBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.IsLive = true;
                                itembettype.MatchId = CastAide.AsInt32(row["matchid"]);

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;
                            }

                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemmatch.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        #region get by matchid, islive=false
                        totalwinlossbylive = 0;
                        totalpositionbylive = 0;
                        totalcommissionbylive = 0;
                        totallinetotalbylive = 0;
                        totalvirtuallinetotalbylive = 0;
                        totalturnoverbylive = 0;

                        rows = dt.Select("matchid=" + matchid + " and islive=0");
                        if (rows.Length > 0)
                        {
                            var itemlive = new NumberGameWinLostByMatchBO();
                            itemlive.children = new List<NumberGameWinLostByMatchBO>();
                            itemlive.Id = CastAide.AsInt32(rows[0]["matchcode"]) + "_" + "NoneLive";
                            itemlive.Name = "None Live";

                            itemmatch.Id = rows[0]["matchcode"].ToString();
                            itemmatch.Name = itemmatch.Id;

                            foreach (DataRow row in rows)
                            {
                                var itembettype = new NumberGameWinLostByMatchBO();
                                itembettype.BetType = CastAide.AsInt32(row["bettype"]);
                                itembettype.Id = itemlive.Id + "_" + itembettype.BetType;
                                itembettype.Name = CacheRef.GetBettype(CastAide.AsInt32(row["bettype"]));
                                itembettype.MatchId = CastAide.AsInt32(row["matchid"]);
                                itembettype.IsLive = false;

                                winloss = CastAide.AsDecimal(row["winlost"]);
                                position = CastAide.AsDecimal(row["Position"]);
                                commission = CastAide.AsDecimal(row["commission"]);
                                linetotal = CastAide.AsDecimal(row["linetotal"]);
                                virtuallinetotal = CastAide.AsDecimal(row["virtuallinetotal"]);
                                turnover = CastAide.AsDecimal(row["turnover"]);
                                if (Math.Round(turnover, 2) != 0)
                                {
                                    margin = linetotal * 100 / turnover;
                                }

                                itembettype.Winlost = NumberFormat.DecFormat(winloss, 2);
                                itembettype.Position = NumberFormat.DecFormat(position, 2);
                                itembettype.Commission = NumberFormat.DecFormat(commission, 2);
                                itembettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                                itembettype.VirtualLineTotal = NumberFormat.DecFormat(virtuallinetotal, 2);
                                itembettype.TurnOver = NumberFormat.DecFormat(turnover, 2);
                                itembettype.Margin = margin != 0 ? NumberFormat.DecFormat(margin, 2) + "%" : "n/a";
                                itembettype.Variance = NumberFormat.DecFormat(linetotal - virtuallinetotal, 2);

                                itemlive.children.Add(itembettype);

                                totalwinlossbylive += winloss;
                                totalpositionbylive += position;
                                totalcommissionbylive += commission;
                                totallinetotalbylive += linetotal;
                                totalvirtuallinetotalbylive += virtuallinetotal;
                                totalturnoverbylive += turnover;

                            }

                            itemlive.Winlost = NumberFormat.DecFormat(totalwinlossbylive, 2); ;
                            itemlive.Position = NumberFormat.DecFormat(totalpositionbylive, 2); ;
                            itemlive.Commission = NumberFormat.DecFormat(totalcommissionbylive, 2); ;
                            itemlive.LineTotal = NumberFormat.DecFormat(totallinetotalbylive, 2); ;
                            itemlive.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbylive, 2); ;
                            itemlive.TurnOver = NumberFormat.DecFormat(totalturnoverbylive, 2); ;
                            itemlive.Margin = Math.Round(totalturnoverbylive, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbylive * 100 / totalturnoverbylive, 2) + "%" : "n/a";
                            itemlive.Variance = NumberFormat.DecFormat(totallinetotalbylive - totalvirtuallinetotalbylive, 2);


                            itemmatch.children.Add(itemlive);

                            totalwinlossbymatch += totalwinlossbylive;
                            totalpositionbymatch += totalpositionbylive;
                            totalcommissionbymatch += totalcommissionbylive;
                            totallinetotalbymatch += totallinetotalbylive;
                            totalvirtuallinetotalbymatch += totalvirtuallinetotalbylive;
                            totalturnoverbymatch += totalturnoverbylive;

                        }
                        #endregion

                        itemmatch.Winlost = NumberFormat.DecFormat(totalwinlossbymatch, 2);
                        itemmatch.Position = NumberFormat.DecFormat(totalpositionbymatch, 2);
                        itemmatch.Commission = NumberFormat.DecFormat(totalcommissionbymatch, 2);
                        itemmatch.LineTotal = NumberFormat.DecFormat(totallinetotalbymatch, 2);
                        itemmatch.VirtualLineTotal = NumberFormat.DecFormat(totalvirtuallinetotalbymatch, 2);
                        itemmatch.TurnOver = NumberFormat.DecFormat(totalturnoverbymatch, 2);
                        itemmatch.Margin = Math.Round(totalturnoverbymatch, 2) != 0 ? NumberFormat.DecFormat(totallinetotalbymatch * 100 / totalturnoverbymatch, 2) + "%" : "n/a";
                        itemmatch.Variance = NumberFormat.DecFormat(totallinetotalbymatch - totalvirtuallinetotalbymatch, 2);

                        grandwinloss += totalwinlossbymatch;
                        grandposition += totalpositionbymatch;
                        grandcommission += totalcommissionbymatch;
                        grandlinetotal += totallinetotalbymatch;
                        grandvirtuallinetotal += totalvirtuallinetotalbymatch;
                        grandturnover += totalturnoverbymatch;

                        lsrow.Add(itemmatch);
                    }
                    granditem.Name = "Grand Total";
                    granditem.Winlost = NumberFormat.DecFormat(grandwinloss, 2);
                    granditem.Position = NumberFormat.DecFormat(grandposition, 2);
                    granditem.Commission = NumberFormat.DecFormat(grandcommission, 2);
                    granditem.LineTotal = NumberFormat.DecFormat(grandlinetotal, 2);
                    granditem.VirtualLineTotal = NumberFormat.DecFormat(grandvirtuallinetotal, 2);
                    granditem.TurnOver = NumberFormat.DecFormat(grandturnover, 2);
                    granditem.Margin = Math.Round(grandturnover, 2) != 0 ? NumberFormat.DecFormat(grandlinetotal * 100 / grandturnover, 2) + "%" : "n/a";
                    granditem.Variance = NumberFormat.DecFormat(grandlinetotal - grandvirtuallinetotal, 2);
                }
                numbergametreegrid.rows = lsrow;
                numbergametreegrid.totalrow = granditem;

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinlossByMatch_SessionKey, numbergametreegrid);
            return numbergametreegrid;
            // return dt;
        }

        public NumberGameDataGridBO NumberGameWinLossByMatchDetails(string date, bool islive, int matchid, string companies, string currencies, int bettype)
        {
            var numbergamedatagrid = new NumberGameDataGridBO();
            DataTable dt = null;
            var _date = CastAide.AsDateTime(date, DateTime.Today);
            currencies = currencies.Replace(";", ",");
            companies = companies.Replace(";", ",");

            dt = DataAccess.IBCDLL.Accounting.AccountReport.NumberGameWinlossByMatchDetail(_date, islive, matchid, companies, currencies, bettype).ResultSet.Tables[0];
            var lsrow = new List<NumberGameWinLostDetailBO>();
            decimal turnover = 0;
            decimal commission = 0;
            decimal memberwinloss = 0;
            decimal membercomm = 0;
            decimal membertotal = 0;
            decimal agentwinloss = 0;
            decimal agentcomm = 0;
            decimal agenttotal = 0;
            decimal masterwinloss = 0;
            decimal mastercomm = 0;
            decimal mastertotal = 0;
            decimal superwinloss = 0;
            decimal supercomm = 0;
            decimal supertotal = 0;
            decimal companytotal = 0;

            decimal sumturnover = 0;
            decimal sumcommission = 0;
            decimal summemberwinloss = 0;
            decimal summembercomm = 0;
            decimal summembertotal = 0;
            decimal sumagentwinloss = 0;
            decimal sumagentcomm = 0;
            decimal sumagenttotal = 0;
            decimal summasterwinloss = 0;
            decimal summastercomm = 0;
            decimal summastertotal = 0;
            decimal sumsuperwinloss = 0;
            decimal sumsupercomm = 0;
            decimal sumsupertotal = 0;
            decimal sumcompanytotal = 0;
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                var item = new NumberGameWinLostDetailBO();

                turnover = CastAide.AsDecimal(row["turnover"]);
                commission = CastAide.AsDecimal(row["grosscomm"]);
                memberwinloss = CastAide.AsDecimal(row["memberwinlost"]);
                membercomm = CastAide.AsDecimal(row["membercomm"]);
                membertotal = CastAide.AsDecimal(row["membertotal"]);
                agentwinloss = CastAide.AsDecimal(row["agwinlost"]);
                agentcomm = CastAide.AsDecimal(row["agentcomm"]);
                agenttotal = CastAide.AsDecimal(row["agenttotal"]);
                masterwinloss = CastAide.AsDecimal(row["masterwinlost"]);
                mastercomm = CastAide.AsDecimal(row["mastercomm"]);
                mastertotal = CastAide.AsDecimal(row["mastertotal"]);
                superwinloss = CastAide.AsDecimal(row["superwinlost"]);
                supercomm = CastAide.AsDecimal(row["supercomm"]);
                supertotal = CastAide.AsDecimal(row["supertotal"]);
                companytotal = CastAide.AsDecimal(row["company"]);



                sumturnover += turnover;
                sumcommission += commission;
                summemberwinloss += memberwinloss;
                summembercomm += membercomm;
                summembertotal += membertotal;
                sumagentwinloss += agentwinloss;
                sumagentcomm += agentcomm;
                sumagenttotal += agenttotal;
                summasterwinloss += masterwinloss;
                summastercomm += mastercomm;
                summastertotal += mastertotal;
                sumsuperwinloss += superwinloss;
                sumsupercomm += supercomm;
                sumsupertotal += supertotal;
                sumcompanytotal += companytotal;

                item.TurnOver = NumberFormat.DecFormat(turnover, 2);
                item.Commission = NumberFormat.DecFormat(commission, 2);
                item.MemberWinlost = NumberFormat.DecFormat(memberwinloss, 2);
                item.MemberComm = NumberFormat.DecFormat(membercomm, 2);
                item.MemberTotal = NumberFormat.DecFormat(membertotal, 2);
                item.AgentWinlost = NumberFormat.DecFormat(agentwinloss, 2);
                item.AgentComm = NumberFormat.DecFormat(agentcomm, 2);
                item.AgentTotal = NumberFormat.DecFormat(agenttotal, 2);
                item.MasterWinlost = NumberFormat.DecFormat(masterwinloss, 2);
                item.MasterComm = NumberFormat.DecFormat(mastercomm, 2);
                item.MasterTotal = NumberFormat.DecFormat(mastertotal, 2);
                item.SuperWinlost = NumberFormat.DecFormat(superwinloss, 2);
                item.SuperComm = NumberFormat.DecFormat(supercomm, 2);
                item.SuperTotal = NumberFormat.DecFormat(supertotal, 2);
                item.CompanyTotal = NumberFormat.DecFormat(companytotal, 2);
                item.Id = i.ToString();//row["custid"].ToString();
                item.Username = row["Username"].ToString();
                i++;
                lsrow.Add(item);
            }
            var totalitem = new NumberGameWinLostDetailBO();
            totalitem.Id = "&nbsp;";
            totalitem.Username = "Total";
            totalitem.TurnOver = NumberFormat.DecFormat(sumturnover, 2);
            totalitem.Commission = NumberFormat.DecFormat(sumcommission, 2);
            totalitem.MemberWinlost = NumberFormat.DecFormat(summemberwinloss, 2);
            totalitem.MemberComm = NumberFormat.DecFormat(summembercomm, 2);
            totalitem.MemberTotal = NumberFormat.DecFormat(summembertotal, 2);
            totalitem.AgentWinlost = NumberFormat.DecFormat(sumagentwinloss, 2);
            totalitem.AgentComm = NumberFormat.DecFormat(sumagentcomm, 2);
            totalitem.AgentTotal = NumberFormat.DecFormat(sumagenttotal, 2);
            totalitem.MasterWinlost = NumberFormat.DecFormat(summasterwinloss, 2);
            totalitem.MasterComm = NumberFormat.DecFormat(summastercomm, 2);
            totalitem.MasterTotal = NumberFormat.DecFormat(summastertotal, 2);
            totalitem.SuperWinlost = NumberFormat.DecFormat(sumsuperwinloss, 2);
            totalitem.SuperComm = NumberFormat.DecFormat(sumsupercomm, 2);
            totalitem.SuperTotal = NumberFormat.DecFormat(sumsupertotal, 2);
            totalitem.CompanyTotal = NumberFormat.DecFormat(sumcompanytotal, 2);


            numbergamedatagrid.rows = lsrow;
            numbergamedatagrid.footer = new List<NumberGameWinLostDetailBO>();
            numbergamedatagrid.footer.Add(totalitem);

            SessionExtension.AddSession(HttpContext.Current.Session, NumgerGameWinLossByMatchDetail_SessionKey, numbergamedatagrid);
            return numbergamedatagrid;
        }
        #endregion

        #region Monthly Winloss

        /// <summary>
        /// Number Game monthly winloss
        /// </summary>
        /// <param name="month">month</param>
        /// <param name="year">year</param>
        /// <returns></returns>
        public MonthlyWinloss MonthlyWinloss(int month, int year)
        {
            var monthlyWinloss = new MonthlyWinloss();
            try
            {
                monthlyWinloss = _numberGameDAO.MonthlyWinloss(month, year);

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            SessionExtension.AddSession(HttpContext.Current.Session, NumberGameMonthlyWinloss_SessionKey, monthlyWinloss);

            return monthlyWinloss;
        }

        public void MonthlyWinlossExportExcel(int month, int year)
        {
            var excelFile = new ExcelFile();
            var reportName = "NumberGameMonthlyWinLoss_" + month + "-" + year;
            try
            {
                var objMonthlyWinloss = SessionExtension.GetSession(HttpContext.Current.Session,
                                                                    NumberGameMonthlyWinloss_SessionKey);

                var monthlyWinloss = objMonthlyWinloss != null
                                         ? (MonthlyWinloss)objMonthlyWinloss
                                         : MonthlyWinloss(month, year);


                ExcelWorksheet wsNumberGameReport = excelFile.Worksheets.Add("NumberGameReport");
                var bgHeader = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Center,
                                           ExcelFont.BoldWeight, false, false);




                #region assign Header

                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Merged = true;
                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Style = bgHeader;
                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Value =
                    string.Format("Number Game Monthly Win Loss {0}/{1}", month, year);

                var indexHeader = 0;
                wsNumberGameReport.Cells[1, indexHeader].Style = bgHeader;
                wsNumberGameReport.Cells[1, indexHeader++].Value = "Winlost";
                wsNumberGameReport.Cells[1, indexHeader].Style = bgHeader;

                wsNumberGameReport.Cells[1, indexHeader++].Value = "Position";
                wsNumberGameReport.Cells[1, indexHeader].Style = bgHeader;
                wsNumberGameReport.Columns[indexHeader].Width = 120 * 30;
                wsNumberGameReport.Cells[1, indexHeader++].Value = "Commission";
                wsNumberGameReport.Cells[1, indexHeader].Style = bgHeader;
                wsNumberGameReport.Cells[1, indexHeader++].Value = "Line Total";
                wsNumberGameReport.Cells[1, indexHeader].Style = bgHeader;
                wsNumberGameReport.Cells[1, indexHeader++].Value = "Turnover";
                wsNumberGameReport.Cells[1, indexHeader].Style = bgHeader;
                wsNumberGameReport.Cells[1, indexHeader++].Value = "Margin";

                #endregion


                var indexBody = 0;

                wsNumberGameReport.Cells[2, indexBody].Style.Font.Color = monthlyWinloss.Winlost < 0 ? Color.Red : Color.Black;
                wsNumberGameReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsNumberGameReport.Cells[2, indexBody++].Value = monthlyWinloss.Winlost;

                wsNumberGameReport.Cells[2, indexBody].Style.Font.Color = monthlyWinloss.Position < 0 ? Color.Red : Color.Black;
                wsNumberGameReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsNumberGameReport.Cells[2, indexBody++].Value = monthlyWinloss.Position;

                wsNumberGameReport.Cells[2, indexBody].Style.Font.Color = monthlyWinloss.Commission < 0 ? Color.Red : Color.Black;
                wsNumberGameReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsNumberGameReport.Cells[2, indexBody++].Value = monthlyWinloss.Commission;

                wsNumberGameReport.Cells[2, indexBody].Style.Font.Color = monthlyWinloss.LineTotal < 0 ? Color.Red : Color.Black;
                wsNumberGameReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsNumberGameReport.Cells[2, indexBody++].Value = monthlyWinloss.LineTotal;

                wsNumberGameReport.Cells[2, indexBody].Style.Font.Color = monthlyWinloss.TurnOver < 0 ? Color.Red : Color.Black;
                wsNumberGameReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsNumberGameReport.Cells[2, indexBody++].Value = monthlyWinloss.TurnOver;

                wsNumberGameReport.Cells[2, indexBody].Style.Font.Color = monthlyWinloss.Margin < 0 ? Color.Red : Color.Black;
                wsNumberGameReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsNumberGameReport.Cells[2, indexBody++].Value = monthlyWinloss.Margin;



            }
            catch (Exception ex)
            {


                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            finally
            {

                #region response excel file
                //excelFile.Worksheets.Add(reportName);
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
        }

        #endregion

        public void ExportToExcel_WinLoss_ByMatch_NumberGame(string date)
        {
            NumberGameTreeGridBO numbergame = new NumberGameTreeGridBO();
            var numbergamesessionobject = SessionExtension.GetSession(HttpContext.Current.Session, NumberGameWinlossByMatch_SessionKey);
            if (numbergamesessionobject != null)
                numbergame = (NumberGameTreeGridBO)numbergamesessionobject;
            else
                numbergame = WinLoss_ByMatch_NumberGame(date);

            Export2Excel(date, numbergame);
        }

        public void ExportToExcelNumberGameWinLossByMatch(string date, string companies, string currencies, string bettypes)
        {
            var numbergame = new NumberGameTreeGridBO();
            var numbergamesessionobject = SessionExtension.GetSession(HttpContext.Current.Session, NumberGameWinlossByMatch_SessionKey);
            if (numbergamesessionobject != null)
                numbergame = (NumberGameTreeGridBO)numbergamesessionobject;
            else
                numbergame = NumberGameWinLossByMatch(date, companies, currencies, bettypes);

            Export2Excel(date, numbergame);
        }

        public void ExportToExcel_WinLoss_ByMatchDetail_NumberGame(string date, bool isLive, int bettype, int matchId)
        {
            var numberGameData = new NumberGameDataGridBO();
            var numberGameSession = SessionExtension.GetSession(HttpContext.Current.Session, NumgerGameWinLossByMatchDetail_SessionKey);
            if (numberGameSession != null)
                numberGameData = (NumberGameDataGridBO)numberGameSession;
            else
                numberGameData = Acc_Rpt_NumberGameMemberWinlossByMatch_details(date, isLive, bettype, matchId);

            Export2ExcelWinLossByMatchDetail(date, numberGameData, isLive, bettype, matchId);
        }

        public void ExportToExcelNumberGameWinLossDetailByMatch(string date, bool isLive, int bettype, int matchId, string companies, string currencies)
        {
            var numberGameData = new NumberGameDataGridBO();
            var numberGameSession = SessionExtension.GetSession(HttpContext.Current.Session, NumgerGameWinLossByMatchDetail_SessionKey);
            if (numberGameSession != null)
                numberGameData = (NumberGameDataGridBO)numberGameSession;
            else
                numberGameData = NumberGameWinLossByMatchDetails(date, isLive, matchId, companies, currencies, bettype);

            Export2ExcelWinLossByMatchDetail(date, numberGameData, isLive, bettype, matchId);
        }

        public void ExportToExcel_WinLoss_ByDate_NumberGame(string fromdate, string todate)
        {
            NumberGameByDateTreeGridBO numbergame = new NumberGameByDateTreeGridBO();
            var numbergamesessionobject = SessionExtension.GetSession(HttpContext.Current.Session, NumberGameWinlossByDate_SessionKey);
            if (numbergamesessionobject != null)
            {
                numbergame = (NumberGameByDateTreeGridBO)numbergamesessionobject;
            }
            else
            {
                numbergame = WinLoss_ByDate_NumberGame(fromdate, todate);
            }

            Export2ExcelNumberGameWinlossByDate(fromdate, todate, numbergame);

        }

        #region export 2 excel
        private void Export2Excel(string date, NumberGameTreeGridBO data)
        {
            #region local variables
            #region Header
            const string matchBetTypeHeader = "Match - Bet Type";
            const string winLostHeader = "Winloss";
            const string positionHeader = "Position";
            const string commHeader = "Commission";
            const string lineTotalHeader = "Line Total";
            const string virutalTotalHeader = "Virtual Total";
            const string varianceHeader = "Variance";
            const string turnOverHeader = "Turnover";
            const string marginHeader = "Margin";
            #endregion
            var reportName = string.Format("MatchWinLostByMatch-NumberGame-{0}", date);
            const int startRowIndex = 2;
            const int startColumnIndex = 0;
            int nextRow = 0, nextColumn = 0;
            ExcelFile excelFile = new ExcelFile();
            ExcelWorksheet wsNumberGameReport = excelFile.Worksheets.Add("MatchWinLostByMatch");
            var bgHeader = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Center, ExcelFont.BoldWeight, false, false);
            #endregion

            try
            {
                #region assign Header
                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, startColumnIndex, 0, startColumnIndex + (6)).Merged = true;
                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, startColumnIndex, 0, startColumnIndex + (6)).Style = bgHeader;
                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, startColumnIndex, 0, startColumnIndex + (6)).Value = string.Format("Number Game Win Loss By Match - Date {0}", date);

                wsNumberGameReport.Columns[startColumnIndex].Width = 20 * 256;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = matchBetTypeHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = winLostHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = positionHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = commHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = lineTotalHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = turnOverHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = marginHeader;
                #endregion

                #region assign Body
                AssignExcelData(ref wsNumberGameReport, data.rows, startRowIndex, startColumnIndex, ref nextRow);
                #endregion

                #region assign footer
                var bgFooter = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, true, false);
                var bgPercentFooter = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
                nextColumn = 0;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = "Grand Total";
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.Winlost);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.Position);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.Commission);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.LineTotal);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.TurnOver);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgPercentFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.Margin.Replace("%", null)) / 100;
                #endregion
            }
            catch
            {
            }
            finally
            {

                #region response excel file
                //excelFile.Worksheets.Add(reportName);
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }

        }

        private void Export2ExcelNumberGameWinlossByDate(string fromdate, string todate, NumberGameByDateTreeGridBO data)
        {
            #region local variables
            #region Header
            const string matchBetTypeHeader = "Date - Bet Type";
            const string winLostHeader = "Winloss";
            const string positionHeader = "Position";
            const string commHeader = "Commission";
            const string lineTotalHeader = "Line Total";
            const string virutalTotalHeader = "Virtual Total";
            const string varianceHeader = "Variance";
            const string turnOverHeader = "Turnover";
            const string marginHeader = "Margin";
            #endregion
            var reportName = string.Format("MatchWinLost-NumberGame-From-{0}To-{1}", new string[] { fromdate, todate });
            const int startRowIndex = 2;
            const int startColumnIndex = 0;
            int nextRow = 0, nextColumn = 0;
            ExcelFile excelFile = new ExcelFile();
            ExcelWorksheet wsNumberGameReport = excelFile.Worksheets.Add("NumberGameReport");
            var bgHeader = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Center, ExcelFont.BoldWeight, false, false);
            #endregion

            try
            {
                #region assign Header
                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, startColumnIndex, 0, startColumnIndex + (6)).Merged = true;
                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, startColumnIndex, 0, startColumnIndex + (6)).Style = bgHeader;
                wsNumberGameReport.Cells.GetSubrangeAbsolute(0, startColumnIndex, 0, startColumnIndex + (6)).Value = string.Format("Number Game Win Loss By Date - From {0} To {1}", fromdate, todate);

                wsNumberGameReport.Columns[startColumnIndex].Width = 20 * 256;

                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = matchBetTypeHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = winLostHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = positionHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = commHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = lineTotalHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = turnOverHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex - 1, startColumnIndex + (nextColumn++)].Value = marginHeader;
                #endregion

                #region assign Body
                AssignExcelDataByDate(ref wsNumberGameReport, data.rows, startRowIndex, startColumnIndex, ref nextRow);
                #endregion

                #region assign footer
                var bgFooter = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, true, false);
                var bgPercentFooter = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
                nextColumn = 0;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgHeader;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = "Grand Total";
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.Winlost);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.Position);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.Commission);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.LineTotal);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDecimal(data.totalrow.TurnOver);
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + nextColumn].Style = bgPercentFooter;
                wsNumberGameReport.Cells[startRowIndex + nextRow, startColumnIndex + (nextColumn++)].Value = CastAide.AsDouble(data.totalrow.Margin.Replace("%", null)) / 100;
                #endregion
            }
            catch
            {
            }
            finally
            {

                #region response excel file
                //excelFile.Worksheets.Add(reportName);
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }

        }

        #region assign body excel   function
        private void AssignExcelData(ref ExcelWorksheet wsNumberGameReport, List<NumberGameWinLostByMatchBO> data, int startRow, int startCol, ref int nextRow)
        {
            if (data != null)
            {
                var bgDecFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, true, false);
                var bgTextFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Left, ExcelFont.BoldWeight, false, false);
                var bgNormalText = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight, false, false);
                var bgNormalDecFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);
                var bgPercentFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
                var bgNormalPercentFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, false, true);

                var idLength = 0;
                var spaceTotal = "";
                #region assign data for each row
                foreach (NumberGameWinLostByMatchBO row in data)
                {
                    var nextColumn = 0;
                    idLength = row.Id.Split('_').Length;
                    spaceTotal = idLength == 1 ? "  " : (idLength == 2 ? "    " : "");

                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgTextFormat : bgNormalText;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = spaceTotal + row.Name;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.Winlost);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.Position);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.Commission);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.LineTotal);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.TurnOver);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgPercentFormat : bgNormalPercentFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.Margin.Replace("%", null)) / 100;

                    nextRow++;
                    if (row.children != null)
                        AssignExcelData(ref wsNumberGameReport, row.children, startRow, startCol, ref nextRow);
                }
                #endregion
            }
        }

        private void AssignExcelDataByDate(ref ExcelWorksheet wsNumberGameReport, List<NumberGameWinLostByDateBO> data, int startRow, int startCol, ref int nextRow)
        {
            if (data != null)
            {
                var bgDecFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, true, false);
                var bgTextFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Left, ExcelFont.BoldWeight, false, false);
                var bgNormalText = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight, false, false);
                var bgNormalDecFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);
                var bgPercentFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
                var bgNormalPercentFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, false, true);

                var idLength = 0;
                var spaceTotal = "";
                foreach (NumberGameWinLostByDateBO row in data)
                {
                    var nextColumn = 0;
                    idLength = row.Id.Split('_').Length;
                    spaceTotal = idLength == 1 ? "  " : (idLength == 2 ? "    " : "");



                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgTextFormat : bgNormalText;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = spaceTotal + row.Name;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.Winlost);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.Position);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.Commission);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.LineTotal);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgDecFormat : bgNormalDecFormat;
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value = CastAide.AsDecimal(row.TurnOver);
                    wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn)].Style = row.children != null ? bgPercentFormat : bgNormalPercentFormat;

                    if (row.Margin.ToLower() != "n/a")
                        wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value =
                            CastAide.AsDouble(row.Margin.Replace("%", null)) / 100;
                    else
                        wsNumberGameReport.Cells[nextRow + startRow, startCol + (nextColumn++)].Value =
                             string.Format("{0}", row.Margin);

                    nextRow++;
                    if (row.children != null)
                        AssignExcelDataByDate(ref wsNumberGameReport, row.children, startRow, startCol, ref nextRow);
                }
            }
        }

        #endregion

        #region winloss by Match Detail
        private void Export2ExcelWinLossByMatchDetail(string date, NumberGameDataGridBO data, bool isLive, int bettype, int matchId)
        {
            #region local variables
            #region Header title
            var userNameHeader = "Username";
            var turnOverHeader = "Turnover";
            var totalComHeader = "Comm.";
            var memberGroupHeader = "Member";
            var agentGrounpHeader = "Agent";
            var masterGroupHeader = "Master";
            var superGroupHeader = "Super";
            var totalHeader = "Total";
            var winlossGroupHeader = "Win/Loss";
            var commGroupHeader = "Comm.";
            var totalGroupHeader = "Total";
            #endregion
            var reportName = string.Format("MatchWinLostDetailByMatch-NumberGame-{0}", date);
            const int startRowIndex = 3;
            const int startColIndex = 0;
            const int mergeCol = 2;
            ExcelFile excelFile = new ExcelFile();
            ExcelWorksheet wsNGReport = excelFile.Worksheets.Add("MatchWinLostDetailByMatch");
            var bgHeader = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Center, ExcelFont.BoldWeight, false, false);
            int nextCol = 0, nextRow = 0;

            var bettypeName = CacheRef.GetBettype(bettype);
            #endregion

            try
            {
                #region assign Header
                wsNGReport.Cells.GetSubrangeAbsolute(0, startColIndex, 0, startColIndex + (14)).Merged = true;
                wsNGReport.Cells.GetSubrangeAbsolute(0, startColIndex, 0, startColIndex + (14)).Style = bgHeader;
                wsNGReport.Cells.GetSubrangeAbsolute(0, startColIndex, 0, startColIndex + (14)).Value = string.Format("Number Game Win Loss By Match - {0} - {1} - {2} - Date {3}", matchId, isLive ? "Live" : "No Live", bettypeName, date);

                wsNGReport.Columns[startColIndex].Width = 15 * 256;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (nextCol), startRowIndex - 1, startColIndex + (nextCol)).Merged = true;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (nextCol), startRowIndex - 1, startColIndex + (nextCol)).Style = bgHeader;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (nextCol), startRowIndex - 1, startColIndex + (nextCol)).Value = userNameHeader;

                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (1), startRowIndex - 1, startColIndex + (1)).Merged = true;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (1), startRowIndex - 1, startColIndex + (1)).Style = bgHeader;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (1), startRowIndex - 1, startColIndex + (1)).Value = turnOverHeader;

                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (2), startRowIndex - 1, startColIndex + (2)).Merged = true;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (2), startRowIndex - 1, startColIndex + (2)).Style = bgHeader;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (2), startRowIndex - 1, startColIndex + (2)).Value = commGroupHeader;

                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (3), startRowIndex - 2, startColIndex + (3 + mergeCol)).Merged = true;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (3), startRowIndex - 2, startColIndex + (3 + mergeCol)).Style = bgHeader;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (3), startRowIndex - 2, startColIndex + (3 + mergeCol)).Value = memberGroupHeader;

                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (6), startRowIndex - 2, startColIndex + (6 + (mergeCol))).Merged = true;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (6), startRowIndex - 2, startColIndex + (6 + (mergeCol))).Style = bgHeader;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (6), startRowIndex - 2, startColIndex + (6 + (mergeCol))).Value = agentGrounpHeader;

                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (9), startRowIndex - 2, startColIndex + (9 + (mergeCol))).Merged = true;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (9), startRowIndex - 2, startColIndex + (9 + (mergeCol))).Style = bgHeader;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (9), startRowIndex - 2, startColIndex + (9 + (mergeCol))).Value = masterGroupHeader;

                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (12), startRowIndex - 2, startColIndex + (12 + (mergeCol))).Merged = true;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (12), startRowIndex - 2, startColIndex + (12 + (mergeCol))).Style = bgHeader;
                wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (12), startRowIndex - 2, startColIndex + (12 + (mergeCol))).Value = superGroupHeader;

                nextCol = 3;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = winlossGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = commGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = totalGroupHeader;

                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = winlossGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = commGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = totalGroupHeader;

                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = winlossGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = commGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = totalGroupHeader;

                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = winlossGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = commGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol++)].Style = bgHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Value = totalGroupHeader;
                wsNGReport.Cells[startRowIndex - 1, startColIndex + (nextCol)].Style = bgHeader;

                //wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (15), startRowIndex - 1, startColIndex + (15)).Merged = true;
                //wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (15), startRowIndex - 1, startColIndex + (15)).Style = bgHeader;
                //wsNGReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColIndex + (15), startRowIndex - 1, startColIndex + (15)).Value = totalGroupHeader;
                #endregion

                #region assign Body
                AssignExcelDataWinLossByMatchDetail(ref wsNGReport, data.rows, startRowIndex, startColIndex, ref nextRow, false);
                #endregion

                #region assign Footer
                var footerData = data.footer;
                AssignExcelDataWinLossByMatchDetail(ref wsNGReport, data.footer, startRowIndex, startColIndex, ref nextRow, true);
                #endregion
            }
            catch
            {
            }
            finally
            {
                #region response excel file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
        }
        private void AssignExcelDataWinLossByMatchDetail(ref ExcelWorksheet ws, List<NumberGameWinLostDetailBO> data, int startRow, int startCol, ref int nextRow, bool isFooter)
        {
            if (data != null)
            {
                var bgNormalText = ExcelFormat(isFooter ? Color.FromArgb(159, 183, 213) : Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, isFooter ? ExcelFont.BoldWeight : ExcelFont.NormalWeight, false, false);
                var bgNormalDecFormat = ExcelFormat(isFooter ? Color.FromArgb(159, 183, 213) : Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, isFooter ? ExcelFont.BoldWeight : ExcelFont.NormalWeight, true, false);

                foreach (NumberGameWinLostDetailBO row in data)
                {
                    var nextCol = 0;
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalText;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = row.Username;
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.TurnOver);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.Commission);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.MemberWinlost);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.MemberComm);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.MemberTotal);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.AgentWinlost);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.AgentComm);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.AgentTotal);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.MasterWinlost);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.MasterComm);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.MasterComm);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.SuperWinlost);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.SuperComm);
                    ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;
                    ws.Cells[nextRow + startRow, startCol + (nextCol++)].Value = CastAide.AsDecimal(row.SuperTotal);
                    //ws.Cells[nextRow + startRow, startCol + (nextCol)].Style = bgNormalDecFormat;

                    nextRow++;
                }
            }
        }
        #endregion
        private static CellStyle ExcelFormat(Color backgroundColor, HorizontalAlignmentStyle alignmentStyle, int font, bool isDecFormat, bool isPercent)
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
                excelFormat.NumberFormat = "#,##0.00_);[RED]-#,##0.00";
            if (isPercent)
                excelFormat.NumberFormat = "#,##0.00%_);[RED]-#,##0.00%";
            return excelFormat;
        }

        #endregion

        private List<int> GetDistinctMatch(DataTable tb)
        {
            return (from row in tb.AsEnumerable()
                    select row.Field<int>("matchid")).Distinct().ToList();
        }

        private List<DateTime> GetDistinctDate(DataTable tb)
        {
            return (from row in tb.AsEnumerable()
                    select row.Field<DateTime>("date")).Distinct().ToList();
        }

        public List<Sunplus.Accounting.Entities.Report.NumberGame.Currency> GetCurrencyList()
        {
            var listCurrency = new List<Currency>();
            var dataSet = SelectCustomer.ExchangeSelRpt(AppSession.SYSTEM);
            if (dataSet != null && dataSet.ResultSet.Tables.Count > 0)
            {
                var dataTable = dataSet.ResultSet.Tables[0];
                foreach (DataRow row in dataTable.Rows)
                {
                    listCurrency.Add(new Currency { Name = row["Currency"].ToString(), Value = row["ExchangeID"].ToString() });
                }
            }
            else
            {
                var reportElements = Sunplus.Accounting.Core.Configurations.ReportSection.Instance.NumberGame["Currency"].Items;
                for (int i = 0; i < reportElements.Count; ++i)
                {
                    listCurrency.Add(new Currency { Name = reportElements[i].Name, Value = reportElements[i].Value });
                }
            }
            return listCurrency;
        }

        public List<Item> GetBettypeList()
        {
            var result = new List<Item>();
            try
            {

                var rs = NumberGameWinLoss.SellAllBettypes();

                if (rs != null && rs.ResultSet.Tables != null && rs.ResultSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                    {
                        var item = new Item()
                        {
                            Id = CastAide.AsString(dr["typeid"]),
                            Name = CastAide.AsString(dr["typenamee"])
                        };
                        result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public List<Item> GetLicensees()
        {
            var result = new List<Item>();
            try
            {
                DbResult rs = NumberGameWinLoss.SelLicensees();
                if (rs != null && rs.ResultSet.Tables != null && rs.ResultSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                    {
                        var item = new Item()
                                       {
                                           Id = CastAide.AsString(dr["licenseeid"]),
                                           Name = CastAide.AsString(dr["licenseename"])
                                       };
                        result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        #region winloss by bettype
        public BettypesWinLossDTO NumberGameWinLossByBettype(string companies, string currencies, string date)
        {
            var result = new BettypesWinLossDTO();
            try
            {

                DbResult rs = null;
                if (CastAide.AsDateTime(date) > ReportUtils.BalanceUp.MoveBetsDay)
                    rs = NumberGameWinLoss.WinlossByBetType(CastAide.AsDateTime(date), currencies, companies);
                else if (CastAide.AsDateTime(date) >= ReportUtils.BalanceUp.BalanceUpDay)
                    rs = NumberGameWinLoss.WinlossByBetType14(CastAide.AsDateTime(date), companies, currencies);
                else
                    rs = NumberGameWinLoss.WinlossByBetType_Old(CastAide.AsDateTime(date), companies, currencies);

                if (rs != null && rs.ResultSet.Tables != null && rs.ResultSet.Tables.Count > 0 && rs.ResultSet.Tables[0].Rows.Count > 0)
                {
                    result.Live = new BettypeWinLossDetail();
                    result.Live.BettypesWinLossRows = new List<BettypesWinLossRow>();
                    result.NoneLive = new BettypeWinLossDetail();
                    result.NoneLive.BettypesWinLossRows = new List<BettypesWinLossRow>();

                    double totalWL = 0,
                           totalPosition = 0,
                           totalComm = 0,
                           totalTurnOver = 0,
                           totalVirtual = 0,
                           totalReal = 0,
                           totalVariance = 0;
                    double totalLiveWL = 0,
                           totalLivePosition = 0,
                           totalLiveComm = 0,
                           totalLiveTurnOver = 0,
                           totalLiveVirtual = 0,
                           totalLiveReal = 0,
                           totalLiveVariance = 0;

                    foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                    {
                        var realLineReal = CastAide.AsDouble(dr["WinLost"]) + CastAide.AsDouble(dr["Position"]) +
                                           CastAide.AsDouble(dr["Commission"]);
                        var margin = (Math.Round(CastAide.AsDouble(dr["TurnOver"]), 2) == 0) ? 0 : ((realLineReal / CastAide.AsDouble(dr["TurnOver"])) * 100);
                        var bettypeName = RemotingRef.RefData.GetBetTypeName(CastAide.AsString(dr["bettype"]), "E");

                        var item = new BettypesWinLossRow()
                                       {
                                           BettypeId = CastAide.AsString(dr["bettype"]),
                                           Bettype = bettypeName,
                                           WinLoss = NumberFormat.DecFormat(dr["WinLost"], 2),
                                           Position = NumberFormat.DecFormat(dr["Position"], 2),
                                           Comm = NumberFormat.DecFormat(dr["Commission"], 2),
                                           Turnover = NumberFormat.DecFormat(dr["TurnOver"], 2),
                                           LineTotalVirtual = NumberFormat.DecFormat(dr["VirtualLineTotal"], 2),
                                           LineTotalReal = NumberFormat.DecFormat(realLineReal, 2),
                                           Variance = NumberFormat.DecFormat(realLineReal - CastAide.AsDouble(dr["VirtualLineTotal"]), 2),
                                           Margin = NumberFormat.DecFormat(margin, 2)

                                       };
                        if (CastAide.AsBoolean(dr["liveindicator"]))
                        {
                            result.Live.BettypesWinLossRows.Add(item);

                            totalLiveWL += CastAide.AsDouble(item.WinLoss);
                            totalLivePosition += CastAide.AsDouble(item.Position);
                            totalLiveComm += CastAide.AsDouble(item.Comm);
                            totalLiveTurnOver += CastAide.AsDouble(item.Turnover);
                            totalLiveReal += CastAide.AsDouble(item.LineTotalReal);
                            totalLiveVirtual += CastAide.AsDouble(item.LineTotalVirtual);
                            totalLiveVariance += CastAide.AsDouble(item.Variance);
                        }
                        else
                        {
                            result.NoneLive.BettypesWinLossRows.Add(item);

                            totalWL += CastAide.AsDouble(item.WinLoss);
                            totalPosition += CastAide.AsDouble(item.Position);
                            totalComm += CastAide.AsDouble(item.Comm);
                            totalTurnOver += CastAide.AsDouble(item.Turnover);
                            totalReal += CastAide.AsDouble(item.LineTotalReal);
                            totalVirtual += CastAide.AsDouble(item.LineTotalVirtual);
                            totalVariance += CastAide.AsDouble(item.Variance);
                        }
                    }

                    result.Live.Total = new BettypesWinLossRow()
                                            {
                                                Comm = NumberFormat.DecFormat(totalLiveComm, 2),
                                                LineTotalReal = NumberFormat.DecFormat(totalLiveReal, 2),
                                                LineTotalVirtual = NumberFormat.DecFormat(totalLiveVirtual, 2),
                                                Margin = (Math.Round(totalLiveTurnOver, 2) != 0 ? NumberFormat.DecFormat((totalLiveReal / totalLiveTurnOver) * 100, 2) : "0.00"),
                                                Position = NumberFormat.DecFormat(totalLivePosition, 2),
                                                Turnover = NumberFormat.DecFormat(totalLiveTurnOver, 2),
                                                Variance = NumberFormat.DecFormat(totalLiveVariance, 2),
                                                WinLoss = NumberFormat.DecFormat(totalLiveWL, 2)
                                            };

                    result.NoneLive.Total = new BettypesWinLossRow()
                                                {
                                                    Comm = NumberFormat.DecFormat(totalComm, 2),
                                                    LineTotalReal = NumberFormat.DecFormat(totalReal, 2),
                                                    LineTotalVirtual = NumberFormat.DecFormat(totalVirtual, 2),
                                                    Margin =
                                                        (Math.Round(totalTurnOver, 2) != 0 ? NumberFormat.DecFormat((totalReal / totalTurnOver) * 100, 2) : "0.00"),
                                                    Position = NumberFormat.DecFormat(totalPosition, 2),
                                                    Turnover = NumberFormat.DecFormat(totalTurnOver, 2),
                                                    Variance = NumberFormat.DecFormat(totalVariance, 2),
                                                    WinLoss = NumberFormat.DecFormat(totalWL, 2)
                                                };
                }
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeSessionKey, result);
            return result;
        }

        public BettypeWinLossDetail NumberGameWinLossDetailByBettype(string date, int isLive, int bettype, string companies, string currencies)
        {
            var result = new BettypeWinLossDetail();
            try
            {
                companies = companies.Replace(";", ",");
                currencies = currencies.Replace(";", ",");
                DbResult rs = null;
                var wlDate = CastAide.AsDateTime(date);
                if (wlDate > ReportUtils.BalanceUp.MoveBetsDay)
                    rs = NumberGameWinLoss.WinlossByBetTypeDetail(wlDate, isLive, bettype, companies, currencies);
                else if (wlDate >= ReportUtils.BalanceUp.BalanceUpDay)
                    rs = NumberGameWinLoss.WinlossByBetTypeDetail14(wlDate, isLive, bettype, companies, currencies);
                else
                    rs = NumberGameWinLoss.WinlossByBetTypeDetail_Old(wlDate, isLive, bettype, companies, currencies);

                if (rs != null && rs.ResultSet.Tables != null && rs.ResultSet.Tables.Count > 0)
                {
                    double winlossTotal = 0,
                           positionTotal = 0,
                           commTotal = 0,
                           lRealTotal = 0,
                           lVirtualTotal = 0,
                           turnoverTotal = 0,
                           varianceTotal = 0;
                    result.BettypesWinLossRows = new List<BettypesWinLossRow>();

                    foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                    {
                        var realLineReal = CastAide.AsDouble(dr["WinLost"]) + CastAide.AsDouble(dr["Position"]) +
                                           CastAide.AsDouble(dr["Commission"]);
                        var margin = (CastAide.AsDouble(dr["TurnOver"]) == 0) ? 0 : ((realLineReal / CastAide.AsDouble(dr["TurnOver"])) * 100);

                        var item = new BettypesWinLossRow()
                                       {
                                           BettypeId = CastAide.AsString(dr["matchcode"]),
                                           Bettype = CastAide.AsString(dr["matchcode"]),
                                           WinLoss = NumberFormat.DecFormat(dr["WinLost"], 2),
                                           Position = NumberFormat.DecFormat(dr["Position"], 2),
                                           Comm = NumberFormat.DecFormat(dr["Commission"], 2),
                                           Turnover = NumberFormat.DecFormat(dr["TurnOver"], 2),
                                           LineTotalVirtual = NumberFormat.DecFormat(dr["VirtualLineTotal"], 2),
                                           LineTotalReal = NumberFormat.DecFormat(realLineReal, 2),
                                           Variance = NumberFormat.DecFormat(realLineReal - CastAide.AsDouble(dr["VirtualLineTotal"]), 2),
                                           Margin = NumberFormat.DecFormat(margin, 2)
                                       };

                        result.BettypesWinLossRows.Add(item);
                        #region assign total
                        winlossTotal += CastAide.AsDouble(dr["WinLost"]);
                        positionTotal += CastAide.AsDouble(dr["Position"]);
                        commTotal += CastAide.AsDouble(dr["Commission"]);
                        lRealTotal += realLineReal;
                        lVirtualTotal += CastAide.AsDouble(dr["VirtualLineTotal"]);
                        turnoverTotal += CastAide.AsDouble(dr["TurnOver"]);
                        varianceTotal += CastAide.AsDouble(CastAide.AsDouble(item.Variance));
                        #endregion
                    }
                    result.Total = new BettypesWinLossRow()
                                       {
                                           Bettype = "Total",
                                           WinLoss = NumberFormat.DecFormat(winlossTotal, 2),
                                           Position = NumberFormat.DecFormat(positionTotal, 2),
                                           Comm = NumberFormat.DecFormat(commTotal, 2),
                                           Turnover = NumberFormat.DecFormat(turnoverTotal, 2),
                                           LineTotalVirtual = NumberFormat.DecFormat(lVirtualTotal, 2),
                                           LineTotalReal = NumberFormat.DecFormat(lRealTotal, 2),
                                           Variance = NumberFormat.DecFormat(varianceTotal, 2),
                                           Margin = NumberFormat.DecFormat((lRealTotal / turnoverTotal) * 100, 2)
                                       };
                }

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _NumberGameWinLossDetailByBettypeSessionKey, result);
            return result;
        }

        public BettypeWinLossDetailDTO NumberGameWinLossDetailByBettypeNew(string date, int isLive, int bettype, string companies, string currencies)
        {
            var result = new BettypeWinLossDetailDTO();
            try
            {
                companies = companies.Replace(";", ",");
                currencies = currencies.Replace(";", ",");
                DbResult rs = null;
                var wlDate = CastAide.AsDateTime(date);
                if (wlDate > ReportUtils.BalanceUp.MoveBetsDay)
                    rs = NumberGameWinLoss.WinlossByBetTypeDetail(wlDate, isLive, bettype, companies, currencies);
                else if (wlDate >= ReportUtils.BalanceUp.BalanceUpDay)
                    rs = NumberGameWinLoss.WinlossByBetTypeDetail14(wlDate, isLive, bettype, companies, currencies);
                else
                    rs = NumberGameWinLoss.WinlossByBetTypeDetail_Old(wlDate, isLive, bettype, companies, currencies);

                if (rs != null && rs.ResultSet.Tables != null && rs.ResultSet.Tables.Count > 0)
                {
                    result.Items = new List<BettypeWinLossDetail>();
                    double bWinlossTotal = 0,
                           bPositionTotal = 0,
                           bCommTotal = 0,
                           bLRealTotal = 0,
                           bLVirtualTotal = 0,
                           bTurnoverTotal = 0,
                           bVarianceTotal = 0;
                    double winlossTotal = 0,
                           positionTotal = 0,
                           commTotal = 0,
                           lRealTotal = 0,
                           lVirtualTotal = 0,
                           turnoverTotal = 0,
                           varianceTotal = 0;
                    double leagueWinlossTotal = 0,
                           leaguePositionTotal = 0,
                           leagueCommTotal = 0,
                           leagueLRealTotal = 0,
                           leagueLVirtualTotal = 0,
                           leagueTurnoverTotal = 0,
                           leagueVarianceTotal = 0;
                    int leagueId = 0, pLeagueId = 0;

                    var items = new BettypeWinLossDetail();
                    items.BettypesWinLossRows = new List<BettypesWinLossRow>();
                    items.Total = new BettypesWinLossRow();

                    foreach (DataRow dr in rs.ResultSet.Tables[0].Rows)
                    {
                        var realLineReal = CastAide.AsDouble(dr["WinLost"]) + CastAide.AsDouble(dr["Position"]) +
                                           CastAide.AsDouble(dr["Commission"]);
                        var margin = (Math.Round(CastAide.AsDouble(dr["TurnOver"]), 2) == 0) ? 0 : ((realLineReal / CastAide.AsDouble(dr["TurnOver"])) * 100);
                        leagueId = CastAide.AsInt32(dr["leagueid"]);

                        #region each row
                        bWinlossTotal = CastAide.AsDouble(dr["WinLost"]);
                        bPositionTotal = CastAide.AsDouble(dr["Position"]);
                        bCommTotal = CastAide.AsDouble(dr["Commission"]);
                        bLRealTotal = realLineReal;
                        bLVirtualTotal = CastAide.AsDouble(dr["VirtualLineTotal"]);
                        bTurnoverTotal = CastAide.AsDouble(dr["TurnOver"]);
                        bVarianceTotal = realLineReal - CastAide.AsDouble(dr["VirtualLineTotal"]);
                        #endregion
                        if (leagueId != pLeagueId && pLeagueId > 0)
                        {
                            items.Name = CacheRef.GetLeagueName(pLeagueId);
                            items.Total = new BettypesWinLossRow()
                                              {
                                                  Bettype = string.Empty,
                                                  Comm = NumberFormat.DecFormat(leagueCommTotal, 2),
                                                  LineTotalReal = NumberFormat.DecFormat(leagueLRealTotal, 2),
                                                  LineTotalVirtual = NumberFormat.DecFormat(leagueLVirtualTotal, 2),
                                                  Margin = NumberFormat.DecFormat(Math.Round(leagueTurnoverTotal, 2) != 0 ? ((leagueLRealTotal / leagueTurnoverTotal) * 100) : 0, 2),
                                                  Position = NumberFormat.DecFormat(leaguePositionTotal, 2),
                                                  Turnover = NumberFormat.DecFormat(leagueTurnoverTotal, 2),
                                                  Variance = NumberFormat.DecFormat(leagueVarianceTotal, 2),
                                                  WinLoss = NumberFormat.DecFormat(leagueWinlossTotal, 2)

                                              };
                            result.Items.Add(items);
                            items = new BettypeWinLossDetail();
                            items.BettypesWinLossRows = new List<BettypesWinLossRow>();
                            items.Total = new BettypesWinLossRow();
                            leagueWinlossTotal = 0;
                            leaguePositionTotal = 0;
                            leagueCommTotal = 0;
                            leagueLRealTotal = 0;
                            leagueLVirtualTotal = 0;
                            leagueTurnoverTotal = 0;
                            leagueVarianceTotal = 0;
                        }

                        #region assign league total
                        leagueWinlossTotal += bWinlossTotal;
                        leaguePositionTotal += bPositionTotal;
                        leagueCommTotal += bCommTotal;
                        leagueLRealTotal += realLineReal;
                        leagueLVirtualTotal += bLVirtualTotal;
                        leagueTurnoverTotal += bTurnoverTotal;
                        leagueVarianceTotal += bVarianceTotal;
                        #endregion

                        #region assign bettype total
                        winlossTotal += bWinlossTotal;
                        positionTotal += bPositionTotal;
                        commTotal += bCommTotal;
                        lRealTotal += realLineReal;
                        lVirtualTotal += bLVirtualTotal;
                        turnoverTotal += bTurnoverTotal;
                        varianceTotal += bVarianceTotal;
                        #endregion

                        #region assign each row
                        var item = new BettypesWinLossRow()
                        {
                            BettypeId = CastAide.AsString(dr["matchcode"]),
                            Bettype = CastAide.AsString(dr["matchcode"]),
                            WinLoss = NumberFormat.DecFormat(bWinlossTotal, 2),
                            Position = NumberFormat.DecFormat(bPositionTotal, 2),
                            Comm = NumberFormat.DecFormat(bCommTotal, 2),
                            Turnover = NumberFormat.DecFormat(bTurnoverTotal, 2),
                            LineTotalVirtual = NumberFormat.DecFormat(bLVirtualTotal, 2),
                            LineTotalReal = NumberFormat.DecFormat(bLRealTotal, 2),
                            Variance = NumberFormat.DecFormat(bVarianceTotal, 2),
                            Margin = NumberFormat.DecFormat(margin, 2)
                        };
                        items.BettypesWinLossRows.Add(item);
                        #endregion

                        pLeagueId = leagueId;
                    }
                    #region assign to last league
                    items.Name = CacheRef.GetLeagueName(pLeagueId);
                    items.Total = new BettypesWinLossRow()
                    {
                        Bettype = string.Empty,
                        Comm = NumberFormat.DecFormat(leagueCommTotal, 2),
                        LineTotalReal = NumberFormat.DecFormat(leagueLRealTotal, 2),
                        LineTotalVirtual = NumberFormat.DecFormat(leagueLVirtualTotal, 2),
                        Margin = NumberFormat.DecFormat(Math.Round(leagueTurnoverTotal, 2) != 0 ? (leagueLRealTotal / leagueTurnoverTotal) * 100 : 0, 2),
                        Position = NumberFormat.DecFormat(leaguePositionTotal, 2),
                        Turnover = NumberFormat.DecFormat(leagueTurnoverTotal, 2),
                        Variance = NumberFormat.DecFormat(leagueVarianceTotal, 2),
                        WinLoss = NumberFormat.DecFormat(leagueWinlossTotal, 2)

                    };
                    result.Items.Add(items);
                    #endregion
                    result.Total = new BettypesWinLossRow()
                    {
                        Bettype = string.Format("{0}{1}", isLive == 1 ? "Live! " : "", "Total"),
                        WinLoss = NumberFormat.DecFormat(winlossTotal, 2),
                        Position = NumberFormat.DecFormat(positionTotal, 2),
                        Comm = NumberFormat.DecFormat(commTotal, 2),
                        Turnover = NumberFormat.DecFormat(turnoverTotal, 2),
                        LineTotalVirtual = NumberFormat.DecFormat(lVirtualTotal, 2),
                        LineTotalReal = NumberFormat.DecFormat(lRealTotal, 2),
                        Variance = NumberFormat.DecFormat(varianceTotal, 2),
                        Margin = NumberFormat.DecFormat((Math.Round(turnoverTotal, 2) != 0 ? (lRealTotal / turnoverTotal) : 0) * 100, 2)
                    };
                }

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _NumberGameWinLossDetailByBettypeSessionKey, result);
            return result;
        }

        #region export 2 excel
        public void ExportToExcelNumberGameWinLossByBettype(string companies, string currencies, string date)
        {
            var startRow = 2;
            var startCol = 0;
            var reportName = "NumberGameWinLossByBettype";
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header
            var headerMergeCols = 8;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Value = "Number Game Win Loss by Bet Type";
            #endregion

            #region content

            var getData =
                SessionExtension.GetSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeSessionKey) != null
                    ? (BettypesWinLossDTO)SessionExtension.GetSession(HttpContext.Current.Session, _NumberGameWinLossByBettypeSessionKey)
                    : NumberGameWinLossByBettype(companies, currencies, date);
            if (getData != null)
            {
                #region none live

                ExportToExcelNumberGameWinLossByBettypeAssignHeader(ref wsReport, startCol, startRow, ref nextRow,
                                                                    "NUMBER GAME \"NON - LIVE\" PRODUCT WIN/LOSS REPORT ");
                if (getData.NoneLive != null && getData.NoneLive.BettypesWinLossRows != null && getData.NoneLive.BettypesWinLossRows.Count > 0)
                {
                    ExportToExcelNumberGameWinLossByBettypeAssignContent(ref wsReport, startCol, startRow, ref nextRow,
                                                                         getData.NoneLive);

                    ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                                        getData.NoneLive.Total, false);
                }
                else
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).Value =
                    "No information is available.";
                }
                nextRow++;
                #endregion

                #region live
                ExportToExcelNumberGameWinLossByBettypeAssignHeader(ref wsReport, startCol, startRow, ref nextRow,
                                                                    "NUMBER GAME \"LIVE\" PRODUCT WIN/LOSS REPORT ");
                if (getData.Live != null && getData.Live.BettypesWinLossRows != null && getData.Live.BettypesWinLossRows.Count > 0)
                {
                    ExportToExcelNumberGameWinLossByBettypeAssignContent(ref wsReport, startCol, startRow, ref nextRow,
                                                                         getData.Live);
                    ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                                        getData.Live.Total, false);
                }
                else
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).Value =
                    "No information is available.";
                }
                #endregion
            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", string.Format("{0}_{1}", reportName, date)));
            excelFile.SaveXls(response.OutputStream);

            #endregion
        }

        private void ExportToExcelNumberGameWinLossByBettypeAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, string title)
        {
            var headerFormart = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).Style
                = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).Value
                = title;

            nextRow += 1;

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + 1, startCol).Merged =
                true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + 1, startCol).Style =
                headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + 1, startCol).Value =
                "Bettype";

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow + 1,
                                               startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Value = "WinLoss";

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow + 1,
                                               startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Value = "Position";

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow + 1,
                                               startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Value = "Comm";

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow,
                                               startCol + (nextCol + 1)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow,
                                               startCol + (nextCol + 1)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow,
                                               startCol + (nextCol + 1)).Value = "Line Total";

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 2), startRow + nextRow + 1,
                                               startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Value = "Turnover";

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow + 1,
                                               startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Value = "Margin";

            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow + 1,
                                               startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Style = headerFormart;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow + 1,
                                               startCol + (nextCol)).Value = "Variance";

            wsReport.Cells[startRow + nextRow + 1, startCol + 4].Style = headerFormart;
            wsReport.Cells[startRow + nextRow + 1, startCol + 4].Value = "Real";

            wsReport.Cells[startRow + nextRow + 1, startCol + 5].Style = headerFormart;
            wsReport.Cells[startRow + nextRow + 1, startCol + 5].Value = "Virtual";
            nextRow += 2;

        }

        private void ExportToExcelNumberGameWinLossByBettypeAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, BettypeWinLossDetail rows)
        {
            var color = Color.FromArgb(255, 255, 255);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
            var bgOtherFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, true);
            var bgPercentFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", color, false, true);
            var bgPercentNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", color, false, false);

            foreach (BettypesWinLossRow row in rows.BettypesWinLossRows)
            {
                var nextCol = 0;
                wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells[startRow + nextRow, startCol].Value = row.Bettype;

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.WinLoss) != 0 ? bgOtherFormat : bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Position);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.LineTotalReal) != 0 ? bgOtherFormat : bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotalReal);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.LineTotalVirtual) != 0 ? bgOtherFormat : bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotalVirtual);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.Margin) != 0 ? bgPercentFormat : bgPercentNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Margin) / 100;

                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.Variance) != 0 ? bgOtherFormat : bgFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Variance);
                nextRow++;
            }
        }

        private void ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, BettypesWinLossRow row, bool isLeagueTotal)
        {
            var color = isLeagueTotal ? Color.FromArgb(221, 246, 207) : Color.FromArgb(211, 225, 200);
            var bgFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, false);
            var bgOtherFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, true, true);
            var bgPercentFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", color, true, true);
            var bgPercentNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00%", color, false, false);

            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells[startRow + nextRow, startCol].Value = row.Bettype;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.WinLoss) != 0 ? bgOtherFormat : bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.WinLoss);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Position);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Comm);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.LineTotalReal) != 0 ? bgOtherFormat : bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotalReal);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.LineTotalVirtual) != 0 ? bgOtherFormat : bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.LineTotalVirtual);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Turnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.Margin) != 0 ? bgPercentFormat : bgPercentNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Margin) / 100;

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = CastAide.AsDouble(row.Variance) != 0 ? bgOtherFormat : bgFormat;
            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(row.Variance);
            nextRow++;
        }

        public void ExportToExcelNumberGameWinLossDetailByBettype(string date, int isLive, int bettype, string bettypeName, string companies, string currencies)
        {
            var startRow = 2;
            var startCol = 0;
            var reportName = bettypeName.Replace(" ", "").Replace("/", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header
            var headerMergeCols = 8;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Value = "Number Game Event Win/Loss Report";
            #endregion

            #region content

            var getData =
                SessionExtension.GetSession(HttpContext.Current.Session, _NumberGameWinLossDetailByBettypeSessionKey) != null
                    ? (BettypeWinLossDetailDTO)SessionExtension.GetSession(HttpContext.Current.Session, _NumberGameWinLossDetailByBettypeSessionKey)
                    : NumberGameWinLossDetailByBettypeNew(date, isLive, bettype, companies, currencies);
            if (getData != null)
            {
                ExportToExcelNumberGameWinLossByBettypeAssignHeader(ref wsReport, startCol, startRow, ref nextRow, bettypeName);
                foreach (var items in getData.Items)
                {
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).
                        Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).
                        Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 8).
                        Value = items.Name;
                    nextRow++;
                    ExportToExcelNumberGameWinLossByBettypeAssignContent(ref wsReport, startCol, startRow, ref nextRow,
                                                                     items);
                    ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                                    items.Total, true);
                }
                nextRow++;

                ExportToExcelNumberGameWinLossByBettypeAssignFooter(ref wsReport, startCol, startRow, ref nextRow,
                                                                     getData.Total, false);

            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion
        }
        #endregion

        #endregion

    }
}
