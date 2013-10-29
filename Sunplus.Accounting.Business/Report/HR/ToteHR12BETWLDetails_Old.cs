using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for ToteHR12BETWLDetails
/// </summary>
public partial class ReportHTML
{
    #region Tote Odds HR 12BET WL Report HTML

    #region TOddsHR12BETWLReport

    public static string TOddsHR12BETWLReport__Old(HttpContext context, ref string strErrMsg)
    {
        DateTime wldate = CastAide.AsDateTime(context.Request["wldate"], DateTime.Today);
        string bettype = context.Request["bettype"];
        var html = new StringBuilder();
        int TotalRowsWrongSetting = 0;
        try
        {
            string tpl = WebUtils.LoadTemplate("WinLost/ToteHR12BETWLDetail.htm");
            if (context.Request["IsExport"] == "1")
                TemplateHandler.Assign(ref tpl, "border", "1");
            else
                TemplateHandler.Assign(ref tpl, "border", "0");
            if (context.Request["IsExport"] == "1")
            {
                TemplateHandler.RemoveBlock(ref tpl, "header");
                string title = "<tr bgcolor='#ffabab'><td colspan='16' align='center'><b>Horse Racing Daily Report: " +
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
                rs = MatchWinLostReport.WinlostHR12BET_Details_14(wldate);
            else
                rs = MatchWinLostReport.WinlostHR12BET_Details(wldate, db);

            DataTable dtWL = rs.ResultSet.Tables[0];

            dtWL.Columns.Add("Order", typeof(int));
            int cid_p = 0;
            foreach (DataRow dr in dtWL.Rows)
            {
                cid_p = CastAide.AsInt32(dr["currency"]);
                if (strArrCurrencyOrder.Contains("[" + cid_p + "]"))
                {
                    dr["Order"] = strArrCurrencyOrder.IndexOf("[" + cid_p + "]");
                }
                else
                {
                    dr["Order"] = 100;
                }
            }

            DataView dv = dtWL.DefaultView;
            dv.Sort = "Order asc, currencyname asc, leagueid asc ";
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
            string strConditionCurrency = "", strTitleCurrency = "", strTotalCurrency = "", bgcolorCurrency = "#d3e1c8";
            string condition_Meeting = "", strTitleMeeting = "", strTotalMeeting = "", bgcolorMeeting = "#ffeedd";
            // #d0e3ca
            string trBlank = "<tr bgcolor='#ffffff'><td style='' colspan='16' class='l fontbold'>&nbsp;</td></tr>";
            string meeting_currency = "";

            var CurrencyIDs = new ArrayList();
            var CurrencyNames = new ArrayList();

            #endregion Declare Variable

            // for testing the loop
            meetingid_p = 0;
            meetingid = 0;
            c_index = 0;
            m_index = 0;
            currency_p = "";
            currency = "";
            //for (int i = 0; i < 30; i++)
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
                            strTotalMeeting = TOddsHR12BETRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting,
                                                                          condition_Meeting, ref TotalRowsWrongSetting);
                            rows.Append(strTotalMeeting);

                            #endregion Total Meeting

                            #region Total Currency

                            strConditionCurrency = "currencyname = '" + currency + "'";

                            if (currency == "RM") // Base RM
                            {
                                strTitleCurrency = currency + " Total";
                                strTotalCurrency = TOddsHR12BETRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency,
                                                                               strConditionCurrency,
                                                                               ref TotalRowsWrongSetting);
                            }
                            else
                            {
                                strTitleCurrency = currency;
                                strTotalCurrency = TOddsHR12BETRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency,
                                                                                strConditionCurrency,
                                                                                ref TotalRowsWrongSetting);
                            }

                            rows.Append(strTotalCurrency);
                            rows.Replace("{rowspan}", (m_index).ToString());

                            rows.Append(trBlank);

                            #endregion Total Currency

                            c_index = 0;
                            meetingid = 0;
                        }
                        c_index++;
                        currency = currency_p;
                        m_index = 0;
                    }

                    #endregion Each Currency

                    #region Each Meeting

                    if (meetingid != meetingid_p)
                    {
                        if (m_index == 0)
                        {
                            meeting_currency = CacheRef.GetLeagueName(meetingid_p);
                            if (meeting_currency.Length > 0) meeting_currency += " - " + currency_p;
                            else meeting_currency += currency_p;
                        }
                        else
                        {
                            rows.Replace("{rowspan}", (m_index).ToString());
                            condition_Meeting = "leagueid = '" + meetingid + "' and currencyname='" + currency + "'";
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid);
                            if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
                            else strTitleMeeting += " - " + currency + " Total";
                            strTotalMeeting = TOddsHR12BETRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting,
                                                                          condition_Meeting, ref TotalRowsWrongSetting);
                            rows.Append(strTotalMeeting);

                            rows.Append(trBlank);

                            meeting_currency = CacheRef.GetLeagueName(meetingid_p);
                            if (meeting_currency.Length > 0) meeting_currency += " - " + currency_p;
                            else meeting_currency += currency_p;
                        }
                        m_index = 0;
                        meetingid = meetingid_p;
                        report_row = (report_row == "bgodd") ? "bgeven" : "bgodd";
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref row, "Blank");
                    }

                    m_index++;

                    #endregion Each Meeting

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

                    TemplateHandler.Assign(ref row, "MemWinTO", NumberFormat.DecFormat(dr["winTOMem_Origin"], 2));
                    TemplateHandler.Assign(ref row, "MemPlaceTO", NumberFormat.DecFormat(dr["placeTOMem_Origin"], 2));
                    TemplateHandler.Assign(ref row, "MemWinPlaceTO",
                                           NumberFormat.DecFormat(dr["winplaceTOMem_Origin"], 2));
                    TemplateHandler.Assign(ref row, "MemTotalTO", NumberFormat.DecFormat(dr["totalTOMem_Origin"], 2));
                    TemplateHandler.Assign(ref row, "MemDividendWon",
                                           NumberFormat.DecFormat(dr["DividendWonMem_Origin"], 2));

                    TemplateHandler.Assign(ref row, "Ag12BETWinTO", NumberFormat.DecFormat(dr["winTOAgency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "Ag12BETPlaceTO",
                                           NumberFormat.DecFormat(dr["placeTOAgency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "Ag12BETWinPlaceTO",
                                           NumberFormat.DecFormat(dr["winplaceTOAgency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "Ag12BETTotalTO",
                                           NumberFormat.DecFormat(dr["totalTOAgency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "Ag12BETPTWL", NumberFormat.DecFormat(dr["PTWLAgency_Origin"], 2));
                    TemplateHandler.Assign(ref row, "Ag12BETCommInc",
                                           NumberFormat.DecFormat(dr["commIncomeAgency_Origin"], 2));

                    TemplateHandler.Assign(ref row, "AmountDueIBC",
                                           NumberFormat.DecFormat(dr["AmountDue2IBC_Origin"], 2));

                    if (CastAide.AsDecimal(dr["totalTOAgency_Origin"], 0) * 2 !=
                        CastAide.AsDecimal(dr["totalTOMem_Origin"], 0))
                    {
                        TotalRowsWrongSetting++;
                        TemplateHandler.Assign(ref row, "wrongsetting", "ws");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref row, "wrongsetting", "");
                    }

                    row = TemplateHandler.RemoveComment(row);

                    rows.Append(row);

                    #endregion Assign Each Row
                }
                rows.Replace("{rowspan}", m_index.ToString());

                #endregion foreach
            }

            #region Total

            #region Total Meeting

            condition_Meeting = "leagueid = '" + meetingid + "' and currencyname='" + currency + "'";
            strTitleMeeting = CacheRef.GetLeagueName(meetingid);
            if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
            else strTitleMeeting += currency + " Total";
            strTotalMeeting = TOddsHR12BETRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting, condition_Meeting,
                                                          ref TotalRowsWrongSetting);
            strTotalMeeting = TemplateHandler.RemoveComment(strTotalMeeting);
            rows.Append(strTotalMeeting);

            #endregion Total Meeting

            #region Total Currency

            strConditionCurrency = "currencyname = '" + currency + "'";

            if (currency == "RM") // Base RM
            {
                strTitleCurrency = currency + " Total";
                strTotalCurrency = TOddsHR12BETRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency,
                                                               strConditionCurrency, ref TotalRowsWrongSetting);
            }
            else
            {
                strTitleCurrency = currency;
                strTotalCurrency = TOddsHR12BETRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency,
                                                                strConditionCurrency, ref TotalRowsWrongSetting);
            }

            rows.Append(strTotalCurrency);

            #endregion Total Currency

            #region Grand Total

            string strGrandTotal = "";
            rows.Append(trBlank);
            strGrandTotal = TOddsHR12BETRowGrandTotalBaseRM(dt, "#8ac9db", "", ref TotalRowsWrongSetting);
            rows.Append(strGrandTotal);

            #endregion Grand Total

            #endregion Total

            TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());
            html.Append(tpl);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            html.Append(ex.Message);
        }
        if (TotalRowsWrongSetting > 1)
            //strErrMsg = "Existed " + TotalRowsWrongSetting + " rows that are wrong setting PT. Please inform to IT to correct them.";
            strErrMsg = "There are " + TotalRowsWrongSetting +
                        " rows reflect wrong PT setting of 12BET agency. Please inform IT to correct them";
        else if (TotalRowsWrongSetting == 1)
            //strErrMsg = "Existed " + TotalRowsWrongSetting + " row that is wrong setting PT. Please inform to IT to correct it.";
            strErrMsg = "There is " + TotalRowsWrongSetting +
                        " row reflect wrong PT setting of 12BET agency. Please inform IT to correct it";
        else
            strErrMsg = "";
        return html.ToString();
    }

    #endregion TOddsHR12BETWLReport

    #region TOddsHR12BETRowTotalMeeting

    private static string TOddsHR12BETRowTotalMeeting_Old(DataTable dt, string TitleRow, string bgcolor, string condition,
                                                      ref int TotalWrongSetting)
    {
        #region GrandTotal

        string strGrandTotal = "<tr bgcolor='" + bgcolor + "' style='font-weight:bold;'>";
        strGrandTotal += "<td colspan='3' nowrap class='c'><b>" + TitleRow + "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOMem_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOMem_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOMem_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r {wrongsetting}'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOMem_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(DividendWonMem_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r {wrongsetting}'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r rspace rborder'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(AmountDue2IBC_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "</tr>";

        if (CastAide.AsDecimal(dt.Compute("sum(totalTOMem_Origin)", condition), 0) !=
            2 * CastAide.AsDecimal(dt.Compute("sum(totalTOAgency_Origin)", condition), 0))
        {
            TotalWrongSetting++;
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "ws");
        }
        else
        {
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "");
        }

        #endregion GrandTotal

        return strGrandTotal;
    }

    #endregion TOddsHR12BETRowTotalMeeting

    #region TOddsHR12BETRowTotalCurrency

    private static string TOddsHR12BETRowTotalCurrency_Old(DataTable dt, string CurrencyName, string bgcolor,
                                                       string condition, ref int TotalWrongSetting)
    {
        #region GrandTotal

        string strGrandTotal = "";

        #region Total Currency

        strGrandTotal = "<tr  bgcolor='" + bgcolor + "' style='font-weight:bold;'>";
        strGrandTotal += "<td nowrap style='vertical-align:middle;' rowspan='2' colspan='2' class='c'><b>" +
                         CurrencyName + " Total</b></td>";
        strGrandTotal += "<td class='c'><b>" + CurrencyName + "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOMem_Origin)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOMem_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOMem_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r {wrongsetting}'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOMem_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(DividendWonMem_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r {wrongsetting}'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency_Origin)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r rspace'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r rspace rborder'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(AmountDue2IBC_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "</tr>";

        if (CastAide.AsDecimal(dt.Compute("sum(totalTOMem_Origin)", condition), 0) !=
            2 * CastAide.AsDecimal(dt.Compute("sum(totalTOAgency_Origin)", condition), 0))
        {
            TotalWrongSetting++;
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "ws");
        }
        else
        {
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "");
        }

        #endregion Total Currency

        #region Total Base (RM)

        strGrandTotal += "<tr bgcolor='" + bgcolor + "' style='font-weight:bold;'>";
        strGrandTotal += "<td class='c'><b>Base (RM)</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOMem)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOMem)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winplaceTOMem)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r {wrongsetting}'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOMem)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(DividendWonMem)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winplaceTOAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r {wrongsetting}'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r'><b>" + NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) +
                         "</b></td>";

        strGrandTotal += "<td class='r rspace rborder'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(AmountDue2IBC)", condition), 2) + "</b></td>";

        strGrandTotal += "</tr>";

        if (CastAide.AsDecimal(dt.Compute("sum(totalTOMem)", condition), 0) !=
            2 * CastAide.AsDecimal(dt.Compute("sum(totalTOAgency)", condition), 0))
        {
            TotalWrongSetting++;
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "ws");
        }
        else
        {
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "");
        }

        #endregion Total Base (RM)

        #endregion GrandTotal

        return strGrandTotal;
    }

    #endregion TOddsHR12BETRowTotalCurrency

    #region TOddsHR12BETRowGrandTotalBaseRM

    private static string TOddsHR12BETRowGrandTotalBaseRM_Old(DataTable dt, string bgcolor, string condition,
                                                          ref int TotalWrongSetting)
    {
        string strGrandTotal = "";

        #region Grand Total Base (RM)

        if (HttpContext.Current.Request["IsExport"] == null)
            strGrandTotal += "<tfoot class='bg'>";
        strGrandTotal += "<tr id='trFooter'  style='font-weight:bold;'>";
        strGrandTotal += "<td colspan='2' class='c total'><b>Grand Total:</b></td>";
        strGrandTotal += "<td class='total'><center><b>Base (RM)</b></center></td>";

        strGrandTotal += "<td class='total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOMem)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOMem)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOMem)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='{wrongsetting} total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOMem)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(DividendWonMem)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='{wrongsetting} total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='rspace rborder total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(AmountDue2IBC)", condition), 2) + "</b></td>";

        strGrandTotal += "</tr>";
        if (HttpContext.Current.Request["IsExport"] == null)
            strGrandTotal += "</tfoot>";

        if (CastAide.AsDecimal(dt.Compute("sum(totalTOMem)", condition), 0) !=
            2 * CastAide.AsDecimal(dt.Compute("sum(totalTOAgency)", condition), 0))
        {
            TotalWrongSetting++;
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "ws");
        }
        else
        {
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "");
        }

        #endregion Grand Total Base (RM)

        return strGrandTotal;
    }

    #endregion TOddsHR12BETRowGrandTotalBaseRM

    #region TOddsHR12BETWLReport

    public static string TOddsHR12BETWLReport_Old(HttpContext context, ref string strErrMsg)
    {
        DateTime wldate = CastAide.AsDateTime(context.Request["wldate"], DateTime.Today);
        string bettype = context.Request["bettype"];
        var html = new StringBuilder();
        int TotalRowsWrongSetting = 0;
        try
        {
            string tpl = WebUtils.LoadTemplate("WinLost/ToteHR12BETWLDetail_Old.htm");
            if (context.Request["IsExport"] == "1")
                TemplateHandler.Assign(ref tpl, "border", "1");
            else
                TemplateHandler.Assign(ref tpl, "border", "0");
            if (context.Request["IsExport"] == "1")
            {
                TemplateHandler.RemoveBlock(ref tpl, "header");
                string title = "<tr bgcolor='#ffabab'><td colspan='16' align='center'><b>Horse Racing Daily Report: " +
                               wldate.ToShortDateString() + " </b></td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "title", title);
            }

            TemplateHandler.Assign(ref tpl, "bettype", bettype);
            TemplateHandler.Assign(ref tpl, "wldate", wldate.ToShortDateString());

            string db = context.Request["db"];
            string WL = context.Request["WL"];

            db = (string.IsNullOrEmpty(db)) ? "rep" : db;

            ToteHR12BETWLDetailsReport_Old dt = ToteHR12BETWLDetailsReport_Old.GetReport(db, wldate);
            DataTable Table = dt.Table;

            #region Declare Variable

            var rows = new StringBuilder();
            string tplRow = TemplateHandler.CopyBlock(tpl, "content");
            int meetingid = 0, meetingid_p = 0;

            string report_row = "bgodd";
            int c_index = 0, m_index = 0;
            string raceNo = "";
            string currency = "", currency_p = "";
            string row = "";
            string strConditionCurrency = "", strTitleCurrency = "", strTotalCurrency = "", bgcolorCurrency = "#d3e1c8";
            string condition_Meeting = "", strTitleMeeting = "", strTotalMeeting = "", bgcolorMeeting = "#ffeedd";
            // #d0e3ca
            string trBlank = "<tr bgcolor='#ffffff'><td style='' colspan='16' class='l fontbold'>&nbsp;</td></tr>";
            string meeting_currency = "";

            #endregion Declare Variable

            // for testing the loop
            meetingid_p = 0;
            meetingid = 0;
            c_index = 0;
            m_index = 0;
            currency_p = "";
            currency = "";
            //for (int i = 0; i < 30; i++)
            {
                #region foreach

                foreach (ToteHR12BETWLDetailsRow_Old dr in dt.Rows)
                {
                    /*if (!CurrencyIDs.Contains(dr.Currency))
                    {
                        CurrencyIDs.Add(dr.Currency);
                        CurrencyNames.Add(dr.CurrencyName);
                    }*/

                    row = tplRow;
                    meetingid_p = dr.Leagueid;
                    currency_p = dr.CurrencyName;

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
                            //strTotalMeeting = TOddsHR12BETRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting, condition_Meeting, ref TotalRowsWrongSetting);
                            rows.Append(strTotalMeeting);

                            #endregion Total Meeting

                            #region Total Currency

                            strConditionCurrency = "currencyname = '" + currency + "'";

                            if (currency == "RM") // Base RM
                            {
                                strTitleCurrency = currency + " Total";
                                strTotalCurrency = TOddsHR12BETRowTotalMeeting_Old(Table, strTitleCurrency, bgcolorCurrency,
                                                                               strConditionCurrency,
                                                                               ref TotalRowsWrongSetting);
                            }
                            else
                            {
                                strTitleCurrency = currency;
                                strTotalCurrency = TOddsHR12BETRowTotalCurrency_Old(Table, strTitleCurrency, bgcolorCurrency,
                                                                                strConditionCurrency,
                                                                                ref TotalRowsWrongSetting);
                            }

                            rows.Append(strTotalCurrency);
                            rows.Replace("{rowspan}", (m_index).ToString());

                            rows.Append(trBlank);

                            #endregion Total Currency

                            c_index = 0;
                            meetingid = 0;
                        }
                        c_index++;
                        currency = currency_p;
                        m_index = 0;
                    }

                    #endregion Each Currency

                    #region Each Meeting

                    if (meetingid != meetingid_p)
                    {
                        if (m_index == 0)
                        {
                            meeting_currency = CacheRef.GetLeagueName(meetingid_p);
                            if (meeting_currency.Length > 0) meeting_currency += " - " + currency_p;
                            else meeting_currency += currency_p;
                        }
                        else
                        {
                            rows.Replace("{rowspan}", (m_index).ToString());
                            condition_Meeting = "leagueid = '" + meetingid + "' and currencyname='" + currency + "'";
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid);
                            if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
                            else strTitleMeeting += " - " + currency + " Total";
                            strTotalMeeting = TOddsHR12BETRowTotalMeeting_Old(Table, strTitleMeeting, bgcolorMeeting,
                                                                          condition_Meeting, ref TotalRowsWrongSetting);
                            rows.Append(strTotalMeeting);

                            rows.Append(trBlank);

                            meeting_currency = CacheRef.GetLeagueName(meetingid_p);
                            if (meeting_currency.Length > 0) meeting_currency += " - " + currency_p;
                            else meeting_currency += currency_p;
                        }
                        m_index = 0;
                        meetingid = meetingid_p;
                        report_row = (report_row == "bgodd") ? "bgeven" : "bgodd";
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref row, "Blank");
                    }

                    m_index++;

                    #endregion Each Meeting

                    #region Assign Each Row

                    TemplateHandler.Assign(ref row, "bgcolor", report_row);

                    int CurrencyID = dr.Currency;
                    string CurrencyName = dr.CurrencyName;
                    raceNo = dr.Groupmatch;
                    //leagueid|raceNo|currencyID|currencyName|winlostdate|db|WL
                    string evt = "ViewBetList('" + meetingid_p + "','" + raceNo + "','" + CurrencyID + "','" +
                                 CurrencyName + "','" + wldate.ToShortDateString() + "','" + db + "','" + WL + "');";
                    string strRace = "<div class='hand' style='color:blue;' onclick=\"" + evt + "\">" + dr.Groupmatch +
                                     "</div>";

                    TemplateHandler.Assign(ref row, "Meeting_Currency", meeting_currency);
                    TemplateHandler.Assign(ref row, "RaceName", strRace);

                    TemplateHandler.Assign(ref row, "MemWinTO", NumberFormat.DecFormat(dr.WinTOMem_Origin, 2));
                    TemplateHandler.Assign(ref row, "MemPlaceTO", NumberFormat.DecFormat(dr.PlaceTOMem_Origin, 2));
                    TemplateHandler.Assign(ref row, "MemWinPlaceTO", NumberFormat.DecFormat(dr.WinplaceTOMem_Origin, 2));
                    TemplateHandler.Assign(ref row, "MemTotalTO", NumberFormat.DecFormat(dr.TotalTOMem_Origin, 2));
                    TemplateHandler.Assign(ref row, "MemDividendWon",
                                           NumberFormat.DecFormat(dr.DividendWonMem_Origin, 2));

                    TemplateHandler.Assign(ref row, "Ag12BETWinTO", NumberFormat.DecFormat(dr.WinTOAgency_Origin, 2));
                    TemplateHandler.Assign(ref row, "Ag12BETPlaceTO", NumberFormat.DecFormat(dr.PlaceTOAgency_Origin, 2));
                    TemplateHandler.Assign(ref row, "Ag12BETWinPlaceTO",
                                           NumberFormat.DecFormat(dr.WinplaceTOAgency_Origin, 2));
                    TemplateHandler.Assign(ref row, "Ag12BETTotalTO", NumberFormat.DecFormat(dr.TotalTOAgency_Origin, 2));
                    TemplateHandler.Assign(ref row, "Ag12BETPTWL", NumberFormat.DecFormat(dr.PTWLAgency_Origin, 2));
                    TemplateHandler.Assign(ref row, "Ag12BETCommInc",
                                           NumberFormat.DecFormat(dr.CommIncomeAgency_Origin, 2));

                    TemplateHandler.Assign(ref row, "AmountDueIBC", NumberFormat.DecFormat(dr.AmountDue2IBC_Origin, 2));

                    if (CastAide.AsDecimal(dr.TotalTOAgency_Origin, 0) * 2 != CastAide.AsDecimal(dr.TotalTOMem_Origin, 0))
                    {
                        TotalRowsWrongSetting++;
                        TemplateHandler.Assign(ref row, "wrongsetting", "ws");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref row, "wrongsetting", "");
                    }

                    row = TemplateHandler.RemoveComment(row);

                    rows.Append(row);

                    #endregion Assign Each Row
                }
                rows.Replace("{rowspan}", m_index.ToString());

                #endregion foreach
            }

            #region Total

            #region Total Meeting

            condition_Meeting = "leagueid = '" + meetingid + "' and currencyname='" + currency + "'";
            strTitleMeeting = CacheRef.GetLeagueName(meetingid);
            if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
            else strTitleMeeting += currency + " Total";
            strTotalMeeting = TOddsHR12BETRowTotalMeeting_Old(Table, strTitleMeeting, bgcolorMeeting, condition_Meeting,
                                                          ref TotalRowsWrongSetting);
            strTotalMeeting = TemplateHandler.RemoveComment(strTotalMeeting);
            rows.Append(strTotalMeeting);

            #endregion Total Meeting

            #region Total Currency

            strConditionCurrency = "currencyname = '" + currency + "'";

            if (currency == "RM") // Base RM
            {
                strTitleCurrency = currency + " Total";
                strTotalCurrency = TOddsHR12BETRowTotalMeeting_Old(Table, strTitleCurrency, bgcolorCurrency,
                                                               strConditionCurrency, ref TotalRowsWrongSetting);
            }
            else
            {
                strTitleCurrency = currency;
                strTotalCurrency = TOddsHR12BETRowTotalCurrency_Old(Table, strTitleCurrency, bgcolorCurrency,
                                                                strConditionCurrency, ref TotalRowsWrongSetting);
            }

            rows.Append(strTotalCurrency);

            #endregion Total Currency

            #region Grand Total

            string strGrandTotal = "";
            rows.Append(trBlank);
            strGrandTotal = TOddsHR12BETRowGrandTotalBaseRM_Old(Table, "#8ac9db", "", ref TotalRowsWrongSetting);
            rows.Append(strGrandTotal);

            #endregion Grand Total

            #endregion Total

            TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());
            html.Append(tpl);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            html.Append(ex.Message);
        }
        if (TotalRowsWrongSetting > 1)
            //strErrMsg = "Existed " + TotalRowsWrongSetting + " rows that are wrong setting PT. Please inform to IT to correct them.";
            strErrMsg = "There are " + TotalRowsWrongSetting +
                        " rows reflect wrong PT setting of 12BET agency. Please inform IT to correct them";
        else if (TotalRowsWrongSetting == 1)
            //strErrMsg = "Existed " + TotalRowsWrongSetting + " row that is wrong setting PT. Please inform to IT to correct it.";
            strErrMsg = "There is " + TotalRowsWrongSetting +
                        " row reflect wrong PT setting of 12BET agency. Please inform IT to correct it";
        else
            strErrMsg = "";
        return html.ToString();
    }

    #endregion TOddsHR12BETWLReport

    #endregion Tote Odds HR 12BET WL Report HTML
}

/// <summary>
/// Class ToteHR12BETWLDetails contain properties get & set value
/// </summary>

#region ToteHR12BETWLDetails

public class ToteHR12BETWLDetailsRow_Old
{
    private string mCurrencyName = string.Empty;
    private string mGroupmatch = string.Empty;
    private decimal mPlaceTOAgency_Origin;
    private decimal mPlaceTOMem_Origin;

    public string CurrencyName
    {
        get { return mCurrencyName; }
        set { mCurrencyName = value; }
    }

    public int Currency { get; set; }

    public int Leagueid { get; set; }

    public string Groupmatch
    {
        get { return mGroupmatch; }
        set { mGroupmatch = value; }
    }

    public decimal PTWLAgency_Origin { get; set; }

    public decimal PTWLAgency { get; set; }

    public decimal WinTOMem_Origin { get; set; }

    public decimal WinTOMem { get; set; }

    public decimal PlaceTOMem_Origin
    {
        get { return mPlaceTOMem_Origin; }
        set { mPlaceTOMem_Origin = value; }
    }

    public decimal PlaceTOMem { get; set; }

    public decimal WinplaceTOMem_Origin { get; set; }

    public decimal WinplaceTOMem { get; set; }

    public decimal TotalTOMem_Origin { get; set; }

    public decimal TotalTOMem { get; set; }

    public decimal DividendWonMem_Origin { get; set; }

    public decimal DividendWonMem { get; set; }

    public decimal WinTOAgency_Origin { get; set; }

    public decimal WinTOAgency { get; set; }

    public decimal PlaceTOAgency_Origin
    {
        get { return mPlaceTOAgency_Origin; }
        set { mPlaceTOAgency_Origin = value; }
    }

    public decimal PlaceTOAgency { get; set; }

    public decimal WinplaceTOAgency_Origin { get; set; }

    public decimal WinplaceTOAgency { get; set; }

    public decimal TotalTOAgency_Origin { get; set; }

    public decimal TotalTOAgency { get; set; }

    public decimal CommIncomeAgency_Origin { get; set; }

    public decimal CommIncomeAgency { get; set; }

    public decimal AmountDue2IBC_Origin { get; set; }

    public decimal AmountDue2IBC { get; set; }
}

#endregion ToteHR12BETWLDetails

public class ToteHR12BETWLDetailsReport_Old
{
    public ToteHR12BETWLDetailsRow_Old[] Rows;
    public DataTable Table = new DataTable();

    public static ToteHR12BETWLDetailsReport_Old GetReport(string db, DateTime wldate)
    {
        var report = new ToteHR12BETWLDetailsReport_Old();

        db = (string.IsNullOrEmpty(db)) ? "rep" : db;

        DbResult rs = null;

        DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
        if (wldate <= movebetsday)
            rs = ReportHR.WinlostHR12BET_Details_14(wldate);
        else
            rs = ReportHR.WinlostHR12BET_Details(wldate, db);

        DataTable dtWL = rs.ResultSet.Tables[0];

        dtWL.Columns.Add("Order", typeof(int));
        int cid_p = 0;
        foreach (DataRow dr in dtWL.Rows)
        {
            cid_p = CastAide.AsInt32(dr["currency"]);
            if (ReportHTML.strArrCurrencyOrder.Contains("[" + cid_p + "]"))
            {
                dr["Order"] = ReportHTML.strArrCurrencyOrder.IndexOf("[" + cid_p + "]");
            }
            else
            {
                dr["Order"] = 100;
            }
        }

        DataView dv = dtWL.DefaultView;
        dv.Sort = "Order asc, currencyname asc, leagueid asc ";
        var dt = new DataTable();
        dt = dv.ToTable();
        report.Table = dt;

        var reportRow = new List<ToteHR12BETWLDetailsRow_Old>();

        foreach (DataRow dr in dt.Select())
        {
            var row = new ToteHR12BETWLDetailsRow_Old();

            row.CurrencyName = CastAide.AsString(dr["CurrencyName"]);
            row.Currency = CastAide.AsInt32(dr["currency"]);
            row.Leagueid = CastAide.AsInt32(dr["leagueid"]);
            row.Groupmatch = CastAide.AsString(dr["groupmatch"]);

            row.PTWLAgency_Origin = CastAide.AsDecimal(dr["CurrencyName"]);
            row.PTWLAgency = CastAide.AsDecimal(dr["PTWLAgency"]);

            row.WinTOMem_Origin = CastAide.AsDecimal(dr["winTOMem_Origin"]);
            row.WinTOMem = CastAide.AsDecimal(dr["winTOMem"]);

            row.PlaceTOMem_Origin = CastAide.AsDecimal(dr["placeTOMem_Origin"]);
            row.PlaceTOMem = CastAide.AsDecimal(dr["placeTOMem"]);

            row.WinplaceTOMem_Origin = CastAide.AsDecimal(dr["winplaceTOMem_Origin"]);
            row.WinplaceTOMem = CastAide.AsDecimal(dr["winplaceTOMem"]);

            row.TotalTOMem_Origin = CastAide.AsDecimal(dr["totalTOMem_Origin"]);
            row.TotalTOMem = CastAide.AsDecimal(dr["totalTOMem"]);

            row.DividendWonMem_Origin = CastAide.AsDecimal(dr["DividendWonMem_Origin"]);
            row.DividendWonMem = CastAide.AsDecimal(dr["DividendWonMem"]);

            row.WinTOAgency_Origin = CastAide.AsDecimal(dr["winTOAgency_Origin"]);
            row.WinTOAgency = CastAide.AsDecimal(dr["winTOAgency"]);

            row.PlaceTOAgency_Origin = CastAide.AsDecimal(dr["placeTOAgency_Origin"]);
            row.PlaceTOAgency = CastAide.AsDecimal(dr["placeTOAgency"]);

            row.WinplaceTOAgency = CastAide.AsDecimal(dr["winplaceTOAgency"]);
            row.WinplaceTOAgency_Origin = CastAide.AsDecimal(dr["winplaceTOAgency_Origin"]);

            row.TotalTOAgency = CastAide.AsDecimal(dr["totalTOAgency"]);
            row.TotalTOAgency_Origin = CastAide.AsDecimal(dr["TotalTOAgency_Origin"]);

            row.CommIncomeAgency_Origin = CastAide.AsDecimal(dr["commIncomeAgency_Origin"]);
            row.CommIncomeAgency = CastAide.AsDecimal(dr["commIncomeAgency"]);

            row.AmountDue2IBC = CastAide.AsDecimal(dr["AmountDue2IBC"]);
            row.AmountDue2IBC_Origin = CastAide.AsDecimal(dr["AmountDue2IBC_Origin"]);

            reportRow.Add(row);
        }

        report.Rows = reportRow.ToArray();

        return report;
    }
}