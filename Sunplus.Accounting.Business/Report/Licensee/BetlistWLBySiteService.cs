using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Fanex.Tarus.LiveCasino.Entities.Betlist;
using Sunplus.Accounting.Business.BetList;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IReportDAO;

namespace Sunplus.Accounting.Business.Report.Deposit
{
    public class BetlistWLBySiteService : IBetlistWLBySiteService
    {
        private readonly ILicenseDAO _licenseReportDAO;

        public BetlistWLBySiteService(ILicenseDAO licenseReportDAO)
        {
            _licenseReportDAO = licenseReportDAO;
        }

        public string RenderHTML(System.Collections.Hashtable input)
        {
            var html = new StringBuilder();
            try
            {
                string tplHeader = WebUtils.LoadTemplate("BetList/DepositReport/Betlist_Header.htm");
                string tplBody = WebUtils.LoadTemplate("BetList/DepositReport/Betlist_Body.htm");
                string tplFooter = WebUtils.LoadTemplate("BetList/DepositReport/Betlist_Footer.htm");
                string tplEvent = WebUtils.LoadTemplate("BetList/DepositReport/BetlistEvent.htm");
                string liveStyle = string.Empty;

                BetlistGenerateEvent blEvent = new BetlistGenerateEvent();

                string fdate = CastAide.AsDateTime(input["fdate"], DateTime.Now).ToShortDateString();
                string tdate = CastAide.AsDateTime(input["tdate"], DateTime.Now).ToShortDateString();
                int baFilter = CastAide.AsInt32(input["bafilter"], 0);
                string MoveBetsDay = Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                string BalanceUpDay = Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.BalanceUpDay.ToShortDateString();
                int custid = CastAide.AsInt32(input["custid"]);
                string username = CastAide.AsString(input["username"]);
                bool showsb = CastAide.AsInt32(input["showsb"], 1) == 1;
                bool showrb = CastAide.AsInt32(input["showrb"], 1) == 1;
                bool showng = CastAide.AsInt32(input["showng"], 1) == 1;
                int excludetest = CastAide.AsInt32(input["excludetest"], 0);
                int drawfilter = CastAide.AsInt32(input["drawfilter"], 1);
                string baseCurrency = CastAide.AsString(input["basecurrency"]).ToLower();
                string site = CastAide.AsString(input["site"]);

                //assign header
                TemplateHandler.Assign(ref tplHeader, "fdate", fdate);
                TemplateHandler.Assign(ref tplHeader, "tdate", tdate);
                TemplateHandler.Assign(ref tplHeader, "username", username);
                if (baseCurrency == "usd")
                    TemplateHandler.Assign(ref tplHeader, "base", " - USD Base Currency");
                else if (baseCurrency == "myr")
                    TemplateHandler.Assign(ref tplHeader, "base", " - RM Base Currency");
                else if (baseCurrency == "eur")
                    TemplateHandler.Assign(ref tplHeader, "base", " - EUR Base Currency");
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

                int drawFilter = CastAide.AsInt32(input["drawFilter"]);
                DbResult dbResult = _licenseReportDAO.SelBetlistByLicensee(custid, fdate, tdate, MoveBetsDay, BalanceUpDay,
                                                                           showsb, showrb, showng, drawfilter, baFilter);

                DataTable dataTable = new DataTable();
                DataTable dataTable2 = new DataTable();
                DataRow[] foundRows = null;
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                {
                    dataTable = dbResult.ResultSet.Tables[0];
                    if (showrb)
                        dataTable2 = dbResult.ResultSet.Tables[1];
                }

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
                            stake = 0,
                            actual_stake = 0;
                    int leagueid = 0, matchid = 0, betType = 0;
                    string status = string.Empty;
                    string winlost_col = string.Empty;
                    string awinlost_col = string.Empty;
                    string stake_col = string.Empty;
                    string comm_col = string.Empty;
                    string acomm_col = string.Empty;
                    string actualstake_col = string.Empty;

                    if (baseCurrency == "usd")
                    {
                        winlost_col = "winlostusd";
                        awinlost_col = "awinlostusd";
                        stake_col = "stakeusd";
                        comm_col = "playercommusd";
                        acomm_col = "acommusd";
                        actualstake_col = "actual_stakeusd";
                    }
                    else if (baseCurrency == "myr")
                    {
                        winlost_col = "winlostmyr";
                        awinlost_col = "awinlostmyr";
                        stake_col = "stakemyr";
                        comm_col = "playercommmyr";
                        acomm_col = "acommmyr";
                        actualstake_col = "actual_stakemyr";
                    }
                    else if (baseCurrency == "eur")
                    {
                        winlost_col = "winlosteur";
                        awinlost_col = "awinlosteur";
                        stake_col = "stakeeur";
                        comm_col = "playercommeur";
                        acomm_col = "acommeur";
                        actualstake_col = "actual_stakeeur";
                    }
                    else
                    {
                        winlost_col = "winlost";
                        awinlost_col = "awinlost";
                        stake_col = "stake";
                        comm_col = "playercomm";
                        acomm_col = "acomm";
                        actualstake_col = "actual_stake";
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
                        actual_stake = CastAide.AsDecimal(dr[actualstake_col]);
                        totalWL += CastAide.AsDecimal(winlost, 2);
                        totalComm += CastAide.AsDecimal(comm, 2);

                        matchid = CastAide.AsInt32(dr["matchid"], 0);
                        leagueid = CastAide.AsInt32(dr["leagueid"], 0);

                        apos = Convert.ToDecimal(dr["apositiontaking"]) * 100;
                        adiscount = Convert.ToDecimal(dr["adiscount"]) * 100;

                        status = CastAide.AsString(dr["status"]);

                        TemplateHandler.Assign(ref strRow, "No", index.ToString());
                        TemplateHandler.Assign(ref strRow, "TransTime", DateFormat.FullDateFormat(dr["transdate"]));

                        //generate event
                        BetlistRow blRow = InputBetlistRow(dr);
                        strEvent = blEvent.GenerateEvent(strEvent, blRow);
                        TemplateHandler.Assign(ref strRow, "event", strEvent);

                        //Stake
                        if (betType == 29)
                        {
                            var stakeFormat = string.Format(
                                "{0}{1}",
                                IBCUtils.FormatHandler.FormatNumber20(actual_stake),
                                (Math.Round(stake) == 0
                                     ? string.Empty
                                     : "<br/>(" + IBCUtils.FormatHandler.FormatNumber20(stake)
                                       + ")"));

                            TemplateHandler.Assign(ref strRow, "Stake", stakeFormat);
                        }

                        else
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

                                        if (hrBettype == 31)//win
                                        {
                                            if (hrStatus.ToLower() == "won")
                                            {
                                                TemplateHandler.Assign(ref Odds_33_win, "1", "Win: <b>" + NumberFormat.DecFormat(foundRows[i]["odds"], 2) + "</b>");
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
                                                TemplateHandler.Assign(ref Odds_33_place, "2", "Place: <b>" + NumberFormat.DecFormat(foundRows[i]["odds"], 2) + "</b>");
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

                            case 9: //mix parlay
                                TemplateHandler.Assign(ref strRow, "Odds", NumberFormat.DecFormat(CastAide.AsDecimal(dr["odds"]), 4));
                                break;

                            case 29:
                                TemplateHandler.Assign(
                                    ref strRow,
                                    "Odds",
                                    (status.ToLower() == "running"
                                         ? "-"
                                         : NumberFormat.DecFormat(CastAide.AsDecimal(dr["modds"]), 3)));
                                break;

                            default:
                                var _OddsTypeName = CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(betType, CastAide.AsInt32(dr["oddstype"])));
                                TemplateHandler.Assign(ref strRow, "Odds", IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["odds"])));
                                if (betType > 80 && betType < 91)
                                    oddsTypeName = _OddsTypeName;
                                if (IsSportBook(betType))
                                {
                                    oddsTypeName = _OddsTypeName;
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
                                                    TemplateHandler.Assign(ref Status_place, "2", "Place: " + "<b>Won</b>");
                                                else if (hrStatus.ToLower() == "refund")
                                                    TemplateHandler.Assign(ref Status_place, "2", "Place: " + "<b>Refund</b>");
                                                else if (hrStatus.ToLower() == "lose")
                                                    TemplateHandler.Assign(ref Status_place, "2", "Place: " + "<b>Lose</b>");
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
                        TemplateHandler.Assign(ref strRow, "PTValue", "<font class='bl_underdog'>" + NumberFormat.DecFormat(awinlost, 2) + "</font><br />");
                        TemplateHandler.Assign(ref strRow, "Comm", NumberFormat.DecFormat(adiscount, 2));
                        TemplateHandler.Assign(ref strRow, "CommValue", NumberFormat.DecFormat(acomm, 2));

                        //TemplateHandler.Assign(ref strRow, "PT", NumberFormat.DecFormat(apos, 0));

                        switch (betType)
                        {
                            case 33:
                            case 43:
                                TemplateHandler.Assign(ref strRow, "PT", "<span class='blue' style='font-weight:normal;'>Win: " + "<b>" + NumberFormat.DecFormat(apos_Win, 0) + "%</b>" + "</span><br>" + "<span class='' style='font-weight:normal;'>Place: " + "<b>" + NumberFormat.DecFormat(apos_Place, 0) + "%</b>" + "</span>" + "<br />");
                                break;

                            default:
                                TemplateHandler.Assign(ref strRow, "PT", "<font class='bl_underdog'>" + NumberFormat.DecFormat(apos, 0) + "%</font>" + "<br />");
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

        #region InputBetlistRow

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
            row.OddsType = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(row.Bettype, CastAide.AsInt32(dr["oddstype"]));
            row.Odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(dr["odds"]), row.Bettype, CastAide.AsInt32(dr["oddstype"]));
            row.Stake = CastAide.AsDecimal(dr["Stake"]);
            row.Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(dr["betteam"].ToString(), row.Bettype, "e");
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

        #endregion InputBetlistRow

        #region InputBetlistRowV2

        private BetlistRow InputBetlistRowV2(DataRow dr)
        {
            var row = new BetlistRow();
            row.Sporttype = CastAide.AsInt32(dr["Sporttype"]);
            row.TransID = CastAide.AsInt64(dr["TransID"]);
            row.BetID = CastAide.AsInt64(dr["BetID"]);
            row.Bettype = CastAide.AsInt32(dr["Bettype"]);
            row.hdp1 = CastAide.AsDecimal(dr["hdp1"]);
            row.hdp2 = CastAide.AsDecimal(dr["hdp2"]);
            row.Modds = CastAide.AsDecimal(dr["Modds"]);
            row.OddsType = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(row.Bettype, CastAide.AsInt32(dr["oddstype"]));
            row.Odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(dr["odds"]), row.Bettype, CastAide.AsInt32(dr["oddstype"]));
            row.Stake = CastAide.AsDecimal(dr["Stake"]);
            row.Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(dr["betteam"].ToString(), row.Bettype, "e");
            row.Status = CastAide.AsString(dr["Status"]);
            row.RefNO = CastAide.AsInt64(dr["RefNO"]);
            row.Transdate = CastAide.AsDateTime(dr["Transdate"]);
            row.Winlostdate = CastAide.AsDateTime(dr["Winlostdate"]);
            row.Winlost = CastAide.AsDecimal(dr["Winlost"]);
            row.awinlost = CastAide.AsDecimal(dr["LicenseeWL"]);
            row.ShowTime = dr["showtime"].ToString();
            //row.PlayerComm = CastAide.AsDecimal(dr["PlayerComm"]);
            row.acomm = CastAide.AsDecimal(dr["comm"]);
            //row.APositiontaking = CastAide.AsDecimal(dr["APositiontaking"]);
            //row.Adiscount = CastAide.AsDecimal(dr["Adiscount"]);
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
            row.AWinlostMYR = CastAide.AsDecimal(dr["LicenseeWLMYR"]);
            // row.PlayerCommMYR = CastAide.AsDecimal(dr["PlayerCommMYR"]);
            row.AcommMYR = CastAide.AsDecimal(dr["commMYR"]);

            row.StakeUSD = CastAide.AsDecimal(dr["StakeUSD"]);
            row.WinlostUSD = CastAide.AsDecimal(dr["WinlostUSD"]);
            row.AWinlostUSD = CastAide.AsDecimal(dr["LicenseeWLUSD"]);
            //row.PlayerCommUSD = CastAide.AsDecimal(dr["PlayerCommUSD"]);
            row.AcommUSD = CastAide.AsDecimal(dr["commUSD"]);

            row.StakeEUR = CastAide.AsDecimal(dr["StakeEUR"]);
            row.WinlostEUR = CastAide.AsDecimal(dr["WinlostEUR"]);
            row.AWinlostEUR = CastAide.AsDecimal(dr["LicenseeWLEUR"]);
            //row.PlayerCommEUR = CastAide.AsDecimal(dr["PlayerCommEUR"]);
            row.AcommEUR = CastAide.AsDecimal(dr["commEUR"]);
            return row;
        }

        #endregion InputBetlistRowV2

        #region IsBetLoseVoidReject

        private bool IsBetLoseVoidReject(string betStatus)
        {
            return (String.Compare(betStatus, "lose", true) == 0
                || String.Compare(betStatus, "void", true) == 0
                || String.Compare(betStatus, "reject", true) == 0);
        }

        #endregion IsBetLoseVoidReject

        #region IsBetSettled

        private bool IsBetSettled(string betStatus)
        {
            return (String.Compare(betStatus, "running", true) != 0
                && String.Compare(betStatus, "waiting", true) != 0
                && String.Compare(betStatus, "reject", true) != 0);
        }

        #endregion IsBetSettled

        #region IsBetRejectVoid

        private bool IsBetRejectVoid(string betStatus)
        {
            return (String.Compare(betStatus, "reject", true) == 0
                || String.Compare(betStatus, "void", true) == 0);
        }

        #endregion IsBetRejectVoid

        #region IsBetLoseRefund

        private bool IsBetLoseRefund(string betStatus)
        {
            return (String.Compare(betStatus, "lose", true) == 0
                || String.Compare(betStatus, "refund", true) == 0);
        }

        #endregion IsBetLoseRefund

        #region IsSportBook

        private bool IsSportBook(int bettype)
        {
            return (bettype == 1 || bettype == 2 || bettype == 3 || bettype == 7 || bettype == 8 || bettype == 12 || bettype == 20);
        }

        #endregion IsSportBook

        #region RenderHTMLV2

        public string RenderHTMLV2(System.Collections.Hashtable input)
        {
            var html = new StringBuilder();
            try
            {
                string tplHeader = WebUtils.LoadTemplate("BetListV2/Betlist_Header.htm");
                string tplBody = WebUtils.LoadTemplate("BetListV2/Betlist_Body.htm");
                string tplFooter = WebUtils.LoadTemplate("BetListV2/Betlist_Footer.htm");
                string tplEvent = WebUtils.LoadTemplate("BetListV2/BetlistEvent.htm");
                string eventTplLC = WebUtils.LoadTemplate("BetList\\EventLC.htm");
                string liveStyle = string.Empty;

                BetlistGenerateEvent blEvent = new BetlistGenerateEvent();

                string fdate = CastAide.AsDateTime(input["fdate"], DateTime.Now).ToShortDateString();
                string tdate = CastAide.AsDateTime(input["tdate"], DateTime.Now).ToShortDateString();
                string MoveBetsDay = Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                string BalanceUpDay = Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.BalanceUpDay.ToShortDateString();
                int custid = CastAide.AsInt32(input["custid"]);
                string username = CastAide.AsString(input["username"]);
                bool showsb = CastAide.AsInt32(input["showsb"], 1) == 1;
                bool showrb = CastAide.AsInt32(input["showrb"], 1) == 1;
                bool showng = CastAide.AsInt32(input["showng"], 1) == 1;
                bool showlc = CastAide.AsInt32(input["showlc"], 1) == 1;
                bool showvs = CastAide.AsInt32(input["showvs"], 1) == 1;
                bool showcs = CastAide.AsInt32(input["showcs"], 1) == 1;

                int showba = CastAide.AsInt32(input["showba"], 0);
                //int excludetest = CastAide.AsInt32(input["excludetest"], 0);
                int drawfilter = CastAide.AsInt32(input["drawfilter"], 1);
                string baseCurrency = CastAide.AsString(input["basecurrency"]).ToLower();
                string site = CastAide.AsString(input["site"]);

                //assign header
                TemplateHandler.Assign(ref tplHeader, "fdate", fdate);
                TemplateHandler.Assign(ref tplHeader, "tdate", tdate);
                TemplateHandler.Assign(ref tplHeader, "username", username);
                if (baseCurrency == "usd")
                    TemplateHandler.Assign(ref tplHeader, "base", " - USD Base Currency");
                else if (baseCurrency == "myr")
                    TemplateHandler.Assign(ref tplHeader, "base", " - RM Base Currency");
                else if (baseCurrency == "eur")
                    TemplateHandler.Assign(ref tplHeader, "base", " - EUR Base Currency");
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

                int drawFilter = CastAide.AsInt32(input["drawFilter"]);
                DbResult dbResult = _licenseReportDAO.SelBetlistByLicenseeV2(custid, fdate, tdate, MoveBetsDay, BalanceUpDay,
                                                                           showsb, showrb, showng, showba, showlc, showvs, showcs, drawfilter);

                DataTable dataTable = new DataTable();
                DataTable dataTable2 = new DataTable();
                DataRow[] foundRows = null;
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                {
                    dataTable = dbResult.ResultSet.Tables[0];
                    if (showrb || showlc)
                        dataTable2 = dbResult.ResultSet.Tables[1];
                }

                DataRow[] rows = dataTable.Select();
                if (rows.Length > 0)
                {
                    int index = 0;

                    decimal winlost = 0, licenseewl = 0, comm = 0, acomm = 0, totalWL = 0, modds = 0, totalGrossProfit = 0, adiscount = 0, apos = 0, totalComm = 0, apos_Win = 0, apos_Place = 0, stake = 0;
                    int leagueid = 0, matchid = 0, betType = 0;
                    string status = string.Empty;
                    string winlost_col = string.Empty;
                    //string awinlost_col = string.Empty;
                    string licenseewl_col = string.Empty;
                    string stake_col = string.Empty;

                    string comm_col = string.Empty;

                    string acomm_col = string.Empty;

                    if (baseCurrency == "usd")
                    {
                        winlost_col = "winlostusd";
                        licenseewl_col = "licenseewlusd";
                        stake_col = "stakeusd";
                        comm_col = "playercommusd";
                        acomm_col = "commusd";
                    }
                    else if (baseCurrency == "myr")
                    {
                        winlost_col = "winlostmyr";
                        licenseewl_col = "licenseewlmyr";
                        stake_col = "stakemyr";
                        comm_col = "playercommmyr";
                        acomm_col = "commmyr";
                    }
                    else if (baseCurrency == "eur")
                    {
                        winlost_col = "winlosteur";
                        licenseewl_col = "licenseewleur";
                        stake_col = "stakeeur";
                        comm_col = "playercommeur";
                        acomm_col = "commeur";
                    }
                    else
                    {
                        winlost_col = "winlost";
                        licenseewl_col = "licenseewl";
                        stake_col = "stake";
                        comm_col = "playercomm";
                        acomm_col = "comm";
                    }

                    foreach (DataRow dr in rows)
                    {
                        string strRow = tplBody;
                        string strEvent = tplEvent;
                        liveStyle = "";

                        index++;

                        betType = CastAide.AsInt32(dr["BetType"], 0);

                        winlost = CastAide.AsDecimal(dr[winlost_col]);
                        licenseewl = CastAide.AsDecimal(dr[licenseewl_col]);
                        stake = CastAide.AsDecimal(dr[stake_col]);

                        //comm
                        comm = CastAide.AsDecimal(dr[comm_col]);
                        //comm = CastAide.AsDecimal(0);

                        acomm = CastAide.AsDecimal(dr[acomm_col]);
                        modds = CastAide.AsDecimal(dr["modds"]);
                        totalGrossProfit += CastAide.AsDecimal(licenseewl, 2) + CastAide.AsDecimal(acomm, 2);
                        totalWL += CastAide.AsDecimal(winlost, 2);
                        totalComm += CastAide.AsDecimal(comm, 2);

                        matchid = CastAide.AsInt32(dr["matchid"], 0);
                        leagueid = CastAide.AsInt32(dr["leagueid"], 0);

                        //apos = Convert.ToDecimal(dr["apositiontaking"]) * 100;
                        //adiscount = Convert.ToDecimal(dr["adiscount"]) * 100;

                        status = CastAide.AsString(dr["status"]);

                        TemplateHandler.Assign(ref strRow, "No", index.ToString());
                        TemplateHandler.Assign(ref strRow, "TransTime", DateFormat.FullDateFormat(dr["transdate"]));

                        if (betType == 71)//RNG Casino
                        {
                            TemplateHandler.Assign(ref strRow, "TransID", CastAide.AsString(dr["refno"]));
                        }
                        else
                        {
                            TemplateHandler.Assign(ref strRow, "TransID", CastAide.AsString(dr["transid"]));
                        }

                        //generate event
                        BetlistRow blRow = InputBetlistRowV2(dr);
                        var _listBetlistEvt = new List<BetlistEvent>();
                        if (betType == 1101 || betType == 1102 || betType == 1103)
                        {
                            if (dataTable2 != null && dataTable2.Rows.Count > 0)
                            {
                                var query = from rowEvt in dataTable2.AsEnumerable()
                                            where rowEvt.Field<Int64>("refno") == blRow.RefNO
                                            select rowEvt;

                                DataTable dtb3 = query.CopyToDataTable();

                                foreach (DataRow rowEvt in query)
                                {
                                    if (betType == 1101)
                                    {
                                        _listBetlistEvt.Add(new BetlistEvent()
                                        {
                                            GameType = CacheRef.GetLiveCasinoResult(CastAide.AsInt32(Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(rowEvt["betteam"].ToString(), blRow.Bettype, "e"))),
                                            Stake = NumberFormat.DecFormat(CastAide.AsDouble(rowEvt["stake" + baseCurrency]), 2),
                                            Odds = NumberFormat.DecFormat(CastAide.AsDouble(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(rowEvt["odds"]), blRow.Bettype, CastAide.AsInt32(blRow.OddsType))), 2)
                                        });
                                    }
                                    else if (betType == 1102)
                                    {
                                        _listBetlistEvt.Add(new BetlistEvent()
                                        {
                                            GameType = CacheRef.GetRouletteBetItem(CastAide.AsInt32(Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(rowEvt["betteam"].ToString(), blRow.Bettype, "e"))),
                                            Stake = NumberFormat.DecFormat(CastAide.AsDouble(rowEvt["stake" + baseCurrency]), 2),
                                            Odds = NumberFormat.DecFormat(CastAide.AsDouble(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(rowEvt["odds"]), blRow.Bettype, CastAide.AsInt32(blRow.OddsType))), 2)
                                        });
                                    }
                                }

                                strEvent = blEvent.GenerateEventLiveCasino(eventTplLC, blRow, _listBetlistEvt);
                            }
                        }
                        else
                            strEvent = blEvent.GenerateEvent(strEvent, blRow);
                        TemplateHandler.Assign(ref strRow, "event", strEvent);

                        //Stake
                        TemplateHandler.Assign(ref strRow, "Stake", NumberFormat.DecFormat(stake, 2));

                        //winlost
                        TemplateHandler.Assign(ref strRow, "WinLoss", NumberFormat.DecFormat(winlost, 2) + "<br/>" + NumberFormat.DecFormat(comm, 2));

                        //player comm
                        //TemplateHandler.Assign(ref strRow, "PlayerComm", NumberFormat.DecFormat(comm, 2));

                        //assign odds

                        #region assign Odds

                        var _odds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(dr["odds"]), betType, CastAide.AsInt32(dr["oddsType"]));
                        string oddsTypeName = "";
                        switch (betType)
                        {
                            case 31:
                            case 32:
                                if (status.ToLower() == "won")
                                    TemplateHandler.Assign(ref strRow, "Odds", NumberFormat.DecFormat(_odds, 2));
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

                                        if (hrBettype == 31)//win
                                        {
                                            if (hrStatus.ToLower() == "won")
                                            {
                                                TemplateHandler.Assign(ref Odds_33_win, "1", "Win: <b>" + NumberFormat.DecFormat(foundRows[i]["odds"], 2) + "</b>");
                                            }
                                            else
                                            {
                                                TemplateHandler.Assign(ref Odds_33_win, "1", "Win: <b>0.00</b>");
                                            }
                                            //apos_Win = CastAide.AsDecimal(foundRows[i]["apositiontaking"]) * 100;
                                        }
                                        else //place
                                        {
                                            if (hrStatus.ToLower() == "won")
                                            {
                                                TemplateHandler.Assign(ref Odds_33_place, "2", "Place: <b>" + NumberFormat.DecFormat(foundRows[i]["odds"], 2) + "</b>");
                                            }
                                            else
                                            {
                                                TemplateHandler.Assign(ref Odds_33_place, "2", "Place: <b>0.00</b>");
                                            }
                                            //apos_Place = CastAide.AsDecimal(foundRows[i]["apositiontaking"]) * 100;
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

                            case 9: //mix parlay
                                TemplateHandler.Assign(ref strRow, "Odds", NumberFormat.DecFormat(_odds, 4));
                                break;

                            case 29:
                                {
                                    if (status.ToLower().Contains("running"))
                                    {
                                        TemplateHandler.Assign(ref strRow, "{Odds}", "-");
                                    }
                                    else
                                    {
                                        TemplateHandler.Assign(ref strRow, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(modds) + "<div class='b bl-leaguename'>" + oddsTypeName + "</div>");
                                    }
                                }
                                break;

                            case 1000:
                            case 1101:
                            case 1102:
                            case 1103:
                                TemplateHandler.Assign(ref strRow, "{Odds}", "-");

                                break;

                            default:
                                TemplateHandler.Assign(ref strRow, "Odds", IBCUtils.FormatHandler.FormatNumber3(_odds));
                                //if (betType > 80 && betType < 90)
                                oddsTypeName = CacheRef.GetOddsTypeName(Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOddsType(betType, CastAide.AsInt32(dr["oddstype"])));
                                //    oddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);
                                if (IsSportBook(betType))
                                {
                                    //oddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);
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
                                                    TemplateHandler.Assign(ref Status_place, "2", "Place: " + "<b>Won</b>");
                                                else if (hrStatus.ToLower() == "refund")
                                                    TemplateHandler.Assign(ref Status_place, "2", "Place: " + "<b>Refund</b>");
                                                else if (hrStatus.ToLower() == "lose")
                                                    TemplateHandler.Assign(ref Status_place, "2", "Place: " + "<b>Lose</b>");
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
                        double convertLicenseeWL = CastAide.AsDouble(licenseewl);
                        double convertComm = CastAide.AsDouble(acomm);

                        if (convertLicenseeWL != 0 && convertComm != 0)
                        {
                            TemplateHandler.Assign(ref strRow, "LicenseeWL", "<font class='bl_underdog'>" + NumberFormat.DecFormat(licenseewl, 2) + "</font><br />");
                            TemplateHandler.Assign(ref strRow, "CommValue", NumberFormat.DecFormat(acomm, 2));
                        }
                        else if ((convertLicenseeWL != 0 && convertComm == 0) || (convertLicenseeWL == 0 && convertComm == 0))
                        {
                            TemplateHandler.Assign(ref strRow, "LicenseeWL", "<font class='bl_underdog'>" + NumberFormat.DecFormat(licenseewl, 2) + "</font><br />");
                            TemplateHandler.Assign(ref strRow, "CommValue", "");
                        }
                        else if (convertLicenseeWL == 0 && convertComm != 0)
                        {
                            TemplateHandler.Assign(ref strRow, "LicenseeWL", NumberFormat.DecFormat(acomm, 2));
                            TemplateHandler.Assign(ref strRow, "CommValue", "");
                        }

                        switch (betType)
                        {
                            case 33:
                            case 43:
                                TemplateHandler.Assign(ref strRow, "PT", "<span class='blue' style='font-weight:normal;'>Win: " + "<b>" + NumberFormat.DecFormat(apos_Win, 0) + "%</b>" + "</span><br>" + "<span class='' style='font-weight:normal;'>Place: " + "<b>" + NumberFormat.DecFormat(apos_Place, 0) + "%</b>" + "</span>" + "<br />");
                                break;

                            default:
                                TemplateHandler.Assign(ref strRow, "PT", "<font class='bl_underdog'>" + NumberFormat.DecFormat(apos, 0) + "%</font>" + "<br />");
                                break;
                        }

                        //append row
                        html.Append(strRow);

                    }
                    //footer row
                    TemplateHandler.Assign(ref tplFooter, "Subtotalwinlost", NumberFormat.DecFormat(totalWL, 2));
                    TemplateHandler.Assign(ref tplFooter, "SubtotalCom", NumberFormat.DecFormat(totalComm, 2));
                    TemplateHandler.Assign(ref tplFooter, "Total", NumberFormat.DecFormat(totalWL + totalComm, 2));
                    TemplateHandler.Assign(ref tplFooter, "TotalGrossProfit", NumberFormat.DecFormat(totalGrossProfit, 2));
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

        #endregion RenderHTMLV2
    }
}