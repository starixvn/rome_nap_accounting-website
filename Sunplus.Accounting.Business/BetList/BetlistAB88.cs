using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for BetlistAB88
/// </summary>
namespace Sunplus.Accounting.Business.BetList
{
    public class BetlistAB88 : BetlistBase
    {
        #region RenderHTML

        public override string RenderHTML(Hashtable input)
        {
            var html = new StringBuilder();
            try
            {
                string tplHeader = WebUtils.LoadTemplate("BetList/BetlistM88_Header.htm");
                string tplBody = WebUtils.LoadTemplate("BetList/BetlistM88_Body.htm");
                string tplFooter = WebUtils.LoadTemplate("BetList/BetlistM88_Footer.htm");
                string tplEvent = WebUtils.LoadTemplate("BetList/BetlistEvent.htm");
                string liveStyle = string.Empty;

                var blEvent = new BetlistGenerateEvent();

                DateTime fdate = CastAide.AsDateTime(input["fdate"], DateTime.Now);
                DateTime tdate = CastAide.AsDateTime(input["tdate"], DateTime.Now);
                DateTime MoveBetsDay = Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.MoveBetsDay;
                int custid = CastAide.AsInt32(input["custid"]);
                string username = CastAide.AsString(input["username"]);
                bool showsb = CastAide.AsInt32(input["showsb"], 1) == 1 ? true : false;
                bool showhr = CastAide.AsInt32(input["showhr"], 1) == 1 ? true : false;
                string baseCurrency = CastAide.AsString(input["basecurrency"]).ToLower();

                //assign header
                TemplateHandler.Assign(ref tplHeader, "fdate", fdate.ToShortDateString());
                TemplateHandler.Assign(ref tplHeader, "tdate", tdate.ToShortDateString());
                TemplateHandler.Assign(ref tplHeader, "username", username);
                if (baseCurrency == "usd")
                    TemplateHandler.Assign(ref tplHeader, "base", " - USD Base Currency");
                else if (baseCurrency == "rm")
                    TemplateHandler.Assign(ref tplHeader, "base", " - RM Base Currency");
                else
                    TemplateHandler.Assign(ref tplHeader, "base", "");

                //append header
                //apply cellspacing for IE & Firefox
                string cellspacing = "1";
                string cssClass = "fxheader";
                if (HttpContext.Current.Request.Browser.Browser == "IE")
                {
                    cellspacing = "0";
                    cssClass = "fxheader";
                }
                else
                {
                    cellspacing = "1";
                    cssClass = "fxheader2";
                }

                TemplateHandler.Assign(ref tplHeader, "cellspacing", cellspacing);
                TemplateHandler.Assign(ref tplHeader, "class", cssClass);
                html.Append(tplHeader);

                TimeSpan span = tdate - fdate;
                if (span.Days > 31)
                {
                    html.Append(
                        "<tr class='c'><td colspan='8'>Bet list can only view in one month data, please choose the date range again!</td></tr></table>");
                    return html.ToString();
                }

                var dt = new DataTable();
                var dt2 = new DataTable();

                var dt_Mem = new DataTable();
                var dt_Mem_2 = new DataTable();
                DbResult rs = null;
                var ds = new DataSet();
                if (FormatHandler.ResetStartDate(tdate) >= ReportUtils.BalanceUp.BalanceUpDay)
                {
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Betlist.AB88_BetList(custid, fdate, tdate, MoveBetsDay, showsb, showhr);
                    if (rs.ResultSet.Tables.Count > 0)
                        dt_Mem = rs.ResultSet.Tables[0];

                    if (rs.ResultSet.Tables.Count > 1)
                        dt_Mem_2 = rs.ResultSet.Tables[1];
                }

                if (FormatHandler.ResetStartDate(fdate) < ReportUtils.BalanceUp.BalanceUpDay)
                {
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Betlist.AB88_BetList_History(custid, fdate, tdate, showsb, showhr);
                    if (rs.ResultSet.Tables.Count > 0)
                        dt = rs.ResultSet.Tables[0];
                    if (rs.ResultSet.Tables.Count > 1)
                        dt2 = rs.ResultSet.Tables[1];
                }

                if (dt.Rows.Count == 0)
                    dt = dt_Mem;
                else
                {
                    foreach (DataRow dr in dt_Mem.Rows)
                        dt.ImportRow(dr);
                }

                if (dt2.Rows.Count == 0)
                    dt2 = dt_Mem_2;
                else
                {
                    foreach (DataRow dr in dt_Mem_2.Rows)
                        dt2.ImportRow(dr);
                }

                ds.Tables.Add(dt.Copy());
                ds.Tables.Add(dt2.Copy());

                DataTable dataTable = null;
                DataTable dataTable2 = null;
                DataRow[] foundRows = null;

                if (ds != null && ds.Tables.Count > 0)
                    dataTable = ds.Tables[0];
                else
                    dataTable = new DataTable();

                if (ds != null && ds.Tables.Count > 1)
                    dataTable2 = ds.Tables[1];

                DataRow[] rows = dataTable.Select();
                if (rows.Length > 0)
                {
                    int index = 0;

                    decimal winlost = 0,
                            awinlost = 0,
                            comm = 0,
                            acomm = 0,
                            totalWL = 0,
                            adiscount = 0,
                            apos = 0,
                            totalComm = 0,
                            apos_Win = 0,
                            apos_Place = 0,
                            stake = 0;
                    int leagueid = 0, matchid = 0, betType = 0;
                    string status = string.Empty;
                    string winlost_col = string.Empty;
                    string awinlost_col = string.Empty;
                    string stake_col = string.Empty;
                    string comm_col = string.Empty;
                    string acomm_col = string.Empty;

                    if (baseCurrency == "usd")
                    {
                        winlost_col = "winlostusd";
                        awinlost_col = "awinlostusd";
                        stake_col = "stakeusd";
                        comm_col = "playercommusd";
                        acomm_col = "acommusd";
                    }
                    else if (baseCurrency == "rm")
                    {
                        winlost_col = "winlostrm";
                        awinlost_col = "awinlostrm";
                        stake_col = "stakerm";
                        comm_col = "playercommrm";
                        acomm_col = "acommrm";
                    }
                    else
                    {
                        winlost_col = "winlost";
                        awinlost_col = "awinlost";
                        stake_col = "stake";
                        comm_col = "playercomm";
                        acomm_col = "acomm";
                    }

                    foreach (DataRow dr in rows)
                    {
                        string strRow = tplBody;
                        string strEvent = tplEvent;
                        liveStyle = "";

                        index++;

                        betType = CastAide.AsInt32(dr["BetType"], 0);

                        winlost = CastAide.AsDecimal(dr[winlost_col]);
                        awinlost = CastAide.AsDecimal(dr[awinlost_col]);
                        stake = CastAide.AsDecimal(dr[stake_col]);
                        comm = CastAide.AsDecimal(dr[comm_col]);
                        acomm = CastAide.AsDecimal(dr[acomm_col]);

                        totalWL += CastAide.AsDecimal(winlost, 2);
                        totalComm += CastAide.AsDecimal(comm, 2);

                        matchid = CastAide.AsInt32(dr["matchid"], 0);
                        leagueid = CastAide.AsInt32(dr["leagueid"], 0);

                        apos = Convert.ToDecimal(dr["apositiontaking"]) * 100;
                        adiscount = Convert.ToDecimal(dr["adiscount"]) * 100;

                        status = CastAide.AsString(dr["status"]);

                        TemplateHandler.Assign(ref strRow, "No", index.ToString());
                        TemplateHandler.Assign(ref strRow, "TransTime", DateFormat.FullDateFormat(dr["transdate"]));
                        TemplateHandler.Assign(ref strRow, "TransID", CastAide.AsString(dr["transid"]));

                        //generate event
                        BetlistRow blRow = InputBetlistRow(dr);
                        strEvent = blEvent.GenerateEvent(strEvent, blRow);
                        TemplateHandler.Assign(ref strRow, "event", strEvent);

                        //Stake
                        TemplateHandler.Assign(ref strRow, "Stake", NumberFormat.DecFormat(stake, 2));

                        //winlost
                        TemplateHandler.Assign(ref strRow, "WinLoss", NumberFormat.DecFormat(winlost, 2));

                        //player comm
                        TemplateHandler.Assign(ref strRow, "PlayerComm", NumberFormat.DecFormat(comm, 2));

                        //assign odds

                        #region assign Odds

                        string oddsTypeName = "";
                        switch (betType)
                        {
                            case 31:
                            case 32:
                                if (status.ToLower() == "won")
                                    TemplateHandler.Assign(ref strRow, "Odds", NumberFormat.DecFormat(dr["odds"], 2));
                                else if (status.ToLower() == "refund")
                                    TemplateHandler.Assign(ref strRow, "Odds", "0.00");
                                else
                                {
                                    TemplateHandler.Assign(ref strRow, "Odds", "-");
                                }
                                break;

                            case 33:
                                string Odds_33_win = "<span class='blue' style='font-weight:normal;'>{1}</span>";
                                string Odds_33_place = "<span class='' style='font-weight:normal;'>{2}</span>";
                                //get data of bettransm for bet type 33,43
                                if (dataTable2 != null && dataTable2.Rows.Count > 0)
                                {
                                    foundRows = dataTable2.Select("refno=" + dr["transid"]);
                                }

                                if (foundRows != null && foundRows.Length > 1)
                                {
                                    for (int i = 0; i < 2; i++)
                                    {
                                        string hrStatus = CastAide.AsString(foundRows[i]["status"]);
                                        int hrBettype = CastAide.AsInt32(foundRows[i]["bettype"]);

                                        if (hrBettype == 31) //win
                                        {
                                            if (hrStatus.ToLower() == "won")
                                            {
                                                TemplateHandler.Assign(ref Odds_33_win, "1",
                                                                       "Win: <b>" +
                                                                       NumberFormat.DecFormat(foundRows[i]["odds"], 2) +
                                                                       "</b>");
                                            }
                                            else
                                            {
                                                TemplateHandler.Assign(ref Odds_33_win, "1", "Win: <b>0.00</b>");
                                            }
                                            apos_Win = CastAide.AsDecimal(foundRows[i]["apositiontaking"]) * 100;
                                        }
                                        else //place
                                        {
                                            if (hrStatus.ToLower() == "won")
                                            {
                                                TemplateHandler.Assign(ref Odds_33_place, "2",
                                                                       "Place: <b>" +
                                                                       NumberFormat.DecFormat(foundRows[i]["odds"], 2) +
                                                                       "</b>");
                                            }
                                            else
                                            {
                                                TemplateHandler.Assign(ref Odds_33_place, "2", "Place: <b>0.00</b>");
                                            }
                                            apos_Place = CastAide.AsDecimal(foundRows[i]["apositiontaking"]) * 100;
                                        }
                                    }

                                    if (status.ToLower() == "won" || status.ToLower() == "lose")
                                        TemplateHandler.Assign(ref strRow, "Odds", Odds_33_win + "<br>" + Odds_33_place);
                                    else
                                        TemplateHandler.Assign(ref strRow, "Odds", "-");
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref strRow, "Odds", "-");
                                }

                                break;

                            case 41:
                            case 42:
                                string oddsMsg = String.Empty;
                                if (IsBetLoseVoidReject(status.ToLower()))
                                    oddsMsg = "-";
                                else
                                {
                                    decimal oddsBet = CastAide.AsDecimal(dr["modds"]);
                                    decimal oddsSettle = CastAide.AsDecimal(dr["odds"]);
                                    oddsMsg = NumberFormat.DecFormat(oddsBet, 2);
                                    //if bet is deducted after settled:
                                    if (IsBetSettled(status.ToLower()) && !oddsSettle.Equals(oddsBet))
                                    //settle && deducted
                                    {
                                        oddsMsg =
                                            String.Format(
                                                "<div style='font-weight:normal;'>{0}: <b>{1}</b><br>{2}: <b>{3}</b></div>",
                                                "bet at", NumberFormat.DecFormat(oddsBet, 2), "settled",
                                                NumberFormat.DecFormat(oddsSettle, 2));
                                    }
                                }
                                TemplateHandler.Assign(ref strRow, "Odds", oddsMsg);
                                break;

                            case 43:
                                //get data of bettransm for bet type 33,43
                                if (dataTable2 != null && dataTable2.Rows.Count > 0)
                                {
                                    foundRows = dataTable2.Select("refno=" + dr["transid"]);
                                }

                                if (foundRows != null && foundRows.Length > 1 && !IsBetRejectVoid(status.ToLower()))
                                {
                                    string msgWin43 = String.Empty, msgPlace43 = String.Empty;
                                    for (int i = 0; i < 2; i++)
                                    {
                                        decimal oddsBet43 = CastAide.AsDecimal(foundRows[i]["finalodds"]);
                                        decimal oddsSettle43 = CastAide.AsDecimal(foundRows[i]["odds"]);
                                        string hrStatus = CastAide.AsString(foundRows[i]["status"]);
                                        DataRow foundRow = foundRows[i];
                                        if (CastAide.AsInt32(foundRow["bettype"]) == 41) //Win bettype
                                        {
                                            if (IsBetLoseRefund(hrStatus.ToLower())) //BetLose -> 0.00
                                            {
                                                oddsBet43 = 0;
                                            }
                                            msgWin43 = String.Format("{0}: <b>{1}</b>", "Win",
                                                                     NumberFormat.DecFormat(oddsBet43, 2));
                                            if (IsBetSettled(hrStatus.ToLower()) && !IsBetLoseRefund(hrStatus.ToLower()) &&
                                                !oddsBet43.Equals(oddsSettle43))
                                            {
                                                msgWin43 = String.Format("{0}: {1}<br>{2}: <b>{3}</b>", "W-bet at",
                                                                         NumberFormat.DecFormat(oddsBet43, 2),
                                                                         "W-settled",
                                                                         NumberFormat.DecFormat(oddsSettle43, 2));
                                            }

                                            apos_Win = CastAide.AsDecimal(foundRow["apositiontaking"]) * 100;
                                        }
                                        else //Place bettype
                                        {
                                            if (IsBetLoseRefund(hrStatus.ToLower())) //BetLose => 0.00
                                            {
                                                oddsBet43 = 0;
                                            }
                                            msgPlace43 = String.Format("{0}: <b>{1}</b>", "Place",
                                                                       NumberFormat.DecFormat(oddsBet43, 2));
                                            if (IsBetSettled(hrStatus.ToLower()) && !IsBetLoseRefund(hrStatus.ToLower()) &&
                                                !oddsBet43.Equals(oddsSettle43))
                                            {
                                                msgPlace43 = String.Format("{0}: {1}<br>{2}: <b>{3}</b>", "P-bet at",
                                                                           NumberFormat.DecFormat(oddsBet43, 2),
                                                                           "P-settled",
                                                                           NumberFormat.DecFormat(oddsSettle43, 2));
                                            }

                                            apos_Place = CastAide.AsDecimal(foundRow["apositiontaking"]) * 100;
                                        }
                                    }
                                    TemplateHandler.Assign(ref strRow, "Odds",
                                                           String.Format(
                                                               "<span class='blue' style='font-weight:normal;'>{0}</span><br><span class='' style='font-weight:normal;'>{1}</span>",
                                                               msgWin43, msgPlace43));
                                    //msgWin43 + "<br>" + msgPlace43);
                                }
                                else
                                {
                                    TemplateHandler.Assign(ref strRow, "Odds", "-");
                                }
                                break;

                            case 9: //mix parlay
                                TemplateHandler.Assign(ref strRow, "Odds",
                                                       NumberFormat.DecFormat(CastAide.AsDecimal(dr["odds"]), 4));
                                break;

                            default:
                                TemplateHandler.Assign(ref strRow, "Odds",
                                                       IBCUtils.FormatHandler.FormatNumber3(
                                                           CastAide.AsDecimal(dr["odds"])));
                                if (IsSportBook(betType))
                                {
                                    oddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);
                                    liveStyle = (CastAide.AsBoolean(dr["liveindicator"], false) ? "event-live" : "");
                                }
                                break;
                        }
                        TemplateHandler.Assign(ref strRow, "OddsStype", oddsTypeName);

                        #endregion assign Odds

                        //assign odd-even row
                        if (string.IsNullOrEmpty(liveStyle))
                        {
                            if (index % 2 != 0)
                                TemplateHandler.Assign(ref strRow, "odd_even", "r-e");
                            else
                                TemplateHandler.Assign(ref strRow, "odd_even", "r-o");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref strRow, "odd_even", liveStyle);
                        }

                        //assign status

                        #region assign betlist status

                        string betListStatus = status.ToLower();
                        if (betType == 33 || betType == 43) //win/place || fixodds win/place
                        {
                            switch (status.ToLower())
                            {
                                case "running":
                                    betListStatus = "Running";
                                    TemplateHandler.Assign(ref strRow, "PlayerComm", "");
                                    break;

                                case "waiting":
                                    betListStatus = "Waiting";
                                    TemplateHandler.Assign(ref strRow, "PlayerComm", "");
                                    break;

                                case "void":
                                case "reject":
                                case "refund":
                                    TemplateHandler.Assign(ref strRow, "PlayerComm", "");
                                    break;

                                default:
                                    string Status_win = "<span class='blue' style='font-weight:normal;'>{1}</span>";
                                    string Status_place = "<span class='' style='font-weight:normal;'>{2}</span>";
                                    if (foundRows != null && foundRows.Length > 1)
                                    {
                                        for (int i = 0; i < 2; i++)
                                        {
                                            int hrBetType = CastAide.AsInt32(foundRows[i]["bettype"]);
                                            string hrStatus = CastAide.AsString(foundRows[i]["status"]);
                                            if (hrBetType == 31 || hrBetType == 41) //win
                                            {
                                                if (hrStatus.ToLower() == "won")
                                                    TemplateHandler.Assign(ref Status_win, "1", "Win: " + "<b>Won</b>");
                                                else
                                                    TemplateHandler.Assign(ref Status_win, "1", "Win: " + "<b>Lose</b>");
                                            }
                                            else //Place
                                            {
                                                if (hrStatus.ToLower() == "won")
                                                    TemplateHandler.Assign(ref Status_place, "2",
                                                                           "Place: " + "<b>Won</b>");
                                                else if (hrStatus.ToLower() == "refund")
                                                    TemplateHandler.Assign(ref Status_place, "2",
                                                                           "Place: " + "<b>Refund</b>");
                                                else if (hrStatus.ToLower() == "lose")
                                                    TemplateHandler.Assign(ref Status_place, "2",
                                                                           "Place: " + "<b>Lose</b>");
                                            }
                                        }
                                        status = Status_win + "<br>" + Status_place;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch (status.ToLower())
                            {
                                case "running":
                                    TemplateHandler.Assign(ref strRow, "PlayerComm", "");
                                    betListStatus = status;
                                    break;

                                case "waiting":
                                    TemplateHandler.Assign(ref strRow, "PlayerComm", "");
                                    betListStatus = status;
                                    break;

                                case "void":
                                case "reject":
                                case "refund":
                                case "draw":
                                    betListStatus = status;
                                    break;

                                case "lose":
                                case "won":
                                    betListStatus = status;
                                    break;
                            }
                        }
                        TemplateHandler.Assign(ref strRow, "Status", betListStatus.ToUpper());
                        TemplateHandler.Assign(ref strRow, "IP", IpManagement.ReplaceVNIP(CastAide.AsString(dr["ip"])));

                        #endregion assign betlist status

                        //PT - Comm
                        TemplateHandler.Assign(ref strRow, "PTValue",
                                               "<font class='bl_underdog'>" + NumberFormat.DecFormat(awinlost, 2) +
                                               "</font><br />");
                        TemplateHandler.Assign(ref strRow, "Comm", NumberFormat.DecFormat(adiscount, 0));
                        TemplateHandler.Assign(ref strRow, "CommValue", NumberFormat.DecFormat(acomm, 2));

                        //TemplateHandler.Assign(ref strRow, "PT", NumberFormat.DecFormat(apos, 0));

                        switch (betType)
                        {
                            case 33:
                            case 43:
                                TemplateHandler.Assign(ref strRow, "PT",
                                                       "<span class='blue' style='font-weight:normal;'>Win: " + "<b>" +
                                                       NumberFormat.DecFormat(apos_Win, 0) + "%</b>" + "</span><br>" +
                                                       "<span class='' style='font-weight:normal;'>Place: " + "<b>" +
                                                       NumberFormat.DecFormat(apos_Place, 0) + "%</b>" + "</span>" +
                                                       "<br />");
                                break;

                            default:
                                TemplateHandler.Assign(ref strRow, "PT",
                                                       "<font class='bl_underdog'>" + NumberFormat.DecFormat(apos, 0) +
                                                       "%</font>" + "<br />");
                                break;
                        }

                        //append row
                        html.Append(strRow);
                    }
                    //footer row
                    TemplateHandler.Assign(ref tplFooter, "Subtotalwinlost", NumberFormat.DecFormat(totalWL, 2));
                    TemplateHandler.Assign(ref tplFooter, "SubtotalCom", NumberFormat.DecFormat(totalComm, 2));
                    TemplateHandler.Assign(ref tplFooter, "Total", NumberFormat.DecFormat(totalWL + totalComm, 2));
                    html.Append(tplFooter);
                }
                else
                {
                    html.Append(AccCommon.GenNoInformation("8"));
                }
            }
            catch (Exception ex)
            {
                html.Append(ex.Message);
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            return html.ToString();
        }

        #endregion RenderHTML

        public virtual string CreateHeader(Hashtable input)
        {
            return "";
        }

        private bool IsBetLoseVoidReject(string betStatus)
        {
            return (String.Compare(betStatus, "lose", true) == 0
                    || String.Compare(betStatus, "void", true) == 0
                    || String.Compare(betStatus, "reject", true) == 0);
        }

        private bool IsBetSettled(string betStatus)
        {
            return (String.Compare(betStatus, "running", true) != 0
                    && String.Compare(betStatus, "waiting", true) != 0
                    && String.Compare(betStatus, "reject", true) != 0);
        }

        private bool IsBetRejectVoid(string betStatus)
        {
            return (String.Compare(betStatus, "reject", true) == 0
                    || String.Compare(betStatus, "void", true) == 0);
        }

        private bool IsBetLoseRefund(string betStatus)
        {
            return (String.Compare(betStatus, "lose", true) == 0
                    || String.Compare(betStatus, "refund", true) == 0);
        }

        private bool IsSportBook(int bettype)
        {
            return (bettype == 1 || bettype == 2 || bettype == 3 || bettype == 7 || bettype == 8 || bettype == 12 ||
                    bettype == 20);
        }

        private BetlistRow InputBetlistRow(DataRow dr)
        {
            var row = new BetlistRow();
            row.Sporttype = CastAide.AsInt32(dr["Sporttype"]);
            row.TransID = CastAide.AsInt64(dr["TransID"]);
            row.BetID = CastAide.AsInt64(dr["BetID"]);
            row.Bettype = CastAide.AsInt32(dr["Bettype"]);
            row.hdp1 = CastAide.AsDecimal(dr["hdp1"]);
            row.hdp2 = CastAide.AsDecimal(dr["hdp2"]);
            row.Modds = CastAide.AsDecimal(dr["Modds"]);
            row.OddsType = CastAide.AsInt32(dr["OddsType"]);
            row.Odds = CastAide.AsDecimal(dr["Odds"]);
            row.Stake = CastAide.AsDecimal(dr["Stake"]);
            row.Betteam = CastAide.AsString(dr["Betteam"]);
            row.Status = CastAide.AsString(dr["Status"]);
            row.RefNO = CastAide.AsInt64(dr["RefNO"]);
            row.Transdate = CastAide.AsDateTime(dr["Transdate"]);
            row.Winlostdate = CastAide.AsDateTime(dr["Winlostdate"]);
            row.Winlost = CastAide.AsDecimal(dr["Winlost"]);
            row.awinlost = CastAide.AsDecimal(dr["awinlost"]);
            row.PlayerComm = CastAide.AsDecimal(dr["PlayerComm"]);
            row.acomm = CastAide.AsDecimal(dr["acomm"]);
            row.APositiontaking = CastAide.AsDecimal(dr["APositiontaking"]);
            row.Adiscount = CastAide.AsDecimal(dr["Adiscount"]);
            row.Liveindicator = CastAide.AsBoolean(dr["Liveindicator"]);
            row.Livehomescore = CastAide.AsInt32(dr["Livehomescore"]);
            row.Liveawayscore = CastAide.AsInt32(dr["Liveawayscore"]);
            row.IP = CastAide.AsString(dr["IP"]);
            row.LeagueID = CastAide.AsInt32(dr["LeagueID"]);
            row.MatchID = CastAide.AsInt32(dr["MatchID"]);
            row.MatchCode = CastAide.AsString(dr["MatchCode"]);
            row.Race = CastAide.AsString(dr["Race"]);
            row.HomeID = CastAide.AsInt32(dr["HomeID"]);
            row.AwayID = CastAide.AsInt32(dr["AwayID"]);
            row.Betstatus = CastAide.AsString(dr["Betstatus"]);
            row.isNeutral = CastAide.AsBoolean(dr["isNeutral"]);

            row.StakeMYR = CastAide.AsDecimal(dr["StakeMYR"]);
            row.WinlostMYR = CastAide.AsDecimal(dr["WinlostMYR"]);
            row.AWinlostMYR = CastAide.AsDecimal(dr["AWinlostMYR"]);
            row.PlayerCommMYR = CastAide.AsDecimal(dr["PlayerCommMYR"]);
            row.AcommMYR = CastAide.AsDecimal(dr["AcommMYR"]);

            row.StakeUSD = CastAide.AsDecimal(dr["StakeUSD"]);
            row.WinlostUSD = CastAide.AsDecimal(dr["WinlostUSD"]);
            row.AWinlostUSD = CastAide.AsDecimal(dr["AWinlostUSD"]);
            row.PlayerCommUSD = CastAide.AsDecimal(dr["PlayerCommUSD"]);
            row.AcommUSD = CastAide.AsDecimal(dr["AcommUSD"]);

            row.StakeEUR = CastAide.AsDecimal(dr["StakeEUR"]);
            row.WinlostEUR = CastAide.AsDecimal(dr["WinlostEUR"]);
            row.AWinlostEUR = CastAide.AsDecimal(dr["AWinlostEUR"]);
            row.PlayerCommEUR = CastAide.AsDecimal(dr["PlayerCommEUR"]);
            row.AcommEUR = CastAide.AsDecimal(dr["AcommEUR"]);

            return row;
        }
    }
}