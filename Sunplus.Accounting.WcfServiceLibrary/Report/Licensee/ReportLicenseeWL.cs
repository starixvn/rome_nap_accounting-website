using System.Collections.Generic;
using System.ServiceModel.Activation;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report.Deposit;
using Sunplus.Accounting.Entities.Report.Deposit;
using Sunplus.Accounting.Entities.Report.Licensee;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.Licensee
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReportLicenseeWL : IReportLicenseeWL
    {
        public List<Item> SelLicensees(bool IsAll)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelLicensees(IsAll);
        }

        public List<Reseller> GetLicenseeByReseller()
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.GetLicenseeByReseller();
        }

        public LicenseeDTO SelCurrency(string siteId)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelCurrency(siteId);
        }

        public LicenseeDTO GetDatetimeOption(string dateOption)
        {
            var deposit = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return deposit.GetDatetimeOption(dateOption);
        }

        public LicenseeDTO SelReportByLicensee(int LicenseeID, int Currency, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelReportByLicensee(LicenseeID, Currency, baseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG,
                                                        ReportType, drawfilter, excludeTest, BAFilter, pageIndex, pageSize, fromCache);
        }

        public LicenseeDTO SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelReportByLicenseeDetail_Ag(LicenseeID, Currency, baseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG,
                                                        ReportType, drawfilter, excludeTest, BAFilter, pageIndex, pageSize, fromCache);
        }

        public LicenseeDTO SelReportByLicenseeDetail(int LicenseeID, int Currency, int baseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int exlcudeTest, int BAFilter, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelReportByLicenseeDetail(LicenseeID, Currency, baseCurrency, CustId, FromDate, ToDate, showSB, ShowRB, ShowNG,
                                                        ReportType, drawfilter, exlcudeTest, BAFilter, pageIndex, pageSize, fromCache);
        }

        public LicenseeTotalTransctionDTO SelTotalTransction(int resellerid, int licenseeid, string date, bool showSB, bool showNG, bool showRB, bool showLC, bool showVS, bool showCS)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelTotalTransction(resellerid, licenseeid, date, showSB, showNG, showRB, showLC, showVS, showCS);
        }

        public NegativeBalanceMemberDTO SelNegativeBalanceMember(string site, int pageIndex, int pageSize, bool fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelNegativeBalanceMember(site, pageIndex, pageSize, fromCache);
        }

        public NegativeBalanceMemberSendMailFail SendMailNegativeBalanceMember(List<NegativeBalanceMemberMailFormat> mails, string reportTitle, string subject, string mailContent)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SendMailNegativeBalanceReport(mails, reportTitle, subject, mailContent);
        }

        #region IReportLicenseeWL Members

        public LicenseeCurrency SelReportByLicenseeV2(int LicenseeID, int Currency, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelReportByLicenseeV2(LicenseeID, Currency, baseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ShowVS, ShowCS, ReportType, drawfilter, excludeTest, pageIndex, pageSize, fromCache);
        }

        #endregion IReportLicenseeWL Members

        public LicenseeDTOV2 SelReportLicenseeDetail_AgV2(int LicenseeID, int Currency, int CustId, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelReportByLicenseeDetail_AgV2(LicenseeID, Currency, CustId, baseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ShowVS, ShowCS, ReportType, drawfilter, excludeTest, pageIndex, pageSize, fromCache);
        }

        public LicenseeDTOV2 SelReportLicenseeDetailV2(int LicenseeID, int Currency, int baseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelReportLicenseeDetailV2(LicenseeID, Currency, baseCurrency, CustId, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ShowVS, ShowCS, ReportType, drawfilter, excludeTest, pageIndex, pageSize, fromCache);
        }

        public ConcurrentUserDTO GetConcurrentUserList(string siteName, string fromDate, string toDate)
        {
            var result = CommonBootStrapper.Locator.GetInstance<IReportWLBySiteService>();
            return result.GetConcurrentUser(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), siteName);
        }

        public void PeakExport2Excel(string siteName, string fromDate, string toDate)
        {
            var result = CommonBootStrapper.Locator.GetInstance<IReportWLBySiteService>();
            result.PeakUserExport2Excel(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), siteName);
        }

        public LicenseeDTOV2 SelReportLicenseeDetail_SAgV2(int LicenseeID, int Currency, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelReportByLicenseeDetail_SAgV2(LicenseeID, Currency, baseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ShowVS, ShowCS, ReportType, drawfilter, excludeTest, pageIndex, pageSize, fromCache);
        }

        public LicenseeDTOV2 SelReportLicenseeDetail_MAgV2(int LicenseeID, int Currency, int CustId, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ShowLC, int ShowVS, int ShowCS, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            return licenseeList.SelReportByLicenseeDetail_MAgV2(LicenseeID, Currency, CustId, baseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG, ShowBA, ShowLC, ShowVS, ShowCS, ReportType, drawfilter, excludeTest, pageIndex, pageSize, fromCache);
        }
    }
}