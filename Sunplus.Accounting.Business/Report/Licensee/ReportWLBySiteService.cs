using System;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Report.Deposit;
using System.Linq;
using System.Collections;
using System.Web;
using Sunplus.Accounting.Entities.Report.Licensee;

namespace Sunplus.Accounting.Business.Report.Deposit
{

    public partial class ReportWLBySiteService : IReportWLBySiteService
    {
        private readonly ILicenseDAO _licenseReportDAO;
        private string _keySelReportByLicensee = AppSession.Username + ".SelReportByLicensee";
        private string _keySelReportByLicenseeV2 = AppSession.Username + ".SelReportByLicenseeV2";
        private string _keySelReportByLicenseeDetail_SAg = AppSession.Username + ".SelReportByLicenseeDetails_SAg";
        private string _keySelReportByLicenseeDetail_MAg = AppSession.Username + ".SelReportByLicenseeDetails_MAg";
        private string _keySelReportByLicenseeDetail_Ag = AppSession.Username + ".SelReportByLicenseeDetails_Ag";
        private string _keySelReportByLicenseeDetail = AppSession.Username + ".SelReportByLicenseeDetails";
        private string _keySelTotalTransction = AppSession.Username + ".SelTotalTransction";
        private double eurCost = 0.13;
        public ReportWLBySiteService(ILicenseDAO licenseDAO)
        {
            _licenseReportDAO = licenseDAO;
        }

        #region SelCurrency
        public LicenseeDTO SelCurrency(string siteId)
        {
            DbResult dbResult = _licenseReportDAO.SelCurrencyByLicensee(siteId);
            LicenseeDTO depositDto = new LicenseeDTO();

            if (dbResult == null)
                return depositDto;

            List<DepositCurrencyBO> list = new List<DepositCurrencyBO>();
            DataTable dataTable = dbResult.ResultSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DepositCurrencyBO deposit = new DepositCurrencyBO()
                {
                    currencyId = CastAide.AsInt32(dataRow["currency"]),
                    currency = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),

                };
                list.Add(deposit);
            }
            depositDto.currency = list;
            return depositDto;
        }
        #endregion

        #region GetDatetimeOption
        public LicenseeDTO GetDatetimeOption(string dateOption)
        {
            //Get FromDate, ToDate
            DateTime fromdate, todate;
            switch (dateOption)
            {
                case "0": //default
                case "1": //today
                    fromdate = todate = DateTime.Now;
                    break;
                case "2": //Yesterday
                    fromdate = DateTime.Now.AddDays(-1);
                    todate = DateTime.Now.AddDays(-1);
                    break;
                case "3": //current week
                    fromdate = DateTime.Now.AddDays(-1 * Convert.ToInt32(DateTime.Now.DayOfWeek));
                    todate = DateTime.Now;
                    break;
                case "4": //last week
                    fromdate = DateTime.Now.AddDays(-1 * Convert.ToInt32(DateTime.Now.DayOfWeek) - 7);
                    todate = fromdate.AddDays(6);
                    break;
                case "5": //Current Month
                    fromdate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1));
                    todate = DateTime.Now;
                    break;
                case "6": //Last Month
                    fromdate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1)).AddMonths(-1);
                    todate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day));
                    break;
                case "7": //Since Last Month
                    fromdate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1)).AddMonths(-1);
                    todate = DateTime.Now;
                    break;

                default:
                    fromdate = todate = DateTime.Now;
                    break;
            }
            LicenseeDTO depositDto = new LicenseeDTO();
            depositDto.fdate = fromdate.ToShortDateString();
            depositDto.tdate = todate.ToShortDateString();
            return depositDto;
        }
        #endregion

        #region formatReportType
        private string formatReportType(string value, string reportType)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            string str = value;

            switch (reportType)
            {
                case "1":
                    if (value.Length == 8)
                    {
                        string mm, dd, yy;
                        mm = value.Substring(4, 2);
                        dd = value.Substring(6, 2);
                        yy = value.Substring(0, 4);
                        str = mm + "/" + dd + "/" + yy;
                    }
                    break;
                case "2":
                    if (value.Length == 6)
                    {
                        string mm, yy;
                        mm = value.Substring(4, 2);
                        yy = value.Substring(0, 4);
                        str = mm + "/" + yy;
                    }
                    break;
                case "3":
                    switch (value)
                    {
                        case "0":
                            str = "Sportsbook";
                            break;
                        case "151":
                            str = "Racebook";
                            break;
                    }
                    break;

            }

            return str;
        }
        #endregion

        #region SelReportByLicenseeFromDB
        LicenseeDTO SelReportByLicenseeFromDB(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, int drawfilter, int excludeTest, int BAFilter)
        {
            LicenseeDTO licenseDto = new LicenseeDTO();
            bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);

            if (BaseCurrency > 0)
                Currency = 0;
            DbResult dbResult = _licenseReportDAO.SelReportByLicensee(LicenseeID, Currency, FromDate, ToDate, showSB,
                                                                          ShowRB, ShowNG, ReportType, MoveBetsDay, IsOld,
                                                                          drawfilter, excludeTest, BAFilter);

            DataTable dataTable = new DataTable();
            if (dbResult == null)
                return licenseDto;

            dataTable = dbResult.ResultSet.Tables[0];
            licenseDto.Items = new List<LicenseeRow>();
            licenseDto.Totals = new List<LicenseeRow>();
            licenseDto.TotalsBaseCurrency = new List<LicenseeRow>();

            //total rows
            //licenseDto.totalrow = dataTable.Rows.Count;

            #region foreach row content

            List<int> lstCurrency = dataTable.AsEnumerable().Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();
            int index = 0;

            string _subfixByBaseCurrency = string.Empty;
            switch (BaseCurrency)
            {
                case 3:
                    _subfixByBaseCurrency = "USD";
                    break;
                case 2:
                    _subfixByBaseCurrency = "MYR";
                    break;
                case 6:
                    _subfixByBaseCurrency = "EUR";
                    break;
                default:
                    _subfixByBaseCurrency = string.Empty;
                    break;
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {
                index++;

                #region AssignRow
                LicenseeRow licenseeRow = new LicenseeRow()
                {
                    index = index,

                    ReportType = formatReportType(CastAide.AsString(dataRow["ReportType"]), ReportType.ToString()),
                    ReportTypeId = CastAide.AsString(dataRow["ReportType"]),
                    //CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),
                    CurrencyName = CastAide.AsString(dataRow["CurrencyName"]),
                    CurrencyId = CastAide.AsString(dataRow["currency"]),

                    Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),
                    Winlost = NumberFormat.DecFormat(dataRow["Winlost" + _subfixByBaseCurrency], 2),
                    Comm = NumberFormat.DecFormat(dataRow["Comm" + _subfixByBaseCurrency], 2),
                    BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["Winlost" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["Comm" + _subfixByBaseCurrency]), 2),

                    AWinlost = NumberFormat.DecFormat(dataRow["AWinlost" + _subfixByBaseCurrency], 2),
                    AComm = NumberFormat.DecFormat(dataRow["AComm" + _subfixByBaseCurrency], 2),
                    ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["AWinlost" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["AComm" + _subfixByBaseCurrency]), 2),

                    Company = NumberFormat.DecFormat(dataRow["Company" + _subfixByBaseCurrency], 2),

                };
                #endregion

                licenseDto.Items.Add(licenseeRow);
                string currency = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"]));
            }
            licenseDto.totalrow = licenseDto.Items.Count;
            #endregion

            string exp = string.Empty;
            #region Total by currency

            foreach (var currency in lstCurrency)
            {
                exp = "currency= " + currency.ToString();
                licenseDto.Totals.Add(new LicenseeRow()
                {
                    CurrencyName = "Total " + CacheUtils.GetExchangeName(currency),
                    CurrencyId = currency.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp), 2),
                    Comm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp), 2),
                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp)), 2),

                    AWinlost = NumberFormat.DecFormat(dataTable.Compute("sum(AWinlost" + _subfixByBaseCurrency + ")", exp), 2),
                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(AComm" + _subfixByBaseCurrency + ")", exp), 2),
                    ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(AWinlost" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(AComm" + _subfixByBaseCurrency + ")", exp)), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + _subfixByBaseCurrency + ")", exp), 2),
                });
            }


            #endregion

            #region Grand Total By Base Currency
            Hashtable baseCurrency = new Hashtable();

            baseCurrency.Add(2, "MYR");
            baseCurrency.Add(3, "USD");
            baseCurrency.Add(6, "EUR");

            foreach (DictionaryEntry curr in baseCurrency)
            {
                exp = string.Empty;
                licenseDto.TotalsBaseCurrency.Add(new LicenseeRow()
                {
                    CurrencyName = "Total " + CacheUtils.GetExchangeName(CastAide.AsInt32(curr.Key)),
                    CurrencyId = curr.Key.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp), 2),
                    Comm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + curr.Value + ")", exp), 2),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(Comm" + curr.Value + ")", exp)), 2),

                    AWinlost = NumberFormat.DecFormat(dataTable.Compute("sum(AWinlost" + curr.Value + ")", exp), 2),
                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(AComm" + curr.Value + ")", exp), 2),
                    ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(AWinlost" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(AComm" + curr.Value + ")", exp)), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + curr.Value + ")", exp), 2),
                });
            }


            #endregion


            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicensee, licenseDto);

            return licenseDto;

        }
        #endregion

        #region SelReportByLicenseeDetail_AgFromDB
        LicenseeDTO SelReportByLicenseeDetail_AgFromDB(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter)
        {

            bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);
            LicenseeDTO licenseDto = new LicenseeDTO();
            try
            {
                licenseDto.Items = new List<LicenseeRow>();
                licenseDto.Totals = new List<LicenseeRow>();
                licenseDto.TotalsBaseCurrency = new List<LicenseeRow>();

                DataTable dataTable = null;
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();

                DbResult dbResult = _licenseReportDAO.SelReportByLicenseeDetail_Ag(LicenseeID, Currency, FromDate, ToDate, showSB, ShowRB, ShowNG, ReportType, MoveBetsDay, IsOld, drawfilter, excludeTest, BAFilter);

                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

                if (dataTable == null)
                    return licenseDto;

                //total rows
                licenseDto.totalrow = dataTable.Rows.Count;

                #region foreach row content
                int from = 0;
                int to = dataTable.Rows.Count;

                int index = 0;

                List<int> lstCurrency = dataTable.AsEnumerable().OrderBy(s => CastAide.AsString(s["currencyName"])).Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();

                string _subfixByBaseCurrency = string.Empty;
                switch (BaseCurrency)
                {
                    case 3:
                        _subfixByBaseCurrency = "USD";
                        break;
                    case 2:
                        _subfixByBaseCurrency = "MYR";
                        break;
                    case 6:
                        _subfixByBaseCurrency = "EUR";
                        break;
                    default:
                        _subfixByBaseCurrency = string.Empty;
                        break;

                }

                string ReportTypeId = string.Empty;
                if (showSB == 1 && ShowRB == 0)
                    ReportTypeId = "Sportsbook";
                else if (showSB == 0 && ShowRB == 1)
                    ReportTypeId = "Racebook";

                for (int i = from; i < to; i++)
                {
                    index++;
                    DataRow dataRow = dataTable.Rows[i];

                    #region AssignRow
                    LicenseeRow licenseeRow = new LicenseeRow()
                    {
                        index = index,

                        ReportTypeId = ReportTypeId,

                        Username = CastAide.AsString(dataRow["username"]),
                        CustId = CastAide.AsString(dataRow["custid"]),
                        CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),
                        CurrencyId = CastAide.AsString(dataRow["currency"]),

                        Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),
                        Winlost = NumberFormat.DecFormat(dataRow["Winlost" + _subfixByBaseCurrency], 2),
                        Comm = NumberFormat.DecFormat(dataRow["Comm" + _subfixByBaseCurrency], 2),
                        BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 2),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["Winlost" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["Comm" + _subfixByBaseCurrency]), 2),

                        AWinlost = NumberFormat.DecFormat(dataRow["AWinlost" + _subfixByBaseCurrency], 2),
                        AComm = NumberFormat.DecFormat(dataRow["AComm" + _subfixByBaseCurrency], 2),
                        ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["AWinlost" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["AComm" + _subfixByBaseCurrency]), 2),

                        Company = NumberFormat.DecFormat(dataRow["Company" + _subfixByBaseCurrency], 2),

                    };
                    #endregion

                    licenseDto.Items.Add(licenseeRow);
                }

                #endregion

                string exp = string.Empty;
                #region Total by currency

                foreach (var currency in lstCurrency)
                {
                    exp = "currency= " + currency.ToString();
                    licenseDto.Totals.Add(new LicenseeRow()
                    {
                        CurrencyName = "Total " + CacheUtils.GetExchangeName(currency),
                        CurrencyId = currency.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),
                        Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp), 2),
                        Comm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp), 2),
                        BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 2),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp)), 2),

                        AWinlost = NumberFormat.DecFormat(dataTable.Compute("sum(AWinlost" + _subfixByBaseCurrency + ")", exp), 2),
                        AComm = NumberFormat.DecFormat(dataTable.Compute("sum(AComm" + _subfixByBaseCurrency + ")", exp), 2),
                        ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(AWinlost" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(AComm" + _subfixByBaseCurrency + ")", exp)), 2),

                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + _subfixByBaseCurrency + ")", exp), 2),
                    });
                }


                #endregion

                #region Grand Total By Base Currency
                Hashtable baseCurrency = new Hashtable();

                baseCurrency.Add(2, "MYR");
                baseCurrency.Add(3, "USD");
                baseCurrency.Add(6, "EUR");

                foreach (DictionaryEntry curr in baseCurrency)
                {
                    exp = string.Empty;
                    licenseDto.TotalsBaseCurrency.Add(new LicenseeRow()
                    {
                        CurrencyName = "Total " + CacheUtils.GetExchangeName(CastAide.AsInt32(curr.Key)),
                        CurrencyId = curr.Key.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),
                        Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp), 2),
                        Comm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + curr.Value + ")", exp), 2),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(Comm" + curr.Value + ")", exp)), 2),

                        AWinlost = NumberFormat.DecFormat(dataTable.Compute("sum(AWinlost" + curr.Value + ")", exp), 2),
                        AComm = NumberFormat.DecFormat(dataTable.Compute("sum(AComm" + curr.Value + ")", exp), 2),
                        ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(AWinlost" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(AComm" + curr.Value + ")", exp)), 2),

                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + curr.Value + ")", exp), 2),
                    });
                }


                #endregion
            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicenseeDetail_Ag, licenseDto);

            return licenseDto;
        }
        #endregion

        #region SelReportByLicenseeDetailFromDB
        LicenseeDTO SelReportByLicenseeDetailFromDB(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, int drawfilter, int excludeTest, int BAFilter)
        {
            LicenseeDTO licenseDto = new LicenseeDTO();
            bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);


            DbResult dbResult = _licenseReportDAO.SelReportByLicenseeDetail(LicenseeID, Currency, CustId, FromDate, ToDate, showSB,
                                                                          ShowRB, ShowNG, ReportType, MoveBetsDay, IsOld,
                                                                          drawfilter, excludeTest, BAFilter);

            DataTable dataTable = new DataTable();
            if (dbResult == null)
                return licenseDto;

            dataTable = dbResult.ResultSet.Tables[0];

            licenseDto.Items = new List<LicenseeRow>();
            licenseDto.Totals = new List<LicenseeRow>();
            licenseDto.TotalsBaseCurrency = new List<LicenseeRow>();
            //total rows
            licenseDto.totalrow = dataTable.Rows.Count;

            #region foreach row content

            int index = 0;

            List<int> lstCurrency = dataTable.AsEnumerable().OrderBy(s => CastAide.AsString(s["currencyName"])).Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();

            string _subfixByBaseCurrency = string.Empty;
            switch (BaseCurrency)
            {
                case 3:
                    _subfixByBaseCurrency = "USD";
                    break;
                case 2:
                    _subfixByBaseCurrency = "MYR";
                    break;
                case 6:
                    _subfixByBaseCurrency = "EUR";
                    break;
                default:
                    _subfixByBaseCurrency = string.Empty;
                    break;

            }

            string ReportTypeId = string.Empty;
            if (showSB == 1 && ShowRB == 0)
                ReportTypeId = "Sportsbook";
            else if (showSB == 0 && ShowRB == 1)
                ReportTypeId = "Racebook";

            foreach (DataRow dataRow in dataTable.Rows)
            {
                index++;

                #region AssignRow
                LicenseeRow licenseeRow = new LicenseeRow()
                {
                    index = index,

                    ReportTypeId = ReportTypeId,

                    Username = CastAide.AsString(dataRow["username"]),
                    CustId = CastAide.AsString(dataRow["custid"]),
                    CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),
                    CurrencyId = CastAide.AsString(dataRow["currency"]),

                    Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),
                    Winlost = NumberFormat.DecFormat(dataRow["Winlost" + _subfixByBaseCurrency], 2),
                    Comm = NumberFormat.DecFormat(dataRow["Comm" + _subfixByBaseCurrency], 2),
                    BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["Winlost" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["Comm" + _subfixByBaseCurrency]), 2),

                    AWinlost = NumberFormat.DecFormat(dataRow["AWinlost" + _subfixByBaseCurrency], 2),
                    AComm = NumberFormat.DecFormat(dataRow["AComm" + _subfixByBaseCurrency], 2),
                    ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["AWinlost" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["AComm" + _subfixByBaseCurrency]), 2),

                    Company = NumberFormat.DecFormat(dataRow["Company" + _subfixByBaseCurrency], 2),

                };
                #endregion

                licenseDto.Items.Add(licenseeRow);
            }

            #endregion

            string exp = string.Empty;
            #region Total by currency

            foreach (var currency in lstCurrency)
            {
                exp = "currency= " + currency.ToString();
                licenseDto.Totals.Add(new LicenseeRow()
                {
                    CurrencyName = "Total " + CacheUtils.GetExchangeName(currency),
                    CurrencyId = currency.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp), 2),
                    Comm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp), 2),
                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp)), 2),

                    AWinlost = NumberFormat.DecFormat(dataTable.Compute("sum(AWinlost" + _subfixByBaseCurrency + ")", exp), 2),
                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(AComm" + _subfixByBaseCurrency + ")", exp), 2),
                    ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(AWinlost" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(AComm" + _subfixByBaseCurrency + ")", exp)), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + _subfixByBaseCurrency + ")", exp), 2),
                });
            }


            #endregion

            #region Grand Total By Base Currency
            Hashtable baseCurrency = new Hashtable();

            baseCurrency.Add(2, "MYR");
            baseCurrency.Add(3, "USD");
            baseCurrency.Add(6, "EUR");

            foreach (DictionaryEntry curr in baseCurrency)
            {
                exp = string.Empty;
                licenseDto.TotalsBaseCurrency.Add(new LicenseeRow()
                {
                    CurrencyName = "Total " + CacheUtils.GetExchangeName(CastAide.AsInt32(curr.Key)),
                    CurrencyId = curr.Key.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp), 2),
                    Comm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + curr.Value + ")", exp), 2),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(Comm" + curr.Value + ")", exp)), 2),
                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),

                    AWinlost = NumberFormat.DecFormat(dataTable.Compute("sum(AWinlost" + curr.Value + ")", exp), 2),
                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(AComm" + curr.Value + ")", exp), 2),
                    ATotal = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(AWinlost" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(AComm" + curr.Value + ")", exp)), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + curr.Value + ")", exp), 2),
                });
            }


            #endregion

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicenseeDetail, licenseDto);

            return licenseDto;

        }
        #endregion

        #region SelReportByLicensee
        public LicenseeDTO SelReportByLicensee(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache)
        {

            LicenseeDTO licenseDto = new LicenseeDTO();
            LicenseeDTO licenseDtoPaging = new LicenseeDTO();

            try
            {
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                           _keySelReportByLicensee);
                if (sessionLicenseDto == null || fromCache == 0)
                    licenseDto = SelReportByLicenseeFromDB(LicenseeID, Currency, BaseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ReportType, MoveBetsDay, drawfilter, excludeTest, BAFilter);
                else
                {
                    licenseDto = (LicenseeDTO)sessionLicenseDto;
                }

                licenseDtoPaging.Items = licenseDto.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                licenseDtoPaging.Totals = licenseDto.Totals;
                licenseDtoPaging.TotalsBaseCurrency = licenseDto.TotalsBaseCurrency;
                licenseDtoPaging.totalrow = licenseDto.totalrow;

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDtoPaging;
        }
        #endregion

        #region ExportDepositReport
        ExcelWorksheet AssignDataExcel(string level, string currency, string baseCurrency, ref ExcelWorksheet ws, int indexRow, DataRow dataRow, string reportType, int r, int g, int b)
        {
            var gembox = new ExcelGemboxUtils();

            string curr = "";
            if (baseCurrency == "3")
                curr = "USD";
            else if (baseCurrency == "2")
                curr = "RM";
            if (curr.Length > 0)
                currency = baseCurrency;

            if (level == "1")
            {
                ws.Cells[indexRow, 0].Value = formatReportType(CastAide.AsString(dataRow["reportType"]), reportType);
                ws.Cells[indexRow, 0].Style = gembox.GetStyleNumber(0, "", r, g, b);

                ws.Cells[indexRow, 1].Value = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"]));
                ws.Cells[indexRow, 1].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
            }
            else
            {
                ws.Cells[indexRow, 0].Value = CastAide.AsString(dataRow["username"]);
                ws.Cells[indexRow, 0].Style = gembox.GetStyleNumber(0, "", r, g, b);

                ws.Cells[indexRow, 1].Value = CacheUtils.GetExchangeName(CastAide.AsInt32(currency));
                ws.Cells[indexRow, 1].Style = gembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
            }


            ws.Cells[indexRow, 2].Value = CastAide.AsDecimal(dataRow["Turnover" + curr]);
            ws.Cells[indexRow, 2].Style = gembox.GetStyleNumber(0, "", r, g, b);

            ws.Cells[indexRow, 3].Value = CastAide.AsDecimal(dataRow["GrossComm" + curr]);
            ws.Cells[indexRow, 3].Style = gembox.GetStyleNumber(0, "", r, g, b);

            ws.Cells[indexRow, 4].Value = CastAide.AsDecimal(dataRow["Winlost" + curr]);
            ws.Cells[indexRow, 4].Style = gembox.GetStyleNumber(0, "", r, g, b);

            ws.Cells[indexRow, 5].Value = CastAide.AsDecimal(dataRow["Comm" + curr]);
            ws.Cells[indexRow, 5].Style = gembox.GetStyleNumber(0, "", r, g, b);

            ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(dataRow["Winlost" + curr]) + CastAide.AsDecimal(dataRow["Comm" + curr]);
            ws.Cells[indexRow, 6].Style = gembox.GetStyleNumber(0, "", r, g, b);

            ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(dataRow["AWinlost" + curr]);
            ws.Cells[indexRow, 7].Style = gembox.GetStyleNumber(0, "", r, g, b);

            ws.Cells[indexRow, 8].Value = CastAide.AsDecimal(dataRow["AComm" + curr]);
            ws.Cells[indexRow, 8].Style = gembox.GetStyleNumber(0, "", r, g, b);

            ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(dataRow["AWinlost" + curr]) + CastAide.AsDecimal(dataRow["AComm" + curr]);
            ws.Cells[indexRow, 9].Style = gembox.GetStyleNumber(0, "", r, g, b);

            ws.Cells[indexRow, 10].Value = CastAide.AsDecimal(dataRow["Company" + curr]);
            ws.Cells[indexRow, 10].Style = gembox.GetStyleNumber(0, "", r, g, b);
            return ws;
        }

        ExcelWorksheet AssignDataTotalCurrencyExcel(string level, ref ExcelWorksheet ws, ref int indexRow, ArrayList arrCurrency, string baseCurrency, DataTable dataTable, int r, int g, int b)
        {
            var gembox = new ExcelGemboxUtils();
            string strCurr = "";
            if (baseCurrency == "3")
                strCurr = "USD";
            else if (baseCurrency == "2")
                strCurr = "RM";

            foreach (int curr in arrCurrency)
            {
                string filler = "currency = " + curr;
                if (level == "2" || level == "3")
                    filler = "";



                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Value = "Total " + CacheUtils.GetExchangeName(curr);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Style = gembox.GetStyleTextTotal(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Merged = true;

                ws.Cells[indexRow, 2].Value = dataTable.Compute("sum(Turnover" + strCurr + ")", filler);
                ws.Cells[indexRow, 2].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 3].Value = dataTable.Compute("sum(GrossComm" + strCurr + ")", filler);
                ws.Cells[indexRow, 3].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 4].Value = dataTable.Compute("sum(Winlost" + strCurr + ")", filler);
                ws.Cells[indexRow, 4].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 5].Value = dataTable.Compute("sum(Comm" + strCurr + ")", filler);
                ws.Cells[indexRow, 5].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + strCurr + ")", filler)) +
                                              CastAide.AsDecimal(dataTable.Compute("sum(Comm" + strCurr + ")", filler));
                ws.Cells[indexRow, 6].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 7].Value = dataTable.Compute("sum(AWinlost" + strCurr + ")", filler);
                ws.Cells[indexRow, 7].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 8].Value = dataTable.Compute("sum(AComm" + strCurr + ")", filler);
                ws.Cells[indexRow, 8].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(dataTable.Compute("sum(AWinlost" + strCurr + ")", filler)) +
                                              CastAide.AsDecimal(dataTable.Compute("sum(AComm" + strCurr + ")", filler));
                ws.Cells[indexRow, 9].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 10].Value = dataTable.Compute("sum(Company" + strCurr + ")", filler);
                ws.Cells[indexRow, 10].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                indexRow++;
            }


            return ws;
        }

        ExcelWorksheet AssignDataGrandTotalCurrencyExcel(ref ExcelWorksheet ws, ref int indexRow, DataTable dataTable, int r, int g, int b)
        {
            var gembox = new ExcelGemboxUtils();
            ArrayList arrCurrency = new ArrayList();
            arrCurrency.Add("RM");
            arrCurrency.Add("USD");
            foreach (string curr in arrCurrency)
            {
                string filler = "";

                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Value = "Total " + (curr == "USD" ? "US$" : curr);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Style = gembox.GetStyleTextTotal(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Merged = true;

                ws.Cells[indexRow, 2].Value = dataTable.Compute("sum(Turnover" + curr + ")", filler);
                ws.Cells[indexRow, 2].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 3].Value = dataTable.Compute("sum(GrossComm" + curr + ")", filler);
                ws.Cells[indexRow, 3].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 4].Value = dataTable.Compute("sum(Winlost" + curr + ")", filler);
                ws.Cells[indexRow, 4].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 5].Value = dataTable.Compute("sum(Comm" + curr + ")", filler);
                ws.Cells[indexRow, 5].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr + ")", filler)) +
                                              CastAide.AsDecimal(dataTable.Compute("sum(Comm" + curr + ")", filler));
                ws.Cells[indexRow, 6].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 7].Value = dataTable.Compute("sum(AWinlost" + curr + ")", filler);
                ws.Cells[indexRow, 7].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 8].Value = dataTable.Compute("sum(AComm" + curr + ")", filler);
                ws.Cells[indexRow, 8].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 9].Value = CastAide.AsDecimal(dataTable.Compute("sum(AWinlost" + curr + ")", filler)) +
                                              CastAide.AsDecimal(dataTable.Compute("sum(AComm" + curr + ")", filler));
                ws.Cells[indexRow, 9].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                ws.Cells[indexRow, 10].Value = dataTable.Compute("sum(Company" + curr + ")", filler);
                ws.Cells[indexRow, 10].Style = gembox.GetStyleNumberTotal(0, "", r, g, b);

                indexRow++;
            }


            return ws;
        }
        public ExcelFile ExportLicenseeReport(string site, string fdate, string tdate, ref bool hasData, string reportType, string level, string currency, string baseCurrency)
        {
            hasData = true;
            ExcelFile excelFile = new ExcelFile();
            string key = "";
            if (level == "1")
                key = _keySelReportByLicensee + "." + site;
            //else
            //    key = _keySelReportDepositDetailsBySite + "." + site;
            var dataTable = AppSession.GetSession(System.Web.HttpContext.Current.Session, key) as DataTable;
            if (dataTable == null)
                hasData = false;

            var gembox = new ExcelGemboxUtils();

            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            ExcelWorksheet ws = null;
            string tag = "Deposit report for " + site;
            ws = worksheets.Add(tag);
            int indexRow = 0;

            #region header
            ws.Cells.GetSubrangeAbsolute(0, 0, 1, 10).Value = "Deposit report for site " + site + " from " + fdate + " to " + tdate;
            ws.Cells.GetSubrangeAbsolute(0, 0, 1, 10).Style = gembox.GetStyleTitle(205, 222, 243);
            ws.Cells.GetSubrangeAbsolute(0, 0, 1, 10).Merged = true;

            ws.Cells.GetSubrangeAbsolute(2, 0, 3, 0).Value = "Date";
            ws.Cells.GetSubrangeAbsolute(2, 0, 3, 0).Style = gembox.GetStyleTitle(205, 222, 243);
            ws.Cells.GetSubrangeAbsolute(2, 0, 3, 0).Merged = true;

            ws.Cells.GetSubrangeAbsolute(2, 1, 3, 1).Value = "Currency";
            ws.Cells.GetSubrangeAbsolute(2, 1, 3, 1).Style = gembox.GetStyleTitle(205, 222, 243);
            ws.Cells.GetSubrangeAbsolute(2, 1, 3, 1).Merged = true;

            ws.Cells.GetSubrangeAbsolute(2, 2, 3, 2).Value = "Turnover";
            ws.Cells.GetSubrangeAbsolute(2, 2, 3, 2).Style = gembox.GetStyleTitle(205, 222, 243);
            ws.Cells.GetSubrangeAbsolute(2, 2, 3, 2).Merged = true;

            ws.Cells.GetSubrangeAbsolute(2, 3, 3, 3).Value = "Gross Comm";
            ws.Cells.GetSubrangeAbsolute(2, 3, 3, 3).Style = gembox.GetStyleTitle(205, 222, 243);
            ws.Cells.GetSubrangeAbsolute(2, 3, 3, 3).Merged = true;

            ws.Cells.GetSubrangeAbsolute(2, 4, 2, 6).Value = "Member";
            ws.Cells.GetSubrangeAbsolute(2, 4, 2, 6).Style = gembox.GetStyleTitle(205, 222, 243);
            ws.Cells.GetSubrangeAbsolute(2, 4, 2, 6).Merged = true;

            ws.Cells.GetSubrangeAbsolute(2, 7, 2, 9).Value = "Agent";
            ws.Cells.GetSubrangeAbsolute(2, 7, 2, 9).Style = gembox.GetStyleTitle(205, 222, 243);
            ws.Cells.GetSubrangeAbsolute(2, 7, 2, 9).Merged = true;

            ws.Cells.GetSubrangeAbsolute(2, 10, 3, 10).Value = "Company";
            ws.Cells.GetSubrangeAbsolute(2, 10, 3, 10).Style = gembox.GetStyleTitle(205, 222, 243);
            ws.Cells.GetSubrangeAbsolute(2, 10, 3, 10).Merged = true;

            ws.Cells[3, 4].Value = "Winloss";
            ws.Cells[3, 4].Style = gembox.GetStyleTitle(205, 222, 243);

            ws.Cells[3, 5].Value = "Comm";
            ws.Cells[3, 5].Style = gembox.GetStyleTitle(205, 222, 243);

            ws.Cells[3, 6].Value = "Total";
            ws.Cells[3, 6].Style = gembox.GetStyleTitle(205, 222, 243);

            ws.Cells[3, 7].Value = "Winloss";
            ws.Cells[3, 7].Style = gembox.GetStyleTitle(205, 222, 243);

            ws.Cells[3, 8].Value = "Comm";
            ws.Cells[3, 8].Style = gembox.GetStyleTitle(205, 222, 243);

            ws.Cells[3, 9].Value = "Total";
            ws.Cells[3, 9].Style = gembox.GetStyleTitle(205, 222, 243);
            #endregion

            #region content

            indexRow = 4;
            int r, g, b;
            ArrayList arrCurrency = new ArrayList();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (indexRow % 2 == 0)
                {
                    r = g = b = 255;
                }
                else
                {
                    r = 255;
                    g = 243;
                    b = 200;
                }
                if (level == "1")
                {
                    if (!arrCurrency.Contains(CastAide.AsInt32(dataRow["currency"])))
                    {
                        arrCurrency.Add(dataRow["currency"]);
                    }
                }
                else
                {
                    if (!arrCurrency.Contains(CastAide.AsInt32(currency)))
                    {
                        arrCurrency.Add(CastAide.AsInt32(currency));
                    }
                }

                AssignDataExcel(level, currency, baseCurrency, ref ws, indexRow, dataRow, reportType, r, g, b);

                indexRow++;
            }
            #endregion

            #region footer

            r = 255;
            g = 238;
            b = 221;
            AssignDataTotalCurrencyExcel(level, ref ws, ref indexRow, arrCurrency, baseCurrency, dataTable, r, g, b);

            r = 205;
            g = 222;
            b = 243;
            AssignDataGrandTotalCurrencyExcel(ref ws, ref indexRow, dataTable, r, g, b);
            #endregion

            for (int i = 0; i <= 11; i++)
            {
                ws.Columns[i].Width = 120 * 30;
            }

            return excelFile;
        }
        #endregion

        #region GetConcurrentUser

        private const string SessionPeakUser = "peakuser";

        public ConcurrentUserDTO GetConcurrentUser(DateTime dateFrom, DateTime dateTo, string licenseSite)
        {
            dateFrom = DNAide.Utils.FormatHandler.ResetStartDate(dateFrom);
            dateTo = DNAide.Utils.FormatHandler.ResetEndDate(dateTo);
            var listConcurrent = new List<ConcurrentUser>();
            var dbResult = _licenseReportDAO.SelConcurrentUser(dateFrom, dateTo, licenseSite);
            double countRow = 0;
            double totalUser = 0;
            var listTmp = new List<int>();
            foreach (DataRow row in dbResult.Rows)
            {
                var concurrent = new ConcurrentUser();
                concurrent.Date = string.Format("{0:MM/dd/yyyy}", row["logdate"]);
                concurrent.TotalUser = CastAide.AsInt32(row["totaluser"]);
                totalUser += concurrent.TotalUser;
                listConcurrent.Add(concurrent);
                listTmp.Add(concurrent.TotalUser);
                countRow++;
            }


            var concurrentUserDto = new ConcurrentUserDTO();
            concurrentUserDto.ListConcurrentUser = listConcurrent;
            if (countRow > 0)
            {
                concurrentUserDto.AverageUser = Convert.ToDouble(NumberFormat.DecFormat(totalUser / countRow, 2));
                concurrentUserDto.MinUser = listTmp.Min();
                concurrentUserDto.MaxUser = listTmp.Max();
                concurrentUserDto.Total = totalUser;
            }
            SessionExtension.AddSession(HttpContext.Current.Session, SessionPeakUser, concurrentUserDto);

            return concurrentUserDto;
        }



        #endregion

        #region PeakUserExport2Excel
        public void PeakUserExport2Excel(DateTime dateFrom, DateTime dateTo, string licenseSite)
        {
            var objListPeakUser = SessionExtension.GetSession(HttpContext.Current.Session, SessionPeakUser);

            var result = CommonBootStrapper.Locator.GetInstance<IReportWLBySiteService>();

            var con = objListPeakUser != null
                                        ? (ConcurrentUserDTO)objListPeakUser
                                        : result.GetConcurrentUser(CastAide.AsDateTime(dateFrom),
                                                                   CastAide.AsDateTime(dateTo), licenseSite);
            var ef = new ExcelFile();
            var ws = ef.Worksheets.Add("PeakConcurentUser");
            var peakGembox = new ExcelGemboxUtils();

            ws.Columns[0].Width = 120 * 70;
            ws.Columns[1].Width = 120 * 50;
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Value = "Peak Concurrent User Report of site " + licenseSite + " from " + String.Format("{0:MM/dd/yyyy}", dateFrom) + " to " +
                                   String.Format("{0:MM/dd/yyyy}", dateTo);
            ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Merged = true;
            ws.Cells[0, 0].Value = "Peak Concurrent User Report of site " + licenseSite + " from " + String.Format("{0:MM/dd/yyyy}", dateFrom) + " to " +
                                   String.Format("{0:MM/dd/yyyy}", dateTo);
            ws.Cells[0, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Cells[1, 0].Value = "Date";
            ws.Cells[1, 0].Style = peakGembox.GetStyleTitle(181, 196, 208);
            ws.Cells[1, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Cells[1, 1].Value = "Peak Concurrent Users";
            ws.Cells[1, 1].Style.Font.Weight = ExcelFont.BoldWeight;
            ws.Cells[1, 1].Style = peakGembox.GetStyleTitle(181, 196, 208);

            if (con != null && con.ListConcurrentUser.Count > 0)
            {
                var i = 2;
                int r = 0, g = 0, b = 0;
                for (var j = 2; j < con.ListConcurrentUser.Count + 2; j++)
                {
                    if (j % 2 == 0)
                    {
                        r = 217;
                        g = 232;
                        b = 235;
                    }
                    else
                    {
                        r = 248;
                        g = 248;
                        b = 248;
                    }
                    ws.Cells[j, 0].Value = con.ListConcurrentUser[j - 2].Date;
                    ws.Cells[j, 0].Style = peakGembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    ws.Cells[j, 1].Value = con.ListConcurrentUser[j - 2].TotalUser;
                    ws.Cells[j, 1].Style = peakGembox.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    i = j;

                }

                ws.Cells[++i, 0].Value = "Total Peak Concurrent User";
                ws.Columns[i].Width = 120 * 70;
                ws.Cells[i, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[i, 0].Style = peakGembox.GetStyleTitle(181, 196, 208);
                ws.Cells[i, 1].Value = con.Total;
                ws.Cells[i, 1].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[i, 1].Style = peakGembox.GetStyleTitle(181, 196, 208);

                ws.Cells[++i, 0].Value = "Average Peak Concurrent User";
                ws.Columns[i].Width = 120 * 70;
                ws.Cells[i, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[i, 0].Style = peakGembox.GetStyleTitle(181, 196, 208);
                ws.Cells[i, 1].Value = con.AverageUser;
                ws.Cells[i, 1].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[i, 1].Style = peakGembox.GetStyleTitle(181, 196, 208);

                ws.Cells[++i, 0].Value = "Max Peak Concurrent User";
                ws.Columns[i].Width = 120 * 70;
                ws.Cells[i, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[i, 0].Style = peakGembox.GetStyleTitle(181, 196, 208);
                ws.Cells[i, 1].Value = con.MaxUser;
                ws.Cells[i, 1].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[i, 1].Style = peakGembox.GetStyleTitle(181, 196, 208);

                ws.Cells[++i, 0].Value = "Min Peak Concurrent User";
                ws.Columns[i].Width = 120 * 70;
                ws.Cells[i, 0].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[i, 0].Style = peakGembox.GetStyleTitle(181, 196, 208);
                ws.Cells[i, 1].Value = con.MinUser;
                ws.Cells[i, 1].Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells[i, 1].Style = peakGembox.GetStyleTitle(181, 196, 208);
            }
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "Application/x-msexcel";
            response.AddHeader("Content-Disposition", "attachment; filename=PeakConcurrentUsers-" + dateFrom.ToShortDateString() + "-" + dateTo.ToShortDateString() + ".xls");
            ef.SaveXls(response.OutputStream);
        }

        public void Export2ExcelReportByLicensee(string reportTitle, int licenseeId, int currency, int BaseCurrency, string fromDate, string toDate, int showSb, int showRb, int reportType, int drawFilter, int pageIndex, int pageSize, int reportSelected, int custId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region SelLicensees
        public List<Item> SelLicensees(bool IsAll)
        {
            List<Item> lstSite = new List<Item>();
            try
            {
                DataTable dataTable = _licenseReportDAO.GetLicenseeList();
                DataRow[] rows = dataTable.Select();
                foreach (DataRow row in rows)
                {
                    var licensee = new Item()
                    {
                        Id = CastAide.AsString(row["LicenseeID"]),
                        Name = CastAide.AsString(row["LicenseeName"]),
                        EnableBA = CastAide.AsBoolean(row["EnableBA"]),
                        BrandName = CastAide.AsString(row["BrandName"]),
                        Level = "2",
                    };

                    if (licensee.Name.ToLower().Equals("12bet"))
                        licensee.Level = "3";
                    lstSite.Add(licensee);
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return lstSite;
        }

        public List<Item> SelLicensees(bool IsAll, bool Checkauthentication)
        {

            List<Item> lstSite = new List<Item>();

            try
            {


                DataTable dataTable = _licenseReportDAO.GetLicenseeList();




                foreach (DataRow row in dataTable.Rows)
                {


                    lstSite.Add(new Item()
                    {
                        Id = CastAide.AsString(row["LicenseeID"]),
                        Name = CastAide.AsString(row["LicenseeName"]),
                        EnableBA = CastAide.AsBoolean(row["EnableBA"]),
                    });
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return lstSite;
        }
        #endregion

        #region SelReportByLicenseeDetail_Ag
        public LicenseeDTO SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache)
        {
            LicenseeDTO licenseDto = new LicenseeDTO();
            LicenseeDTO licenseDtoPaging = new LicenseeDTO();
            var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                          _keySelReportByLicenseeDetail_Ag);


            try
            {

                DataTable dataTable = null;

                if (sessionLicenseDto == null || fromCache == 0)
                    licenseDto = SelReportByLicenseeDetail_AgFromDB(LicenseeID, Currency, BaseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ReportType, drawfilter, excludeTest, BAFilter);
                else
                {
                    licenseDto = (LicenseeDTO)sessionLicenseDto;
                }
                licenseDtoPaging.Items = licenseDto.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                licenseDtoPaging.Totals = licenseDto.Totals;
                licenseDtoPaging.TotalsBaseCurrency = licenseDto.TotalsBaseCurrency;
                licenseDtoPaging.totalrow = licenseDto.totalrow;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDtoPaging;
        }
        #endregion

        #region SelReportByLicenseeDetail
        public LicenseeDTO SelReportByLicenseeDetail(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache)
        {
            LicenseeDTO licenseDto = new LicenseeDTO();
            LicenseeDTO licenseDtoPaging = new LicenseeDTO();
            var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                          _keySelReportByLicenseeDetail);
            try
            {

                DataTable dataTable = null;
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();

                if (sessionLicenseDto == null || fromCache == 0)
                    licenseDto = SelReportByLicenseeDetailFromDB(LicenseeID, Currency, BaseCurrency, CustId, FromDate, ToDate, showSB, ShowRB, ShowNG, ReportType, MoveBetsDay, drawfilter, excludeTest, BAFilter);
                else
                {
                    licenseDto = (LicenseeDTO)sessionLicenseDto;

                }
                licenseDtoPaging.Items = licenseDto.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                licenseDtoPaging.Totals = licenseDto.Totals;
                licenseDtoPaging.TotalsBaseCurrency = licenseDto.TotalsBaseCurrency;
                licenseDtoPaging.totalrow = licenseDto.totalrow;
            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDtoPaging;
        }
        #endregion

        #region SelDateTotalTransction
        public LicenseeTotalTransctionDTO SelDateTotalTransction()
        {
            LicenseeTotalTransctionDTO transctionDto = new LicenseeTotalTransctionDTO();
            transctionDto.Items = new List<Reseller>();
            transctionDto.SelDate = new List<string>();
            transctionDto.Total = new Reseller();

            try
            {

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return transctionDto;
        }
        #endregion

        #region SelTotalTransction

        public List<Reseller> GetLicenseeByReseller()
        {
            var result = new List<Reseller>();

            try
            {
                DataTable tb = _licenseReportDAO.SelLicenseeByReseller();
                //tb.DefaultView.Sort = "ResellerID asc";
                DataRow[] resellerRows = tb.DefaultView.ToTable(true, "ResellerID", "ResellerName").Select();

                foreach (DataRow row in resellerRows)
                {
                    var reseller = new Reseller()
                    {
                        Licensees = new List<Licensee>(),
                        ResellerID = row["ResellerID"].ToString(),
                        ResellerName = row["ResellerName"].ToString()
                    };

                    DataRow[] licenseeRows = tb.Select("ResellerID=" + reseller.ResellerID);
                    foreach (DataRow _row in licenseeRows)
                    {
                        reseller.Licensees.Add(new Licensee()
                        {
                            LicenseeID = _row["LicenseeID"].ToString(),
                            LicenseeName = _row["LicenseeName"].ToString()
                        });
                    }
                    result.Add(reseller);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;

        }

        public LicenseeTotalTransctionDTO SelTotalTransction(int resellerid, int licenseeid, string date, bool showSB, bool showNG, bool showRB, bool showLC, bool showVS)
        {
            LicenseeTotalTransctionDTO transctionDto = new LicenseeTotalTransctionDTO();
            transctionDto.Items = new List<Reseller>();
            transctionDto.SelDate = new List<string>();

            try
            {
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                string BalanceUpDay = ReportUtils.BalanceUp.BalanceUpDay.ToShortDateString();


                DataTable rates = _licenseReportDAO.GetRates(date);
                DataTable tb = _licenseReportDAO.SelTotalTicket(resellerid, licenseeid, date, MoveBetsDay, BalanceUpDay, showSB, showNG, showRB, showLC, showVS);
                if (tb == null)
                    return transctionDto;

                //tb.DefaultView.Sort = "ResellerName asc";
                double total = CastAide.AsDouble(tb.Compute("sum(BetCount)", string.Empty));
                DataRow[] resellerRows = tb.DefaultView.ToTable(true, "ResellerName", "ResellerID").Select();
                var totalEuro = 0.0;


                foreach (DataRow row in resellerRows)
                {
                    var resellerName = row["ResellerName"].ToString();
                    var resllerId = CastAide.AsInt32(row["ResellerID"]);
                    var reseller = new Reseller()
                    {
                        Licensees = new List<Licensee>(),
                        ResellerName = resellerName
                    };

                    var licenseeRows = tb.Select("ResellerName = '" + resellerName + "'");

                    foreach (DataRow _row in licenseeRows)
                    {
                        var betCount = CastAide.AsDouble(_row["BetCount"]);
                        //var rate = this.GetResellerRate(rates, resllerId, betCount);

                        reseller.Licensees.Add(new Licensee()
                        {
                            LicenseeName = _row["LicenseeName"].ToString(),
                            Total = NumberFormat.DecFormat(betCount, 0),

                            EURCost = resellerName.ToLower() == "vcasia" ? "N/A" : FormatHandler.FormatNumber2(betCount * this.GetResellerRate(rates, resllerId, betCount), "en-EN")
                        });
                    }
                    double subtotal = reseller.Licensees.Sum(l => CastAide.AsDouble(l.Total));
                    reseller.Total = NumberFormat.DecFormat(subtotal, 0);

                    double subtotalEuro;

                    if (resellerName.ToLower() == "vcasia")
                    {
                        reseller.EURCost = "N/A";
                        subtotalEuro = 0;
                    }
                    else
                    {
                        subtotalEuro = subtotal * this.GetResellerRate(rates, resllerId, subtotal);
                        reseller.EURCost = FormatHandler.FormatNumber2(subtotalEuro, "en-EN");
                    }

                    totalEuro += subtotalEuro;

                    transctionDto.Items.Add(reseller);
                }

                transctionDto.Total = new Reseller()
                {
                    ResellerName = "Total",
                    Total = NumberFormat.DecFormat(total, 0),
                    EURCost = FormatHandler.FormatNumber2(totalEuro, "en-EN")
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelTotalTransction, transctionDto);
            return transctionDto;
        }


        private double GetResellerRate(DataTable rates, int resellerId, double betCount)
        {
            try
            {
                if (rates == null || rates.Rows.Count == 0) return 0;


                var result =
                    rates.Select(
                        "resellerid = " + resellerId + " AND mintrans <= " + betCount + " AND maxtrans >= " + betCount);

                var rate = CastAide.AsDouble(result.First()["rate"]);
                return rate;


            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }
            return 0;
        }

        #endregion


        public LicenseeCurrency SelReportByLicenseeV2FromDB(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, string MoveBetsDay, int drawfilter, int excludeTest)
        {
            LicenseeCurrency licenseDto = new LicenseeCurrency();
           // bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);

            if (BaseCurrency > 0)
                Currency = 0;
            DbResult dbResult = _licenseReportDAO.SelReportByLicenseeV2(LicenseeID, Currency, FromDate, ToDate, showSB,
                                                                          ShowRB, ShowNG, ShowBA, ShowLC, ReportType, MoveBetsDay, false,
                                                                          drawfilter, excludeTest);

            DataTable dataTable = new DataTable();
            if (dbResult == null)
                return licenseDto;

            dataTable = dbResult.ResultSet.Tables[0];
            licenseDto.Items = new List<LicenseeByDate>();
            licenseDto.Totals = new List<LicenseeRowV2>();
            licenseDto.TotalsBaseCurrency = new List<LicenseeRowV2>();

            //total rows
            licenseDto.totalrow = dataTable.Rows.Count;

            #region foreach row content

            List<int> lstCurrency = dataTable.AsEnumerable().Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();
            int index = 0;

            string _subfixByBaseCurrency = string.Empty;
            switch (BaseCurrency)
            {
                case 3:
                    _subfixByBaseCurrency = "USD";
                    break;
                case 2:
                    _subfixByBaseCurrency = "MYR";
                    break;
                case 6:
                    _subfixByBaseCurrency = "EUR";
                    break;
                default:
                    _subfixByBaseCurrency = string.Empty;
                    break;

            }
            var query = from q in dataTable.AsEnumerable()
                        group q by q.Field<string>("ReportType") into dtb
                        select new
                        {
                            Key = dtb.Key
                        };

            foreach (var rowDate in query)
            {
                var licenseeByDate = new LicenseeByDate();
                var licenseeDetailList = new List<LicenseeRowV2>();
                var reportType = rowDate.Key;
                var queryDate = from d in dataTable.AsEnumerable()
                                where d.Field<string>("ReportType") == reportType
                                select d;
                licenseeByDate.DateTime = formatReportType(CastAide.AsString(rowDate.Key), ReportType.ToString());
                foreach (DataRow dataRow in queryDate)
                {
                    var memberWinloss = CastAide.AsDecimal(dataRow["Winlost" + _subfixByBaseCurrency]);
                    var memberComm = CastAide.AsDecimal(dataRow["PlayerComm" + _subfixByBaseCurrency]);
                    var memberWLTotal = memberComm + memberWinloss;
                    var licenseeDetail = new LicenseeRowV2()
                    {
                        CurrencyName = CastAide.AsString(dataRow["currencyName"]),
                        CurrencyId = CastAide.AsString(dataRow["currency"]),
                        Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),

                        CustomerWL = NumberFormat.DecFormat(memberWinloss, 2),
                        CustomerComm = NumberFormat.DecFormat(memberComm, 2),
                        CustomerWLTotal = NumberFormat.DecFormat(memberWLTotal, 2),

                        LicenseeWL = NumberFormat.DecFormat(dataRow["LicenseeWL" + _subfixByBaseCurrency], 2),
                        BA = NumberFormat.DecFormat(dataRow["CommBA" + _subfixByBaseCurrency], 2),
                        Racing = NumberFormat.DecFormat(dataRow["CommRB" + _subfixByBaseCurrency], 2),
                        WinlossRB = NumberFormat.DecFormat(dataRow["WinlossRB" + _subfixByBaseCurrency], 2),
                        AmtDuetoBA = NumberFormat.DecFormat(dataRow["AmtDuetoBA" + _subfixByBaseCurrency], 2),
                        AmtDuetoRB = NumberFormat.DecFormat(dataRow["AmtDuetoRB" + _subfixByBaseCurrency], 2),
                        Company = NumberFormat.DecFormat(dataRow["Company" + _subfixByBaseCurrency], 2),
                        //Comm = NumberFormat.DecFormat(dataRow["Comm" + _subfixByBaseCurrency], 2),
                        BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["CommRB" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["WinlossRB" + _subfixByBaseCurrency]), 2)
                    };
                    licenseeDetailList.Add(licenseeDetail);
                }
                licenseeByDate.LicenseeByDateList = licenseeDetailList;
                licenseDto.Items.Add(licenseeByDate);
            }
            licenseDto.totalrow = licenseDto.Items.Count;
            #endregion
            string exp = string.Empty;
            #region Total by currency

            foreach (var currency in lstCurrency)
            {
                exp = "currency= " + currency.ToString();
                var totalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp));
                var totalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + _subfixByBaseCurrency + ")", exp));
                var totalMemberWLTotal = totalMemberComm + totalMemberWinloss;

                licenseDto.Totals.Add(new LicenseeRowV2()
                {
                    CurrencyName = CacheUtils.GetExchangeName(currency),
                    CurrencyId = currency.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),

                    CustomerWL = NumberFormat.DecFormat(totalMemberWinloss, 2),
                    CustomerComm = NumberFormat.DecFormat(totalMemberComm, 2),
                    CustomerWLTotal = NumberFormat.DecFormat(totalMemberWLTotal, 2),

                    LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + _subfixByBaseCurrency + ")", exp), 2),
                    BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + _subfixByBaseCurrency + ")", exp), 2),
                    Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp), 2),
                    WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp), 2),
                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),
                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + _subfixByBaseCurrency + ")", exp), 2),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp)), 2),
                    AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + _subfixByBaseCurrency + ")", exp), 2),
                    AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + _subfixByBaseCurrency + ")", exp), 2),

                });
            }


            #endregion

            #region Grand Total By Base Currency
            Hashtable baseCurrency = new Hashtable();

            baseCurrency.Add(2, "MYR");
            baseCurrency.Add(3, "USD");
            baseCurrency.Add(6, "EUR");

            foreach (DictionaryEntry curr in baseCurrency)
            {
                exp = string.Empty;

                var gtotalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp));
                var gtotalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + curr.Value + ")", exp));
                var gtotalMemberWLTotal = gtotalMemberComm + gtotalMemberWinloss;

                licenseDto.TotalsBaseCurrency.Add(new LicenseeRowV2()
                {
                    CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(curr.Key)),
                    CurrencyId = curr.Key.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),

                    CustomerWL = NumberFormat.DecFormat(gtotalMemberWinloss, 2),
                    CustomerComm = NumberFormat.DecFormat(gtotalMemberComm, 2),
                    CustomerWLTotal = NumberFormat.DecFormat(gtotalMemberWLTotal, 2),

                    LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + curr.Value + ")", exp), 2),
                    BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + curr.Value + ")", exp), 2),
                    Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp), 2),
                    WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp), 2),
                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + curr.Value + ")", exp), 2),
                    AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                    AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + curr.Value + ")", exp), 2),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp)), 2),
                });
            }
            licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

            #endregion


            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicensee, licenseDto);

            return licenseDto;
        }


        public LicenseeDTOV2 SelReportByLicenseeDetailV2FromDB(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, string MoveBetsDay, int drawfilter, int excludeTest)
        {
            LicenseeDTOV2 licenseDto = new LicenseeDTOV2();
            //bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);


            DbResult dbResult = _licenseReportDAO.SelReportByLicenseeDetailV2(LicenseeID, Currency, CustId, FromDate, ToDate, showSB,
                                                                          ShowRB, ShowNG, ShowBA, ShowLC, ReportType, MoveBetsDay, false,
                                                                          drawfilter, excludeTest);

            DataTable dataTable = new DataTable();
            if (dbResult == null)
                return licenseDto;

            dataTable = dbResult.ResultSet.Tables[0];

            licenseDto.Items = new List<LicenseeRowV2>();
            licenseDto.Totals = new List<LicenseeRowV2>();
            licenseDto.TotalsBaseCurrency = new List<LicenseeRowV2>();
            //total rows
            licenseDto.totalrow = dataTable.Rows.Count;

            #region foreach row content

            int index = 0;

            List<int> lstCurrency = dataTable.AsEnumerable().OrderBy(s => CastAide.AsString(s["currencyName"])).Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();

            string _subfixByBaseCurrency = string.Empty;
            switch (BaseCurrency)
            {
                case 3:
                    _subfixByBaseCurrency = "USD";
                    break;
                case 2:
                    _subfixByBaseCurrency = "MYR";
                    break;
                case 6:
                    _subfixByBaseCurrency = "EUR";
                    break;
                default:
                    _subfixByBaseCurrency = string.Empty;
                    break;

            }

            string ReportTypeId = string.Empty;
            if (showSB == 1 && ShowRB == 0)
                ReportTypeId = "Sportsbook";
            else if (showSB == 0 && ShowRB == 1)
                ReportTypeId = "Racebook";

            foreach (DataRow dataRow in dataTable.Rows)
            {
                index++;

                #region AssignRow
                var memberWinloss = CastAide.AsDecimal(dataRow["Winlost" + _subfixByBaseCurrency]);
                var memberComm = CastAide.AsDecimal(dataRow["PlayerComm" + _subfixByBaseCurrency]);
                var memberWLTotal = memberComm + memberWinloss;

                LicenseeRowV2 licenseeRow = new LicenseeRowV2()
                {
                    index = index,

                    ReportTypeId = ReportTypeId,

                    Username = CastAide.AsString(dataRow["username"]),
                    CustId = CastAide.AsString(dataRow["custid"]),
                    CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),
                    CurrencyId = CastAide.AsString(dataRow["currency"]),

                    Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),

                    CustomerWL = NumberFormat.DecFormat(memberWinloss, 2),
                    CustomerComm = NumberFormat.DecFormat(memberComm, 2),
                    CustomerWLTotal = NumberFormat.DecFormat(memberWLTotal, 2),

                    LicenseeWL = NumberFormat.DecFormat(dataRow["LicenseeWL" + _subfixByBaseCurrency], 2),
                    BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),
                    BA = NumberFormat.DecFormat(dataRow["CommBA" + _subfixByBaseCurrency], 2),
                    Racing = NumberFormat.DecFormat(dataRow["CommRB" + _subfixByBaseCurrency], 2),
                    Company = NumberFormat.DecFormat(dataRow["Company" + _subfixByBaseCurrency], 2),
                    WinlossRB = NumberFormat.DecFormat(dataRow["WinlossRB" + _subfixByBaseCurrency], 2),
                    AmtDuetoBA = NumberFormat.DecFormat(dataRow["AmtDuetoBA" + _subfixByBaseCurrency], 2),
                    AmtDuetoRB = NumberFormat.DecFormat(dataRow["AmtDuetoRB" + _subfixByBaseCurrency], 2),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["CommRB" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["WinlossRB" + _subfixByBaseCurrency]), 2),



                };
                #endregion

                licenseDto.Items.Add(licenseeRow);
            }

            #endregion

            string exp = string.Empty;
            #region Total by currency

            foreach (var currency in lstCurrency)
            {
                exp = "currency= " + currency.ToString();
                var totalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp));
                var totalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + _subfixByBaseCurrency + ")", exp));
                var totalMemberWLTotal = totalMemberComm + totalMemberWinloss;

                licenseDto.Totals.Add(new LicenseeRowV2()
                {
                    CurrencyName = CacheUtils.GetExchangeName(currency),
                    CurrencyId = currency.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),

                    CustomerWL = NumberFormat.DecFormat(totalMemberWinloss, 2),
                    CustomerComm = NumberFormat.DecFormat(totalMemberComm, 2),
                    CustomerWLTotal = NumberFormat.DecFormat(totalMemberWLTotal, 2),

                    LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + _subfixByBaseCurrency + ")", exp), 2),
                    BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + _subfixByBaseCurrency + ")", exp), 2),
                    Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp), 2),
                    WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp), 2),
                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + _subfixByBaseCurrency + ")", exp), 2),
                    AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + _subfixByBaseCurrency + ")", exp), 2),
                    AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + _subfixByBaseCurrency + ")", exp), 2),
                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp)), 2),

                });
            }


            #endregion

            #region Grand Total By Base Currency
            Hashtable baseCurrency = new Hashtable();

            baseCurrency.Add(2, "MYR");
            baseCurrency.Add(3, "USD");
            baseCurrency.Add(6, "EUR");

            foreach (DictionaryEntry curr in baseCurrency)
            {
                exp = string.Empty;
                var gtotalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp));
                var gtotalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + curr.Value + ")", exp));
                var gtotalMemberWLTotal = gtotalMemberComm + gtotalMemberWinloss;

                licenseDto.TotalsBaseCurrency.Add(new LicenseeRowV2()
                {
                    CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(curr.Key)),
                    CurrencyId = curr.Key.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),

                    CustomerWL = NumberFormat.DecFormat(gtotalMemberWinloss, 2),
                    CustomerComm = NumberFormat.DecFormat(gtotalMemberComm, 2),
                    CustomerWLTotal = NumberFormat.DecFormat(gtotalMemberWLTotal, 2),

                    LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + curr.Value + ")", exp), 2),
                    BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + curr.Value + ")", exp), 2),
                    Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp), 2),
                    WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp), 2),
                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + curr.Value + ")", exp), 2),
                    AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                    AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + curr.Value + ")", exp), 2),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp)), 2),


                });
            }
            licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

            #endregion

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicenseeDetail, licenseDto);

            return licenseDto;
        }

        public LicenseeDTOV2 SelReportByLicenseeDetail_AgV2FromDB(int LicenseeID, int Currency, int CustId, int BaseCurrency, string FromDate, string ToDate, int ShowSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, int drawfilter, int excludeTest)
        {
            //bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);
            LicenseeDTOV2 licenseDto = new LicenseeDTOV2();
            try
            {
                licenseDto.Items = new List<LicenseeRowV2>();
                licenseDto.Totals = new List<LicenseeRowV2>();
                licenseDto.TotalsBaseCurrency = new List<LicenseeRowV2>();

                DataTable dataTable = null;
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();

                DbResult dbResult = _licenseReportDAO.SelReportByLicenseeDetail_AgV2(LicenseeID, Currency, CustId, FromDate, ToDate, ShowSB, ShowRB, ShowNG, ShowBA, ShowLC, ReportType, MoveBetsDay, false, drawfilter, excludeTest);

                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

                if (dataTable == null)
                    return licenseDto;

                //total rows
                licenseDto.totalrow = dataTable.Rows.Count;

                #region foreach row content
                int from = 0;
                int to = dataTable.Rows.Count;

                int index = 0;

                List<int> lstCurrency = dataTable.AsEnumerable().OrderBy(s => CastAide.AsString(s["currencyName"])).Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();

                string _subfixByBaseCurrency = string.Empty;
                switch (BaseCurrency)
                {
                    case 3:
                        _subfixByBaseCurrency = "USD";
                        break;
                    case 2:
                        _subfixByBaseCurrency = "MYR";
                        break;
                    case 6:
                        _subfixByBaseCurrency = "EUR";
                        break;
                    default:
                        _subfixByBaseCurrency = string.Empty;
                        break;

                }

                string ReportTypeId = string.Empty;
                if (ShowSB == 1 && ShowRB == 0)
                    ReportTypeId = "Sportsbook";
                else if (ShowSB == 0 && ShowRB == 1)
                    ReportTypeId = "Racebook";

                for (int i = from; i < to; i++)
                {
                    index++;
                    DataRow dataRow = dataTable.Rows[i];

                    #region AssignRow
                    var memberWinloss = CastAide.AsDecimal(dataRow["Winlost" + _subfixByBaseCurrency]);
                    var memberComm = CastAide.AsDecimal(dataRow["PlayerComm" + _subfixByBaseCurrency]);
                    var memberWLTotal = memberComm + memberWinloss;
                    LicenseeRowV2 licenseeRow = new LicenseeRowV2()
                    {
                        index = index,

                        ReportTypeId = ReportTypeId,

                        Username = CastAide.AsString(dataRow["username"]),
                        CustId = CastAide.AsString(dataRow["custid"]),
                        CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),
                        CurrencyId = CastAide.AsString(dataRow["currency"]),

                        Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),

                        CustomerWL = NumberFormat.DecFormat(memberWinloss, 2),
                        CustomerComm = NumberFormat.DecFormat(memberComm, 2),
                        CustomerWLTotal = NumberFormat.DecFormat(memberWLTotal, 2),

                        LicenseeWL = NumberFormat.DecFormat(dataRow["LicenseeWL" + _subfixByBaseCurrency], 2),
                        BA = NumberFormat.DecFormat(dataRow["CommBA" + _subfixByBaseCurrency], 2),
                        Racing = NumberFormat.DecFormat(dataRow["CommRB" + _subfixByBaseCurrency], 2),
                        WinlossRB = NumberFormat.DecFormat(dataRow["WinlossRB" + _subfixByBaseCurrency], 2),
                        Company = NumberFormat.DecFormat(dataRow["Company" + _subfixByBaseCurrency], 2),
                        AmtDuetoBA = NumberFormat.DecFormat(dataRow["AmtDuetoBA" + _subfixByBaseCurrency], 2),
                        AmtDuetoRB = NumberFormat.DecFormat(dataRow["AmtDuetoRB" + _subfixByBaseCurrency], 2),
                        BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["CommRB" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["WinlossRB" + _subfixByBaseCurrency]), 2),



                    };
                    #endregion

                    licenseDto.Items.Add(licenseeRow);
                }

                #endregion

                string exp = string.Empty;
                #region Total by currency

                foreach (var currency in lstCurrency)
                {
                    exp = "currency= " + currency.ToString();
                    var totalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp));
                    var totalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + _subfixByBaseCurrency + ")", exp));
                    var totalMemberWLTotal = totalMemberComm + totalMemberWinloss;

                    licenseDto.Totals.Add(new LicenseeRowV2()
                    {
                        CurrencyName = CacheUtils.GetExchangeName(currency),
                        CurrencyId = currency.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),

                        CustomerWL = NumberFormat.DecFormat(totalMemberWinloss, 2),
                        CustomerComm = NumberFormat.DecFormat(totalMemberComm, 2),
                        CustomerWLTotal = NumberFormat.DecFormat(totalMemberWLTotal, 2),

                        LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + _subfixByBaseCurrency + ")", exp), 2),
                        BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp)), 2),

                        BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + _subfixByBaseCurrency + ")", exp), 2),
                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + _subfixByBaseCurrency + ")", exp), 2),
                        Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp), 2),
                        WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp), 2),
                        AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + _subfixByBaseCurrency + ")", exp), 2),
                        AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + _subfixByBaseCurrency + ")", exp), 2),


                    });
                }


                #endregion

                #region Grand Total By Base Currency
                Hashtable baseCurrency = new Hashtable();

                baseCurrency.Add(2, "MYR");
                baseCurrency.Add(3, "USD");
                baseCurrency.Add(6, "EUR");

                foreach (DictionaryEntry curr in baseCurrency)
                {
                    exp = string.Empty;
                    var gtotalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp));
                    var gtotalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + curr.Value + ")", exp));
                    var gtotalMemberWLTotal = gtotalMemberComm + gtotalMemberWinloss;

                    licenseDto.TotalsBaseCurrency.Add(new LicenseeRowV2()
                    {
                        CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(curr.Key)),
                        CurrencyId = curr.Key.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),

                        CustomerWL = NumberFormat.DecFormat(gtotalMemberWinloss, 2),
                        CustomerComm = NumberFormat.DecFormat(gtotalMemberComm, 2),
                        CustomerWLTotal = NumberFormat.DecFormat(gtotalMemberWLTotal, 2),

                        LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + curr.Value + ")", exp), 2),
                        BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + curr.Value + ")", exp), 2),
                        Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp), 2),
                        WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp), 2),
                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + curr.Value + ")", exp), 2),
                        AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                        AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + curr.Value + ")", exp), 2),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp)), 2),
                    });
                }
                licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

                #endregion
            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicenseeDetail_Ag, licenseDto);

            return licenseDto;
        }


        public LicenseeCurrency SelReportByLicenseeV2(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {

            var licenseDto = new LicenseeCurrency();
            var licenseDtoPaging = new LicenseeCurrency();

            try
            {
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                           _keySelReportByLicensee);
                if (sessionLicenseDto == null || fromCache == 0)
                    licenseDto = SelReportByLicenseeV2FromDB(LicenseeID, Currency, BaseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ReportType, MoveBetsDay, drawfilter, excludeTest);
                else
                {
                    licenseDto = (LicenseeCurrency)sessionLicenseDto;
                }

                if (pageSize != -1)
                {
                    licenseDtoPaging.Items = licenseDto.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }
                else
                {
                    licenseDtoPaging.Items = licenseDto.Items;
                }

                licenseDtoPaging.Totals = licenseDto.Totals;
                licenseDtoPaging.TotalsBaseCurrency = licenseDto.TotalsBaseCurrency;
                licenseDtoPaging.totalrow = licenseDto.totalrow;

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDtoPaging;
        }

        public LicenseeDTOV2 SelReportByLicenseeDetail_AgV2(int LicenseeID, int Currency, int CustId, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            LicenseeDTOV2 licenseDto = new LicenseeDTOV2();
            LicenseeDTOV2 licenseDtoPaging = new LicenseeDTOV2();
            var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                          _keySelReportByLicenseeDetail_Ag);


            try
            {

                DataTable dataTable = null;

                if (sessionLicenseDto == null || fromCache == 0)
                    licenseDto = SelReportByLicenseeDetail_AgV2FromDB(LicenseeID, Currency, CustId, BaseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ReportType, drawfilter, excludeTest);
                else
                {
                    licenseDto = (LicenseeDTOV2)sessionLicenseDto;
                }

                if (pageSize != -1)
                {
                    licenseDtoPaging.Items = licenseDto.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }
                else
                {
                    licenseDtoPaging.Items = licenseDto.Items;
                }

                licenseDtoPaging.Totals = licenseDto.Totals;
                licenseDtoPaging.TotalsBaseCurrency = licenseDto.TotalsBaseCurrency;
                licenseDtoPaging.totalrow = licenseDto.totalrow;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDtoPaging;
        }

        public LicenseeDTOV2 SelReportLicenseeDetailV2(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            LicenseeDTOV2 licenseDto = new LicenseeDTOV2();
            LicenseeDTOV2 licenseDtoPaging = new LicenseeDTOV2();
            var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                          _keySelReportByLicenseeDetail);
            try
            {

                DataTable dataTable = null;
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();

                if (sessionLicenseDto == null || fromCache == 0)
                    licenseDto = SelReportByLicenseeDetailV2FromDB(LicenseeID, Currency, BaseCurrency, CustId, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ReportType, MoveBetsDay, drawfilter, excludeTest);
                else
                {
                    licenseDto = (LicenseeDTOV2)sessionLicenseDto;

                }

                if (pageSize != -1)
                {
                    licenseDtoPaging.Items = licenseDto.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }
                else
                {
                    licenseDtoPaging.Items = licenseDto.Items;
                }

                licenseDtoPaging.Totals = licenseDto.Totals;
                licenseDtoPaging.TotalsBaseCurrency = licenseDto.TotalsBaseCurrency;
                licenseDtoPaging.totalrow = licenseDto.totalrow;
            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDtoPaging;
        }

        public LicenseeDTOV2 SelReportByLicenseeDetail_SAgV2FromDB(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int ShowSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, int drawfilter, int excludeTest)
        {
            //bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);
            LicenseeDTOV2 licenseDto = new LicenseeDTOV2();
            try
            {
                licenseDto.Items = new List<LicenseeRowV2>();
                licenseDto.Totals = new List<LicenseeRowV2>();
                licenseDto.TotalsBaseCurrency = new List<LicenseeRowV2>();

                DataTable dataTable = null;
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();

                DbResult dbResult = _licenseReportDAO.SelReportByLicenseeDetail_SAgV2(LicenseeID, Currency, FromDate, ToDate, ShowSB, ShowRB, ShowNG, ShowBA, ShowLC, ReportType, MoveBetsDay, false, drawfilter, excludeTest);

                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

                if (dataTable == null)
                    return licenseDto;

                //total rows
                licenseDto.totalrow = dataTable.Rows.Count;

                #region foreach row content
                int from = 0;
                int to = dataTable.Rows.Count;

                int index = 0;

                List<int> lstCurrency = dataTable.AsEnumerable().OrderBy(s => CastAide.AsString(s["currencyName"])).Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();

                string _subfixByBaseCurrency = string.Empty;
                switch (BaseCurrency)
                {
                    case 3:
                        _subfixByBaseCurrency = "USD";
                        break;
                    case 2:
                        _subfixByBaseCurrency = "MYR";
                        break;
                    case 6:
                        _subfixByBaseCurrency = "EUR";
                        break;
                    default:
                        _subfixByBaseCurrency = string.Empty;
                        break;

                }

                string ReportTypeId = string.Empty;
                if (ShowSB == 1 && ShowRB == 0)
                    ReportTypeId = "Sportsbook";
                else if (ShowSB == 0 && ShowRB == 1)
                    ReportTypeId = "Racebook";

                for (int i = from; i < to; i++)
                {
                    index++;
                    DataRow dataRow = dataTable.Rows[i];

                    #region AssignRow
                    var memberWinloss = CastAide.AsDecimal(dataRow["Winlost" + _subfixByBaseCurrency]);
                    var memberComm = CastAide.AsDecimal(dataRow["PlayerComm" + _subfixByBaseCurrency]);
                    var memberWLTotal = memberComm + memberWinloss;
                    LicenseeRowV2 licenseeRow = new LicenseeRowV2()
                    {
                        index = index,

                        ReportTypeId = ReportTypeId,

                        Username = CastAide.AsString(dataRow["username"]),
                        CustId = CastAide.AsString(dataRow["custid"]),
                        CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),
                        CurrencyId = CastAide.AsString(dataRow["currency"]),

                        Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),

                        CustomerWL = NumberFormat.DecFormat(memberWinloss, 2),
                        CustomerComm = NumberFormat.DecFormat(memberComm, 2),
                        CustomerWLTotal = NumberFormat.DecFormat(memberWLTotal, 2),

                        LicenseeWL = NumberFormat.DecFormat(dataRow["LicenseeWL" + _subfixByBaseCurrency], 2),
                        BA = NumberFormat.DecFormat(dataRow["CommBA" + _subfixByBaseCurrency], 2),
                        Racing = NumberFormat.DecFormat(dataRow["CommRB" + _subfixByBaseCurrency], 2),
                        WinlossRB = NumberFormat.DecFormat(dataRow["WinlossRB" + _subfixByBaseCurrency], 2),
                        Company = NumberFormat.DecFormat(dataRow["Company" + _subfixByBaseCurrency], 2),
                        AmtDuetoBA = NumberFormat.DecFormat(dataRow["AmtDuetoBA" + _subfixByBaseCurrency], 2),
                        AmtDuetoRB = NumberFormat.DecFormat(dataRow["AmtDuetoRB" + _subfixByBaseCurrency], 2),
                        BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["CommRB" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["WinlossRB" + _subfixByBaseCurrency]), 2),



                    };
                    #endregion

                    licenseDto.Items.Add(licenseeRow);
                }

                #endregion

                string exp = string.Empty;
                #region Total by currency

                foreach (var currency in lstCurrency)
                {
                    exp = "currency= " + currency.ToString();
                    var totalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp));
                    var totalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + _subfixByBaseCurrency + ")", exp));
                    var totalMemberWLTotal = totalMemberComm + totalMemberWinloss;

                    licenseDto.Totals.Add(new LicenseeRowV2()
                    {
                        CurrencyName = CacheUtils.GetExchangeName(currency),
                        CurrencyId = currency.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),

                        CustomerWL = NumberFormat.DecFormat(totalMemberWinloss, 2),
                        CustomerComm = NumberFormat.DecFormat(totalMemberComm, 2),
                        CustomerWLTotal = NumberFormat.DecFormat(totalMemberWLTotal, 2),

                        LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + _subfixByBaseCurrency + ")", exp), 2),
                        BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp)), 2),

                        BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + _subfixByBaseCurrency + ")", exp), 2),
                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + _subfixByBaseCurrency + ")", exp), 2),
                        Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp), 2),
                        WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp), 2),
                        AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + _subfixByBaseCurrency + ")", exp), 2),
                        AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + _subfixByBaseCurrency + ")", exp), 2),


                    });
                }


                #endregion

                #region Grand Total By Base Currency
                Hashtable baseCurrency = new Hashtable();

                baseCurrency.Add(2, "MYR");
                baseCurrency.Add(3, "USD");
                baseCurrency.Add(6, "EUR");

                foreach (DictionaryEntry curr in baseCurrency)
                {
                    exp = string.Empty;
                    var gtotalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp));
                    var gtotalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + curr.Value + ")", exp));
                    var gtotalMemberWLTotal = gtotalMemberComm + gtotalMemberWinloss;

                    licenseDto.TotalsBaseCurrency.Add(new LicenseeRowV2()
                    {
                        CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(curr.Key)),
                        CurrencyId = curr.Key.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),

                        CustomerWL = NumberFormat.DecFormat(gtotalMemberWinloss, 2),
                        CustomerComm = NumberFormat.DecFormat(gtotalMemberComm, 2),
                        CustomerWLTotal = NumberFormat.DecFormat(gtotalMemberWLTotal, 2),

                        LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + curr.Value + ")", exp), 2),
                        BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + curr.Value + ")", exp), 2),
                        Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp), 2),
                        WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp), 2),
                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + curr.Value + ")", exp), 2),
                        AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                        AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + curr.Value + ")", exp), 2),
                        Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp)), 2),
                    });
                }
                licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

                #endregion
            }
            catch (Exception ex)
            {

                LogExtensions.ExceptionHandler(ex);
            }

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicenseeDetail_SAg, licenseDto);

            return licenseDto;
        }

        public LicenseeDTOV2 SelReportByLicenseeDetail_SAgV2(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            LicenseeDTOV2 licenseDto = new LicenseeDTOV2();
            LicenseeDTOV2 licenseDtoPaging = new LicenseeDTOV2();
            var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                          _keySelReportByLicenseeDetail_SAg);


            try
            {

                DataTable dataTable = null;

                if (sessionLicenseDto == null || fromCache == 0)
                    licenseDto = SelReportByLicenseeDetail_SAgV2FromDB(LicenseeID, Currency, BaseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ReportType, drawfilter, excludeTest);
                else
                {
                    licenseDto = (LicenseeDTOV2)sessionLicenseDto;
                }

                if (pageSize != -1)
                {
                    licenseDtoPaging.Items = licenseDto.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }
                else
                {
                    licenseDtoPaging.Items = licenseDto.Items;
                }

                licenseDtoPaging.Totals = licenseDto.Totals;
                licenseDtoPaging.TotalsBaseCurrency = licenseDto.TotalsBaseCurrency;
                licenseDtoPaging.totalrow = licenseDto.totalrow;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDtoPaging;
        }

        public LicenseeDTOV2 SelReportByLicenseeDetail_MAgV2FromDB(int LicenseeID, int Currency, int CustId, int BaseCurrency, string FromDate, string ToDate, int ShowSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, int drawfilter, int excludeTest)
        {
            LicenseeDTOV2 licenseDto = new LicenseeDTOV2();
           // bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);

            DataTable dataTable = null;
            string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();

            DbResult dbResult = _licenseReportDAO.SelReportByLicenseeDetail_MAgV2(LicenseeID, Currency, CustId, FromDate, ToDate, ShowSB,
                                                                          ShowRB, ShowNG, ShowBA, ShowLC, ReportType, MoveBetsDay, false,
                                                                          drawfilter, excludeTest);

            if (dbResult == null)
                return licenseDto;

            dataTable = dbResult.ResultSet.Tables[0];

            licenseDto.Items = new List<LicenseeRowV2>();
            licenseDto.Totals = new List<LicenseeRowV2>();
            licenseDto.TotalsBaseCurrency = new List<LicenseeRowV2>();
            //total rows
            licenseDto.totalrow = dataTable.Rows.Count;

            #region foreach row content

            int index = 0;

            List<int> lstCurrency = dataTable.AsEnumerable().OrderBy(s => CastAide.AsString(s["currencyName"])).Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();

            string _subfixByBaseCurrency = string.Empty;
            switch (BaseCurrency)
            {
                case 3:
                    _subfixByBaseCurrency = "USD";
                    break;
                case 2:
                    _subfixByBaseCurrency = "MYR";
                    break;
                case 6:
                    _subfixByBaseCurrency = "EUR";
                    break;
                default:
                    _subfixByBaseCurrency = string.Empty;
                    break;

            }

            string ReportTypeId = string.Empty;
            if (ShowSB == 1 && ShowRB == 0)
                ReportTypeId = "Sportsbook";
            else if (ShowSB == 0 && ShowRB == 1)
                ReportTypeId = "Racebook";

            foreach (DataRow dataRow in dataTable.Rows)
            {
                index++;

                #region AssignRow
                var memberWinloss = CastAide.AsDecimal(dataRow["Winlost" + _subfixByBaseCurrency]);
                var memberComm = CastAide.AsDecimal(dataRow["PlayerComm" + _subfixByBaseCurrency]);
                var memberWLTotal = memberComm + memberWinloss;

                LicenseeRowV2 licenseeRow = new LicenseeRowV2()
                {
                    index = index,

                    ReportTypeId = ReportTypeId,

                    Username = CastAide.AsString(dataRow["username"]),
                    CustId = CastAide.AsString(dataRow["custid"]),
                    CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),
                    CurrencyId = CastAide.AsString(dataRow["currency"]),

                    Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),

                    CustomerWL = NumberFormat.DecFormat(memberWinloss, 2),
                    CustomerComm = NumberFormat.DecFormat(memberComm, 2),
                    CustomerWLTotal = NumberFormat.DecFormat(memberWLTotal, 2),

                    LicenseeWL = NumberFormat.DecFormat(dataRow["LicenseeWL" + _subfixByBaseCurrency], 2),
                    BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),
                    BA = NumberFormat.DecFormat(dataRow["CommBA" + _subfixByBaseCurrency], 2),
                    Racing = NumberFormat.DecFormat(dataRow["CommRB" + _subfixByBaseCurrency], 2),
                    Company = NumberFormat.DecFormat(dataRow["Company" + _subfixByBaseCurrency], 2),
                    WinlossRB = NumberFormat.DecFormat(dataRow["WinlossRB" + _subfixByBaseCurrency], 2),
                    AmtDuetoBA = NumberFormat.DecFormat(dataRow["AmtDuetoBA" + _subfixByBaseCurrency], 2),
                    AmtDuetoRB = NumberFormat.DecFormat(dataRow["AmtDuetoRB" + _subfixByBaseCurrency], 2),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataRow["CommRB" + _subfixByBaseCurrency]) + CastAide.AsDecimal(dataRow["WinlossRB" + _subfixByBaseCurrency]), 2),



                };
                #endregion

                licenseDto.Items.Add(licenseeRow);
            }

            #endregion

            string exp = string.Empty;
            #region Total by currency

            foreach (var currency in lstCurrency)
            {
                exp = "currency= " + currency.ToString();
                var totalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp));
                var totalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + _subfixByBaseCurrency + ")", exp));
                var totalMemberWLTotal = totalMemberComm + totalMemberWinloss;

                licenseDto.Totals.Add(new LicenseeRowV2()
                {
                    CurrencyName = CacheUtils.GetExchangeName(currency),
                    CurrencyId = currency.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),

                    CustomerWL = NumberFormat.DecFormat(totalMemberWinloss, 2),
                    CustomerComm = NumberFormat.DecFormat(totalMemberComm, 2),
                    CustomerWLTotal = NumberFormat.DecFormat(totalMemberWLTotal, 2),

                    LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + _subfixByBaseCurrency + ")", exp), 2),
                    BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + _subfixByBaseCurrency + ")", exp), 2),
                    Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp), 2),
                    WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp), 2),
                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + _subfixByBaseCurrency + ")", exp), 2),
                    AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + _subfixByBaseCurrency + ")", exp), 2),
                    AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + _subfixByBaseCurrency + ")", exp), 2),
                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + _subfixByBaseCurrency + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + _subfixByBaseCurrency + ")", exp)), 2),

                });
            }


            #endregion

            #region Grand Total By Base Currency
            Hashtable baseCurrency = new Hashtable();

            baseCurrency.Add(2, "MYR");
            baseCurrency.Add(3, "USD");
            baseCurrency.Add(6, "EUR");

            foreach (DictionaryEntry curr in baseCurrency)
            {
                exp = string.Empty;
                var gtotalMemberWinloss = CastAide.AsDecimal(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp));
                var gtotalMemberComm = CastAide.AsDecimal(dataTable.Compute("sum(PlayerComm" + curr.Value + ")", exp));
                var gtotalMemberWLTotal = gtotalMemberComm + gtotalMemberWinloss;

                licenseDto.TotalsBaseCurrency.Add(new LicenseeRowV2()
                {
                    CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(curr.Key)),
                    CurrencyId = curr.Key.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),

                    CustomerWL = NumberFormat.DecFormat(gtotalMemberWinloss, 2),
                    CustomerComm = NumberFormat.DecFormat(gtotalMemberComm, 2),
                    CustomerWLTotal = NumberFormat.DecFormat(gtotalMemberWLTotal, 2),

                    LicenseeWL = NumberFormat.DecFormat(dataTable.Compute("sum(LicenseeWL" + curr.Value + ")", exp), 2),
                    BA = NumberFormat.DecFormat(dataTable.Compute("sum(CommBA" + curr.Value + ")", exp), 2),
                    Racing = NumberFormat.DecFormat(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp), 2),
                    WinlossRB = NumberFormat.DecFormat(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp), 2),
                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(Company" + curr.Value + ")", exp), 2),
                    AmtDuetoBA = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                    AmtDuetoRB = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoRB" + curr.Value + ")", exp), 2),
                    Total = NumberFormat.DecFormat(CastAide.AsDecimal(dataTable.Compute("sum(CommRB" + curr.Value + ")", exp)) + CastAide.AsDecimal(dataTable.Compute("sum(WinlossRB" + curr.Value + ")", exp)), 2),


                });
            }
            licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

            #endregion

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicenseeDetail_MAg, licenseDto);

            return licenseDto;
        }

        public LicenseeDTOV2 SelReportByLicenseeDetail_MAgV2(int LicenseeID, int Currency, int custId, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            LicenseeDTOV2 licenseDto = new LicenseeDTOV2();
            LicenseeDTOV2 licenseDtoPaging = new LicenseeDTOV2();
            var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                          _keySelReportByLicenseeDetail_MAg);


            try
            {

                DataTable dataTable = null;

                if (sessionLicenseDto == null || fromCache == 0)
                    licenseDto = SelReportByLicenseeDetail_MAgV2FromDB(LicenseeID, Currency, custId, BaseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ReportType, drawfilter, excludeTest);
                else
                {
                    licenseDto = (LicenseeDTOV2)sessionLicenseDto;
                }

                if (pageSize != -1)
                {
                    licenseDtoPaging.Items = licenseDto.Items.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                }
                else
                {
                    licenseDtoPaging.Items = licenseDto.Items;
                }

                licenseDtoPaging.Totals = licenseDto.Totals;
                licenseDtoPaging.TotalsBaseCurrency = licenseDto.TotalsBaseCurrency;
                licenseDtoPaging.totalrow = licenseDto.totalrow;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDtoPaging;
        }


    }
}
