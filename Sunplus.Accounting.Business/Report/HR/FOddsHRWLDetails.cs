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
/// Summary description for FOddsHRWLDetails
/// </summary>
public partial class ReportHTML
{
    #region Fixed Odds HR WL Report HTML

    #region FOddsHRWLReport

    public static string FOddsHRWLReport(HttpContext context)
    {
        DateTime wldate = CastAide.AsDateTime(context.Request["wldate"], DateTime.Today);
        string bettype = context.Request["bettype"];
        var html = new StringBuilder();
        try
        {
            string tpl = WebUtils.LoadTemplate("WinLost/FOddsHRWLDetail.htm");
            if (context.Request["IsExport"] == "1")
                TemplateHandler.Assign(ref tpl, "border", "1");
            else
                TemplateHandler.Assign(ref tpl, "border", "0");
            TemplateHandler.RemoveBlock(ref tpl, "export");

            if (context.Request["IsExport"] == "1")
            {
                TemplateHandler.RemoveBlock(ref tpl, "header");
                string title =
                    "<tr bgcolor='#ffabab'><td colspan='28' align='center'><b>FOdds Horse Racing Daily Report: " +
                    wldate.ToShortDateString() + " </b></td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "title", title);
            }

            TemplateHandler.Assign(ref tpl, "bettype", bettype);
            TemplateHandler.Assign(ref tpl, "wldate", wldate.ToShortDateString());

            string db = context.Request["db"];
            string WL = context.Request["WL"];

            db = (string.IsNullOrEmpty(db)) ? "rep" : db;

            DbResult rs = null;

            DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
            if (wldate <= movebetsday)
                rs = MatchWinLostReport.WinlostFOddsHR_Details_14(wldate);
            else
                rs = MatchWinLostReport.WinlostFOddsHR_Details(wldate, db);

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
            string trBlank = "<tr bgcolor='#ffffff'><td colspan='27' class='l fontbold'></td></tr>";
                //#efefef            
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

            #endregion

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
                            strTotalMeeting = FOddsRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting,
                                                                   condition_Meeting);
                            rows.Append(strTotalMeeting);

                            #endregion

                            #region Total Currency

                            {
                                strConditionCurrency = "currencyname = '" + currency + "'";

                                if (currency == "RM") // Base RM
                                {
                                    strTitleCurrency = currency + " Total";
                                    strTotalCurrency = FOddsRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency,
                                                                            strConditionCurrency);
                                }
                                else
                                {
                                    strTitleCurrency = currency;
                                    strTotalCurrency = FOddsRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency,
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
                                strTotalCurrency = FOddsRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
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
                                strTotalCurrency = FOddsRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
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
                                strTotalCurrency = FOddsRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
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
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid);
                            if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
                            else strTitleMeeting += currency + " Total";
                            strTotalMeeting = FOddsRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting,
                                                                   condition_Meeting);
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
                    //TemplateHandler.Assign(ref row, "IBCPGIAmoteFee", NumberFormat.DecFormat(dr["FeeDue2_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCPTWL", NumberFormat.DecFormat(dr["PTWLIBC_Origin"], 2));
                    TemplateHandler.Assign(ref row, "IBCNetInc", NumberFormat.DecFormat(dr["netIncomeIBC_Origin"], 2));

                    TemplateHandler.Assign(ref row, "BWWinTO", NumberFormat.DecFormat(dr["winTOBW_origin"], 2));
                    TemplateHandler.Assign(ref row, "BWPlaceTO", NumberFormat.DecFormat(dr["placeTOBW_origin"], 2));
                    TemplateHandler.Assign(ref row, "BWWinPlaceTO", NumberFormat.DecFormat(dr["winplaceTOBW_origin"], 2));
                    TemplateHandler.Assign(ref row, "BWTotalTO", NumberFormat.DecFormat(dr["totalTOBW_Origin"], 2));
                    TemplateHandler.Assign(ref row, "BWDividendWon", NumberFormat.DecFormat(dr["PayoutBW_Origin"], 2));
                    TemplateHandler.Assign(ref row, "BWCommExp", NumberFormat.DecFormat(dr["commIncomeIBC_Origin"], 2));
                    //TemplateHandler.Assign(ref row, "PhuPGIAmoteFee", NumberFormat.DecFormat(dr["FeeDue2_Origin"], 2));
                    TemplateHandler.Assign(ref row, "BWAmountDue", NumberFormat.DecFormat(dr["AmountDue_Origin"], 2));

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
            strTotalMeeting = FOddsRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting, condition_Meeting);
            strTotalMeeting = TemplateHandler.RemoveComment(strTotalMeeting);
            rows.Append(strTotalMeeting);

            #endregion

            #region Total Currency

            strConditionCurrency = "currencyname = '" + currency + "'";
            {
                if (currency == "RM") // Base RM
                {
                    strTitleCurrency = currency + " Total";
                    strTotalCurrency = FOddsRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency, strConditionCurrency);
                }
                else
                {
                    strTitleCurrency = currency;
                    strTotalCurrency = FOddsRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency, strConditionCurrency);
                }

                rows.Append(strTotalCurrency);
                rows.Append(trBlank);
            }

            #endregion

            #region Sub Total Currency by USD

            if (!IsTotalUSD && last_cid_usd != 101)
            {
                strTitleCurrency = "US$ Group";
                strTotalCurrency = FOddsRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
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
                strTotalCurrency = FOddsRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
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
                strTotalCurrency = FOddsRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency_sub,
                                                         strConditionSubTotalGroupCurrencyIN);
                rows.Append(strTotalCurrency);
                rows.Append(trBlank);

                IsTotalIN = true;
            }

            #endregion

            #region Grand Total

            string strGrandTotal = "";

            strGrandTotal = FOddsRowGrandTotalBaseRM(dt, "#8ac9db", "");

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

    #region FOddsRowTotalMeeting

    private static string FOddsRowTotalMeeting(DataTable dt, string TitleRow, string bgcolor, string condition)
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
        //strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLIBC_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(netIncomeIBC_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOBW_origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOBW_origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOBW_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(totalTOBW_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PayoutBW_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC_Origin)", condition), 2) + "</b></td>";
        //strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(AmountDue_Origin)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td nowrap colspan='3' class='c'><b>" + TitleRow + "</b></td>";

        strGrandTotal += "</tr>";

        #endregion

        return strGrandTotal;
    }

    #endregion

    #region FOddsRowTotalCurrency

    private static string FOddsRowTotalCurrency(DataTable dt, string CurrencyName, string bgcolor, string condition)
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
        //strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLIBC_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(netIncomeIBC_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOBW_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOBW_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOBW_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(totalTOBW_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PayoutBW_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC_Origin)", condition), 2) + "</b></td>";
        //strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2_Origin)", condition), 2) + "</b></td>";
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
        //strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLIBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(netIncomeIBC)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winplaceTOBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(totalTOBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PayoutBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC)", condition), 2) +
                         "</b></td>";
        //strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(AmountDue)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='c'><b>Base (RM)</b></td>";

        strGrandTotal += "</tr>";

        #endregion

        #endregion

        return strGrandTotal;
    }

    #endregion

    #region FOddsRowGrandTotalBaseRM

    private static string FOddsRowGrandTotalBaseRM(DataTable dt, string bgcolor, string condition)
    {
        string strGrandTotal = "";

        #region Grand Total Base (RM)

        if (HttpContext.Current.Request["IsExport"] == null)
            strGrandTotal += "<tfoot class='bg'>";
        strGrandTotal += "<tr id='trFooter' style='font-weight:bold;'>";
        strGrandTotal += "<td class='c total' colspan='2'><b>Grand Total:</b></td>";
        strGrandTotal += "<td class='c total'><b>Base (RM)</b></td>";


        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTO_Agency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTO_Agency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_Agency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTO_IBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTO_IBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTO_IBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(totalTOIBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) + "</b></td>";
        //strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLIBC)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(netIncomeIBC)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOBW)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(totalTOBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PayoutBW)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeIBC)", condition), 2) + "</b></td>";
        //strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(FeeDue2)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(AmountDue)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='c total'><b>Base (RM)</b></td>";
        strGrandTotal += "<td class='c total' colspan='2'><b>Grand Total:</b></td>";

        strGrandTotal += "</tr>";
        if (HttpContext.Current.Request["IsExport"] == null)
            strGrandTotal += "</tfoot>";

        #endregion

        return strGrandTotal;
    }

    #endregion

    #endregion
}