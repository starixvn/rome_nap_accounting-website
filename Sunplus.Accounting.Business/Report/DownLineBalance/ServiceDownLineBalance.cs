using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.ServiceModel.Activation;
using System.Web;
using BLL.Report.DownLineBalance;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Report.DownLinceBalance;

/// <summary>
/// Summary description for DailyTransManager
/// </summary>
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class ServiceDownLineBalance : IServiceDownLineBalance
{
    #region IServiceDownLineBalance Members

    private string _superBalanceSessionKey = "superbalancesessionkey";
    private string _masterBalanceSessionKey = "masterbalancesessionkey";
    private string _agentBalanceSessionKey = "agentbalancesessionkey";
    private string _memberBalanceSessionKey = "memberbalancesessionkey";

    public string LoadHeader()
    {
        throw new NotImplementedException();
    }

    public string MasterBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch,
                                string firstletter, string currency)
    {
        var s = new DownLineBalanceManager();
        return s.MasterBalance(db, roleid, custid, isYesterday, usernameSearch, firstletter, currency);
    }

    public string SuperBalance(string db, bool isYesterday, string usernameSearch, string firstletter, string currency)
    {
        var s = new DownLineBalanceManager();
        return s.SuperBalance(db, isYesterday, usernameSearch, firstletter, currency);
    }

    public string GetCompanyBalanceHistory()
    {
        var s = new DownLineBalanceManager();
        return s.GetCompanyBalanceHistory();
    }

    public ExcelFile ExportToExcel()
    {
        throw new NotImplementedException();
    }

    #endregion IServiceDownLineBalance Members

    public CurrencyDTO GetCurrenciesBySystem()
    {
        var currencies = new CurrencyDTO();
        currencies.Currencies = new List<Currency>();
        DbResult ds = SelectCustomer.ExchangeSelRpt(AppSession.SYSTEM);
        if (ds != null && ds.ResultSet.Tables != null && ds.ResultSet.Tables.Count > 0)
        {
            foreach (DataRow dr in ds.ResultSet.Tables[0].Rows)
            {
                var currency = new Currency()
                {
                    Id = CastAide.AsString(dr["exchangeid"]),
                    Value = CastAide.AsString(dr["currency"]),
                    Code = CastAide.AsString(dr["exchangeid"])
                };
                currencies.Currencies.Add(currency);
            }
        }
        return currencies;
    }

    public DownLineBalanceDTO GetSuperBalance(string db, bool isYesterday, string usernameSearch, string firstletter, string currency, bool isViewTotalOnly)
    {
        var result = new DownLineBalanceDTO();
        result.DownLineBalane = new List<DownLineBalaneRow>();
        result.BaseTotal = new DownLineBalaneRow();
        try
        {
            var param = new Hashtable();
            if (!string.IsNullOrEmpty(usernameSearch) && !usernameSearch.ToLower().Equals("all"))
                param.Add("keyword", usernameSearch);
            if (!string.IsNullOrEmpty(firstletter) && !firstletter.ToLower().Equals("all"))
                param.Add("firstchar", firstletter);
            if (!string.IsNullOrEmpty(currency) && !currency.ToLower().Equals("0"))
                param.Add("currency", currency);
            param.Add("isviewtotalonly", isViewTotalOnly);
            param.Add("total", 0);
            if (!isViewTotalOnly)
            {
                var dtResult = IBCProvider.ReportProvider.DownLineBalance().RptSABalance(db, isYesterday, param).ResultSet.Tables[0];

                if (dtResult != null && dtResult.Rows != null && dtResult.Rows.Count > 0)
                {
                    Dictionary<int, string> exchange = CacheUtils.Exchange;
                    double total = 0;
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["balance"], 2)) == 0 &&
                                CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                            continue;
                        else
                        {
                            var balanceInfo = new DownLineBalaneRow()
                            {
                                Username = CastAide.AsString(dr["username"]),
                                CustId = CastAide.AsString(dr["custid"]),
                                RoleId = "3",
                                CurrencyId = CastAide.AsString(dr["currency"]),
                                Currency = CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["currency"])),
                                Balance = NumberFormat.DecFormat(dr["balance"], 2),
                                BaseBalance = NumberFormat.DecFormat(dr["basebalance"], 2)
                            };
                            result.DownLineBalane.Add(balanceInfo);
                            total += CastAide.AsDouble(dr["basebalance"]);
                        }
                    }
                    result.BaseTotal.BaseBalance = NumberFormat.DecFormat(total, 2);
                }
            }
            else
            {
                string total = IBCProvider.ReportProvider.DownLineBalance().RptSABalance(db, isYesterday, param).Outputs["total"].ToString();
                result.BaseTotal.BaseBalance = NumberFormat.DecFormat(total, 2);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _superBalanceSessionKey, result);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }

        return result;
    }

    public DownLineBalanceDTO GetMasterBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch, string firstletter, string currency)
    {
        var result = new DownLineBalanceDTO();
        result.DownLineBalane = new List<DownLineBalaneRow>();
        result.Total = new DownLineBalaneRow();
        result.BaseTotal = new DownLineBalaneRow();
        try
        {
            var dtResult = IBCProvider.ReportProvider.DownLineBalance().RptMABalance(db, roleid, custid, isYesterday, usernameSearch, firstletter, currency).ResultSet.Tables[0];

            if (dtResult != null && dtResult.Rows != null && dtResult.Rows.Count > 0)
            {
                Dictionary<int, string> exchange = CacheUtils.Exchange;
                double baseTotal = 0;
                double total = 0;
                foreach (DataRow dr in dtResult.Rows)
                {
                    if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["balance"], 2)) == 0 &&
                            CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                        continue;
                    else
                    {
                        var balanceInfo = new DownLineBalaneRow()
                        {
                            Username = CastAide.AsString(dr["username"]),
                            CustId = CastAide.AsString(dr["custid"]),
                            RoleId = "2",
                            CurrencyId = CastAide.AsString(dr["currency"]),
                            Currency = CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["currency"])),
                            Balance = NumberFormat.DecFormat(dr["balance"], 2),
                            BaseBalance = NumberFormat.DecFormat(dr["basebalance"], 2)
                        };
                        result.DownLineBalane.Add(balanceInfo);
                        baseTotal += CastAide.AsDouble(dr["basebalance"]);
                        total += CastAide.AsDouble(dr["balance"]);
                    }
                }
                result.Total.BaseBalance = NumberFormat.DecFormat(total, 2);
                result.BaseTotal.BaseBalance = NumberFormat.DecFormat(baseTotal, 2);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _masterBalanceSessionKey, result);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }

        return result;
    }

    public DownLineBalanceDTO GetAgentBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch, string firstletter, string currency)
    {
        var result = new DownLineBalanceDTO();
        result.DownLineBalane = new List<DownLineBalaneRow>();
        result.Total = new DownLineBalaneRow();
        result.BaseTotal = new DownLineBalaneRow();
        try
        {
            var dtResult = IBCProvider.ReportProvider.DownLineBalance().RptMABalance(db, roleid, custid, isYesterday, usernameSearch, firstletter, currency).ResultSet.Tables[0];

            if (dtResult != null && dtResult.Rows != null && dtResult.Rows.Count > 0)
            {
                Dictionary<int, string> exchange = CacheUtils.Exchange;
                double baseTotal = 0;
                double total = 0;
                foreach (DataRow dr in dtResult.Rows)
                {
                    if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["bal"], 2)) == 0 &&
                            CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                        continue;
                    else
                    {
                        var balanceInfo = new DownLineBalaneRow()
                        {
                            Username = CastAide.AsString(dr["username"]),
                            CustId = CastAide.AsString(dr["custid"]),
                            RoleId = "1",
                            CurrencyId = CastAide.AsString(dr["currency"]),
                            Currency = CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["currency"])),
                            Balance = NumberFormat.DecFormat(dr["bal"], 2),
                            BaseBalance = NumberFormat.DecFormat(dr["basebalance"], 2)
                        };
                        result.DownLineBalane.Add(balanceInfo);
                        baseTotal += CastAide.AsDouble(dr["basebalance"]);
                        total += CastAide.AsDouble(dr["bal"]);
                    }
                }
                result.Total.BaseBalance = NumberFormat.DecFormat(total, 2);
                result.BaseTotal.BaseBalance = NumberFormat.DecFormat(baseTotal, 2);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _agentBalanceSessionKey, result);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }

        return result;
    }

    public DownLineBalanceDTO GetMemberBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch, string firstletter, string currency)
    {
        var result = new DownLineBalanceDTO();
        result.DownLineBalane = new List<DownLineBalaneRow>();
        result.Total = new DownLineBalaneRow();
        result.BaseTotal = new DownLineBalaneRow();
        try
        {
            var dtResult = IBCProvider.ReportProvider.DownLineBalance().RptMABalance(db, roleid, custid, isYesterday, usernameSearch, firstletter, currency).ResultSet.Tables[0];

            if (dtResult != null && dtResult.Rows != null && dtResult.Rows.Count > 0)
            {
                Dictionary<int, string> exchange = CacheUtils.Exchange;
                double baseTotal = 0;
                double total = 0;
                foreach (DataRow dr in dtResult.Rows)
                {
                    if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["balance"], 2)) == 0 &&
                            CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                        continue;
                    else
                    {
                        var balanceInfo = new DownLineBalaneRow()
                        {
                            Username = CastAide.AsString(dr["username"]),
                            CustId = CastAide.AsString(dr["custid"]),
                            RoleId = "1",
                            CurrencyId = CastAide.AsString(dr["currency"]),
                            Currency = CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["currency"])),
                            Balance = NumberFormat.DecFormat(dr["balance"], 2),
                            BaseBalance = NumberFormat.DecFormat(dr["basebalance"], 2)
                        };
                        result.DownLineBalane.Add(balanceInfo);
                        baseTotal += CastAide.AsDouble(dr["basebalance"]);
                        total += CastAide.AsDouble(dr["balance"]);
                    }
                }
                result.Total.BaseBalance = NumberFormat.DecFormat(total, 2);
                result.BaseTotal.BaseBalance = NumberFormat.DecFormat(baseTotal, 2);
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _memberBalanceSessionKey, result);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }

        return result;
    }

    #region export 2 excel downline balance

    public void ExportExcelDownLineBalance(int roleid, int custid, bool isYesterday, string usernameSearch, string firstletter, string currency, string reportTitle, bool isViewTotalOnly)
    {
        try
        {
            var getData = new DownLineBalanceDTO();
            string level = "";

            #region get data

            switch (roleid)
            {
                case 4:
                    getData = SessionExtension.GetSession(HttpContext.Current.Session, _superBalanceSessionKey) != null
                                  ? (DownLineBalanceDTO)
                                    SessionExtension.GetSession(HttpContext.Current.Session, _superBalanceSessionKey)
                                  : GetSuperBalance("", isYesterday, usernameSearch, firstletter, currency, isViewTotalOnly);
                    level = "Super";
                    break;

                case 3:
                    getData = SessionExtension.GetSession(HttpContext.Current.Session, _masterBalanceSessionKey) != null
                                  ? (DownLineBalanceDTO)
                                    SessionExtension.GetSession(HttpContext.Current.Session, _masterBalanceSessionKey)
                                  : GetMasterBalance("", roleid, custid, isYesterday, usernameSearch, firstletter,
                                                     currency);
                    level = "Master";
                    break;

                case 2:
                    getData = SessionExtension.GetSession(HttpContext.Current.Session, _agentBalanceSessionKey) != null
                                  ? (DownLineBalanceDTO)
                                    SessionExtension.GetSession(HttpContext.Current.Session, _agentBalanceSessionKey)
                                  : GetAgentBalance("", roleid, custid, isYesterday, usernameSearch, firstletter,
                                                    currency);
                    level = "Agent";
                    break;

                case 1:
                    getData = SessionExtension.GetSession(HttpContext.Current.Session, _memberBalanceSessionKey) != null
                                  ? (DownLineBalanceDTO)
                                    SessionExtension.GetSession(HttpContext.Current.Session, _memberBalanceSessionKey)
                                  : GetMemberBalance("", roleid, custid, isYesterday, usernameSearch, firstletter,
                                                     currency);
                    level = "Member";
                    break;

                default:
                    break;
            }

            #endregion get data

            var startRow = 2;
            var startCol = 0;
            var reportName = reportTitle.Replace(" ", "_");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(reportName);
            var nextRow = 0;

            #region Header

            const int headerMergeCols = 4;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + headerMergeCols).
                Value =
                reportTitle;
            ExportExcelDownLineBalanceAssignHeader(ref wsReport, startCol, startRow, level);

            #endregion Header

            if (getData != null && getData.DownLineBalane != null && getData.DownLineBalane.Count > 0)
            {
                var index = 1;
                foreach (DownLineBalaneRow downlineBalance in getData.DownLineBalane)
                {
                    var color = index % 2 > 0 ? Color.FromArgb(255, 255, 255) : Color.FromArgb(255, 243, 200);
                    ExportExcelDownLineBalanceAssignContent(ref wsReport, startCol, startRow, ref nextRow,
                                                            downlineBalance, color, index);
                    index++;
                }
                ExportExcelDownLineBalanceAssignFooter(ref wsReport, startCol, startRow, ref nextRow, getData.BaseTotal);
            }
            else
            {
                ExportExcelDownLineBalanceAssignFooter(ref wsReport, startCol, startRow, ref nextRow, getData.BaseTotal);
            }

            #region response file

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion response file
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    private static void ExportExcelDownLineBalanceAssignHeader(ref ExcelWorksheet wsReport, int startCol, int startRow, string level)
    {
        var headerFormart = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
        var nextCol = 0;
        wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
        wsReport.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "No");
        wsReport.Columns[startCol].Width = 5 * 255;
        wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
        wsReport.Cells[startRow - 1, startCol + nextCol].Value = level;
        wsReport.Columns[startCol + nextCol].Width = 20 * 255;
        wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
        wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Curr");
        wsReport.Columns[startCol + nextCol].Width = 10 * 255;
        wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
        wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Balance");
        wsReport.Columns[startCol + nextCol].Width = 15 * 255;
        wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
        wsReport.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "BaseRM");
        wsReport.Columns[startCol + nextCol].Width = 15 * 255;
    }

    private static void ExportExcelDownLineBalanceAssignContent(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, DownLineBalaneRow downlineBalance, Color color, int index)
    {
        var bgFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", color, false, false);
        var bgIndex = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", color, false, false);
        var nextCol = 0;

        wsReport.Cells[startRow + nextRow, startCol].Style = bgIndex;
        wsReport.Cells[startRow + nextRow, startCol].Value = index;
        wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Style.NumberFormat = "#,##0.00";
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = downlineBalance.Username;
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment =
            HorizontalAlignmentStyle.Center;
        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Style.NumberFormat = "#,##0.00";
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = downlineBalance.Currency;
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment =
            HorizontalAlignmentStyle.Center;
        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Style.NumberFormat = "#,##0.00";
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = downlineBalance.Currency;
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(downlineBalance.Balance);
        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Style.NumberFormat = "#,##0.00";
        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(downlineBalance.BaseBalance);

        nextRow++;
    }

    private static void ExportExcelDownLineBalanceAssignFooter(ref ExcelWorksheet wsReport, int startCol, int startRow, ref int nextRow, DownLineBalaneRow downlineBalance)
    {
        var bgFormat = new ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(211, 225, 200), true, false);

        const int mergCols = 3;
        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergCols).Merged
            = true;
        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergCols).Style
            = bgFormat;
        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergCols).Value
            = "TOTAL";

        wsReport.Cells[startRow + nextRow, startCol + (mergCols + 1)].Style = bgFormat;
        wsReport.Cells[startRow + nextRow, startCol + (mergCols + 1)].Style.NumberFormat = "#,##0.00";
        wsReport.Cells[startRow + nextRow, startCol + (mergCols + 1)].Value = CastAide.AsDouble(downlineBalance.BaseBalance);
    }

    #endregion export 2 excel downline balance

    public DifferenceBalanceInfo GetDifferenceBalance()
    {
        var result = new DifferenceBalanceInfo();
        try
        {
            var dt =
                IBCProvider.ReportProvider.DownLineBalance().BalanceCompHistory(ReportUtils.BalanceUp.BalanceUpDay).
                    ResultSet.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                result.DiffBalance = IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDecimal(dt.Rows[0]["Difference"]));
                result.DiffDate = ReportUtils.BalanceUp.BalanceUpDay.ToString("MMMM dd");
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return result;
    }
}