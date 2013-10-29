using System;
using System.Collections;
using System.Data;
using System.Drawing;

using DNAide.Core;
using DNAide.Data;
using ExcelUtil.Spreadsheet;

using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for OpenToteSettlementExportExceManager
/// </summary>

namespace BLL.Report.OpenToteSettlement
{
    public class OpenToteSettlementExportExceManager : BaseManager
    {
        private void ExcelAssignValueForCell_OpenTote(ref ExcelWorksheet ws, DataRow dataRow, string suffix,
                                                      int indexRow, bool IsEven, ExcelGemboxUtils gembox)
        {
            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC", "FeeDue2",
                               "commIncomeAgency", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };

            for (int i = 0; i < arr.Length; i++)
            {
                ws.Cells[indexRow, i + 2].Value = dataRow[arr[i] + suffix];
                ws.Cells[indexRow, i + 2].Style = gembox.GetStyleNumber(0, IsEven, "");
            }
        }

        private void ExcelAssignValueForCellTotalPoolOrBaseCurrency_OpenTote(ref ExcelWorksheet ws, DataTable dataTable,
                                                                             int poolCurrency, string partnerType,
                                                                             string suffix, int indexRow,
                                                                             ExcelGemboxUtils gembox)
        {
            string exp = "PoolCurrency='" + poolCurrency + "' and partnerType='" + partnerType + "'";
            int red = 211, green = 225, blue = 200;
            if (poolCurrency == 0)
            {
                exp = "";
                red = 230;
                green = 184;
                blue = 183;
            }

            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC", "FeeDue2",
                               "commIncomeAgency", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };

            if (suffix.Length == 0)
            {
                ws.Cells[indexRow, 0].Value = partnerType + " Total";
                ws.Cells[indexRow, 0].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);

                ws.Cells[indexRow, 1].Value = "Base (RM)";
                ws.Cells[indexRow, 1].Style = gembox.GetStyleTextCenter(red, green, blue);

                ws.Cells[indexRow, 25].Value = "Base (RM)";
                ws.Cells[indexRow, 25].Style = gembox.GetStyleTextCenter(red, green, blue);

                ws.Cells[indexRow, 26].Value = partnerType + " Total";
                ws.Cells[indexRow, 26].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
            }
            else
            {
                int delta = 1;
                if (poolCurrency == 2) //Base RM
                    delta = 0;
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + delta, 0).Value = partnerType + " Total";
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + delta, 0).Style = gembox.GetStyleNumberTotal(0, "",
                                                                                                                  red,
                                                                                                                  green,
                                                                                                                  blue);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + delta, 0).Merged = true;

                ws.Cells[indexRow, 1].Value = CacheUtils.GetExchangeName(poolCurrency);
                ws.Cells[indexRow, 1].Style = gembox.GetStyleTextCenter(red, green, blue);

                ws.Cells[indexRow, 25].Value = CacheUtils.GetExchangeName(poolCurrency);
                ws.Cells[indexRow, 25].Style = gembox.GetStyleTextCenter(red, green, blue);

                ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + delta, 26).Value = partnerType + " Total";
                ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + delta, 26).Style = gembox.GetStyleNumberTotal(0,
                                                                                                                    "",
                                                                                                                    red,
                                                                                                                    green,
                                                                                                                    blue);
                ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + delta, 26).Merged = true;
            }

            ws.Cells[indexRow, 1].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
            ws.Cells[indexRow, 25].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);

            for (int i = 0; i < arr.Length; i++)
            {
                ws.Cells[indexRow, i + 2].Value = dataTable.Compute("sum(" + arr[i] + suffix + ")", exp);
                ws.Cells[indexRow, i + 2].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
            }
        }

        private void ExcelAssignRowGrandTotal_OpenTote(ref ExcelWorksheet ws, DataTable dataTable, string condition,
                                                       string TitleRow, ref int indexRow, ExcelGemboxUtils gembox,
                                                       int red, int green, int blue)
        {
            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC", "FeeDue2",
                               "commIncomeAgency", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };
           
            int colspan = 0;

            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Value = TitleRow;
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Style = gembox.GetStyleTitle(red, green, blue);
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Merged = true;

            ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + 1, 26).Value = TitleRow;
            ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + 1, 26).Style = gembox.GetStyleTitle(red, green, blue);
            ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + 1, 26).Merged = true;

            string[] arrCurr = { "GBP", "Base (RM)" };
            string suffix = "";
            for (int i = 0; i < arrCurr.Length; i++)
            {
                if (i == 0)
                    suffix = "_PoolCurr"; //GBP
                else
                    suffix = ""; //Base RM
                ws.Cells[indexRow + i, 1].Value = arrCurr[i];
                ws.Cells[indexRow + i, 1].Style = gembox.GetStyleTextTotal(red, green, blue);

                ws.Cells[indexRow + i, 25].Value = arrCurr[i];
                ws.Cells[indexRow + i, 25].Style = gembox.GetStyleTextTotal(red, green, blue);

                for (int j = 0; j < arr.Length; j++)
                {
                    ws.Cells[indexRow + i, j + 2].Value = dataTable.Compute("sum(" + arr[j] + suffix + ")", condition);
                    ws.Cells[indexRow + i, j + 2].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
                }
            }
        }

        private void ExcelAssignRowGrandTotal_OpenToteNew(ref ExcelWorksheet ws, DataTable dataTable, string condition,
                                                       string TitleRow, ref int indexRow, ExcelGemboxUtils gembox,
                                                       int red, int green, int blue, string poolCurrency)
        {
            string[] arr = {
                               "winTO_Agency", "placeTO_Agency", "winplaceTO_Agency", "totalTOAgency",
                               "commIncomeAgency", "PTWLAgency",
                               "winTO_IBC", "placeTO_IBC", "winplaceTO_IBC", "totalTOIBC", "commIncomeIBC", "FeeDue2",
                               "commIncomeAgency", "PTWLIBC", "netIncomeIBC",
                               "winTOPartner", "placeTOPartner", "winplaceTOPartner", "totalTOPartner", "dividendWon",
                               "commIncomeIBC", "FeeDue2", "AmountDue"
                           };

            int rowspan = 0;
            int colspan = 0;

            string[] validPoolCurrencyArr = poolCurrency.Split(';');

            //string[] arrCurr = { "GBP", "Base (RM)" };
            string[] arrCurr = new string[validPoolCurrencyArr.Length];
            for (int i = 0; i < validPoolCurrencyArr.Length; i++)
            {
                arrCurr[i] = CacheUtils.GetExchangeName(CastAide.AsInt32(validPoolCurrencyArr[i]));
            }
            arrCurr[validPoolCurrencyArr.Length - 1] = "Base (RM)";

            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + arrCurr.Length - 1, 0).Value = TitleRow;
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + arrCurr.Length - 1, 0).Style = gembox.GetStyleTitle(red, green, blue);
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + arrCurr.Length - 1, 0).Merged = true;

            ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + arrCurr.Length - 1, 26).Value = TitleRow;
            ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + arrCurr.Length - 1, 26).Style = gembox.GetStyleTitle(red, green, blue);
            ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + arrCurr.Length - 1, 26).Merged = true;

            string suffix = "";
            for (int i = 0; i < arrCurr.Length; i++)
            {
                string exp = (i < validPoolCurrencyArr.Length - 1) ? "PoolCurrency = '" + validPoolCurrencyArr[i] + "'" : "";

                if (i < arrCurr.Length - 1)
                    suffix = "_PoolCurr";
                else
                    suffix = ""; //Base RM

                ws.Cells[indexRow + i, 1].Value = arrCurr[i];
                ws.Cells[indexRow + i, 1].Style = gembox.GetStyleTextTotal(red, green, blue);

                ws.Cells[indexRow + i, 25].Value = arrCurr[i];
                ws.Cells[indexRow + i, 25].Style = gembox.GetStyleTextTotal(red, green, blue);

                for (int j = 0; j < arr.Length; j++)
                {
                    ws.Cells[indexRow + i, j + 2].Value = dataTable.Compute("sum(" + arr[j] + suffix + ")", exp);
                    ws.Cells[indexRow + i, j + 2].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
                }
            }
        }

        private void ExcelAssignValueforCoparision(ref ExcelWorksheet ws, string winlostdate, decimal ibcTO,
                                                   decimal ibcDividendWon, ref int indexRow, ExcelGemboxUtils gembox)
        {
            var dt = (DataTable)GetCachedObjData(OpenToteSettlementCommon.cachekeycompare);
            if (dt == null)
            {
                var hash = new Hashtable();
                hash.Add("winlostdate", winlostdate);
                dt = IBCProvider.ReportProvider.OpenToteSettlement().Horse_OpenToteTO3rd(hash).ResultSet.Tables[0];
            }

            if (dt.Rows.Count == 0)
                return;

            ws.Cells.GetSubrangeAbsolute(indexRow + 2, 20, indexRow + 2, 21).Value = "OptenTote Comparision";
            ws.Cells.GetSubrangeAbsolute(indexRow + 2, 20, indexRow + 2, 21).Style = gembox.GetStyleTextTotal(255, 211,
                                                                                                              32);
            ws.Cells.GetSubrangeAbsolute(indexRow + 2, 20, indexRow + 2, 21).Merged = true;

            decimal _3rdTO = 0, _3rdDividendWon = 0;
            string poolCurrency = "";
            foreach (DataRow dr in dt.Rows)
            {
                _3rdTO = CastAide.AsDecimal(dr["TO3rd"]);
                _3rdDividendWon = CastAide.AsDecimal(dr["DividendWon3rd"]);
                poolCurrency = CastAide.AsString(dr["currencyName"]);
            }

            if (OpenToteSettlementCommon.IsMatched(_3rdTO, ibcTO))
            {
                ws.Cells[indexRow + 3, 20].Value = "OK";
                ws.Cells[indexRow + 3, 20].Style = gembox.GetStyleText(Color.Blue, 247, 240, 247,
                                                                       HorizontalAlignmentStyle.Center);
            }
            else
            {
                ws.Cells[indexRow + 3, 20].Value = "Not Matched";
                ws.Cells[indexRow + 3, 20].Style = gembox.GetStyleText(Color.Red, 247, 240, 247,
                                                                       HorizontalAlignmentStyle.Center);
            }

            if (OpenToteSettlementCommon.IsMatched(_3rdDividendWon, ibcDividendWon))
            {
                ws.Cells[indexRow + 3, 21].Value = "OK";
                ws.Cells[indexRow + 3, 21].Style = gembox.GetStyleText(Color.Blue, 247, 240, 247,
                                                                       HorizontalAlignmentStyle.Center);
            }
            else
            {
                ws.Cells[indexRow + 3, 21].Value = "Not Matched";
                ws.Cells[indexRow + 3, 21].Style = gembox.GetStyleText(Color.Red, 247, 240, 247,
                                                                       HorizontalAlignmentStyle.Center);
            }

            #region temporary don't user it in excel file

            /*
            if (!OpenToteSettlementCommon.IsMatched(_3rdTO, ibcTO) || !OpenToteSettlementCommon.IsMatched(_3rdDividendWon, ibcDividendWon))
            {
                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 18, indexRow + 5, 18).Value = "Currency";
                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 18, indexRow + 5, 18).Style = gembox.GetStyleTextTotal(208, 232, 251);
                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 18, indexRow + 5, 18).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 19, indexRow + 4, 20).Value = "Partner";
                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 19, indexRow + 4, 20).Style = gembox.GetStyleTextTotal(208, 232, 251);
                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 19, indexRow + 4, 20).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 21, indexRow + 4, 22).Value = "Company";
                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 21, indexRow + 4, 22).Style = gembox.GetStyleTextTotal(208, 232, 251);
                ws.Cells.GetSubrangeAbsolute(indexRow + 4, 21, indexRow + 4, 22).Merged = true;

                ws.Cells[indexRow + 5, 19].Value = "TO";
                ws.Cells[indexRow + 5, 19].Style = gembox.GetStyleTextTotal(208, 232, 251);

                ws.Cells[indexRow + 5, 20].Value = "Dividend WON";
                ws.Cells[indexRow + 5, 20].Style = gembox.GetStyleTextTotal(208, 232, 251);

                ws.Cells[indexRow + 5, 21].Value = "TO";
                ws.Cells[indexRow + 5, 21].Style = gembox.GetStyleTextTotal(208, 232, 251);

                ws.Cells[indexRow + 5, 22].Value = "Dividend WON";
                ws.Cells[indexRow + 5, 22].Style = gembox.GetStyleTextTotal(208, 232, 251);

                ws.Cells[indexRow + 6, 18].Value = poolCurrency;
                ws.Cells[indexRow + 6, 18].Style = gembox.GetStyleText(255, 255, 255);

                decimal comparisionTO = -1, comparisionDividend = -1;
                if (OpenToteSettlementCommon.IsMatched(_3rdTO, ibcTO))
                    comparisionTO = 1;
                if (OpenToteSettlementCommon.IsMatched(_3rdDividendWon, ibcDividendWon))
                    comparisionDividend = 1;

                ws.Cells[indexRow + 6, 19].Value = _3rdTO;
                ws.Cells[indexRow + 6, 19].Style = gembox.GetStyleNumber(comparisionTO, "", 255, 255, 255);

                ws.Cells[indexRow + 6, 20].Value = _3rdDividendWon;
                ws.Cells[indexRow + 6, 20].Style = gembox.GetStyleNumber(comparisionDividend, "", 255, 255, 255);

                ws.Cells[indexRow + 6, 21].Value = ibcTO;
                ws.Cells[indexRow + 6, 21].Style = gembox.GetStyleNumber(comparisionTO, "", 255, 255, 255);

                ws.Cells[indexRow + 6, 22].Value = ibcDividendWon;
                ws.Cells[indexRow + 6, 22].Style = gembox.GetStyleNumber(comparisionDividend, "", 255, 255, 255);
            }*/

            #endregion temporary don't user it in excel file
        }

        public ExcelFile ExportToExcel_OpenTote(string winlostdate, string db, bool IsTest)
        {
            var excelFile = new ExcelFile();
            try
            {
                var dataTable = new DataTable();
                dataTable = GetCachedObjData(OpenToteSettlementCommon.cachekeyreport) as DataTable;

                if (dataTable == null)
                {
                    var hashtable = new Hashtable();
                    hashtable.Add("winlostdate", winlostdate);
                    hashtable.Add("db", db);

                    DbResult rs;
                    DateTime movebetsday = ReportUtils.BalanceUp.MoveBetsDay;
                    if (IsTest)
                    {
                        if (CastAide.AsDateTime(winlostdate) <= movebetsday)
                            rs = IBCProvider.ReportProvider.OpenToteSettlement().WinlostHR_OpenTote14_Test(hashtable);
                        else
                            rs = IBCProvider.ReportProvider.OpenToteSettlement().WinlostHR_OpenTote_Test(hashtable);
                    }
                    else
                    {
                        if (CastAide.AsDateTime(winlostdate) <= movebetsday)
                            rs = IBCProvider.ReportProvider.OpenToteSettlement().WinlostHR_OpenTote14(hashtable);
                        else
                            rs = IBCProvider.ReportProvider.OpenToteSettlement().WinlostHR_OpenTote(hashtable);
                    }

                    dataTable = rs.ResultSet.Tables[0];
                }

                var gembox = new ExcelGemboxUtils();

                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string filename = "WinlostHR_OpenTote-" + DateTime.Today.ToShortDateString().Replace("/", "-");
                string strTitle = "";
                ExcelWorksheet ws = null;
                string tag = "WinlostHR_OpenTote";
                ws = worksheets.Add(tag);

                #region Header

                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Style = gembox.GetStyleTitle(211, 255, 200);
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Value = "Horse Racing OpenTote Daily Report - Date: " +
                                                                 winlostdate;
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Style = gembox.GetStyleTitle(181, 196, 208);
                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Value = "Pool-Currency";
                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 1, 2, 1).Style = gembox.GetStyleTitle(181, 196, 208);
                ws.Cells.GetSubrangeAbsolute(1, 1, 2, 1).Value = "Currency";
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
                ws.Cells.GetSubrangeAbsolute(1, 25, 1, 25).Value = "Currency";
                ws.Cells.GetSubrangeAbsolute(1, 25, 2, 25).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 26, 2, 26).Style = gembox.GetStyleTitle(181, 196, 208);
                ws.Cells.GetSubrangeAbsolute(1, 26, 1, 26).Value = "Pool-Currency";
                ws.Cells.GetSubrangeAbsolute(1, 26, 2, 26).Merged = true;

                string[] arrAgency = { "Win TO", "Place TO", "W/P TO", "Total TO", "Comm Income", "PT Win/Loss" };
                for (int i = 0; i < arrAgency.Length; i++)
                {
                    ws.Cells.GetSubrangeAbsolute(2, i + 2, 2, i + 2).Style = gembox.GetStyleTitle(208, 232, 251);
                    ws.Cells.GetSubrangeAbsolute(2, i + 2, 2, i + 2).Value = arrAgency[i];
                }

                string[] arrIBC = {
                                      "Win TO", "Place TO", "W/P TO", "Total TO", "Comm Income", "Tote Fee", "Comm Expense"
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

                #endregion Header

                string partnertype = "", partnertype_p = "";
                bool IsEven = false;
                DataRow[] dataRows = dataTable.Select("");
                int indexRow = 3, indexPartner = 0, poolcurrency = 0, poolcurrency_p = 0;
                string poolCurrencyArr = string.Empty;

                #region foreach

                foreach (DataRow dataRow in dataRows)
                {
                    partnertype_p = CastAide.AsString(dataRow["PartnerType"]);
                    poolcurrency_p = CastAide.AsInt32(dataRow["PoolCurrency"]);
                    if (partnertype != partnertype_p)
                    {
                        int countPartner =
                            CastAide.AsInt32(dataTable.Compute("count(PartnerType)",
                                                               "PartnerType='" + partnertype_p + "'"));
                        IsEven = indexPartner % 2 == 1;
                        if (poolcurrency > 0)
                        {
                            //_PoolCurr
                            ExcelAssignValueForCellTotalPoolOrBaseCurrency_OpenTote(ref ws, dataTable, poolcurrency,
                                                                                    partnertype, "_PoolCurr", indexRow,
                                                                                    gembox);
                            indexRow++;

                            if (poolcurrency != 2) //If currency is RM then no need to show the row Base (RM)
                            {
                                //Base RM
                                ExcelAssignValueForCellTotalPoolOrBaseCurrency_OpenTote(ref ws, dataTable, poolcurrency,
                                                                                        partnertype, "", indexRow,
                                                                                        gembox);
                                indexRow++;
                            }

                            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 26).Merged = true;
                            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 26).Style =
                                gembox.GetStyleTextCenter(false);
                            indexRow++;
                        }

                        ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + countPartner - 1, 0).Merged = true;
                        ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + countPartner - 1, 0).Style =
                            gembox.GetStyleTextCenter(IsEven);
                        ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + countPartner - 1, 0).Value = partnertype_p;

                        ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + countPartner - 1, 26).Merged = true;
                        ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + countPartner - 1, 26).Style =
                            gembox.GetStyleTextCenter(IsEven);
                        ws.Cells.GetSubrangeAbsolute(indexRow, 26, indexRow + countPartner - 1, 26).Value =
                            partnertype_p;

                        if (poolcurrency_p != poolcurrency && poolCurrencyArr.Contains(poolcurrency_p.ToString()) == false)
                        {
                            poolCurrencyArr += (poolcurrency_p + ";");
                        }

                        partnertype = partnertype_p;
                        poolcurrency = poolcurrency_p;
                        indexPartner++;
                    }

                    //Currency
                    ws.Cells[indexRow, 1].Value = dataRow["CurrencyName"];
                    ws.Cells[indexRow, 1].Style = gembox.GetStyleTextCenter(IsEven);

                    ws.Cells[indexRow, 25].Value = dataRow["CurrencyName"];
                    ws.Cells[indexRow, 25].Style = gembox.GetStyleTextCenter(IsEven);

                    ExcelAssignValueForCell_OpenTote(ref ws, dataRow, "_Origin", indexRow, IsEven, gembox);

                    indexRow++;
                }

                #endregion foreach

                #region Lastest

                //_PoolCurr
                ExcelAssignValueForCellTotalPoolOrBaseCurrency_OpenTote(ref ws, dataTable, poolcurrency, partnertype,
                                                                        "_PoolCurr", indexRow, gembox);
                indexRow++;

                if (poolcurrency != 2) //If currency is RM then no need to show the row Base (RM)
                {
                    //Base RM
                    ExcelAssignValueForCellTotalPoolOrBaseCurrency_OpenTote(ref ws, dataTable, poolcurrency, partnertype,
                                                                            "", indexRow, gembox);
                    indexRow++;
                }

                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 26).Merged = true;
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 26).Style = gembox.GetStyleTextCenter(false);
                indexRow++;

                #endregion Lastest

                #region Grand Total

                ExcelAssignRowGrandTotal_OpenToteNew(ref ws, dataTable, "", "Grand Total", ref indexRow, gembox, 196, 208,
                                                  215, poolCurrencyArr);

                #endregion Grand Total

                #region Comparision

                //indexRow++;
                //decimal ibcTO = CastAide.AsDecimal(dataTable.Compute("sum(totalTOPartner_PoolCurr)", ""));
                //decimal ibcDividendWon = CastAide.AsDecimal(dataTable.Compute("sum(dividendWon_PoolCurr)", ""));
                //ExcelAssignValueforCoparision(ref ws, winlostdate, ibcTO, ibcDividendWon, ref indexRow, gembox);

                #endregion Comparision
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return excelFile;
        }
    }
}