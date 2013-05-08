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
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for TOddsHRWLDetails
/// </summary>
public partial class ReportHTML
{
    #region Tote Odds HR WL Report HTML

    #region ToteHRWLReport

    public static string ToteHRWLReport(HttpContext context)
    {
        DateTime wldate = CastAide.AsDateTime(context.Request["wldate"], DateTime.Today);
        string bettype = context.Request["bettype"];
        var html = new StringBuilder();
        try
        {
            string tpl = WebUtils.LoadTemplate("WinLost/ToteHRWLDetail.htm");
            if (context.Request["IsExport"] == "1")
                TemplateHandler.Assign(ref tpl, "border", "1");
            else
                TemplateHandler.Assign(ref tpl, "border", "0");
            //if (context.Request["type"] == "Direct")
            TemplateHandler.RemoveBlock(ref tpl, "export");

            if (context.Request["IsExport"] == "1")
            {
                TemplateHandler.RemoveBlock(ref tpl, "header");
                string title =
                    "<tr bgcolor='#ffabab'><td colspan='28' align='center'><b>Tote Horse Racing Daily Report: " +
                    wldate.ToShortDateString() + " </b></td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "title", title);
            }

            TemplateHandler.Assign(ref tpl, "bettype", bettype);
            TemplateHandler.Assign(ref tpl, "wldate", wldate.ToShortDateString());

            string db = context.Request["db"];
            string WL = context.Request["WL"];

            db = (string.IsNullOrEmpty(db)) ? "rep" : db;

            DbResult rs = null;
            
            if (wldate>ReportUtils.BalanceUp.MoveBetsDay)
                rs = MatchWinLostReport.WinlostHR_Details(wldate, db);
            else if(wldate>=ReportUtils.BalanceUp.BalanceUpDay)
                rs = MatchWinLostReport.WinlostHR_Details_14(wldate);
            else
                rs = MatchWinLostReport.WinlostHR_Details_WH(wldate);
                

            #region Sort sub total by group currency

            DataTable dtWL = rs.ResultSet.Tables[0];
            dtWL.Columns.Add("Order", typeof (int));
            int cid = 0, cid_p = 0, last_cid_usd = 101, last_cid_thb = 101, last_cid_in = 101;
            foreach (DataRow dr in dtWL.Rows)
            {
                cid_p = CastAide.AsInt32(dr["currency"]);

                if (strArrCurrencyOrder.Contains("[" + cid_p + "]"))
                {
                    dr["Order"] = strArrCurrencyOrder.IndexOf("[" + cid_p + "]");
                }
                else
                {
                    dr["Order"] = 101;
                }
            }

            #endregion

            DataView dv = dtWL.DefaultView;
            dv.Sort = "Order asc, currency asc, leagueid asc ";
            var dt = new DataTable();
            dt = dv.ToTable();

            #region Declare Variable

            var rows = new StringBuilder();
            string tplRow = TemplateHandler.CopyBlock(tpl, "content");
            int meetingid = 0, meetingid_p = 0;

            string report_row = "bgodd";
            int c_index = 0, m_index = 0;
            string raceNo = "";
            string currency = "", currency_p = "";
            string row = "";
            string strConditionCurrency = "",
                   strTitleCurrency = "",
                   strTotalCurrency = "",
                   bgcolorCurrency = "#d3e1c8",
                   bgcolorCurrency_sub = "#ffffc4";
            string condition_Meeting = "", strTitleMeeting = "", strTotalMeeting = "", bgcolorMeeting = "#ffeedd";
                // #d0e3ca
            string trBlank = "<tr><td colspan='29' class='l fontbold white'></td></tr>"; //#efefef            
            string meeting_currency = "",
                   strConditionSubTotalGroupCurrencyUSD = "",
                   strConditionSubTotalGroupCurrencyTHB = "",
                   strConditionSubTotalGroupCurrencyIN = "";
            bool IsTotalUSD = false, IsTotalTHB = false, IsTotalIN = false;
            int CountCurrencyGroupUSD = 0,
                CountCurrencyGroupTHB = 0,
                CountCurrencyGroupIN = 0,
                cid_usd = 0,
                cid_thb = 0,
                cid_in = 0;

            #endregion

            #region get lastest currency id

            foreach (DataRow dr in dt.Rows)
            {
                cid_p = CastAide.AsInt32(dr["currency"]);
                if (strCurrencySubTotal_USD.Contains("[" + cid_p + "]"))
                {
                    last_cid_usd = cid_p;
                    if (cid_usd != cid_p)
                    {
                        cid_usd = cid_p;
                        CountCurrencyGroupUSD++;
                    }
                }

                if (strCurrencySubTotal_THB.Contains("[" + cid_p + "]"))
                {
                    last_cid_thb = cid_p;
                    if (cid_thb != cid_p)
                    {
                        cid_thb = cid_p;
                        CountCurrencyGroupTHB++;
                    }
                }

                if (strCurrencySubTotal_IN.Contains("[" + cid_p + "]"))
                {
                    last_cid_in = cid_p;
                    if (cid_in != cid_p)
                    {
                        cid_in = cid_p;
                        CountCurrencyGroupIN++;
                    }
                }
            }
            if (CountCurrencyGroupUSD < 2) IsTotalUSD = true;
            if (CountCurrencyGroupTHB < 2) IsTotalTHB = true;
            if (CountCurrencyGroupIN < 2) IsTotalIN = true;

            #endregion

            #region strConditionSubTotalGroupCurrencyUSD

            foreach (int curr in ArrCurrencySubTotal_USD)
            {
                strConditionSubTotalGroupCurrencyUSD += " currency = " + curr + " or ";
            }
            strConditionSubTotalGroupCurrencyUSD += " currency = 0";

            #endregion

            #region strConditionSubTotalGroupCurrencyTHB

            foreach (int curr in ArrCurrencySubTotal_THB)
            {
                strConditionSubTotalGroupCurrencyTHB += " currency = " + curr + " or ";
            }
            strConditionSubTotalGroupCurrencyTHB += " currency = 0";

            #endregion

            #region strConditionSubTotalGroupCurrencyIN

            foreach (int curr in ArrCurrencySubTotal_IN)
            {
                strConditionSubTotalGroupCurrencyIN += " currency = " + curr + " or ";
            }
            strConditionSubTotalGroupCurrencyIN += " currency = 0";

            #endregion

            var CurrencyIDs = new ArrayList();
            var CurrencyNames = new ArrayList();


            // for testing loop
            // for (int i = 0; i < 30; i++)
            {
                #region foreach

                foreach (DataRow dr in dt.Rows)
                {
                    if (!CurrencyIDs.Contains(dr["Currency"]))
                    {
                        CurrencyIDs.Add(dr["Currency"]);
                        CurrencyNames.Add(dr["CurrencyName"]);
                    }

                    row = tplRow;
                    meetingid_p = CastAide.AsInt32(dr["leagueid"]);
                    currency_p = CastAide.AsString(dr["currencyname"]);
                    cid_p = CastAide.AsInt32(dr["currency"]);

                    #region Each Currency

                    if (currency_p != currency)
                    {
                        if (c_index > 0)
                        {
                            #region Total Meeting

                            condition_Meeting = "leagueid = '" + meetingid + "' and currencyname='" + currency + "'";
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid);
                            if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
                            else strTitleMeeting += currency + " Total";

                            strTotalMeeting = ToteRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting, condition_Meeting);
                            rows.Append(strTotalMeeting);

                            #endregion

                            #region Total Currency

                            {
                                strConditionCurrency = "currencyname = '" + currency + "'";

                                if (currency == "RM") // Base RM
                                {
                                    strTitleCurrency = currency + " Total";
                                    strTotalCurrency = ToteRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency,
                                                                           strConditionCurrency);
                                }
                                else
                                {
                                    strTitleCurrency = currency;
                                    strTotalCurrency = ToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency,
                                                                            strConditionCurrency);
                                }

                                rows.Append(strTotalCurrency);
                            }
                            rows.Replace("{rowspan}", (m_index).ToString());
                            rows.Append(trBlank);

                            #endregion

                            #region Sub Total Currency by USD

                            if (last_cid_usd == cid && strCurrencySubTotal_USD.Contains("[" + cid + "]") && !IsTotalUSD)
                            {
                                strTitleCurrency = "US$";
                                strTotalCurrency = ToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
                                                                        strConditionSubTotalGroupCurrencyUSD);
                                rows.Append(strTotalCurrency);
                                rows.Append(trBlank);

                                IsTotalUSD = true;
                            }

                            #endregion

                            #region Sub Total Currency by THB

                            if (last_cid_thb == cid && strCurrencySubTotal_THB.Contains("[" + cid + "]") && !IsTotalTHB)
                            {
                                strTitleCurrency = "TB Group";
                                strTotalCurrency = ToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
                                                                        strConditionSubTotalGroupCurrencyTHB);
                                rows.Append(strTotalCurrency);
                                rows.Append(trBlank);

                                IsTotalTHB = true;
                            }

                            #endregion

                            #region Sub Total Currency by IN

                            if (last_cid_in == cid && strCurrencySubTotal_IN.Contains("[" + cid + "]") && !IsTotalIN)
                            {
                                strTitleCurrency = "IN Group";
                                strTotalCurrency = ToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
                                                                        strConditionSubTotalGroupCurrencyIN);
                                rows.Append(strTotalCurrency);
                                rows.Append(trBlank);

                                IsTotalIN = true;
                            }

                            #endregion

                            c_index = 0;
                            meetingid = 0;
                        }
                        c_index++;
                        currency = currency_p;
                        cid = cid_p;
                        m_index = 0;
                    }

                    #endregion

                    #region Each Meeting

                    if (meetingid != meetingid_p)
                    {
                        if (m_index == 0)
                        {
                            meeting_currency = CacheRef.GetLeagueName(meetingid_p) + " - " + currency_p;
                        }
                        else
                        {
                            report_row = (report_row == "bgodd") ? "bgeven" : "bgodd";
                            rows.Replace("{rowspan}", (m_index).ToString());
                            condition_Meeting = "leagueid = '" + meetingid + "' and currencyname='" + currency + "'";
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid) + " - " + currency + " Total";
                            strTotalMeeting = ToteRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting, condition_Meeting);
                            rows.Append(strTotalMeeting);


                            rows.Append(trBlank);

                            meeting_currency = CacheRef.GetLeagueName(meetingid_p) + " - " + currency_p;
                        }
                        m_index = 0;
                        meetingid = meetingid_p;
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref row, "Blank");
                        TemplateHandler.RemoveBlock(ref row, "Blank2");
                    }


                    m_index++;

                    #endregion

                    #region Assign Each Row

                    TemplateHandler.Assign(ref row, "bgcolor", report_row);

                    int CurrencyID = CastAide.AsInt32(dr["Currency"]);
                    string CurrencyName = CastAide.AsString(dr["CurrencyName"]);
                    raceNo = CastAide.AsString(dr["groupmatch"]);
                    //leagueid|raceNo|currencyID|currencyName|winlostdate|db|WL
                    string evt = "ViewBetList('" + meetingid_p + "','" + raceNo + "','" + CurrencyID + "','" +
                                 CurrencyName + "','" + wldate.ToShortDateString() + "','" + db + "','" + WL + "');";
                    string strRace = "<div class='hand' style='color:blue;' onclick=\"" + evt + "\">" + dr["groupmatch"] +
                                     "</div>";

                    TemplateHandler.Assign(ref row, "Meeting_Currency", meeting_currency);
                    TemplateHandler.Assign(ref row, "RaceName", strRace);
                    TemplateHandler.Assign(ref row, "AgWinTO", NumberFormat.DecFormat(dr["winTO_Agency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "AgPlaceTO", NumberFormat.DecFormat(dr["placeTO_Agency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "AgWinPlaceTO",
                                           NumberFormat.DecFormat(dr["winplaceTO_Agency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "AgTotalTO", NumberFormat.DecFormat(dr["totalTOAgency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "AgCommInc",
                                           NumberFormat.DecFormat(dr["commIncomeAgency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "AgWL", NumberFormat.DecFormat(dr["PTWLAgency_Origin"], 2));

                    TemplateHandler.Assign(ref row, "IBCWinTO", NumberFormat.DecFormat(dr["winTO_IBC_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCPlaceTO", NumberFormat.DecFormat(dr["placeTO_IBC_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCWinPlaceTO",
                                           NumberFormat.DecFormat(dr["winplaceTO_IBC_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCTotalTO", NumberFormat.DecFormat(dr["totalTOIBC_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCCommInc", NumberFormat.DecFormat(dr["commIncomeIBC_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCCommExp",
                                           NumberFormat.DecFormat(dr["commIncomeAgency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCPGIAmoteFee", NumberFormat.DecFormat(dr["FeeDue2_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCPTWL", NumberFormat.DecFormat(dr["PTWLIBC_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCNetInc", NumberFormat.DecFormat(dr["netIncomeIBC_Origin"], 2));

                    TemplateHandler.Assign(ref row, "PhuWinTO", NumberFormat.DecFormat(dr["winTOPhum_Origin"], 2));
                    TemplateHandler.Assign(ref row, "PhuPlaceTO", NumberFormat.DecFormat(dr["placeTOPhum_Origin"], 2));
                    TemplateHandler.Assign(ref row, "PhuWinPlaceTO",
                                           NumberFormat.DecFormat(dr["winplaceTOPhum_Origin"], 2));
                    TemplateHandler.Assign(ref row, "PhuTotalTO", NumberFormat.DecFormat(dr["totalTOPhum_Origin"], 2));
                    TemplateHandler.Assign(ref row, "PhuDividendWon",
                                           NumberFormat.DecFormat(dr["dividendWon_Origin"], 2));
                    TemplateHandler.Assign(ref row, "PhuCommExp", NumberFormat.DecFormat(dr["commIncomeIBC_Origin"], 2));
                    TemplateHandler.Assign(ref row, "PhuPGIAmoteFee", NumberFormat.DecFormat(dr["FeeDue2_Origin"], 2));
                    TemplateHandler.Assign(ref row, "PhuAmountDue", NumberFormat.DecFormat(dr["AmountDue_Origin"], 2));

                    row = TemplateHandler.RemoveComment(row);

                    rows.Append(row);

                    #endregion
                }
                rows.Replace("{rowspan}", m_index.ToString());

                #endregion
            }

            #region Total

            #region Total Meeting

            condition_Meeting = "leagueid = '" + meetingid + "' and currencyname='" + currency + "'";
            strTitleMeeting = CacheRef.GetLeagueName(meetingid);
            if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
            else strTitleMeeting += currency + " Total";
            strTotalMeeting = ToteRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting, condition_Meeting);
            strTotalMeeting = TemplateHandler.RemoveComment(strTotalMeeting);
            rows.Append(strTotalMeeting);

            #endregion

            #region Total Currency

            strConditionCurrency = "currencyname = '" + currency + "'";
            {
                if (currency == "RM") // Base RM
                {
                    strTitleCurrency = currency + " Total";
                    strTotalCurrency = ToteRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency, strConditionCurrency);
                }
                else
                {
                    strTitleCurrency = currency;
                    strTotalCurrency = ToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency, strConditionCurrency);
                }

                rows.Append(strTotalCurrency);
                rows.Append(trBlank);
            }

            #endregion

            #region Sub Total Currency by USD

            if (!IsTotalUSD && last_cid_usd != 101)
            {
                strTitleCurrency = "US$ Group";
                strTotalCurrency = ToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
                                                        strConditionSubTotalGroupCurrencyUSD);
                rows.Append(strTotalCurrency);
                rows.Append(trBlank);

                IsTotalUSD = true;
            }

            #endregion

            #region Sub Total Currency by THB

            if (!IsTotalTHB && last_cid_thb != 101)
            {
                strTitleCurrency = "TB Group";
                strTotalCurrency = ToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
                                                        strConditionSubTotalGroupCurrencyTHB);
                rows.Append(strTotalCurrency);
                rows.Append(trBlank);

                IsTotalTHB = true;
            }

            #endregion

            #region Sub Total Currency by IN

            if (!IsTotalIN && last_cid_in != 101)
            {
                strTitleCurrency = "IN Group";
                strTotalCurrency = ToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
                                                        strConditionSubTotalGroupCurrencyIN);
                rows.Append(strTotalCurrency);
                rows.Append(trBlank);

                IsTotalIN = true;
            }

            #endregion

            #region Grand Total

            string strGrandTotal = "";

            strGrandTotal = ToteRowGrandTotalBaseRM(dt, "#8ac9db", "");

            rows.Append(strGrandTotal);

            #endregion

            #endregion

            TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());
            html.Append(tpl);
        }
        catch (Exception ex)
        {
            
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            html.Append(ex.Message);
        }
        return html.ToString();
    }

    #endregion

    #region ToteRowTotalMeeting

    private static string ToteRowTotalMeeting(DataTable dt, string TitleRow, string bgcolor, string condition)
    {
        #region GrandTotal

        string strGrandTotal = "<tr bgcolor='" + bgcolor + "' style='font-weight:bold;'>";
        strGrandTotal += "<td nowrap colspan='3' class='c'><b>" + TitleRow + "</b></td>";

        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTO_Agency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTO_Agency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_Agency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTO_IBC_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTO_IBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_IBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOIBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLIBC_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(netIncomeIBC_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOPhum_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOPhum_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOPhum_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOPhum_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(dividendWon_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(AmountDue_Origin)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td nowrap colspan='3' class='c'><b>" + TitleRow + "</b></td>";

        strGrandTotal += "</tr>";

        #endregion

        return strGrandTotal;
    }

    #endregion

    #region ToteRowTotalCurrency

    private static string ToteRowTotalCurrency(DataTable dt, string CurrencyName, string bgcolor, string condition)
    {
        #region GrandTotal

        string strGrandTotal = "";

        #region Total Currency

        strGrandTotal = "<tr  bgcolor='" + bgcolor + "' style='font-weight:bold;'>";
        strGrandTotal += "<td nowrap rowspan='2' colspan='2' class='c'><b>" + CurrencyName + " Total</b></td>";
        strGrandTotal += "<td class='c'><b>" + CurrencyName + "</b></td>";

        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTO_Agency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTO_Agency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_Agency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTO_IBC_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTO_IBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_IBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOIBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLIBC_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(netIncomeIBC_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOPhum_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOPhum_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOPhum_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOPhum_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(dividendWon_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(AmountDue_Origin)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='c'><b>" + CurrencyName + "</b></td>";
        strGrandTotal += "<td nowrap rowspan='2' colspan='2' class='c rborder'><b>" + CurrencyName + " Total</b></td>";

        strGrandTotal += "</tr>";

        #endregion

        #region Total Base (RM)

        strGrandTotal += "<tr bgcolor='" + bgcolor + "' style='font-weight:bold;'>";
        strGrandTotal += "<td class='c'><b>Base (RM)</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTO_Agency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTO_Agency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_Agency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTO_IBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTO_IBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_IBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(totalTOIBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLIBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(netIncomeIBC)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOPhum)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOPhum)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winplaceTOPhum)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(totalTOPhum)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(dividendWon)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(AmountDue)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='c'><b>Base (RM)</b></td>";

        strGrandTotal += "</tr>";

        #endregion

        #endregion

        return strGrandTotal;
    }

    #endregion

    #region ToteRowGrandTotalBaseRM

    private static string ToteRowGrandTotalBaseRM(DataTable dt, string bgcolor, string condition)
    {
        string strGrandTotal = "";

        #region Grand Total Base (RM)

        if (HttpContext.Current.Request["IsExport"] == null)
            strGrandTotal += "<tfoot class='bg'>";
        strGrandTotal += "<tr id='trFooter'  style='font-weight:bold;'>";
        strGrandTotal += "<td class='total' colspan='2' style=\"text-align:center;\"><b>Grand Total:</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:center;\"><b>Base (RM)</b></td>";


        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTO_Agency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTO_Agency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_Agency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTO_IBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTO_IBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_IBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOIBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(FeeDue2)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(PTWLIBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(netIncomeIBC)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTOPhum)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOPhum)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOPhum)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOPhum)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(dividendWon)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(FeeDue2)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total' style=\"text-align:right;\"><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(AmountDue)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='total' style=\"text-align:center;\"><b>Base (RM)</b></td>";
        strGrandTotal += "<td class='total' colspan='2' style=\"text-align:center;\"><b>Grand Total:</b></td>";

        strGrandTotal += "</tr>";
        if (HttpContext.Current.Request["IsExport"] == null)
            strGrandTotal += "</tfoot>";

        #endregion

        return strGrandTotal;
    }

    #endregion

    #endregion
}