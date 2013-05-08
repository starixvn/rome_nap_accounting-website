using System;
using System.Collections;
using System.Data;

using DNAide.Core;
using DNAide.Data;
using ExcelUtil.Spreadsheet;

using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for OpenToteSettlementExportExceManager
/// </summary>

namespace BLL.Report.OpenToteSettlement
{
    public class OpenToteSettlementDetailsExportExceManager : BaseManager
    {
        private void ExcelAssignRowTotal_OpenToteDetail(ref ExcelWorksheet ws, DataTable dataTable, string condition,
                                                        string TitleRow, int cid, string suffix, ref int indexRow,
                                                        bool IsTotalMeeting, int countRacePerMeeting,
                                                        ExcelGemboxUtils gembox, bool IsEven, bool IsColspan, int red,
                                                        int green, int blue)
        {
            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC",
                               "commIncomeAgency", "FeeDue2", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };

            int rowspan = 0;
            int colspan = 0;

            if (IsTotalMeeting)
            {
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Value = TitleRow;
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Style = gembox.GetStyleTitle(red, green, blue);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 25, indexRow, 26).Value = TitleRow;
                ws.Cells.GetSubrangeAbsolute(indexRow, 25, indexRow, 26).Style = gembox.GetStyleTitle(red, green, blue);
                ws.Cells.GetSubrangeAbsolute(indexRow, 25, indexRow, 26).Merged = true;

                for (int i = 0; i < arr.Length; i++)
                {
                    ws.Cells[indexRow, i + 2].Value = dataTable.Compute("sum(" + arr[i] + suffix + ")", condition);
                    ws.Cells[indexRow, i + 2].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
                }

                ws.Cells.GetSubrangeAbsolute(indexRow - countRacePerMeeting, 0, indexRow - 1, 0).Value =
                    TitleRow.Replace("Total", "");
                ws.Cells.GetSubrangeAbsolute(indexRow - countRacePerMeeting, 0, indexRow - 1, 0).Style =
                    gembox.GetStyleText(IsEven);
                ws.Cells.GetSubrangeAbsolute(indexRow - countRacePerMeeting, 0, indexRow - 1, 0).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow - countRacePerMeeting, 26, indexRow - 1, 26).Value =
                    TitleRow.Replace("Total", "");
                ws.Cells.GetSubrangeAbsolute(indexRow - countRacePerMeeting, 26, indexRow - 1, 26).Style =
                    gembox.GetStyleText(IsEven);
                ws.Cells.GetSubrangeAbsolute(indexRow - countRacePerMeeting, 26, indexRow - 1, 26).Merged = true;

                indexRow++;
            }
            else
            {
                if (cid == 2) //RM Currency
                {
                    rowspan = 0;
                    colspan = 1;
                }
                else
                {
                    rowspan = 1;
                    colspan = 0;

                    string strCurr = TitleRow;
                    if (condition.Length == 0) //Grand Total
                    {
                        strCurr = CacheUtils.GetExchangeName(cid);
                    }

                    ws.Cells.GetSubrangeAbsolute(indexRow, 1, indexRow, 1).Value = strCurr;
                    ws.Cells.GetSubrangeAbsolute(indexRow, 1, indexRow, 1).Style = gembox.GetStyleTitle(red, green, blue);

                    ws.Cells.GetSubrangeAbsolute(indexRow, 25, indexRow, 25).Value = strCurr;
                    ws.Cells.GetSubrangeAbsolute(indexRow, 25, indexRow, 25).Style = gembox.GetStyleTitle(red, green,
                                                                                                          blue);

                    ws.Cells.GetSubrangeAbsolute(indexRow + 1, 1, indexRow + 1, 1).Value = "Base (RM)";
                    ws.Cells.GetSubrangeAbsolute(indexRow + 1, 1, indexRow + 1, 1).Style = gembox.GetStyleTitle(red,
                                                                                                                green,
                                                                                                                blue);

                    ws.Cells.GetSubrangeAbsolute(indexRow + 1, 25, indexRow + 1, 25).Value = "Base (RM)";
                    ws.Cells.GetSubrangeAbsolute(indexRow + 1, 25, indexRow + 1, 25).Style = gembox.GetStyleTitle(red,
                                                                                                                  green,
                                                                                                                  blue);
                }

                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + rowspan, colspan).Value = TitleRow; // +" Total";
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + rowspan, colspan).Style = gembox.GetStyleTitle(
                    red, green, blue);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + rowspan, colspan).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 26 - colspan, indexRow + rowspan, 26).Value = TitleRow;
                    // +" Total";
                ws.Cells.GetSubrangeAbsolute(indexRow, 26 - colspan, indexRow + rowspan, 26).Style =
                    gembox.GetStyleTitle(red, green, blue);
                ws.Cells.GetSubrangeAbsolute(indexRow, 26 - colspan, indexRow + rowspan, 26).Merged = true;

                for (int i = 0; i < arr.Length; i++)
                {
                    ws.Cells[indexRow, i + 2].Value = dataTable.Compute("sum(" + arr[i] + suffix + ")", condition);
                    ws.Cells[indexRow, i + 2].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
                }
                indexRow++;

                if (cid != 2) //Base RM
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        ws.Cells[indexRow, i + 2].Value = dataTable.Compute("sum(" + arr[i] + ")", condition);
                        ws.Cells[indexRow, i + 2].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
                    }
                    indexRow++;
                }
            }

            if (IsColspan)
            {
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 26).Merged = true;
                indexRow++;
            }
        }

        private void ExcelAssignRow_OpenToteDetail(ref ExcelWorksheet ws, DataRow dataRow, bool IsEven, string suffix,
                                                   ref int indexRow, ExcelGemboxUtils gembox)
        {
            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC",
                               "commIncomeAgency", "FeeDue2", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };

            ws.Cells[indexRow, 1].Value = CastAide.AsInt32(dataRow["RaceNo"]);
            ws.Cells[indexRow, 1].Style = gembox.GetStyleNumberCenter(0, IsEven, "");

            ws.Cells[indexRow, 25].Value = CastAide.AsInt32(dataRow["RaceNo"]);
            ws.Cells[indexRow, 25].Style = gembox.GetStyleNumberCenter(0, IsEven, "");

            for (int i = 0; i < arr.Length; i++)
            {
                ws.Cells[indexRow, i + 2].Value = dataRow[arr[i] + suffix];
                ws.Cells[indexRow, i + 2].Style = gembox.GetStyleNumber(0, IsEven, "");
            }
            indexRow++;
        }

        public ExcelFile ExportToExcel_OpenToteDetail(string winlostdate, string PartnerType, string PoolCurrency,
                                                      string db, bool IsTest)
        {
            var excelFile = new ExcelFile();

            try
            {
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

                var gembox = new ExcelGemboxUtils();

                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string filename = "WinlostHR_OpenTote-" + DateTime.Today.ToShortDateString().Replace("/", "-");
                string strTitle = "";
                ExcelWorksheet ws = null;
                string tag = "HRFor" + PartnerType;
                ws = worksheets.Add(tag);

                #region Header

                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Style = gembox.GetStyleTitle(211, 255, 200);
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Value = "Horse Racing Daily Report For Racing " + PartnerType +
                                                                 "  - Date: " + winlostdate;
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Style = gembox.GetStyleTitle(181, 196, 208);
                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Value = "Meeting-Currency";
                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Merged = true;


                ws.Cells.GetSubrangeAbsolute(1, 1, 2, 1).Style = gembox.GetStyleTitle(181, 196, 208);
                ws.Cells.GetSubrangeAbsolute(1, 1, 2, 1).Value = "Race";
                ws.Cells.GetSubrangeAbsolute(1, 1, 2, 1).Merged = true;


                ws.Cells.GetSubrangeAbsolute(1, 2, 1, 7).Style = gembox.GetStyleTitle(208, 232, 251);
                ws.Cells.GetSubrangeAbsolute(1, 2, 1, 7).Value = "Agency";
                ws.Cells.GetSubrangeAbsolute(1, 2, 1, 7).Merged = true;


                ws.Cells.GetSubrangeAbsolute(1, 8, 1, 16).Style = gembox.GetStyleTitle(181, 196, 208);
                ws.Cells.GetSubrangeAbsolute(1, 8, 1, 16).Value = "IBC";
                ws.Cells.GetSubrangeAbsolute(1, 8, 1, 16).Merged = true;


                ws.Cells.GetSubrangeAbsolute(1, 17, 1, 24).Style = gembox.GetStyleTitle(208, 232, 251);
                ws.Cells.GetSubrangeAbsolute(1, 17, 1, 24).Value = "Partner";
                ws.Cells.GetSubrangeAbsolute(1, 17, 1, 24).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 25, 2, 25).Style = gembox.GetStyleTitle(181, 196, 208);
                ws.Cells.GetSubrangeAbsolute(1, 25, 1, 25).Value = "Race";
                ws.Cells.GetSubrangeAbsolute(1, 25, 2, 25).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 26, 2, 26).Style = gembox.GetStyleTitle(181, 196, 208);
                ws.Cells.GetSubrangeAbsolute(1, 26, 1, 26).Value = "Meeting-Currency";
                ws.Cells.GetSubrangeAbsolute(1, 26, 2, 26).Merged = true;

                string[] arrAgency = {"Win TO", "Place TO", "W/P TO", "Total TO", "Comm Income", "PT Win/Loss"};
                for (int i = 0; i < arrAgency.Length; i++)
                {
                    ws.Cells.GetSubrangeAbsolute(2, i + 2, 2, i + 2).Style = gembox.GetStyleTitle(208, 232, 251);
                    ws.Cells.GetSubrangeAbsolute(2, i + 2, 2, i + 2).Value = arrAgency[i];
                }

                string[] arrIBC = {
                                      "Win TO", "Place TO", "W/P TO", "Total TO", "Comm Income", "Comm Expense", "Tote Fee"
                                      , "PT Win/Loss", "Net Income"
                                  };
                for (int i = 0; i < arrIBC.Length; i++)
                {
                    ws.Cells.GetSubrangeAbsolute(2, i + 8, 2, i + 8).Style = gembox.GetStyleTitle(181, 196, 208);
                    ws.Cells.GetSubrangeAbsolute(2, i + 8, 2, i + 8).Value = arrIBC[i];
                }

                string[] arrPartner = {
                                          "Win TO", "Place TO", "W/P TO", "Total TO", "Dividend Won", "Comm Expense",
                                          "Tote Fee", "Amount Due"
                                      };
                for (int i = 0; i < arrPartner.Length; i++)
                {
                    ws.Cells.GetSubrangeAbsolute(2, i + 17, 2, i + 17).Style = gembox.GetStyleTitle(208, 232, 251);
                    ws.Cells.GetSubrangeAbsolute(2, i + 17, 2, i + 17).Value = arrPartner[i];
                }

                #endregion

                #region Sort sub total by group currency

                DataTable dtWL = rs.ResultSet.Tables[0];
                int cid = 0, cid_p = 0, last_cid_usd = 101, last_cid_thb = 101, last_cid_in = 101;

                #endregion

                DataView dv = dtWL.DefaultView;
                dv.Sort = "currency asc, meetingid asc ";
                var dt = new DataTable();
                dt = dv.ToTable();

                #region Declare Variable

                int meetingid = 0, meetingid_p = 0;

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

                #region foreach

                int indexRow = 3, count_meeting = 0;
                bool IsEven = false;
                foreach (DataRow dr in dt.Rows)
                {
                    meetingid_p = CastAide.AsInt32(dr["meetingid"]);
                    currency_p = CastAide.AsString(dr["currencyname"]);
                    cid_p = CastAide.AsInt32(dr["currency"]);

                    #region Each Currency

                    if (currency_p != currency)
                    {
                        if (c_index > 0)
                        {
                            #region Total Meeting

                            condition_Meeting = "meetingid = '" + meetingid + "' and currencyname='" + currency + "'";
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid);
                            if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
                            else strTitleMeeting += currency + " Total";

                            ExcelAssignRowTotal_OpenToteDetail(ref ws, dt, condition_Meeting, strTitleMeeting, cid,
                                                               "_Origin", ref indexRow, true, m_index, gembox, IsEven,
                                                               false, 255, 238, 221);

                            #endregion

                            #region Total Currency

                            {
                                strConditionCurrency = "currencyname = '" + currency + "'";

                                if (currency == "RM") // Base RM
                                {
                                    strTitleCurrency = currency + " Total";
                                }
                                else
                                {
                                    strTitleCurrency = currency;
                                }
                                ExcelAssignRowTotal_OpenToteDetail(ref ws, dt, strConditionCurrency, strTitleCurrency,
                                                                   cid, "_Origin", ref indexRow, false, m_index, gembox,
                                                                   IsEven, true, 211, 225, 200);
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
                            condition_Meeting = "meetingid = '" + meetingid + "' and currencyname='" + currency + "'";
                            strTitleMeeting = CacheRef.GetLeagueName(meetingid) + " - " + currency + " Total";
                            ExcelAssignRowTotal_OpenToteDetail(ref ws, dt, condition_Meeting, strTitleMeeting, cid,
                                                               "_Origin", ref indexRow, true, m_index, gembox, IsEven,
                                                               true, 255, 238, 221);
                            meeting_currency = CacheRef.GetLeagueName(meetingid_p) + " - " + currency_p;
                        }
                        m_index = 0;
                        meetingid = meetingid_p;
                        count_meeting++;

                        IsEven = count_meeting%2 == 0;
                    }

                    m_index++;

                    #endregion

                    #region Assign Each Row

                    int CurrencyID = CastAide.AsInt32(dr["Currency"]);
                    string CurrencyName = CastAide.AsString(dr["CurrencyName"]);
                    raceNo = CastAide.AsString(dr["raceNo"]);

                    ExcelAssignRow_OpenToteDetail(ref ws, dr, IsEven, "_Origin", ref indexRow, gembox);

                    #endregion
                }

                #endregion

                #region Total

                #region Total Meeting

                condition_Meeting = "meetingid = '" + meetingid + "' and currencyname='" + currency + "'";
                strTitleMeeting = CacheRef.GetLeagueName(meetingid);
                if (strTitleMeeting.Length > 0) strTitleMeeting += " - " + currency + " Total";
                else strTitleMeeting += currency + " Total";
                ExcelAssignRowTotal_OpenToteDetail(ref ws, dt, condition_Meeting, strTitleMeeting, cid, "_Origin",
                                                   ref indexRow, true, m_index, gembox, IsEven, false, 255, 238, 221);

                #endregion

                #region Total Currency

                strConditionCurrency = "currencyname = '" + currency + "'";
                strTitleCurrency = currency + " Total";
                ExcelAssignRowTotal_OpenToteDetail(ref ws, dt, strConditionCurrency, strTitleCurrency, cid, "_Origin",
                                                   ref indexRow, false, m_index, gembox, IsEven, true, 211, 225, 200);

                #endregion

                #region Grand Total

                string strGrandTotal = "";

                ExcelAssignRowTotal_OpenToteDetail(ref ws, dt, "", "Grand Total", CastAide.AsInt32(PoolCurrency),
                                                   "_PoolCurr", ref indexRow, false, count_meeting, gembox, IsEven,
                                                   false, 146, 208, 80);

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return excelFile;
        }
    }
}