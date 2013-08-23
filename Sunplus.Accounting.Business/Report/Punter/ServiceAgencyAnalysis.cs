using System.ServiceModel.Activation;
using BLL.Report.Punter;

/// <summary>
/// Summary description for DailyTransManager
/// </summary>
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class ServiceAgencyAnalysis : IServiceAgencyAnalysis
{
    #region IServiceAgencyAnalysis Members

    public string GeneratePeriodLongRange(string currfMonthValue, string currfYearValue, string currtMonthValue,
                                          string currtYearValue)
    {
        var agency = new AgencyAnalysisManager();
        return agency.GeneratePeriodLongRange(currfMonthValue, currfYearValue, currtMonthValue, currtYearValue);
    }

    public string GeneratePeriodLongRangeNone()
    {
        var agency = new AgencyAnalysisManager();
        return agency.LoadHeader();
    }

    public string DirectCustomerReport(string fMonth, string fYear, string tMonth, string tYear, string statistic,
                                       string username)
    {
        var agency = new AgencyAnalysisManager();
        return agency.DirectCustomerReport(fMonth, fYear, tMonth, tYear, statistic, username);
    }

    public void DirectCustomerReportExport(string fMonth, string fYear, string tMonth, string tYear, string statistic,
                                           string username)
    {
        var agency = new AgencyAnalysisManager();
        agency.DirectCustomerReportExport(fMonth, fYear, tMonth, tYear, statistic, username);
    }

    public void GenerateChartByCustIdExport(string custId, string username, string fMonth, string fYear, string tMonth,
                                            string tYear)
    {
        var agency = new GenerateChartAgencyAnalysis();
        agency.GenerateChartByCustId(custId, username, fMonth, fYear, tMonth, tYear);
    }

    #endregion IServiceAgencyAnalysis Members

    public string DirectCustomerReportExportOnly(string fMonth, string fYear, string tMonth, string tYear,
                                                 string statistic, string username)
    {
        var agency = new AgencyAnalysisManager();
        return agency.DirectCustomerReport(fMonth, fYear, tMonth, tYear, statistic, username);
    }
}