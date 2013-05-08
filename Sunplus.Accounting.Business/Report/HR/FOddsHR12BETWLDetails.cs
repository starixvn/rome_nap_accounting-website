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
//using Sunplus.Core.LoggingLib;


/// <summary>
/// Summary description for FOddsHR12BETWLDetails
/// </summary>
public partial class ReportHTML
{
    #region Fixed Odds HR 12BET WL Report HTML

    #region FOddsHR12BETWLReport

    public static string FOddsHR12BETWLReport(HttpContext context, ref string strErrMsg)
    {
        DateTime wldate = CastAide.AsDateTime(context.Request["wldate"], DateTime.Today);
        string bettype = context.Request["bettype"];
        var html = new StringBuilder();
        int TotalRowsWrongSetting = 0;
        try
        {
            string tpl = WebUtils.LoadTemplate("WinLost/FOddsHR12BETWLDetail.htm");

            if (context.Request["IsExport"] == "1")
            {
                TemplateHandler.Assign(ref tpl, "border", "1");
                TemplateHandler.RemoveBlock(ref tpl, "header");
                string title = "<tr bgcolor='#ffabab'><td colspan='15' align='center'><b>Horse Racing Daily Report: " +
                               wldate.ToShortDateString() + " </b></td></tr>";
                TemplateHandler.ReplaceBlock(ref tpl, "title", title);
            }
            else
                TemplateHandler.Assign(ref tpl, "border", "0");

            TemplateHandler.Assign(ref tpl, "bettype", bettype);
            TemplateHandler.Assign(ref tpl, "wldate", wldate.ToShortDateString());

            string db = context.Request["db"];
            string WL = context.Request["WL"];

            db = (string.IsNullOrEmpty(db)) ? "rep" : db;

            DbResult rs = null;

            DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
            if (wldate <= movebetsday)
                rs = MatchWinLostReport.WinlostFOddsHR12BET_Details_14(wldate);
            else
                rs = MatchWinLostReport.WinlostFOddsHR12BET_Details(wldate, db);

            DataTable dtWL = rs.ResultSet.Tables[0];

            dtWL.Columns.Add("Order", typeof (int));
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
            string trBlank = "<tr bgcolor='#ffffff'><td style='' colspan='15' class='l fontbold'>&nbsp;</td></tr>";
            string meeting_currency = "";

            var CurrencyIDs = new ArrayList();
            var CurrencyNames = new ArrayList();

            #endregion

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
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid) + " - " + currency + " Total";
                            strTotalMeeting = FOddsHR12BETRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting,
                                                                          condition_Meeting, ref TotalRowsWrongSetting);
                            rows.Append(strTotalMeeting);

                            #endregion

                            #region Total Currency

                            strConditionCurrency = "currencyname = '" + currency + "'";

                            if (currency == "RM") // Base RM
                            {
                                strTitleCurrency = currency + " Total";
                                strTotalCurrency = FOddsHR12BETRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency,
                                                                               strConditionCurrency,
                                                                               ref TotalRowsWrongSetting);
                            }
                            else
                            {
                                strTitleCurrency = currency;
                                strTotalCurrency = FOddsHR12BETRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency,
                                                                                strConditionCurrency,
                                                                                ref TotalRowsWrongSetting);
                            }

                            rows.Append(strTotalCurrency);
                            rows.Replace("{rowspan}", (m_index).ToString());


                            rows.Append(trBlank);

                            #endregion

                            c_index = 0;
                            meetingid = 0;
                        }
                        c_index++;
                        currency = currency_p;
                        m_index = 0;
                    }

                    #endregion

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
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid) + " - " + currency + " Total";
                            strTotalMeeting = FOddsHR12BETRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting,
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

                    if (CastAide.AsDecimal(dr["totalTOAgency_Origin"], 0)*2 !=
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
            strTotalMeeting = FOddsHR12BETRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting, condition_Meeting,
                                                          ref TotalRowsWrongSetting);
            strTotalMeeting = TemplateHandler.RemoveComment(strTotalMeeting);
            rows.Append(strTotalMeeting);

            #endregion

            #region Total Currency

            strConditionCurrency = "currencyname = '" + currency + "'";


            if (currency == "RM") // Base RM
            {
                strTitleCurrency = currency + " Total";
                strTotalCurrency = FOddsHR12BETRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency,
                                                               strConditionCurrency, ref TotalRowsWrongSetting);
            }
            else
            {
                strTitleCurrency = currency;
                strTotalCurrency = FOddsHR12BETRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency,
                                                                strConditionCurrency, ref TotalRowsWrongSetting);
            }

            rows.Append(strTotalCurrency);

            #endregion

            #region Grand Total

            string strGrandTotal = "";
            rows.Append(trBlank);
            strGrandTotal = FOddsHR12BETRowGrandTotalBaseRM(dt, "#8ac9db", "", ref TotalRowsWrongSetting);
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
        if (TotalRowsWrongSetting > 1)
            strErrMsg = "There are " + TotalRowsWrongSetting +
                        " rows reflect wrong PT setting of 12BET agency. Please inform IT to correct them";
        else if (TotalRowsWrongSetting == 1)
            strErrMsg = "There is " + TotalRowsWrongSetting +
                        " row reflect wrong PT setting of 12BET agency. Please inform IT to correct it";
        else
            strErrMsg = "";
        return html.ToString();
    }

    #endregion

    #region FOddsHR12BETRowTotalMeeting

    private static string FOddsHR12BETRowTotalMeeting(DataTable dt, string TitleRow, string bgcolor, string condition,
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
            2*CastAide.AsDecimal(dt.Compute("sum(totalTOAgency_Origin)", condition), 0))
        {
            TotalWrongSetting++;
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "ws");
        }
        else
        {
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "");
        }

        #endregion

        return strGrandTotal;
    }

    #endregion

    #region FOddsHR12BETRowTotalCurrency

    private static string FOddsHR12BETRowTotalCurrency(DataTable dt, string CurrencyName, string bgcolor,
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
        strGrandTotal += "<td class='r'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency_Origin)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r rspace rborder'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(AmountDue2IBC_Origin)", condition), 2) + "</b></td>";


        strGrandTotal += "</tr>";

        if (CastAide.AsDecimal(dt.Compute("sum(totalTOMem_Origin)", condition), 0) !=
            2*CastAide.AsDecimal(dt.Compute("sum(totalTOAgency_Origin)", condition), 0))
        {
            TotalWrongSetting++;
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "ws");
        }
        else
        {
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "");
        }

        #endregion

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
            2*CastAide.AsDecimal(dt.Compute("sum(totalTOAgency)", condition), 0))
        {
            TotalWrongSetting++;
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "ws");
        }
        else
        {
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "");
        }

        #endregion

        #endregion

        return strGrandTotal;
    }

    #endregion

    #region FOddsHR12BETRowGrandTotalBaseRM

    private static string FOddsHR12BETRowGrandTotalBaseRM(DataTable dt, string bgcolor, string condition,
                                                          ref int TotalWrongSetting)
    {
        string strGrandTotal = "";

        #region Grand Total Base (RM)

        if (HttpContext.Current.Request["IsExport"] == null)
            strGrandTotal += "<tfoot class='bg'>";
        strGrandTotal += "<tr id='trFooter' style='font-weight:bold;'>";
        strGrandTotal += "<td colspan='2' class='c total'><b>Grand Total:</b></td>";
        strGrandTotal += "<td class='c total'><b>Base (RM)</b></td>";

        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(winTOMem)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(placeTOMem)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOMem)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r {wrongsetting} total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOMem)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(DividendWonMem)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(placeTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(winplaceTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r {wrongsetting} total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(totalTOAgency)", condition), 2) + "</b></td>";
        strGrandTotal += "<td class='r total'><b>" + NumberFormat.DecFormat(dt.Compute("sum(PTWLAgency)", condition), 2) +
                         "</b></td>";
        strGrandTotal += "<td class='r total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(commIncomeAgency)", condition), 2) + "</b></td>";

        strGrandTotal += "<td class='r rspace rborder total'><b>" +
                         NumberFormat.DecFormat(dt.Compute("sum(AmountDue2IBC)", condition), 2) + "</b></td>";

        strGrandTotal += "</tr>";
        if (HttpContext.Current.Request["IsExport"] == null)
            strGrandTotal += "</tfoot>";

        if (CastAide.AsDecimal(dt.Compute("sum(totalTOMem)", condition), 0) !=
            2*CastAide.AsDecimal(dt.Compute("sum(totalTOAgency)", condition), 0))
        {
            TotalWrongSetting++;
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "ws");
        }
        else
        {
            TemplateHandler.Assign(ref strGrandTotal, "wrongsetting", "");
        }

        #endregion

        return strGrandTotal;
    }

    #endregion

    #endregion
}