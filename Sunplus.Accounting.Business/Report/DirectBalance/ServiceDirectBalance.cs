using System;
using System.ServiceModel.Activation;
using BLL.Report.DirectBalance;
using ExcelUtil.Spreadsheet;

/// <summary>
/// Summary description for DailyTransManager
/// </summary>
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class ServiceDirectBalance : IServiceDirectBalance
{
    #region IServiceDirectBalance Members

    public string LoadHeader()
    {
        throw new NotImplementedException();
    }

    public string GetDirectBalance(string displayType)
    {
        var s = new DirectBalanceManager();
        return s.GetDirectBalance(displayType);
    }

    public string GetDirectBalance_Downline(int custid, int pcustid, int roleid, string user)
    {
        var s = new DirectBalanceManager();
        return s.GetDirectBalance_Downline(custid, pcustid, roleid, user);
    }


    public ExcelFile ExportToExcel()
    {
        throw new NotImplementedException();
    }

    public string getHeaderDownLine(int roleid, string user, string custid, string puser, int pcustid)
    {
        var s = new DirectBalanceManager();
        return s.getHeaderDownLine(roleid, user, custid, puser, pcustid);
    }

    #endregion
}