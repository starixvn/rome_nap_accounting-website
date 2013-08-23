using System;
using System.Collections;
using System.Data;
using System.Text;

using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;

namespace BLL.Report.OpenToteSettlement
{
    public class OpenToteSettlementDetailsManager : ReportBase
    {
        private bool hasError;
        private string msg = "<span class='error-message'>{content}</span>";
        private string user = "[" + AppSession.Username + "]";

        #region WinlostHR_OpenTote_Details

        public string WinlostHR_OpenTote_Details(string winlostdate, string PartnerType, string PoolCurrency, string db,
                                                 bool IsTest)
        {
            var html = new StringBuilder();
            try
            {
                string tpl = WebUtils.LoadTemplate("Report/OpenToteSettlement/OpenToteSettlementReportDetails.htm");

                var hashtable = new Hashtable();
                hashtable.Add("winlostdate", winlostdate);
                hashtable.Add("partnertype", PartnerType);
                hashtable.Add("poolcurrency", PoolCurrency);
                hashtable.Add("db", db);

                DbResult rs = null;

                DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;

                if (IsTest)
                {
                    if (CastAide.AsDateTime(winlostdate) <= movebetsday)
                        rs = IBCProvider.ReportProvider.OpenToteSettlement().WinlostHR_OpenTote_Details14_Test(hashtable);
                    else
                        rs = IBCProvider.ReportProvider.OpenToteSettlement().WinlostHR_OpenTote_Details_Test(hashtable);
                }
                else
                {
                    if (CastAide.AsDateTime(winlostdate) <= movebetsday)
                        rs = IBCProvider.ReportProvider.OpenToteSettlement().WinlostHR_OpenTote_Details14(hashtable);
                    else
                        rs = IBCProvider.ReportProvider.OpenToteSettlement().WinlostHR_OpenTote_Details(hashtable);
                }

                DataTable dtWL = rs.ResultSet.Tables[0];
                DataView dv = dtWL.DefaultView;
                dv.Sort = "currency asc, meetingid asc ";
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
                int cid_p = 0, cid = 0;

                #endregion Declare Variable

                // for testing loop
                // for (int i = 0; i < 30; i++)
                {
                    #region foreach

                    foreach (DataRow dr in dt.Rows)
                    {
                        row = tplRow;
                        meetingid_p = CastAide.AsInt32(dr["meetingid"]);
                        currency_p = CastAide.AsString(dr["currencyname"]);
                        cid_p = CastAide.AsInt32(dr["currency"]);

                        #region Each Currency

                        if (currency_p != currency)
                        {
                            if (c_index > 0)
                            {
                                #region Total Meeting

                                condition_Meeting = "meetingid = '" + meetingid + "' and currencyname='" + currency +
                                                    "'";
                                strTitleMeeting = CacheRef.GetLeagueName(meetingid);
                                if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
                                else strTitleMeeting += currency + " Total";

                                strTotalMeeting = OpenToteRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting,
                                                                          condition_Meeting);
                                rows.Append(strTotalMeeting);

                                #endregion Total Meeting

                                #region Total Currency

                                {
                                    strConditionCurrency = "currencyname = '" + currency + "'";

                                    if (currency == "RM") // Base RM
                                    {
                                        strTitleCurrency = currency + " Total";
                                        strTotalCurrency = OpenToteRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency,
                                                                                   strConditionCurrency);
                                    }
                                    else
                                    {
                                        strTitleCurrency = currency;
                                        strTotalCurrency = OpenToteRowTotalCurrency(dt, strTitleCurrency,
                                                                                    bgcolorCurrency,
                                                                                    strConditionCurrency);
                                    }

                                    rows.Append(strTotalCurrency);
                                }
                                rows.Replace("{rowspan}", (m_index).ToString());
                                rows.Append(trBlank);

                                #endregion Total Currency

                                c_index = 0;
                                meetingid = 0;
                            }
                            c_index++;
                            currency = currency_p;
                            cid = cid_p;
                            m_index = 0;
                        }

                        #endregion Each Currency

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
                                condition_Meeting = "meetingid = '" + meetingid + "' and currencyname='" + currency +
                                                    "'";
                                strTitleMeeting = CacheRef.GetLeagueName(meetingid) + " - " + currency + " Total";
                                strTotalMeeting = OpenToteRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting,
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

                        #endregion Each Meeting

                        #region Assign Each Row

                        TemplateHandler.Assign(ref row, "bgcolor", report_row);

                        int CurrencyID = CastAide.AsInt32(dr["Currency"]);
                        string CurrencyName = CastAide.AsString(dr["CurrencyName"]);
                        raceNo = CastAide.AsString(dr["raceNo"]);
                        //meetingid|raceNo|currencyID|winlostdate|db|meeting_currency
                        string evt = "ViewBetList('" + meetingid_p + "','" + raceNo + "','" + CurrencyID + "','" +
                                     winlostdate + "','" + db + "','" + ((IsTest) ? 1 : 0) + "');";
                        string strRace = "<div class='hand' style='color:blue;' onclick=\"" + evt + "\">" + dr["raceNo"] +
                                         "</div>";

                        TemplateHandler.Assign(ref row, "Meeting_Currency", meeting_currency);
                        TemplateHandler.Assign(ref row, "RaceName", strRace);
                        TemplateHandler.Assign(ref row, "AgWinTO", NumberFormat.DecFormat(dr["winTO_Agency_Origin"], 2));
                        TemplateHandler.Assign(ref row, "AgPlaceTO",
                                               NumberFormat.DecFormat(dr["placeTO_Agency_Origin"], 2));
                        TemplateHandler.Assign(ref row, "AgWinPlaceTO",
                                               NumberFormat.DecFormat(dr["winplaceTO_Agency_Origin"], 2));
                        TemplateHandler.Assign(ref row, "AgTotalTO",
                                               NumberFormat.DecFormat(dr["totalTOAgency_Origin"], 2));
                        TemplateHandler.Assign(ref row, "AgCommInc",
                                               NumberFormat.DecFormat(dr["commIncomeAgency_Origin"], 2));
                        TemplateHandler.Assign(ref row, "AgWL", NumberFormat.DecFormat(dr["PTWLAgency_Origin"], 2));

                        TemplateHandler.Assign(ref row, "IBCWinTO", NumberFormat.DecFormat(dr["winTO_IBC_Origin"], 2));
                        TemplateHandler.Assign(ref row, "IBCPlaceTO",
                                               NumberFormat.DecFormat(dr["placeTO_IBC_Origin"], 2));
                        TemplateHandler.Assign(ref row, "IBCWinPlaceTO",
                                               NumberFormat.DecFormat(dr["winplaceTO_IBC_Origin"], 2));
                        TemplateHandler.Assign(ref row, "IBCTotalTO", NumberFormat.DecFormat(dr["totalTOIBC_Origin"], 2));
                        TemplateHandler.Assign(ref row, "IBCCommInc",
                                               NumberFormat.DecFormat(dr["commIncomeIBC_Origin"], 2));
                        TemplateHandler.Assign(ref row, "IBCCommExp",
                                               NumberFormat.DecFormat(dr["commIncomeAgency_Origin"], 2));
                        TemplateHandler.Assign(ref row, "IBCToteFee", NumberFormat.DecFormat(dr["FeeDue2_Origin"], 2));
                        TemplateHandler.Assign(ref row, "IBCPTWL", NumberFormat.DecFormat(dr["PTWLIBC_Origin"], 2));
                        TemplateHandler.Assign(ref row, "IBCNetInc",
                                               NumberFormat.DecFormat(dr["netIncomeIBC_Origin"], 2));

                        TemplateHandler.Assign(ref row, "PartnerWinTO",
                                               NumberFormat.DecFormat(dr["winTOPartner_Origin"], 2));
                        TemplateHandler.Assign(ref row, "PartnerPlaceTO",
                                               NumberFormat.DecFormat(dr["placeTOPartner_Origin"], 2));
                        TemplateHandler.Assign(ref row, "PartnerWinPlaceTO",
                                               NumberFormat.DecFormat(dr["winplaceTOPartner_Origin"], 2));
                        TemplateHandler.Assign(ref row, "PartnerTotalTO",
                                               NumberFormat.DecFormat(dr["totalTOPartner_Origin"], 2));
                        TemplateHandler.Assign(ref row, "PartnerDividendWon",
                                               NumberFormat.DecFormat(dr["dividendWon_Origin"], 2));
                        TemplateHandler.Assign(ref row, "PartnerCommExp",
                                               NumberFormat.DecFormat(dr["commIncomeIBC_Origin"], 2));
                        TemplateHandler.Assign(ref row, "PartnerAmountDue",
                                               NumberFormat.DecFormat(dr["AmountDue_Origin"], 2));

                        row = TemplateHandler.RemoveComment(row);

                        rows.Append(row);

                        #endregion Assign Each Row
                    }
                    rows.Replace("{rowspan}", m_index.ToString());

                    #endregion foreach
                }

                #region Total

                #region Total Meeting

                condition_Meeting = "meetingid = '" + meetingid + "' and currencyname='" + currency + "'";
                strTitleMeeting = CacheRef.GetLeagueName(meetingid);
                if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
                else strTitleMeeting += currency + " Total";
                strTotalMeeting = OpenToteRowTotalMeeting(dt, strTitleMeeting, bgcolorMeeting, condition_Meeting);
                //TemplateHandler.RemoveComment(ref strTotalMeeting);
                rows.Append(strTotalMeeting);

                #endregion Total Meeting

                #region Total Currency

                strConditionCurrency = "currencyname = '" + currency + "'";
                {
                    if (currency == "RM") // Base RM
                    {
                        strTitleCurrency = currency + " Total";
                        strTotalCurrency = OpenToteRowTotalMeeting(dt, strTitleCurrency, bgcolorCurrency,
                                                                   strConditionCurrency);
                    }
                    else
                    {
                        strTitleCurrency = currency;
                        strTotalCurrency = OpenToteRowTotalCurrency(dt, strTitleCurrency, bgcolorCurrency,
                                                                    strConditionCurrency);
                    }

                    rows.Append(strTotalCurrency);
                    rows.Append(trBlank);
                }

                #endregion Total Currency

                #region Grand Total

                string strGrandTotal = "";
                strGrandTotal = OpenToteRowGrandTotalBaseRM(dt, PoolCurrency);
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
            return html.ToString();
        }

        #endregion WinlostHR_OpenTote_Details

        #region ToteRowTotalMeeting

        private string OpenToteRowTotalMeeting(DataTable dt, string TitleRow, string bgcolor, string condition)
        {
            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC",
                               "commIncomeAgency", "FeeDue2", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };

            #region GrandTotal

            var strGrandTotal = new StringBuilder();
            strGrandTotal.Append("<tr bgcolor='" + bgcolor + "' style='font-weight:bold;'>");
            strGrandTotal.Append("<td nowrap colspan='3' class='c'><b>" + TitleRow + "</b></td>");

            for (int i = 0; i < arr.Length; i++)
            {
                strGrandTotal.Append("<td class='r'><b>" +
                                     NumberFormat.DecFormat(dt.Compute("sum(" + arr[i] + "_Origin)", condition), 2) +
                                     "</b></td>");
            }

            strGrandTotal.Append("<td nowrap colspan='3' class='c'><b>" + TitleRow + "</b></td>");

            strGrandTotal.Append("</tr>");

            #endregion GrandTotal

            return strGrandTotal.ToString();
        }

        #endregion ToteRowTotalMeeting

        #region OpenToteRowTotalCurrency

        private string OpenToteRowTotalCurrency(DataTable dt, string CurrencyName, string bgcolor, string condition)
        {
            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC",
                               "commIncomeAgency", "FeeDue2", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };

            #region GrandTotal

            var strGrandTotal = new StringBuilder();

            #region Total Currency

            strGrandTotal.Append("<tr  bgcolor='" + bgcolor + "' style='font-weight:bold;'>");
            strGrandTotal.Append("<td nowrap rowspan='2' colspan='2' class='c'><b>" + CurrencyName + " Total</b></td>");
            strGrandTotal.Append("<td class='c'><b>" + CurrencyName + "</b></td>");

            for (int i = 0; i < arr.Length; i++)
            {
                strGrandTotal.Append("<td class='r'><b>" +
                                     NumberFormat.DecFormat(dt.Compute("sum(" + arr[i] + "_Origin" + ")", condition), 2) +
                                     "</b></td>");
            }

            strGrandTotal.Append("<td class='c'><b>" + CurrencyName + "</b></td>");
            strGrandTotal.Append("<td nowrap rowspan='2' colspan='2' class='c rborder'><b>" + CurrencyName +
                                 " Total</b></td>");

            strGrandTotal.Append("</tr>");

            #endregion Total Currency

            #region Total Base (RM)

            strGrandTotal.Append("<tr bgcolor='" + bgcolor + "' style='font-weight:bold;'>");
            strGrandTotal.Append("<td class='c'><b>Base (RM)</b></td>");

            for (int i = 0; i < arr.Length; i++)
            {
                strGrandTotal.Append("<td class='r'><b>" +
                                     NumberFormat.DecFormat(dt.Compute("sum(" + arr[i] + ")", condition), 2) +
                                     "</b></td>");
            }

            strGrandTotal.Append("<td class='c'><b>Base (RM)</b></td>");

            strGrandTotal.Append("</tr>");

            #endregion Total Base (RM)

            #endregion GrandTotal

            return strGrandTotal.ToString();
        }

        #endregion OpenToteRowTotalCurrency

        #region OpenToteRowGrandTotalBaseRM

        private string OpenToteRowGrandTotalBaseRM(DataTable dt, string poolCurr)
        {
            var strGrandTotal = new StringBuilder();

            #region Grand Total Base (RM)

            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC",
                               "commIncomeAgency", "FeeDue2", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };

            strGrandTotal.Append("<tfoot>");
            strGrandTotal.Append("<tr class='report-footer bg' style='font-weight:bold;'>");
            strGrandTotal.Append("<td class='c report-footer' colspan='2'></td>");

            //Pool Cureency
            strGrandTotal.Append("<td style='text-align:center' class='report-footer'><b>" +
                                 CacheUtils.GetExchangeName(CastAide.AsInt32(poolCurr)) + "</b></td>");
            for (int i = 0; i < arr.Length; i++)
            {
                strGrandTotal.Append("<td class='r report-footer'><b>" +
                                     NumberFormat.DecFormat(dt.Compute("sum(" + arr[i] + "_PoolCurr" + ")", ""), 2) +
                                     "</b></td>");
            }
            strGrandTotal.Append("<td style='text-align:center' class='report-footer'><b>" +
                                 CacheUtils.GetExchangeName(CastAide.AsInt32(poolCurr)) + "</b></td>");

            strGrandTotal.Append("<td class='c report-footer' colspan='2'></td>");
            strGrandTotal.Append("</tr>");

            //Base RM
            strGrandTotal.Append("<tr class='report-footer bg' id='trFooter'  style='font-weight:bold;'>");
            strGrandTotal.Append("<td class='c' colspan='2'><b>Grand Total</b></td>");
            strGrandTotal.Append("<td class='c report-footer'><b>Base (RM)</b></td>");
            for (int i = 0; i < arr.Length; i++)
            {
                strGrandTotal.Append("<td class='r report-footer'><b>" +
                                     NumberFormat.DecFormat(dt.Compute("sum(" + arr[i] + ")", ""), 2) + "</b></td>");
            }
            strGrandTotal.Append("<td class='c report-footer'><b>Base (RM)</b></td>");
            strGrandTotal.Append("<td class='c'  colspan='2'><b>Grand Total</b></td>");
            strGrandTotal.Append("</tr>");

            strGrandTotal.Append("</tfoot>");

            #endregion Grand Total Base (RM)

            return strGrandTotal.ToString();
        }

        #endregion OpenToteRowGrandTotalBaseRM

        public override string RenderHTML(Hashtable input)
        {
            throw new NotImplementedException();
        }
    }
}