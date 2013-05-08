using System.ServiceModel.Activation;
using BLL.Report.NonePT;
using ExcelUtil.Spreadsheet;

/// <summary>
/// Summary description for DailyTransManager
/// </summary>
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class ServiceNonePTMemSummary : IServiceNonePTMemSummary
{
    #region IServiceNonePTMemSummary Members

    public string LoadHeader()
    {
        var s = new NonePTMemSummaryManager();
        return s.LoadHeader();
    }

    public string getNonePTMembers(string username, string currpage)
    {
        var s = new NonePTMemSummaryManager();
        return s.getNonePTMembers(username, currpage);
    }

    public ExcelFile ExportToExcel_NonePTMemSummary(string username, string currpage)
    {
        var s = new NonePTMemSummaryExportExcelManager();
        return s.ExportToExcel_NonePTMemSummary(username, currpage);
    }

    #endregion
}