using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Business.BetList;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Helper.IP;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.AdoNet.ReportDAO;
using Sunplus.Accounting.DataAccess.IBetAggregatorDAO;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.BetAggregator;
using Sunplus.Accounting.Entities.Report.Deposit;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.Business.BetAggregator
{
    public partial class ReportWlbaBySiteService : IReportWLBABySiteService
    {
        private ILicenseDAO _licenseReportDAO;
        private string _baWinLossByCommLevelSessionKey = "bawinlossbycommlevelsessionkey";
        private string _keySelReportByLicensee = "selectreportbylicensee";
        private string _keySelBAReportByLicensee = "selectbareportbylicenseev2";
        private string _keySelReportByLicenseeDetail_Ag = "selectreportbylicenseedetailagent";
        private string _keySelReportByLicenseeDetail_AgV2 = "selreportbylicenseedetailagentv2";
        private string _keySelReportByLicenseeDetail = "selectreportbylicenseedetail";
        private string _keySelReportByLicenseeDetailV2 = "selreportbylicenseedetailv2";
        private string _keySelTotalTransction = "selecttotaltransaction";

        public ReportWlbaBySiteService()
        {
            _licenseReportDAO = new LicenseDAO();
        }

        public ReportWlbaBySiteService(ILicenseDAO licenseDAO)
        {
            _licenseReportDAO = licenseDAO;
            _keySelReportByLicensee = AppSession.Username + ".SelReportByLicensee";
            _keySelReportByLicenseeDetail_Ag = AppSession.Username + ".SelReportByLicenseeDetails_Ag";
            _keySelReportByLicenseeDetail = AppSession.Username + ".SelReportByLicenseeDetails";
            _keySelTotalTransction = AppSession.Username + ".SelTotalTransction";
        }

        #region SelCurrency

        public Report_LicenseeWLBA_DTO SelCurrency(string siteId)
        {
            var dbResult = _licenseReportDAO.SelCurrencyByLicensee(siteId);

            var depositDto = new Report_LicenseeWLBA_DTO();

            if (dbResult == null)
                return depositDto;

            var list = new List<DepositCurrencyBO>();
            var dataTable = dbResult.ResultSet.Tables[0];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DepositCurrencyBO deposit = new DepositCurrencyBO()
                                  {
                                      currencyId = CastAide.AsInt32(dataRow["currency"]),
                                      currency = CastAide.AsString(dataRow["currencyname"]),
                                  };
                list.Add(deposit);
            }
            depositDto.currency = list;
            return depositDto;
        }

        #endregion SelCurrency

        #region GetDatetimeOption

        public Report_LicenseeWLBA_DTO GetDatetimeOption(string dateOption)
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

            Report_LicenseeWLBA_DTO depositDto = new Report_LicenseeWLBA_DTO();
            depositDto.fdate = fromdate.ToShortDateString();
            depositDto.tdate = todate.ToShortDateString();

            return depositDto;
        }

        #endregion GetDatetimeOption

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

        #endregion formatReportType

        #region SelReportByLicenseeFromDB

        private Report_LicenseeWLBA_DTO SelReportByLicenseeFromDB(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, int drawfilter, int excludeTest, int ShowBA, int pageIndex, int pageSize)
        {
            Report_LicenseeWLBA_DTO licenseDto = new Report_LicenseeWLBA_DTO();
            bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);

            if (BaseCurrency > 0)
                Currency = 0;

            //DbResult dbResult = _licenseReportDAO.SelReportByLicenseeV2(LicenseeID, Currency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ReportType, MoveBetsDay, IsOld, drawfilter, excludeTest);
            DbResult dbResult = _licenseReportDAO.SelBAWinLossByLincensee(LicenseeID, Currency, FromDate, ToDate,
                                                                         ReportType, MoveBetsDay, drawfilter,
                                                                         excludeTest);
            DataTable dataTable = new DataTable();
            if (dbResult == null)
                return licenseDto;

            dataTable = dbResult.ResultSet.Tables[0];
            licenseDto.Items = new List<Report_LicenseeWLBA_Row>();
            licenseDto.Totals = new List<Report_LicenseeWLBA_Row>();
            licenseDto.TotalsBaseCurrency = new List<Report_LicenseeWLBA_Row>();
            licenseDto.DateGroup = new List<int>();
            //total rows
            licenseDto.totalrow = dataTable.Rows.Count;

            #region foreach row content

            //List<int> lstCurrency = dataTable.AsEnumerable().Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();
            var lstCurrency =
                                 (from r in dataTable.AsEnumerable()
                                  select new { id = r.Field<int>("currency"), name = r.Field<string>("currencyname") }).Distinct();

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

            var dateGroup = new List<int>();
            var totalDate = 0;
            string currReportType = string.Empty, pReportType = string.Empty;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                index++;

                currReportType = CastAide.AsString(dataRow["ReportType"]);

                #region check date

                if (!currReportType.Equals(pReportType) && pReportType.Length > 0 && currReportType.Length > 0)
                {
                    dateGroup.Add(totalDate);
                    totalDate = 0;
                }
                totalDate++;

                #endregion check date

                #region AssignRow

                Report_LicenseeWLBA_Row licenseeRow = new Report_LicenseeWLBA_Row()
                {
                    index = index,

                    ReportTypeId = formatReportType(CastAide.AsString(dataRow["ReportType"]), ReportType.ToString()),
                    //CurrencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"])),
                    CurrencyName = CastAide.AsString(dataRow["currencyName"]),
                    CurrencyId = CastAide.AsString(dataRow["currency"]),

                    Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),
                    Winlost = NumberFormat.DecFormat(dataRow["Winlost" + _subfixByBaseCurrency], 2),

                    BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),

                    AComm = NumberFormat.DecFormat(dataRow["Comm" + _subfixByBaseCurrency], 2),
                    Company = NumberFormat.DecFormat(dataRow["AmtDuetoBA" + _subfixByBaseCurrency], 2),
                };

                #endregion AssignRow

                pReportType = currReportType;
                licenseDto.Items.Add(licenseeRow);
                string currencyname = CacheUtils.GetExchangeName(CastAide.AsInt32(dataRow["currency"]));
            }

            #endregion foreach row content

            string exp = string.Empty;
            dateGroup.Add(totalDate);
            licenseDto.DateGroup = dateGroup;

            #region Total by currency

            foreach (var currency in lstCurrency)
            {
                exp = "currency= " + currency.id;
                licenseDto.Totals.Add(new Report_LicenseeWLBA_Row()
                {
                    CurrencyName = currency.name,
                    CurrencyId = currency.id.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp), 2),

                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),

                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + _subfixByBaseCurrency + ")", exp), 2),
                });
            }

            #endregion Total by currency

            #region Grand Total By Base Currency

            Hashtable baseCurrency = new Hashtable();

            baseCurrency.Add(2, "MYR");
            baseCurrency.Add(3, "USD");
            baseCurrency.Add(6, "EUR");

            foreach (DictionaryEntry curr in baseCurrency)
            {
                exp = string.Empty;
                licenseDto.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                {
                    CurrencyName = CastAide.AsString(curr.Value),
                    CurrencyId = curr.Key.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp), 2),

                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + curr.Value + ")", exp), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                });
            }
            licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

            #endregion Grand Total By Base Currency

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicensee, licenseDto);

            return licenseDto;
        }

        #endregion SelReportByLicenseeFromDB

        #region SelReportByLicenseeDetail_AgFromDB

        private Report_LicenseeWLBA_DTO SelReportByLicenseeDetail_AgFromDB(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int ShowBA, int pageIndex, int pageSize)
        {
            bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);
            Report_LicenseeWLBA_DTO licenseDto = new Report_LicenseeWLBA_DTO();
            try
            {
                licenseDto.Items = new List<Report_LicenseeWLBA_Row>();
                licenseDto.Totals = new List<Report_LicenseeWLBA_Row>();
                licenseDto.TotalsBaseCurrency = new List<Report_LicenseeWLBA_Row>();
                licenseDto.DateGroup = new List<int>();

                DataTable dataTable = null;
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();

                //DbResult dbResult = _licenseReportDAO.SelReportByLicenseeDetail_AgV2(LicenseeID, Currency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ReportType, MoveBetsDay, IsOld, drawfilter, excludeTest);
                var dbResult = _licenseReportDAO.SelBAWinLossReportByLicenseeAgent(LicenseeID, Currency, FromDate,
                                                                                        ToDate, MoveBetsDay, drawfilter,
                                                                                        excludeTest);

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

                //List<int> lstCurrency = dataTable.AsEnumerable().OrderBy(s => CastAide.AsString(s["currencyName"])).Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();
                var lstCurrency =
                                (from r in dataTable.AsEnumerable()
                                 select new { id = r.Field<int>("currency"), name = r.Field<string>("currencyname") }).Distinct();

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

                    Report_LicenseeWLBA_Row licenseeRow = new Report_LicenseeWLBA_Row()
                    {
                        index = index,

                        ReportTypeId = ReportTypeId,

                        Username = CastAide.AsString(dataRow["username"]),
                        CustId = CastAide.AsString(dataRow["custid"]),
                        CurrencyName = CastAide.AsString(dataRow["currencyname"]),
                        CurrencyId = CastAide.AsString(dataRow["currency"]),

                        Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),
                        Winlost = NumberFormat.DecFormat(dataRow["Winlost" + _subfixByBaseCurrency], 2),

                        BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),

                        AComm = NumberFormat.DecFormat(dataRow["Comm" + _subfixByBaseCurrency], 2),

                        Company = NumberFormat.DecFormat(dataRow["AmtDuetoBA" + _subfixByBaseCurrency], 2),
                    };

                    #endregion AssignRow

                    licenseDto.Items.Add(licenseeRow);
                }

                #endregion foreach row content

                string exp = string.Empty;

                #region Total by currency

                foreach (var currency in lstCurrency)
                {
                    exp = "currency= " + currency.id;
                    licenseDto.Totals.Add(new Report_LicenseeWLBA_Row()
                    {
                        CurrencyName = currency.name,
                        CurrencyId = currency.id.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),
                        Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp), 2),

                        BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),

                        AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp), 2),

                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + _subfixByBaseCurrency + ")", exp), 2),
                    });
                }

                #endregion Total by currency

                #region Grand Total By Base Currency

                Hashtable baseCurrency = new Hashtable();

                baseCurrency.Add(2, "MYR");
                baseCurrency.Add(3, "USD");
                baseCurrency.Add(6, "EUR");

                foreach (DictionaryEntry curr in baseCurrency)
                {
                    exp = string.Empty;
                    licenseDto.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                    {
                        CurrencyName = CastAide.AsString(curr.Value),
                        CurrencyId = curr.Key.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),
                        Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp), 2),

                        AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + curr.Value + ")", exp), 2),

                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                    });
                }
                licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

                #endregion Grand Total By Base Currency
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicenseeDetail_Ag, licenseDto);

            return licenseDto;
        }

        #endregion SelReportByLicenseeDetail_AgFromDB

        #region SelReportByLicenseeDetailFromDB

        private Report_LicenseeWLBA_DTO SelReportByLicenseeDetailFromDB(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, int drawfilter, int excludeTest, int ShowBA, int pageIndex, int pageSize)
        {
            Report_LicenseeWLBA_DTO licenseDto = new Report_LicenseeWLBA_DTO();
            bool IsOld = CastAide.AsDateTime(FromDate) < new DateTime(2009, 01, 01);

            //DbResult dbResult = _licenseReportDAO.SelReportByLicenseeDetailV2(LicenseeID, Currency, CustId, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ReportType, MoveBetsDay, IsOld, drawfilter, excludeTest);
            var dbResult = _licenseReportDAO.SelBAWinLossByLicenseeDetail(LicenseeID, Currency, CustId, FromDate,
                                                                               ToDate, MoveBetsDay, drawfilter,
                                                                               excludeTest);
            DataTable dataTable = new DataTable();
            if (dbResult == null)
                return licenseDto;

            dataTable = dbResult.ResultSet.Tables[0];

            licenseDto.Items = new List<Report_LicenseeWLBA_Row>();
            licenseDto.Totals = new List<Report_LicenseeWLBA_Row>();
            licenseDto.TotalsBaseCurrency = new List<Report_LicenseeWLBA_Row>();
            licenseDto.DateGroup = new List<int>();
            //total rows
            licenseDto.totalrow = dataTable.Rows.Count;

            #region foreach row content

            int index = 0;

            //List<int> lstCurrency = dataTable.AsEnumerable().OrderBy(s => CastAide.AsString(s["currencyName"])).Select(s => CastAide.AsInt32(s["Currency"])).Distinct().ToList();
            var lstCurrency =
                                (from r in dataTable.AsEnumerable()
                                 select new { id = r.Field<int>("currency"), name = r.Field<string>("currencyname") }).Distinct();

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

                Report_LicenseeWLBA_Row licenseeRow = new Report_LicenseeWLBA_Row()
                {
                    index = index,

                    ReportTypeId = ReportTypeId,

                    Username = CastAide.AsString(dataRow["username"]),
                    CustId = CastAide.AsString(dataRow["custid"]),
                    CurrencyName = CastAide.AsString(dataRow["currencyname"]),
                    CurrencyId = CastAide.AsString(dataRow["currency"]),

                    Turnover = NumberFormat.DecFormat(dataRow["Turnover" + _subfixByBaseCurrency], 2),
                    Winlost = NumberFormat.DecFormat(dataRow["Winlost" + _subfixByBaseCurrency], 2),

                    BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),

                    AComm = NumberFormat.DecFormat(dataRow["Comm" + _subfixByBaseCurrency], 2),

                    Company = NumberFormat.DecFormat(dataRow["AmtDuetoBA" + _subfixByBaseCurrency], 2),
                };

                #endregion AssignRow

                licenseDto.Items.Add(licenseeRow);
            }

            #endregion foreach row content

            string exp = string.Empty;

            #region Total by currency

            foreach (var currency in lstCurrency)
            {
                exp = "currency= " + currency.id;
                licenseDto.Totals.Add(new Report_LicenseeWLBA_Row()
                {
                    CurrencyName = currency.name,
                    CurrencyId = currency.id.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + _subfixByBaseCurrency + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + _subfixByBaseCurrency + ")", exp), 2),

                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),

                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + _subfixByBaseCurrency + ")", exp), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + _subfixByBaseCurrency + ")", exp), 2),
                });
            }

            #endregion Total by currency

            #region Grand Total By Base Currency

            Hashtable baseCurrency = new Hashtable();

            baseCurrency.Add(2, "MYR");
            baseCurrency.Add(3, "USD");
            baseCurrency.Add(6, "EUR");

            foreach (DictionaryEntry curr in baseCurrency)
            {
                exp = string.Empty;
                licenseDto.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                {
                    CurrencyName = CastAide.AsString(curr.Value),
                    CurrencyId = curr.Key.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp), 2),

                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + curr.Value + ")", exp), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                });
            }
            licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

            #endregion Grand Total By Base Currency

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySelReportByLicenseeDetail, licenseDto);

            return licenseDto;
        }

        #endregion SelReportByLicenseeDetailFromDB

        #region SelReportByLicensee

        public Report_LicenseeWLBA_DTO SelReportByLicensee(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int ShowBA, int pageIndex, int pageSize, int fromCache)
        {
            Report_LicenseeWLBA_DTO licenseDto = new Report_LicenseeWLBA_DTO();
            Report_LicenseeWLBA_DTO licenseDtoPaging = new Report_LicenseeWLBA_DTO();

            try
            {
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                licenseDto = SelReportByLicenseeFromDB(LicenseeID, Currency, BaseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ReportType, MoveBetsDay, drawfilter, excludeTest, ShowBA, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDto;
        }

        public LicenseesWLBA SelReportByLicensees(int licenseeId, int currency, int baseCurrency, string fromDate, string toDate, int reportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var result = new LicenseesWLBA();
            try
            {
                if (baseCurrency > 0)
                    currency = 0;
                var moveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                var dbResult = _licenseReportDAO.SelBAWinLossByLincensee(licenseeId, currency, fromDate, toDate,
                                                                         reportType, moveBetsDay, drawfilter,
                                                                         excludeTest);
                if (dbResult != null && dbResult.ResultSet.Tables != null && dbResult.ResultSet.Tables.Count > 0)
                {
                    var lincenseesData = new DataTable();
                    var tblEmployees = dbResult.ResultSet.Tables[0];
                    var dv = tblEmployees.DefaultView;
                    dv.Sort = "licenseeid ASC, ReportType DESC";
                    lincenseesData = dv.ToTable();

                    result.Items = new List<LicenseeWLBA>();
                    result.TotalsBaseCurrency = new List<Report_LicenseeWLBA_Row>();
                    int currLicenseeId = 0, pLicenseeId = 0;
                    int currCurrency = 0,
                        pCurrency = 0,
                        licTotalTrans = 0,
                        totalTrans = 0,
                        grandTotalTrans = 0;
                    string currLicenseeName = string.Empty,
                           pLicenseeName = string.Empty,
                           currCurrencyName = string.Empty,
                           pCurrencyName = string.Empty;
                    double licEURTurnover = 0, licEURCustWL = 0, licEURLicenseeComm = 0, licEURBA = 0;
                    double licMYRTurnover = 0, licMYRCustWL = 0, licMYRLicenseeComm = 0, licMYRBA = 0;
                    double licUSDTurnover = 0, licUSDCustWL = 0, licUSDLicenseeComm = 0, licUSDBA = 0;
                    double turnover = 0, custWL = 0, licenseeComm = 0, amtDueTOBA = 0;

                    double totalEURTurnover = 0, totalEURCustWL = 0, totalEURLicenseeComm = 0, totalEURBA = 0;
                    double totalMYRTurnover = 0, totalMYRCustWL = 0, totalMYRLicenseeComm = 0, totalMYRBA = 0;
                    double totalUSDTurnover = 0, totalUSDCustWL = 0, totalUSDLicenseeComm = 0, totalUSDBA = 0;

                    var subfixByBaseCurrency = string.Empty;
                    switch (baseCurrency)
                    {
                        case 3:
                            subfixByBaseCurrency = "USD";
                            break;

                        case 2:
                            subfixByBaseCurrency = "MYR";
                            break;

                        case 6:
                            subfixByBaseCurrency = "EUR";
                            break;

                        default:
                            subfixByBaseCurrency = string.Empty;
                            break;
                    }

                    var licenseeWL = new LicenseeWLBA();
                    licenseeWL.Items = new List<Report_LicenseeWLBA_Row>();
                    licenseeWL.Totals = new List<Report_LicenseeWLBA_Row>();
                    licenseeWL.TotalsBaseCurrency = new List<Report_LicenseeWLBA_Row>();
                    var dateGroup = new List<int>();
                    var totalDate = 0;
                    string currReportType = string.Empty, pReportType = string.Empty;
                    foreach (DataRow dr in lincenseesData.Rows)
                    {
                        currLicenseeId = CastAide.AsInt32(dr["licenseeid"]);
                        currLicenseeName = CastAide.AsString(dr["licenseename"]);
                        currCurrency = CastAide.AsInt32(dr["currency"]);
                        currCurrencyName = CastAide.AsString(dr["currencyname"]);
                        currReportType = CastAide.AsString(dr["ReportType"]);

                        #region licensee

                        if (currLicenseeId != pLicenseeId && pLicenseeId != 0)
                        {
                            dateGroup.Add(totalDate);
                            licenseeWL.LicenseeId = pLicenseeId.ToString();
                            licenseeWL.Licensee = pLicenseeName;
                            licenseeWL.DateGroup = dateGroup;

                            #region total by currency

                            var currencies =
                                (from r in lincenseesData.AsEnumerable()
                                 where r.Field<int>("LicenseeID") == pLicenseeId
                                 select new { id = r.Field<int>("currency"), name = r.Field<string>("currencyname") }).Distinct();

                            foreach (var item in currencies)
                            {
                                var id = CastAide.AsString(item.id);
                                var name = CastAide.AsString(item.name);
                                var exp = "currency=" + id + " and licenseeid = " + pLicenseeId;
                                licenseeWL.Totals.Add(new Report_LicenseeWLBA_Row()
                                                          {
                                                              CurrencyId = id,
                                                              CurrencyName = name,
                                                              Turnover = NumberFormat.DecFormat(lincenseesData.Compute("sum(Turnover" + subfixByBaseCurrency + ")", exp), 2),
                                                              Winlost = NumberFormat.DecFormat(lincenseesData.Compute("sum(Winlost" + subfixByBaseCurrency + ")", exp), 2),
                                                              AComm = NumberFormat.DecFormat(lincenseesData.Compute("sum(Comm" + subfixByBaseCurrency + ")", exp), 2),
                                                              Company = NumberFormat.DecFormat(lincenseesData.Compute("sum(AmtDuetoBA" + subfixByBaseCurrency + ")", exp), 2),
                                                              BetCount = NumberFormat.DecFormat(lincenseesData.Compute("sum(BetCount)", exp), 0)
                                                          });
                            }

                            #endregion total by currency

                            #region total - base currencies

                            licenseeWL.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                                  {
                                                                      CurrencyId = 6.ToString(),
                                                                      CurrencyName = "EUR",
                                                                      Turnover =
                                                                          NumberFormat.DecFormat(licEURTurnover, 2),
                                                                      Winlost = NumberFormat.DecFormat(licEURCustWL, 2),
                                                                      BetCount =
                                                                          NumberFormat.DecFormat(licTotalTrans, 0),
                                                                      AComm =
                                                                          NumberFormat.DecFormat(licEURLicenseeComm, 2),
                                                                      Company = NumberFormat.DecFormat(licEURBA, 2)
                                                                  });
                            licenseeWL.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                                  {
                                                                      CurrencyId = 3.ToString(),
                                                                      CurrencyName = "USD",
                                                                      Turnover =
                                                                          NumberFormat.DecFormat(licUSDTurnover, 2),
                                                                      Winlost = NumberFormat.DecFormat(licUSDCustWL, 2),
                                                                      BetCount =
                                                                          NumberFormat.DecFormat(licTotalTrans, 0),
                                                                      AComm =
                                                                          NumberFormat.DecFormat(licUSDLicenseeComm, 2),
                                                                      Company = NumberFormat.DecFormat(licUSDBA, 2)
                                                                  });
                            licenseeWL.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                                  {
                                                                      CurrencyId = 2.ToString(),
                                                                      CurrencyName = "MYR",
                                                                      Turnover =
                                                                          NumberFormat.DecFormat(licMYRTurnover, 2),
                                                                      Winlost = NumberFormat.DecFormat(licMYRCustWL, 2),
                                                                      BetCount =
                                                                          NumberFormat.DecFormat(licTotalTrans, 0),
                                                                      AComm =
                                                                          NumberFormat.DecFormat(licMYRLicenseeComm, 2),
                                                                      Company = NumberFormat.DecFormat(licMYRBA, 2)
                                                                  });

                            #endregion total - base currencies

                            result.Items.Add(licenseeWL);

                            #region reset

                            licTotalTrans = 0;
                            licenseeWL = new LicenseeWLBA();
                            licenseeWL.Items = new List<Report_LicenseeWLBA_Row>();
                            licenseeWL.Totals = new List<Report_LicenseeWLBA_Row>();
                            licenseeWL.TotalsBaseCurrency = new List<Report_LicenseeWLBA_Row>();
                            licEURTurnover = 0;
                            licEURCustWL = 0;
                            licEURLicenseeComm = 0;
                            licEURBA = 0;
                            licMYRTurnover = 0;
                            licMYRCustWL = 0;
                            licMYRLicenseeComm = 0;
                            licMYRBA = 0;
                            licUSDTurnover = 0;
                            licUSDCustWL = 0;
                            licUSDLicenseeComm = 0;
                            licUSDBA = 0;
                            dateGroup = new List<int>();
                            totalDate = 0;

                            #endregion reset
                        }

                        #region check date

                        if (!currReportType.Equals(pReportType) && pReportType.Length > 0 && currReportType.Length > 0 && currLicenseeId == pLicenseeId)
                        {
                            dateGroup.Add(totalDate);
                            totalDate = 0;
                        }
                        totalDate++;

                        #endregion check date

                        #endregion licensee

                        #region each row

                        turnover = CastAide.AsDouble(dr["Turnover" + subfixByBaseCurrency]);
                        custWL = CastAide.AsDouble(dr["Winlost" + subfixByBaseCurrency]);
                        totalTrans = CastAide.AsInt32(dr["BetCount"]);
                        licenseeComm = CastAide.AsDouble(dr["Comm" + subfixByBaseCurrency]);
                        amtDueTOBA = CastAide.AsDouble(dr["AmtDuetoBA" + subfixByBaseCurrency]);
                        var licenseeRow = new Report_LicenseeWLBA_Row()
                                              {
                                                  ReportTypeId = formatReportType(CastAide.AsString(dr["ReportType"]), reportType.ToString()),
                                                  CurrencyName = CastAide.AsString(dr["currencyName"]),
                                                  CurrencyId = CastAide.AsString(dr["currency"]),
                                                  Turnover = NumberFormat.DecFormat(turnover, 2),
                                                  Winlost = NumberFormat.DecFormat(custWL, 2),
                                                  BetCount = NumberFormat.DecFormat(totalTrans, 0),
                                                  AComm = NumberFormat.DecFormat(licenseeComm, 2),
                                                  Company = NumberFormat.DecFormat(amtDueTOBA, 2),
                                              };

                        licenseeWL.Items.Add(licenseeRow);

                        #endregion each row

                        licTotalTrans += totalTrans;
                        grandTotalTrans += totalTrans;

                        #region licensee Total - MYR

                        licMYRTurnover += CastAide.AsDouble(dr["TurnoverMYR"]);
                        licMYRCustWL += CastAide.AsDouble(dr["WinlostMYR"]);
                        licMYRLicenseeComm += CastAide.AsDouble(dr["CommMYR"]);
                        licMYRBA += CastAide.AsDouble(dr["AmtDuetoBAMYR"]);

                        #endregion licensee Total - MYR

                        #region Grand Total - MYR

                        totalMYRTurnover += CastAide.AsDouble(dr["TurnoverMYR"]);
                        totalMYRCustWL += CastAide.AsDouble(dr["WinlostMYR"]);
                        totalMYRLicenseeComm += CastAide.AsDouble(dr["CommMYR"]);
                        totalMYRBA += CastAide.AsDouble(dr["AmtDuetoBAMYR"]);

                        #endregion Grand Total - MYR

                        #region licensee Total - USD

                        licUSDTurnover += CastAide.AsDouble(dr["TurnoverUSD"]);
                        licUSDCustWL += CastAide.AsDouble(dr["WinlostUSD"]);
                        licUSDLicenseeComm += CastAide.AsDouble(dr["CommUSD"]);
                        licUSDBA += CastAide.AsDouble(dr["AmtDuetoBAUSD"]);

                        #endregion licensee Total - USD

                        #region Grand Total - USD

                        totalUSDTurnover += CastAide.AsDouble(dr["TurnoverUSD"]);
                        totalUSDCustWL += CastAide.AsDouble(dr["WinlostUSD"]);
                        totalUSDLicenseeComm += CastAide.AsDouble(dr["CommUSD"]);
                        totalUSDBA += CastAide.AsDouble(dr["AmtDuetoBAUSD"]);

                        #endregion Grand Total - USD

                        #region licensee Total - EUR

                        licEURTurnover += CastAide.AsDouble(dr["TurnoverEUR"]);
                        licEURCustWL += CastAide.AsDouble(dr["WinlostEUR"]);
                        licEURLicenseeComm += CastAide.AsDouble(dr["CommEUR"]);
                        licEURBA += CastAide.AsDouble(dr["AmtDuetoBAEUR"]);

                        #endregion licensee Total - EUR

                        #region Grand Total - EUR

                        totalEURTurnover += CastAide.AsDouble(dr["TurnoverEUR"]);
                        totalEURCustWL += CastAide.AsDouble(dr["WinlostEUR"]);
                        totalEURLicenseeComm += CastAide.AsDouble(dr["CommEUR"]);
                        totalEURBA += CastAide.AsDouble(dr["AmtDuetoBAEUR"]);

                        #endregion Grand Total - EUR

                        pLicenseeId = currLicenseeId;
                        pLicenseeName = currLicenseeName;
                        pCurrency = currCurrency;
                        pCurrencyName = currCurrencyName;
                        pReportType = currReportType;
                    }

                    #region last licensee

                    dateGroup.Add(totalDate);
                    licenseeWL.LicenseeId = pLicenseeId.ToString();
                    licenseeWL.Licensee = pLicenseeName;
                    licenseeWL.DateGroup = dateGroup;

                    #region total by currency

                    var lastCurrencies =
                                (from r in lincenseesData.AsEnumerable()
                                 where r.Field<int>("LicenseeID") == currLicenseeId
                                 select new { id = r.Field<int>("currency"), name = r.Field<string>("currencyname") }).Distinct();

                    foreach (var item in lastCurrencies)
                    {
                        var id = CastAide.AsString(item.id);
                        var name = CastAide.AsString(item.name);
                        var exp = "currency=" + id + " and licenseeid = " + currLicenseeId;
                        licenseeWL.Totals.Add(new Report_LicenseeWLBA_Row()
                        {
                            CurrencyId = id,
                            CurrencyName = name,
                            Turnover = NumberFormat.DecFormat(lincenseesData.Compute("sum(Turnover" + subfixByBaseCurrency + ")", exp), 2),
                            Winlost = NumberFormat.DecFormat(lincenseesData.Compute("sum(Winlost" + subfixByBaseCurrency + ")", exp), 2),
                            AComm = NumberFormat.DecFormat(lincenseesData.Compute("sum(Comm" + subfixByBaseCurrency + ")", exp), 2),
                            Company = NumberFormat.DecFormat(lincenseesData.Compute("sum(AmtDuetoBA" + subfixByBaseCurrency + ")", exp), 2),
                            BetCount = NumberFormat.DecFormat(lincenseesData.Compute("sum(BetCount)", exp), 0)
                        });
                    }

                    #endregion total by currency

                    #region total - base currencies

                    licenseeWL.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                          {
                                                              CurrencyId = 6.ToString(),
                                                              CurrencyName = "EUR",
                                                              Turnover =
                                                                  NumberFormat.DecFormat(licEURTurnover, 2),
                                                              Winlost = NumberFormat.DecFormat(licEURCustWL, 2),
                                                              BetCount =
                                                                  NumberFormat.DecFormat(licTotalTrans, 0),
                                                              AComm =
                                                                  NumberFormat.DecFormat(licEURLicenseeComm, 2),
                                                              Company = NumberFormat.DecFormat(licEURBA, 2)
                                                          });
                    licenseeWL.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                          {
                                                              CurrencyId = 3.ToString(),
                                                              CurrencyName = "USD",
                                                              Turnover =
                                                                  NumberFormat.DecFormat(licUSDTurnover, 2),
                                                              Winlost = NumberFormat.DecFormat(licUSDCustWL, 2),
                                                              BetCount =
                                                                  NumberFormat.DecFormat(licTotalTrans, 0),
                                                              AComm =
                                                                  NumberFormat.DecFormat(licUSDLicenseeComm, 2),
                                                              Company = NumberFormat.DecFormat(licUSDBA, 2)
                                                          });
                    licenseeWL.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                          {
                                                              CurrencyId = 2.ToString(),
                                                              CurrencyName = "MYR",
                                                              Turnover =
                                                                  NumberFormat.DecFormat(licMYRTurnover, 2),
                                                              Winlost = NumberFormat.DecFormat(licMYRCustWL, 2),
                                                              BetCount =
                                                                  NumberFormat.DecFormat(licTotalTrans, 0),
                                                              AComm =
                                                                  NumberFormat.DecFormat(licMYRLicenseeComm, 2),
                                                              Company = NumberFormat.DecFormat(licMYRBA, 2)
                                                          });

                    #endregion total - base currencies

                    result.Items.Add(licenseeWL);

                    #endregion last licensee

                    #region grand total

                    result.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                      {
                                                          CurrencyId = 6.ToString(),
                                                          CurrencyName = "EUR",
                                                          Turnover =
                                                              NumberFormat.DecFormat(totalEURTurnover, 2),
                                                          Winlost = NumberFormat.DecFormat(totalEURCustWL, 2),
                                                          BetCount =
                                                              NumberFormat.DecFormat(grandTotalTrans, 0),
                                                          AComm =
                                                              NumberFormat.DecFormat(totalEURLicenseeComm, 2),
                                                          Company = NumberFormat.DecFormat(totalEURBA, 2)
                                                      });
                    result.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                      {
                                                          CurrencyId = 3.ToString(),
                                                          CurrencyName = "USD",
                                                          Turnover =
                                                              NumberFormat.DecFormat(totalUSDTurnover, 2),
                                                          Winlost = NumberFormat.DecFormat(totalUSDCustWL, 2),
                                                          BetCount =
                                                              NumberFormat.DecFormat(grandTotalTrans, 0),
                                                          AComm =
                                                              NumberFormat.DecFormat(totalUSDLicenseeComm, 2),
                                                          Company = NumberFormat.DecFormat(totalUSDBA, 2)
                                                      });
                    result.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                                                      {
                                                          CurrencyId = 2.ToString(),
                                                          CurrencyName = "MYR",
                                                          Turnover =
                                                              NumberFormat.DecFormat(totalMYRTurnover, 2),
                                                          Winlost = NumberFormat.DecFormat(totalMYRCustWL, 2),
                                                          BetCount =
                                                              NumberFormat.DecFormat(grandTotalTrans, 0),
                                                          AComm =
                                                              NumberFormat.DecFormat(totalMYRLicenseeComm, 2),
                                                          Company = NumberFormat.DecFormat(totalMYRBA, 2)
                                                      });

                    #endregion grand total
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            SessionExtension.AddSession(HttpContext.Current.Session, _keySelBAReportByLicensee, result);
            return result;
        }

        #endregion SelReportByLicensee

        #region ExportDepositReport

        private ExcelWorksheet AssignDataExcel(string level, string currency, string baseCurrency, ref ExcelWorksheet ws, int indexRow, DataRow dataRow, string reportType, int r, int g, int b)
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

        private ExcelWorksheet AssignDataTotalCurrencyExcel(string level, ref ExcelWorksheet ws, ref int indexRow, ArrayList arrCurrency, string baseCurrency, DataTable dataTable, int r, int g, int b)
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

        private ExcelWorksheet AssignDataGrandTotalCurrencyExcel(ref ExcelWorksheet ws, ref int indexRow, DataTable dataTable, int r, int g, int b)
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

            #endregion header

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

            #endregion content

            #region footer

            r = 255;
            g = 238;
            b = 221;
            AssignDataTotalCurrencyExcel(level, ref ws, ref indexRow, arrCurrency, baseCurrency, dataTable, r, g, b);

            r = 205;
            g = 222;
            b = 243;
            AssignDataGrandTotalCurrencyExcel(ref ws, ref indexRow, dataTable, r, g, b);

            #endregion footer

            for (int i = 0; i <= 11; i++)
            {
                ws.Columns[i].Width = 120 * 30;
            }

            return excelFile;
        }

        #endregion ExportDepositReport

        #region SelLicensees

        public List<Item> SelLicensees(bool IsAll)
        {
            List<Item> lstSite = new List<Item>();

            try
            {
                DataTable dataTable = _licenseReportDAO.GetLicenseeList();

                DataTable dtLicenseePermission = new DataTable();
                dtLicenseePermission.Columns.Add("site", typeof(string));
                dtLicenseePermission.Columns.Add("Level", typeof(string));
                //dtLicenseePermission.Columns.Add("To", typeof(string));
                //dtLicenseePermission.Columns.Add("Cc", typeof(string));
                //dtLicenseePermission.Columns.Add("Bcc", typeof(string));

                UserControlCollection collection =
                    UserControlSection.Instance.Report[UserControlSection.StoreCollection.Licensee.ToString()].Stores;

                foreach (UserControlElement element in collection)
                {
                    bool HasPermission = Permissions.HasPermission(element.Function.ToLower());
                    if (IsAll)
                        HasPermission = true;
                    if (HasPermission)
                    {
                        DataRow dataRow = dtLicenseePermission.NewRow();
                        dataRow["site"] = element.Site.ToLower();
                        dataRow["Level"] = element.Level;

                        //MailElement mailElement =
                        //    MailSection.Instance[MailSection.Setting.Send]["Licensee"].Mails[element.Site.ToLower()];

                        //dataRow["To"] = mailElement.To;
                        //dataRow["Cc"] = mailElement.Cc;
                        //dataRow["Bcc"] = mailElement.Bcc;

                        dtLicenseePermission.Rows.Add(dataRow);
                    }
                }

                DataRow[] rows = dataTable.Select();
                foreach (DataRow row in rows)
                {
                    DataRow[] rowPermission = dtLicenseePermission.Select("site='" + row["LicenseeName"] + "'");
                    bool HasPermission = rowPermission.Length > 0;
                    if (HasPermission)
                    {
                        lstSite.Add(new Item()
                                        {
                                            Id = CastAide.AsString(row["LicenseeID"]),
                                            Name = CastAide.AsString(row["LicenseeName"]),
                                            EnableBA = CastAide.AsBoolean(row["EnableBA"]),
                                            Level = CastAide.AsString(rowPermission[0]["Level"]),
                                            //To = CastAide.AsString(rowPermission[0]["To"]),
                                            //Bcc = CastAide.AsString(rowPermission[0]["Bcc"]),
                                            //Cc = CastAide.AsString(rowPermission[0]["Cc"]),
                                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return lstSite;
        }

        #endregion SelLicensees

        #region SelReportByLicenseeDetail_Ag

        public Report_LicenseeWLBA_DTO SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int ShowBA, int pageIndex, int pageSize, int fromCache)
        {
            Report_LicenseeWLBA_DTO licenseDto = new Report_LicenseeWLBA_DTO();
            Report_LicenseeWLBA_DTO licenseDtoPaging = new Report_LicenseeWLBA_DTO();
            var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                          _keySelReportByLicenseeDetail_Ag);

            try
            {
                DataTable dataTable = null;
                licenseDto = SelReportByLicenseeDetail_AgFromDB(LicenseeID, Currency, BaseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ReportType, drawfilter, excludeTest, ShowBA, pageSize, pageIndex);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDto;
        }

        public Report_LicenseeWLBA_DTO SelWinLossReportByLicenseeDetailAgent(int licenseeId, int currencyId, int baseCurrencyId, string fromDate, string toDate, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            Report_LicenseeWLBA_DTO licenseDto = new Report_LicenseeWLBA_DTO();
            try
            {
                licenseDto.Items = new List<Report_LicenseeWLBA_Row>();
                licenseDto.Totals = new List<Report_LicenseeWLBA_Row>();
                licenseDto.TotalsBaseCurrency = new List<Report_LicenseeWLBA_Row>();

                DataTable dataTable = null;
                var moveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();

                var dbResult = _licenseReportDAO.SelBAWinLossReportByLicenseeAgent(licenseeId, currencyId, fromDate,
                                                                                        toDate, moveBetsDay, drawfilter,
                                                                                        excludeTest);

                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

                if (dataTable == null)
                    return licenseDto;

                //total rows
                licenseDto.totalrow = dataTable.Rows.Count;

                #region foreach row content

                var from = 0;
                var to = dataTable.Rows.Count;

                var index = 0;

                var lstCurrency =
                                (from r in dataTable.AsEnumerable()
                                 select new { id = r.Field<int>("currency"), name = r.Field<string>("currencyname") }).Distinct();

                var subfixByBaseCurrency = string.Empty;
                switch (baseCurrencyId)
                {
                    case 3:
                        subfixByBaseCurrency = "USD";
                        break;

                    case 2:
                        subfixByBaseCurrency = "MYR";
                        break;

                    case 6:
                        subfixByBaseCurrency = "EUR";
                        break;

                    default:
                        subfixByBaseCurrency = string.Empty;
                        break;
                }

                var ReportTypeId = string.Empty;

                for (int i = from; i < to; i++)
                {
                    index++;
                    DataRow dataRow = dataTable.Rows[i];

                    #region AssignRow

                    var licenseeRow = new Report_LicenseeWLBA_Row()
                                          {
                                              index = index,

                                              ReportTypeId = ReportTypeId,

                                              Username = CastAide.AsString(dataRow["username"]),
                                              CustId = CastAide.AsString(dataRow["custid"]),
                                              CurrencyName =
                                                  CastAide.AsString(dataRow["currencyname"]),
                                              CurrencyId = CastAide.AsString(dataRow["currency"]),

                                              Turnover =
                                                  NumberFormat.DecFormat(dataRow["Turnover" + subfixByBaseCurrency], 2),
                                              Winlost =
                                                  NumberFormat.DecFormat(dataRow["Winlost" + subfixByBaseCurrency], 2),

                                              BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),

                                              AComm = NumberFormat.DecFormat(dataRow["Comm" + subfixByBaseCurrency], 2),

                                              Company =
                                                  NumberFormat.DecFormat(dataRow["AmtDuetoBA" + subfixByBaseCurrency],
                                                                         2),
                                          };

                    #endregion AssignRow

                    licenseDto.Items.Add(licenseeRow);
                }

                #endregion foreach row content

                string exp = string.Empty;

                #region Total by currency

                foreach (var currency in lstCurrency)
                {
                    exp = "currency= " + currency.id.ToString();
                    licenseDto.Totals.Add(new Report_LicenseeWLBA_Row()
                    {
                        CurrencyName = CastAide.AsString(currency.name),
                        CurrencyId = CastAide.AsString(currency.id),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + subfixByBaseCurrency + ")", exp), 2),
                        Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + subfixByBaseCurrency + ")", exp), 2),

                        BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),

                        AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + subfixByBaseCurrency + ")", exp), 2),

                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + subfixByBaseCurrency + ")", exp), 2),
                    });
                }

                #endregion Total by currency

                #region Grand Total By Base Currency

                var baseCurrency = new Hashtable();

                baseCurrency.Add(2, "MYR");
                baseCurrency.Add(3, "USD");
                baseCurrency.Add(6, "EUR");

                foreach (DictionaryEntry curr in baseCurrency)
                {
                    exp = string.Empty;
                    licenseDto.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                    {
                        CurrencyName = CastAide.AsString(curr.Value),
                        CurrencyId = curr.Key.ToString(),

                        Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),
                        Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp), 2),

                        AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + curr.Value + ")", exp), 2),

                        Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                    });
                }
                licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

                #endregion Grand Total By Base Currency
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            AppSession.AddSession(HttpContext.Current.Session, _keySelReportByLicenseeDetail_AgV2, licenseDto);

            return licenseDto;
        }

        #endregion SelReportByLicenseeDetail_Ag

        #region SelReportByLicenseeDetail

        public Report_LicenseeWLBA_DTO SelReportByLicenseeDetail(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int ShowBA, int pageIndex, int pageSize, int fromCache)
        {
            Report_LicenseeWLBA_DTO licenseDto = new Report_LicenseeWLBA_DTO();
            Report_LicenseeWLBA_DTO licenseDtoPaging = new Report_LicenseeWLBA_DTO();
            var sessionLicenseDto = AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                          _keySelReportByLicenseeDetail);
            try
            {
                DataTable dataTable = null;
                string MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                licenseDto = SelReportByLicenseeDetailFromDB(LicenseeID, Currency, BaseCurrency, CustId, FromDate, ToDate, showSB, ShowRB, ShowNG, ReportType, MoveBetsDay, drawfilter, excludeTest, ShowBA, pageSize, pageIndex);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return licenseDto;
        }

        public Report_LicenseeWLBA_DTO SelWinLossReportByLicenseeDetail(int licenseeId, int currencyId, int baseCurrencyId, int custId, string fromDate, string toDate, int drawfilter, int excludeTest, int reportType, int pageIndex, int pageSize, int fromCache)
        {
            var licenseDto = new Report_LicenseeWLBA_DTO();

            var moveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
            var dbResult = _licenseReportDAO.SelBAWinLossByLicenseeDetail(licenseeId, currencyId, custId, fromDate,
                                                                               toDate, moveBetsDay, drawfilter,
                                                                               excludeTest);

            var dataTable = new DataTable();
            if (dbResult == null)
                return licenseDto;

            dataTable = dbResult.ResultSet.Tables[0];

            licenseDto.Items = new List<Report_LicenseeWLBA_Row>();
            licenseDto.Totals = new List<Report_LicenseeWLBA_Row>();
            licenseDto.TotalsBaseCurrency = new List<Report_LicenseeWLBA_Row>();
            //total rows
            licenseDto.totalrow = dataTable.Rows.Count;

            #region foreach row content

            var index = 0;

            var lstCurrency =
                                (from r in dataTable.AsEnumerable()
                                 select new { id = r.Field<int>("currency"), name = r.Field<string>("currencyname") }).Distinct();

            var subfixByBaseCurrency = string.Empty;
            switch (baseCurrencyId)
            {
                case 3:
                    subfixByBaseCurrency = "USD";
                    break;

                case 2:
                    subfixByBaseCurrency = "MYR";
                    break;

                case 6:
                    subfixByBaseCurrency = "EUR";
                    break;

                default:
                    subfixByBaseCurrency = string.Empty;
                    break;
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {
                index++;

                #region AssignRow

                var licenseeRow = new Report_LicenseeWLBA_Row()
                                      {
                                          index = index,
                                          ReportTypeId = reportType.ToString(),
                                          Username = CastAide.AsString(dataRow["username"]),
                                          CustId = CastAide.AsString(dataRow["custid"]),
                                          CurrencyName =
                                              CastAide.AsString(dataRow["currencyname"]),
                                          CurrencyId = CastAide.AsString(dataRow["currency"]),
                                          Turnover =
                                              NumberFormat.DecFormat(dataRow["Turnover" + subfixByBaseCurrency], 2),
                                          Winlost = NumberFormat.DecFormat(dataRow["Winlost" + subfixByBaseCurrency], 2),
                                          BetCount = NumberFormat.DecFormat(dataRow["BetCount"], 0),
                                          AComm = NumberFormat.DecFormat(dataRow["Comm" + subfixByBaseCurrency], 2),
                                          Company =
                                              NumberFormat.DecFormat(dataRow["AmtDuetoBA" + subfixByBaseCurrency], 2),
                                      };

                #endregion AssignRow

                licenseDto.Items.Add(licenseeRow);
            }

            #endregion foreach row content

            var exp = string.Empty;

            #region Total by currency

            foreach (var currency in lstCurrency)
            {
                exp = "currency= " + currency.id.ToString();
                licenseDto.Totals.Add(new Report_LicenseeWLBA_Row()
                {
                    CurrencyName = CastAide.AsString(currency.name),
                    CurrencyId = CastAide.AsString(currency.id),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + subfixByBaseCurrency + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + subfixByBaseCurrency + ")", exp), 2),

                    BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", exp), 0),

                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + subfixByBaseCurrency + ")", exp), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + subfixByBaseCurrency + ")", exp), 2),
                });
            }

            #endregion Total by currency

            #region Grand Total By Base Currency

            var baseCurrency = new Hashtable();

            baseCurrency.Add(2, "MYR");
            baseCurrency.Add(3, "USD");
            baseCurrency.Add(6, "EUR");

            foreach (DictionaryEntry curr in baseCurrency)
            {
                exp = string.Empty;
                licenseDto.TotalsBaseCurrency.Add(new Report_LicenseeWLBA_Row()
                {
                    CurrencyName = CastAide.AsString(curr.Value),
                    CurrencyId = curr.Key.ToString(),

                    Turnover = NumberFormat.DecFormat(dataTable.Compute("sum(Turnover" + curr.Value + ")", exp), 2),
                    Winlost = NumberFormat.DecFormat(dataTable.Compute("sum(Winlost" + curr.Value + ")", exp), 2),

                    AComm = NumberFormat.DecFormat(dataTable.Compute("sum(Comm" + curr.Value + ")", exp), 2),

                    Company = NumberFormat.DecFormat(dataTable.Compute("sum(AmtDuetoBA" + curr.Value + ")", exp), 2),
                });
            }
            licenseDto.TotalsBaseCurrency[0].BetCount = NumberFormat.DecFormat(dataTable.Compute("sum(BetCount)", string.Empty), 0);

            #endregion Grand Total By Base Currency

            AppSession.AddSession(HttpContext.Current.Session, _keySelReportByLicenseeDetailV2, licenseDto);

            return licenseDto;
        }

        #endregion SelReportByLicenseeDetail

        #region renderBetList

        public string RenderHTML(Hashtable input)
        {
            var html = new StringBuilder();
            try
            {
                string tplHeader = WebUtils.LoadTemplate("BABetList/Betlist_Header.htm");
                string tplBody = WebUtils.LoadTemplate("BABetList/Betlist_Body.htm");
                string tplFooter = WebUtils.LoadTemplate("BABetList/Betlist_Footer.htm");
                string tplEvent = WebUtils.LoadTemplate("BABetList/BetlistEvent.htm");
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
                bool showrb = CastAide.AsInt32(input["showrb"], 1) == 0;
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

                //DbResult dbResult = _licenseReportDAO.SelBetlistByLicenseeV2(custid, fdate, tdate, MoveBetsDay, BalanceUpDay, showsb, showrb, showng, baFilter, drawfilter);
                DbResult dbResult = _licenseReportDAO.SelBABetListByLicensee(custid, fdate, tdate, MoveBetsDay, BalanceUpDay, drawfilter);

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

                    decimal winlost = 0, awinlost = 0, comm = 0, acomm = 0, totalWL = 0, adiscount = 0, apos = 0, totalComm = 0, apos_Win = 0, apos_Place = 0, stake = 0, actual_stake = 0, modds = 0;
                    int leagueid = 0, matchid = 0, betType = 0;
                    string status = string.Empty;
                    string winlost_col = string.Empty;
                    string awinlost_col = string.Empty;
                    string stake_col = string.Empty;
                    string comm_col = string.Empty;
                    string acomm_col = string.Empty;

                    if (baseCurrency == "usd")
                    {
                        winlost_col = "winlostusd";
                        //awinlost_col = "awinlostusd";
                        stake_col = "stakeusd";
                        //comm_col = "playercommusd";
                        acomm_col = "commusd";
                    }
                    else if (baseCurrency == "myr")
                    {
                        winlost_col = "winlostmyr";
                        //awinlost_col = "awinlostmyr";
                        stake_col = "stakemyr";
                        //comm_col = "playercommmyr";
                        acomm_col = "commmyr";
                    }
                    else if (baseCurrency == "eur")
                    {
                        winlost_col = "winlosteur";
                        //awinlost_col = "awinlosteur";
                        stake_col = "stakeeur";
                        //comm_col = "playercommeur";
                        acomm_col = "commeur";
                    }
                    else
                    {
                        winlost_col = "winlost";
                        //awinlost_col = "awinlost";
                        stake_col = "stake";
                        //comm_col = "playercomm";
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
                        //awinlost = CastAide.AsDecimal(dr[awinlost_col]);
                        stake = CastAide.AsDecimal(dr[stake_col]);
                        actual_stake = CastAide.AsDecimal(dr["actual_stake"]);
                        modds = CastAide.AsDecimal(dr["modds"]);
                        //comm = CastAide.AsDecimal(dr[comm_col]);
                        acomm = CastAide.AsDecimal(dr[acomm_col]);

                        totalWL += CastAide.AsDecimal(winlost, 2);
                        //totalComm += CastAide.AsDecimal(comm, 2);
                        string _OddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);

                        matchid = CastAide.AsInt32(dr["matchid"], 0);
                        leagueid = CastAide.AsInt32(dr["leagueid"], 0);

                        //apos = Convert.ToDecimal(dr["apositiontaking"]) * 100;
                        //adiscount = Convert.ToDecimal(dr["adiscount"]) * 100;

                        status = CastAide.AsString(dr["status"]);

                        TemplateHandler.Assign(ref strRow, "No", index.ToString());
                        TemplateHandler.Assign(ref strRow, "TransTime", DateFormat.FullDateFormat(dr["transdate"]));
                        TemplateHandler.Assign(ref strRow, "TransID", CastAide.AsString(dr["transid"]));

                        #region generate event

                        BetlistRow blRow = BAInputBetlistRow(dr);
                        switch (betType)
                        {
                            case 29:
                                {
                                    var refno = CastAide.AsString(dr["refno"]);
                                    var transid = CastAide.AsString(dr["transid"]);
                                    var strCombinationName = CacheRef.GetCombinationName(CastAide.AsString(dr["betteam"]));
                                    strEvent = string.Empty;
                                    strEvent = "<a style='color:gray' href='javascript:void(\"\");' onclick='showCMP(event,\"" +
                                                       refno + "\",\"" + "1" + "\");'><span style='color:blue'>System Parlay</span> - " + strCombinationName + "</a>"
                                                       + "<div id='spEvent_" + refno + transid + "' class='hide'></div>";
                                    strEvent += "<br/>";
                                    strEvent += "<a style='float:right' href='javascript:void(\"\");' onclick='javascript:showDetailsCMP(\"" + refno + "\",\"System Parlay - " + strCombinationName + "\");' onmouseover='javascript:cmpMouseOverDetailButton(event);' onmouseout='javascript:cmpMouseOutDetailButton(event);'>";
                                    strEvent += "<table border='0' width='60' cellpadding='0' cellspacing='0' style='border-collapse:collapse'>";
                                    strEvent += "<tr>";

                                    strEvent += "<td style='width:20px; border:0px none'>";
                                    strEvent += "<div style='width:14px; height:14px; background:url(\"Images/a_Icon.png\") no-repeat scroll 0 0 transparent'></div>";
                                    strEvent += "</td>";

                                    strEvent += "<td style='width:40px; border:0px none'>";
                                    strEvent += "<a style='color:#0094E7; font-weight:bold'>Details</a>";
                                    strEvent += "</td>";

                                    strEvent += "</tr>";
                                    strEvent += "</table></a>";
                                    TemplateHandler.Assign(ref strRow, "event", strEvent);
                                }
                                break;

                            default:
                                strEvent = blEvent.GenerateEvent(strEvent, blRow);
                                TemplateHandler.Assign(ref strRow, "event", strEvent);
                                break;
                        }

                        #endregion generate event

                        #region stake

                        string strStake = NumberFormat.DecFormat(stake, 2);
                        string strActualStake = IBCUtils.FormatHandler.FormatNumber20(actual_stake);
                        if (betType == 29)
                        {
                            TemplateHandler.Assign(ref strRow, "{Stake}", strActualStake + (Math.Round(stake, 0) == 0 ? string.Empty : "<br/>(" + strStake + ")"));
                        }
                        else
                        {
                            TemplateHandler.Assign(ref strRow, "{Stake}", strStake);
                        }

                        #endregion stake

                        //winlost
                        TemplateHandler.Assign(ref strRow, "WinLoss", NumberFormat.DecFormat(winlost, 2));

                        //player comm
                        //TemplateHandler.Assign(ref strRow, "PlayerComm", NumberFormat.DecFormat(comm, 2));

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

                            case 29:
                                {
                                    if (status.ToLower().Contains("running"))
                                    {
                                        TemplateHandler.Assign(ref strRow, "{Odds}", "-");
                                    }
                                    else
                                    {
                                        TemplateHandler.Assign(ref strRow, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(modds) + "<div class='b bl-leaguename'>" + _OddsTypeName + "</div>");
                                    }
                                }
                                break;

                            case 9: //mix parlay
                                TemplateHandler.Assign(ref strRow, "Odds", NumberFormat.DecFormat(CastAide.AsDecimal(dr["odds"]), 4));
                                break;

                            default:
                                TemplateHandler.Assign(ref strRow, "Odds", IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["odds"])));
                                if (IsSportBook(betType))
                                {
                                    oddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);
                                    liveStyle = (CastAide.AsBoolean(dr["liveindicator"], false) ? "event-live" : "");
                                }
                                if (betType > 80 && betType < 91)
                                    oddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);
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
                        // TemplateHandler.Assign(ref strRow, "PTValue", "<font class='bl_underdog'>" + NumberFormat.DecFormat(awinlost, 2) + "</font><br />");
                        // TemplateHandler.Assign(ref strRow, "Comm", NumberFormat.DecFormat(adiscount, 2));
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
                    //TemplateHandler.Assign(ref tplFooter, "Subtotalwinlost", NumberFormat.DecFormat(totalWL, 2));
                    //TemplateHandler.Assign(ref tplFooter, "SubtotalCom", NumberFormat.DecFormat(totalComm, 2));
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

        public string RenderHTMLV2(Hashtable input)
        {
            var html = new StringBuilder();
            try
            {
                var tplHeader = WebUtils.LoadTemplate("BABetList/Betlist_Header.htm");
                var tplBody = WebUtils.LoadTemplate("BABetList/Betlist_Body.htm");
                var tplFooter = WebUtils.LoadTemplate("BABetList/Betlist_Footer.htm");
                var tplEvent = WebUtils.LoadTemplate("BABetList/BetlistEvent.htm");
                var liveStyle = string.Empty;

                var blEvent = new BetlistGenerateEvent();

                var fdate = CastAide.AsDateTime(input["fdate"], DateTime.Now).ToShortDateString();
                var tdate = CastAide.AsDateTime(input["tdate"], DateTime.Now).ToShortDateString();
                var baFilter = CastAide.AsInt32(input["bafilter"], 0);
                var moveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
                var balanceUpDay = ReportUtils.BalanceUp.BalanceUpDay.ToShortDateString();
                var custid = CastAide.AsInt32(input["custid"]);
                var username = CastAide.AsString(input["username"]);
                var showsb = CastAide.AsInt32(input["showsb"], 1) == 1;
                var showrb = CastAide.AsInt32(input["showrb"], 1) == 0;
                var showng = CastAide.AsInt32(input["showng"], 1) == 1;
                var excludetest = CastAide.AsInt32(input["excludetest"], 0);
                var drawfilter = CastAide.AsInt32(input["drawfilter"], 1);
                var baseCurrency = CastAide.AsString(input["basecurrency"]).ToLower();
                var site = CastAide.AsString(input["site"]);

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

                DbResult dbResult = _licenseReportDAO.SelBABetListByLicensee(custid, fdate, tdate, moveBetsDay, balanceUpDay, drawfilter);

                var dataTable = new DataTable();
                var dataTable2 = new DataTable();
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

                    decimal winlost = 0, acomm = 0, totalWL = 0, apos = 0, totalComm = 0, apos_Win = 0, apos_Place = 0, stake = 0, actual_stake = 0, modds = 0;
                    int leagueid = 0, matchid = 0, betType = 0;
                    var status = string.Empty;
                    var winlost_col = string.Empty;
                    var stake_col = string.Empty;
                    var acomm_col = string.Empty;

                    if (baseCurrency == "usd")
                    {
                        winlost_col = "winlostusd";
                        stake_col = "stakeusd";
                        acomm_col = "commusd";
                    }
                    else if (baseCurrency == "myr")
                    {
                        winlost_col = "winlostmyr";
                        stake_col = "stakemyr";
                        acomm_col = "commmyr";
                    }
                    else if (baseCurrency == "eur")
                    {
                        winlost_col = "winlosteur";
                        stake_col = "stakeeur";
                        acomm_col = "commeur";
                    }
                    else
                    {
                        winlost_col = "winlost";
                        stake_col = "stake";
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
                        stake = CastAide.AsDecimal(dr[stake_col]);
                        actual_stake = CastAide.AsDecimal(dr["actual_stake"]);
                        modds = CastAide.AsDecimal(dr["modds"]);
                        acomm = CastAide.AsDecimal(dr[acomm_col]);

                        totalWL += CastAide.AsDecimal(winlost, 2);
                        var _OddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);

                        matchid = CastAide.AsInt32(dr["matchid"], 0);
                        leagueid = CastAide.AsInt32(dr["leagueid"], 0);
                        status = CastAide.AsString(dr["status"]);

                        TemplateHandler.Assign(ref strRow, "No", index.ToString());
                        TemplateHandler.Assign(ref strRow, "TransTime", DateFormat.FullDateFormat(dr["transdate"]));
                        TemplateHandler.Assign(ref strRow, "TransID", CastAide.AsString(dr["transid"]));

                        #region generate event

                        BetlistRow blRow = BAInputBetlistRow(dr);
                        switch (betType)
                        {
                            case 29:
                                {
                                    var refno = CastAide.AsString(dr["refno"]);
                                    var transid = CastAide.AsString(dr["transid"]);
                                    var strCombinationName = CacheRef.GetCombinationName(CastAide.AsString(dr["betteam"]));
                                    strEvent = string.Empty;
                                    strEvent = "<a style='color:gray' href='javascript:void(\"\");' onclick='showCMP(event,\"" +
                                                       refno + "\",\"" + "1" + "\");'><span style='color:blue'>System Parlay</span> - " + strCombinationName + "</a>"
                                                       + "<div id='spEvent_" + refno + transid + "' class='hide'></div>";
                                    strEvent += "<br/>";
                                    strEvent += "<a style='float:right' href='javascript:void(\"\");' onclick='javascript:showDetailsCMP(\"" + refno + "\",\"System Parlay - " + strCombinationName + "\");' onmouseover='javascript:cmpMouseOverDetailButton(event);' onmouseout='javascript:cmpMouseOutDetailButton(event);'>";
                                    strEvent += "<table border='0' width='60' cellpadding='0' cellspacing='0' style='border-collapse:collapse'>";
                                    strEvent += "<tr>";

                                    strEvent += "<td style='width:20px; border:0px none'>";
                                    strEvent += "<div style='width:14px; height:14px; background:url(\"Images/a_Icon.png\") no-repeat scroll 0 0 transparent'></div>";
                                    strEvent += "</td>";

                                    strEvent += "<td style='width:40px; border:0px none'>";
                                    strEvent += "<a style='color:#0094E7; font-weight:bold'>Details</a>";
                                    strEvent += "</td>";

                                    strEvent += "</tr>";
                                    strEvent += "</table></a>";
                                    TemplateHandler.Assign(ref strRow, "event", strEvent);
                                }
                                break;

                            default:
                                strEvent = blEvent.GenerateEvent(strEvent, blRow);
                                TemplateHandler.Assign(ref strRow, "event", strEvent);
                                break;
                        }

                        #endregion generate event

                        #region stake

                        string strStake = NumberFormat.DecFormat(stake, 2);
                        string strActualStake = IBCUtils.FormatHandler.FormatNumber20(actual_stake);
                        if (betType == 29)
                        {
                            TemplateHandler.Assign(ref strRow, "{Stake}", strActualStake + (Math.Round(stake, 0) == 0 ? string.Empty : "<br/>(" + strStake + ")"));
                        }
                        else
                        {
                            TemplateHandler.Assign(ref strRow, "{Stake}", strStake);
                        }

                        #endregion stake

                        //winlost
                        TemplateHandler.Assign(ref strRow, "WinLoss", NumberFormat.DecFormat(winlost, 2));

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

                            case 29:
                                {
                                    if (status.ToLower().Contains("running"))
                                    {
                                        TemplateHandler.Assign(ref strRow, "{Odds}", "-");
                                    }
                                    else
                                    {
                                        TemplateHandler.Assign(ref strRow, "{Odds}", IBCUtils.FormatHandler.FormatNumber3(modds) + "<div class='b bl-leaguename'>" + _OddsTypeName + "</div>");
                                    }
                                }
                                break;

                            case 9: //mix parlay
                                TemplateHandler.Assign(ref strRow, "Odds", NumberFormat.DecFormat(CastAide.AsDecimal(dr["odds"]), 4));
                                break;

                            default:
                                TemplateHandler.Assign(ref strRow, "Odds", IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["odds"])));
                                if (IsSportBook(betType))
                                {
                                    oddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);
                                    liveStyle = (CastAide.AsBoolean(dr["liveindicator"], false) ? "event-live" : "");
                                }
                                if (betType > 80 && betType < 91)
                                    oddsTypeName = CacheRef.GetOddsTypeName(dr["oddstype"]);
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
                        // TemplateHandler.Assign(ref strRow, "PTValue", "<font class='bl_underdog'>" + NumberFormat.DecFormat(awinlost, 2) + "</font><br />");
                        // TemplateHandler.Assign(ref strRow, "Comm", NumberFormat.DecFormat(adiscount, 2));
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

        #endregion renderBetList

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
            row.OddsType = CastAide.AsInt32(dr["OddsType"]);
            row.Odds = CastAide.AsDecimal(dr["Odds"]);
            row.Stake = CastAide.AsDecimal(dr["Stake"]);
            row.Betteam = CastAide.AsString(dr["Betteam"]);
            row.Status = CastAide.AsString(dr["Status"]);
            row.RefNO = CastAide.AsInt64(dr["RefNO"]);
            row.Transdate = CastAide.AsDateTime(dr["Transdate"]);
            row.Winlostdate = CastAide.AsDateTime(dr["Winlostdate"]);
            row.Winlost = CastAide.AsDecimal(dr["Winlost"]);
            row.awinlost = CastAide.AsDecimal(dr["LicenseeWL"]);
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

        private BetlistRow BAInputBetlistRow(DataRow dr)
        {
            var row = new BetlistRow();
            row.Sporttype = CastAide.AsInt32(dr["Sporttype"]);
            row.TransID = CastAide.AsInt64(dr["TransID"]);
            row.BetID = CastAide.AsInt64(dr["BetID"]);
            row.Bettype = CastAide.AsInt32(dr["Bettype"]);
            row.hdp1 = CastAide.AsDecimal(dr["hdp1"]);
            row.hdp2 = CastAide.AsDecimal(dr["hdp2"]);
            row.Modds = CastAide.AsDecimal(dr["Modds"]);
            row.OddsType = CastAide.AsInt32(dr["OddsType"]);
            row.Odds = CastAide.AsDecimal(dr["Odds"]);
            row.Stake = CastAide.AsDecimal(dr["Stake"]);
            row.Betteam = CastAide.AsString(dr["Betteam"]);
            row.Status = CastAide.AsString(dr["Status"]);
            row.RefNO = CastAide.AsInt64(dr["RefNO"]);
            row.Transdate = CastAide.AsDateTime(dr["Transdate"]);
            row.Winlostdate = CastAide.AsDateTime(dr["Winlostdate"]);
            row.Winlost = CastAide.AsDecimal(dr["Winlost"]);
            row.awinlost = CastAide.AsDecimal(dr["LicenseeWL"]);
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
            //row.Race = CastAide.AsString(dr["Race"]);
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

        #endregion InputBetlistRow

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
            row.OddsType = CastAide.AsInt32(dr["OddsType"]);
            row.Odds = CastAide.AsDecimal(dr["Odds"]);
            row.Stake = CastAide.AsDecimal(dr["Stake"]);
            row.Betteam = CastAide.AsString(dr["Betteam"]);
            row.Status = CastAide.AsString(dr["Status"]);
            row.RefNO = CastAide.AsInt64(dr["RefNO"]);
            row.Transdate = CastAide.AsDateTime(dr["Transdate"]);
            row.Winlostdate = CastAide.AsDateTime(dr["Winlostdate"]);
            row.Winlost = CastAide.AsDecimal(dr["Winlost"]);
            row.awinlost = CastAide.AsDecimal(dr["LicenseeWL"]);
            row.acomm = CastAide.AsDecimal(dr["comm"]);
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
            row.AcommMYR = CastAide.AsDecimal(dr["commMYR"]);

            row.StakeUSD = CastAide.AsDecimal(dr["StakeUSD"]);
            row.WinlostUSD = CastAide.AsDecimal(dr["WinlostUSD"]);
            row.AWinlostUSD = CastAide.AsDecimal(dr["LicenseeWLUSD"]);
            row.AcommUSD = CastAide.AsDecimal(dr["commUSD"]);

            row.StakeEUR = CastAide.AsDecimal(dr["StakeEUR"]);
            row.WinlostEUR = CastAide.AsDecimal(dr["WinlostEUR"]);
            row.AWinlostEUR = CastAide.AsDecimal(dr["LicenseeWLEUR"]);
            row.AcommEUR = CastAide.AsDecimal(dr["commEUR"]);
            return row;
        }

        #region IReportWLBABySiteService Members

        public List<BetListDetail> SelBetlistDepositReport(int custId, string fdate, string tdate, bool showsb, bool showng, bool showrb, int showba, bool showlc, int drawFilter)
        {
            var listBetlistDetail = new List<BetListDetail>();
            string MoveBetsDay = Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
            string BalanceUpDay = Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.BalanceUpDay.ToShortDateString();
            DbResult dbResult = _licenseReportDAO.SelBetlistByLicenseeV2(custId, fdate, tdate, MoveBetsDay, BalanceUpDay, showsb, showrb, showng, showba, showlc, false, false, drawFilter);
            DataTable dataTable = dbResult.ResultSet.Tables[0];
            if (dataTable != null)
            {
                var IsAdminCanViewVNIP = (WebSettings.IPVNReplace == "1") ? true : AppSession.ViewIPVN;

                foreach (DataRow row in dataTable.Rows)
                {
                    var bettran = new Bettran()
                    {
                        Transid = CastAide.AsString(row["transid"]),
                        HomeId = CastAide.AsString(row["homeid"]),
                        AwayId = CastAide.AsString(row["awayid"]),
                        Bettype = CastAide.AsInt32(row["bettype"]),
                        SportId = CastAide.AsString(row["sporttype"]),
                        Refno = CastAide.AsString(row["refno"]),
                        Matchid = CastAide.AsInt32(row["matchid"]),
                        Betid = CastAide.AsString(row["betid"]),
                        Hdp1 = CastAide.AsString(row["hdp1"]),
                        Hdp2 = CastAide.AsString(row["hdp2"]),
                        Odds = CastAide.AsString(row["odds"]),
                        OddsType = CastAide.AsInt32(row["OddsType"]),
                        Status = CastAide.AsString(row["status"]),
                        Stake = CastAide.AsString(row["stake"]),
                        Transdate = CastAide.AsString(row["transdate"]),
                        Betteam = CastAide.AsString(row["betteam"]),
                        Liveindicator = CastAide.AsBoolean(row["liveindicator"]),
                        Modds = CastAide.AsString(row["modds"]),
                        Custid = CastAide.AsInt32(row["custid"]),
                        Livehomescore = CastAide.AsString(row["livehomescore"]),
                        Liveawayscore = CastAide.AsString(row["liveawayscore"]),
                        LeagueId = CastAide.AsString(row["leagueid"]),
                        IsNeutral = CastAide.AsBoolean(row["isNeutral"]),
                        CurrencyName = CastAide.AsString(row["CurrencyName"]),
                        UserName = CastAide.AsString(row["UserName"]),
                        MatchCode = CastAide.AsString(row["matchcode"]),
                        Ip = CastAide.AsString(row["ip"]),
                        Winlost = CastAide.AsString(row["winlost"]),
                        Acomm = CastAide.AsString(row["comm"])
                    };
                    var betlistDetail = new BetListDetail()
                    {
                        Transid = bettran.Transid,
                        BettranDetail = bettran
                    };
                    if (!IsAdminCanViewVNIP)
                    {
                        var ipmanager = new IpManager(IpManagement.IPVNList);
                        betlistDetail.Ip = ipmanager.ReplaceVnIp(betlistDetail.Ip, WebSettings.IPVNReplace);
                    }

                    listBetlistDetail.Add(betlistDetail);
                }
            }
            return listBetlistDetail;
        }

        #endregion IReportWLBABySiteService Members

        /// <summary>
        /// Thomas is being use this function
        /// </summary>
        /// <param name="wldate"></param>
        /// <returns></returns>
        public List<Sunplus.Accounting.Entities.BetAggregator.BALicWinlossDaily> GetBALicWinlossDaily(System.DateTime wldate)
        {
            var listResult = new List<BALicWinlossDaily>();
            var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
            var dataTable = licenseeConfigDAO.GetBALicWinlossDaily(wldate);
            if (dataTable == null) return listResult;
            foreach (DataRow row in dataTable.Rows)
            {
                listResult.Add(new BALicWinlossDaily
                {
                    LicenseeName = DNAide.Core.CastAide.AsString(row["LicenseeName"]),
                    LicenseeID = DNAide.Core.CastAide.AsInt32(row["LicenseeID"]),
                    Currency = DNAide.Core.CastAide.AsInt32(row["Currency"]),
                    CurrencyName = DNAide.Core.CastAide.AsString(row["CurrencyName"]),
                    TurnOverRM = DNAide.Core.CastAide.AsDouble(row["turnoverRM"]),
                    WinlostVirtual = DNAide.Core.CastAide.AsDouble(row["WinlostVirtual"]),
                    WinlostRM = DNAide.Core.CastAide.AsDouble(row["WinlostRM"]),
                    AmtDueToBAVirtual = DNAide.Core.CastAide.AsDouble(row["AmtDueToBAVirtual"]),
                    AmtDueToBARM = DNAide.Core.CastAide.AsDouble(row["AmtDueToBARM"]),
                    CommVirtual = DNAide.Core.CastAide.AsDouble(row["CommVirtual"]),
                    CommRM = DNAide.Core.CastAide.AsDouble(row["CommRM"]),
                    BetCount = DNAide.Core.CastAide.AsInt32(row["BetCount"]),
                    LiveIndicator = DNAide.Core.CastAide.AsBoolean(row["liveindicator"])
                });
            }
            return listResult;
        }

        /// <summary>
        /// Thomas is being use this function
        /// </summary>
        /// <param name="wldate"></param>
        /// <returns></returns>
        public List<Sunplus.Accounting.Entities.BetAggregator.BALicWinlossDaily> GetBALicWinlossMonthToDate(System.DateTime wldate)
        {
            var listResult = new List<BALicWinlossDaily>();
            var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
            var dataTable = licenseeConfigDAO.GetBALicWinlossMonthToDate(wldate);
            if (dataTable == null) return listResult;
            foreach (DataRow row in dataTable.Rows)
            {
                listResult.Add(new BALicWinlossDaily
                {
                    LicenseeName = DNAide.Core.CastAide.AsString(row["LicenseeName"]),
                    LicenseeID = DNAide.Core.CastAide.AsInt32(row["LicenseeID"]),
                    Currency = DNAide.Core.CastAide.AsInt32(row["Currency"]),
                    CurrencyName = DNAide.Core.CastAide.AsString(row["CurrencyName"]),
                    TurnOverRM = DNAide.Core.CastAide.AsDouble(row["turnoverRM"]),
                    WinlostVirtual = DNAide.Core.CastAide.AsDouble(row["WinlostVirtual"]),
                    WinlostRM = DNAide.Core.CastAide.AsDouble(row["WinlostRM"]),
                    AmtDueToBAVirtual = DNAide.Core.CastAide.AsDouble(row["AmtDueToBAVirtual"]),
                    AmtDueToBARM = DNAide.Core.CastAide.AsDouble(row["AmtDueToBARM"]),
                    CommVirtual = DNAide.Core.CastAide.AsDouble(row["CommVirtual"]),
                    CommRM = DNAide.Core.CastAide.AsDouble(row["CommRM"]),
                    BetCount = DNAide.Core.CastAide.AsInt32(row["BetCount"])
                    //LiveIndicator = DNAide.Core.CastAide.AsBoolean(row["liveindicator"])
                });
            }
            return listResult;
        }

        public List<Sunplus.Accounting.Entities.BetAggregator.BALicWinlossDaily> GetBALicWinlossMonthly(System.DateTime wldate)
        {
            var listResult = new List<BALicWinlossDaily>();
            var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
            var dataTable = licenseeConfigDAO.GetBALicWinlossMonthly(wldate);
            if (dataTable == null) return listResult;
            foreach (DataRow row in dataTable.Rows)
            {
                listResult.Add(new BALicWinlossDaily
                {
                    LicenseeName = DNAide.Core.CastAide.AsString(row["LicenseeName"]),
                    LicenseeID = DNAide.Core.CastAide.AsInt32(row["LicenseeID"]),
                    Currency = DNAide.Core.CastAide.AsInt32(row["Currency"]),
                    CurrencyName = DNAide.Core.CastAide.AsString(row["CurrencyName"]),
                    TurnOverRM = DNAide.Core.CastAide.AsDouble(row["turnoverRM"]),
                    WinlostVirtual = DNAide.Core.CastAide.AsDouble(row["WinlostVirtual"]),
                    WinlostRM = DNAide.Core.CastAide.AsDouble(row["WinlostRM"]),
                    AmtDueToBAVirtual = DNAide.Core.CastAide.AsDouble(row["AmtDueToBAVirtual"]),
                    AmtDueToBARM = DNAide.Core.CastAide.AsDouble(row["AmtDueToBARM"]),
                    CommVirtual = DNAide.Core.CastAide.AsDouble(row["CommVirtual"]),
                    CommRM = DNAide.Core.CastAide.AsDouble(row["CommRM"]),
                    BetCount = DNAide.Core.CastAide.AsInt32(row["BetCount"]),
                    WinlostDate = DNAide.Core.CastAide.AsDateTime(row["winlostdate"]),
                });
            }
            return listResult;
        }

        /// <summary>
        /// Thomas is being use this function
        /// </summary>
        /// <param name="wldate"></param>
        /// <returns></returns>
        public List<Sunplus.Accounting.Entities.BetAggregator.BALicWinlossDaily> GetBALicWinlossYearToDate(System.DateTime wldate)
        {
            var listResult = new List<BALicWinlossDaily>();
            var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
            var dataTable = licenseeConfigDAO.GetBALicWinlossYearToDate(wldate);
            if (dataTable == null) return listResult;
            foreach (DataRow row in dataTable.Rows)
            {
                listResult.Add(new BALicWinlossDaily
                {
                    LicenseeName = DNAide.Core.CastAide.AsString(row["LicenseeName"]),
                    LicenseeID = DNAide.Core.CastAide.AsInt32(row["LicenseeID"]),
                    Currency = DNAide.Core.CastAide.AsInt32(row["Currency"]),
                    CurrencyName = DNAide.Core.CastAide.AsString(row["CurrencyName"]),
                    TurnOverRM = DNAide.Core.CastAide.AsDouble(row["turnoverRM"]),
                    WinlostVirtual = DNAide.Core.CastAide.AsDouble(row["WinlostVirtual"]),
                    WinlostRM = DNAide.Core.CastAide.AsDouble(row["WinlostRM"]),
                    AmtDueToBAVirtual = DNAide.Core.CastAide.AsDouble(row["AmtDueToBAVirtual"]),
                    AmtDueToBARM = DNAide.Core.CastAide.AsDouble(row["AmtDueToBARM"]),
                    CommVirtual = DNAide.Core.CastAide.AsDouble(row["CommVirtual"]),
                    CommRM = DNAide.Core.CastAide.AsDouble(row["CommRM"]),
                    BetCount = DNAide.Core.CastAide.AsInt32(row["BetCount"])
                    //LiveIndicator = DNAide.Core.CastAide.AsBoolean(row["liveindicator"])
                });
            }
            return listResult;
        }

        #region BA Winloss by Comm level

        public LicenseeWinLossCommDTO LicenseeWinLossByCommLevel(int licenseeId, int currencyId, bool isExcludeTest, string fromDate, string toDate, int drawFilter)
        {
            var result = new LicenseeWinLossCommDTO();
            try
            {
                var licenseeConfigDAO = CommonBootStrapper.Locator.GetInstance<ILicenseeConfigDAO>();
                var dt = licenseeConfigDAO.SelBAWinLossByCommLevel(licenseeId, CastAide.AsDateTime(fromDate),
                                                                   CastAide.AsDateTime(toDate), currencyId,
                                                                   drawFilter, isExcludeTest);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.Items = new List<LicenseeWinLossCommBO>();

                    double baseTurnover = 0, baseCustWL = 0, baseLicenseeComm = 0, baseAmountDue = 0;
                    double subBaseTurnover = 0, subBaseCustWL = 0, subBaseLincenseeComm = 0, subBaseAmountDue = 0;
                    double subTurnover = 0, subCustWL = 0, subLincenseeComm = 0, subAmountDue = 0;
                    int totalTrans = 0, subTotalTrans = 0;
                    var baseEuroSubfix = "EUR";
                    var curDate = DateTime.Now;
                    var pDate = DateTime.Now;
                    var curCurrency = 0;
                    var pCurrency = 0;
                    var currencyName = string.Empty;
                    var pCurrencyName = string.Empty;
                    var wlCommDate = new LicenseeWinLossCommBO();
                    wlCommDate.Items = new List<LicenseeWinLossCurrency>();

                    var wlCommCurrency = new LicenseeWinLossCurrency();
                    wlCommCurrency.Items = new List<LicenseeWinLossCommLevel>();
                    int index = 0, totalChilds = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        curDate = CastAide.AsDateTime(dr["winlostdate"]);
                        curCurrency = CastAide.AsInt32(dr["Currency"]);
                        currencyName = CastAide.AsString(dr["CurrencyName"]);

                        #region winloss date

                        if (curDate != pDate && index > 0)
                        {
                            #region last group currency

                            wlCommCurrency.Currency = pCurrencyName;
                            wlCommCurrency.CurrencyId = pCurrency.ToString();
                            wlCommDate.Items.Add(wlCommCurrency);

                            wlCommCurrency = new LicenseeWinLossCurrency();
                            wlCommCurrency.Items = new List<LicenseeWinLossCommLevel>();

                            #endregion last group currency

                            wlCommDate.TotalChilds = totalChilds;
                            wlCommDate.ReportDate = pDate.ToShortDateString();
                            wlCommDate.SubTotal = new LicenseeWinLossCommLevel()
                                                      {
                                                          AmountDue = NumberFormat.DecFormat(subAmountDue, 2),
                                                          BaseAmountDue = NumberFormat.DecFormat(subBaseAmountDue, 2),
                                                          BaseCustWinLoss = NumberFormat.DecFormat(subBaseCustWL, 2),
                                                          BaseLinceseeComm = NumberFormat.DecFormat(subBaseLincenseeComm, 2),
                                                          BaseTurnover = NumberFormat.DecFormat(subBaseTurnover, 2),
                                                          CustWinLoss = NumberFormat.DecFormat(subCustWL, 2),
                                                          LicenseeComm = NumberFormat.DecFormat(subLincenseeComm, 2),
                                                          Turnover = NumberFormat.DecFormat(subTurnover, 2),
                                                          TotalTrans = NumberFormat.DecFormat(subTotalTrans, 0)
                                                      };
                            result.Items.Add(wlCommDate);

                            totalChilds = 0;
                            pCurrency = 0;
                            subBaseTurnover = 0;
                            subBaseCustWL = 0;
                            subBaseLincenseeComm = 0;
                            subBaseAmountDue = 0;
                            subTurnover = 0;
                            subCustWL = 0;
                            subLincenseeComm = 0;
                            subAmountDue = 0;
                            subTotalTrans = 0;
                            wlCommDate = new LicenseeWinLossCommBO();
                            wlCommDate.Items = new List<LicenseeWinLossCurrency>();
                        }

                        #endregion winloss date

                        #region currency level

                        if (curCurrency != pCurrency && pCurrency > 0)
                        {
                            wlCommCurrency.Currency = pCurrencyName;
                            wlCommCurrency.CurrencyId = pCurrency.ToString();
                            wlCommDate.Items.Add(wlCommCurrency);

                            wlCommCurrency = new LicenseeWinLossCurrency();
                            wlCommCurrency.Items = new List<LicenseeWinLossCommLevel>();
                        }

                        #endregion currency level

                        #region detail row

                        var item = new LicenseeWinLossCommLevel()
                                       {
                                           Comm = CastAide.AsString(dr["discount"]),
                                           Bettype = CastAide.AsString(dr["Bettype"]),
                                           TotalTrans = NumberFormat.DecFormat(dr["Betcount"], 0),
                                           AmountDue = NumberFormat.DecFormat(dr["AmtDueToBA"], 2),
                                           CustWinLoss = NumberFormat.DecFormat(dr["MemberWinlost"], 2),
                                           LicenseeComm = NumberFormat.DecFormat(dr["LicenseeComm"], 2),
                                           Turnover = NumberFormat.DecFormat(dr["Turnover"], 2),
                                           BaseAmountDue = NumberFormat.DecFormat(dr["AmtDueToBA" + baseEuroSubfix], 2),
                                           BaseCustWinLoss = NumberFormat.DecFormat(dr["MemberWinLost" + baseEuroSubfix], 2),
                                           BaseLinceseeComm = NumberFormat.DecFormat(dr["LicenseeComm" + baseEuroSubfix], 2),
                                           BaseTurnover = NumberFormat.DecFormat(dr["Turnover" + baseEuroSubfix], 2)
                                       };
                        wlCommCurrency.Items.Add(item);

                        #endregion detail row

                        #region subtotal

                        subBaseTurnover += CastAide.AsDouble(dr["Turnover" + baseEuroSubfix]);
                        subBaseCustWL += CastAide.AsDouble(dr["MemberWinLost" + baseEuroSubfix]);
                        subBaseLincenseeComm += CastAide.AsDouble(dr["LicenseeComm" + baseEuroSubfix]);
                        subBaseAmountDue += CastAide.AsDouble(dr["AmtDueToBA" + baseEuroSubfix]);

                        subTurnover += CastAide.AsDouble(dr["Turnover"]);
                        subCustWL += CastAide.AsDouble(dr["MemberWinLost"]);
                        subLincenseeComm += CastAide.AsDouble(dr["LicenseeComm"]);
                        subAmountDue += CastAide.AsDouble(dr["AmtDueToBA"]);

                        subTotalTrans += CastAide.AsInt32(dr["Betcount"]);

                        #endregion subtotal

                        #region grand total

                        baseTurnover += CastAide.AsDouble(dr["Turnover" + baseEuroSubfix]);
                        baseCustWL += CastAide.AsDouble(dr["MemberWinLost" + baseEuroSubfix]);
                        baseLicenseeComm += CastAide.AsDouble(dr["LicenseeComm" + baseEuroSubfix]);
                        baseAmountDue += CastAide.AsDouble(dr["AmtDueToBA" + baseEuroSubfix]);
                        totalTrans += CastAide.AsInt32(dr["Betcount"]);

                        #endregion grand total

                        pDate = curDate;
                        pCurrency = curCurrency;
                        pCurrencyName = currencyName;
                        index++;
                        totalChilds++;
                    }

                    #region last date

                    #region last group currency

                    wlCommCurrency.Currency = pCurrencyName;
                    wlCommCurrency.CurrencyId = pCurrency.ToString();
                    wlCommDate.Items.Add(wlCommCurrency);

                    #endregion last group currency

                    wlCommDate.TotalChilds = totalChilds;
                    wlCommDate.ReportDate = pDate.ToShortDateString();
                    wlCommDate.SubTotal = new LicenseeWinLossCommLevel()
                    {
                        AmountDue = NumberFormat.DecFormat(subAmountDue, 2),
                        BaseAmountDue = NumberFormat.DecFormat(subBaseAmountDue, 2),
                        BaseCustWinLoss = NumberFormat.DecFormat(subBaseCustWL, 2),
                        BaseLinceseeComm = NumberFormat.DecFormat(subBaseLincenseeComm, 2),
                        BaseTurnover = NumberFormat.DecFormat(subBaseTurnover, 2),
                        CustWinLoss = NumberFormat.DecFormat(subCustWL, 2),
                        LicenseeComm = NumberFormat.DecFormat(subLincenseeComm, 2),
                        Turnover = NumberFormat.DecFormat(subTurnover, 2),
                        TotalTrans = NumberFormat.DecFormat(subTotalTrans, 0)
                    };
                    result.Items.Add(wlCommDate);

                    #endregion last date

                    #region grand total

                    result.Total = new LicenseeWinLossCommLevel()
                                     {
                                         TotalTrans = NumberFormat.DecFormat(totalTrans, 0),
                                         BaseAmountDue = NumberFormat.DecFormat(baseAmountDue, 2),
                                         BaseCustWinLoss = NumberFormat.DecFormat(baseCustWL, 2),
                                         BaseLinceseeComm = NumberFormat.DecFormat(baseLicenseeComm, 2),
                                         BaseTurnover = NumberFormat.DecFormat(baseTurnover, 2)
                                     };

                    #endregion grand total
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            SessionExtension.AddSession(HttpContext.Current.Session, _baWinLossByCommLevelSessionKey, result);
            return result;
        }

        #endregion BA Winloss by Comm level
    }
}