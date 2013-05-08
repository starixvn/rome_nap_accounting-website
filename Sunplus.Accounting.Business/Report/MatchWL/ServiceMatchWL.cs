using System;
using System.Collections;
using System.ServiceModel.Activation;
using BLL.Report.MatchWL;
using ExcelUtil.Spreadsheet;

/// <summary>
/// Summary description for DailyTransManager
/// </summary>
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class ServiceMatchWL : IServiceMatchWL
{
    #region IServiceMatchWL Members

    public string LoadHeaderMatchWLSingle(Hashtable input)
    {
        var s = new MatchWLSingleManager();
        return s.LoadHeaderMatchWLSingle(input);
    }

    public string MatchWLSingle(Hashtable input)
    {
        var s = new MatchWLSingleManager();
        return s.MatchWLSingle(input);
    }

    public void ExportExcelMatchWLSingle(Hashtable input)
    {
        var s = new MatchWLSingleManager();
        s.ExportExcelMatchWLSingle(input);
    }

    public ExcelFile ExportToExcel()
    {
        throw new NotImplementedException();
    }


    public string MatchWL2a(string db, string type, string fdate)
    {
        var s = new MatchWLManager();
        return s.MatchWL2a(db, type, fdate);
    }

    public string MatchWLByBettype(string db, string fdate)
    {
        var s = new MatchWLManager();
        return s.WLByBettype(db, fdate);
    }

    #endregion
}