using System;
using System.Collections.Generic;
using System.ServiceModel;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Entities.Report.Deposit;
using Sunplus.Accounting.Entities.Report.Licensee;

namespace Sunplus.Accounting.Business.Report.Deposit
{
    [ServiceContract]
    public interface IReportWLBySiteService
    {
        List<Item> SelLicensees(bool IsAll);

        List<Item> SelLicensees(bool IsAll, bool Checkauthentication);

        LicenseeDTO SelCurrency(string siteId);

        LicenseeDTO GetDatetimeOption(string dateOption);

        LicenseeDTO SelReportByLicensee(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFiter, int pageIndex, int pageSize, int fromCache);

        LicenseeCurrency SelReportByLicenseeV2(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        LicenseeDTO SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        LicenseeDTOV2 SelReportByLicenseeDetail_SAgV2(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        LicenseeDTOV2 SelReportByLicenseeDetail_MAgV2(int LicenseeID, int Currency, int custId, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        LicenseeDTOV2 SelReportByLicenseeDetail_AgV2(int LicenseeID, int Currency, int custId, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        LicenseeDTO SelReportByLicenseeDetail(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache);

        LicenseeDTOV2 SelReportLicenseeDetailV2(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        ExcelFile ExportLicenseeReport(string site, string fdate, string tdate, ref bool hasData, string reportType, string level, string currency, string baseCurrency);

        ConcurrentUserDTO GetConcurrentUser(DateTime dateFrom, DateTime dateTo, string licenseSite);

        void PeakUserExport2Excel(DateTime dateFrom, DateTime dateTo, string licenseSite);

        void Export2ExcelReportByLicensee(string reportTitle, int licenseeId, int currency, int BaseCurrency, string fromDate,
                                          string toDate, int showSb, int showRb, int showNg, int reportType, int drawFilter, int excludeTest, int BAFilter,
                                          int pageIndex, int pageSize, int reportSelected, int custId);

        void Export2ExcelReportByLicenseeV2(string reportTitle, int licenseeId, int currency, int BaseCurrency, string fromDate,
                                          string toDate, int showSb, int showRb, int showNg, int showBa, int showLc, int ShowVS, int ShowCS, int reportType, int drawFilter, int excludeTest,
                                          int pageIndex, int pageSize, int reportSelected, int custId);

        LicenseeTotalTransctionDTO SelDateTotalTransction();

        List<Reseller> GetLicenseeByReseller();

        LicenseeTotalTransctionDTO SelTotalTransction(int resellerid, int licenseeid, string date, bool showSB, bool showNG, bool showRB, bool showLC, bool showVS, bool showCS);

        void Export2ExcelTotalTransaction(string reportTitle, int resellerid, int licenseeid, string date, bool showSB, bool showNG, bool showRB, bool showLC, bool showVS, bool showCS);

        NegativeBalanceMemberDTO SelNegativeBalanceMember(string site, int pageIndex, int pageSize, bool fromCache);

        void Export2ExcelNegativeBalanceReport(string reportTitle, string site);

        NegativeBalanceMemberSendMailFail SendMailNegativeBalanceReport(List<NegativeBalanceMemberMailFormat> mails, string reportTitle, string subject, string mailContent);

        //void Export2ExcelReportByLicensee(string reportTitle, int licensseId, int currency, int baseCurrency, string fromDate, string toDate, int showSb, int showRb, int reportType, int drawFilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int reportSelected, int custId);

        #region NAPExport

        void ExportCurrencyReport(string reportTitle, int licenseeId,
           int currency, int BaseCurrency,
           string fromDate, string toDate,
           int showSb, int showRb,
           int showNg, int showBa,
           int showLc, int ShowVS, int ShowCS, int reportType,
           int drawFilter, int excludeTest,
           int pageIndex, int pageSize,
           int reportSelected, int custId);

        void ExportSuperReport(string reportTitle, int licenseeId,
           int currency, int BaseCurrency,
           string fromDate, string toDate,
           int showSb, int showRb,
           int showNg, int showBa,
           int showLc, int ShowVS, int ShowCS, int reportType,
           int drawFilter, int excludeTest,
           int pageIndex, int pageSize,
           int reportSelected, int custId);

        void ExportMasterReport(string reportTitle, int licenseeId,
           int currency, int BaseCurrency,
           string fromDate, string toDate,
           int showSb, int showRb,
           int showNg, int showBa,
           int showLc, int showVS, int showCS, int reportType,
           int drawFilter, int excludeTest,
           int pageIndex, int pageSize,
           int reportSelected, int custId);

        void ExportAgentReport(string reportTitle, int licenseeId,
           int currency, int BaseCurrency,
           string fromDate, string toDate,
           int showSb, int showRb,
           int showNg, int showBa,
           int showLc, int showVS, int showCS, int reportType,
           int drawFilter, int excludeTest,
           int pageIndex, int pageSize,
           int reportSelected, int custId);

        void ExportMemberReport(string reportTitle, int licenseeId,
           int currency, int BaseCurrency,
           string fromDate, string toDate,
           int showSb, int showRb,
           int showNg, int showBa,
           int showLc, int showVS, int ShowCS, int reportType,
           int drawFilter, int excludeTest,
           int pageIndex, int pageSize,
           int reportSelected, int custId);

        #endregion NAPExport
    }
}