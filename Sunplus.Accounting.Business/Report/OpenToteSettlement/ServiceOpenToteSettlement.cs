using System.ServiceModel.Activation;
using BLL.Report.OpenToteSettlement;
using ExcelUtil.Spreadsheet;

/// <summary>
/// Summary description for DailyTransManager
/// </summary>
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class ServiceOpenToteSettlement : IServiceOpenToteSettlement
{
    #region IServiceOpenToteSettlement Members

    public string LoadHeader(string winlostdate, string db)
    {
        var s = new OpenToteSettlementManager();
        return s.LoadHeader(winlostdate, db);
    }

    public string WinlostHR_OpenTote(string winlostdate, string db, bool IsTest)
    {
        var s = new OpenToteSettlementManager();
        return s.WinlostHR_OpenTote(winlostdate, db, IsTest);
    }

    public string WinlostHR_OpenTote_Details(string winlostdate, string partnertype, string poolcurrency, string db,
                                             bool IsTest)
    {
        var s = new OpenToteSettlementDetailsManager();
        return s.WinlostHR_OpenTote_Details(winlostdate, partnertype, poolcurrency, db, IsTest);
    }

    public string ViewBetList(string leagueid, string raceno, string currencyid, string winlostdate, string db,
                              bool IsTest)
    {
        var s = new OpenToteSettlementBetlistManager();
        return s.ViewBetList(leagueid, raceno, currencyid, winlostdate, db, IsTest);
    }

    public ExcelFile Export2ExcelWinlostHR_OpenTote(string winlostdate, string db, bool IsTest)
    {
        var s = new OpenToteSettlementExportExceManager();
        return s.ExportToExcel_OpenTote(winlostdate, db, IsTest);
    }

    public ExcelFile ExportToExcel_OpenToteDetail(string winlostdate, string PartnerType, string PoolCurrency, string db,
                                                  bool IsTest)
    {
        var s = new OpenToteSettlementDetailsExportExceManager();
        return s.ExportToExcel_OpenToteDetail(winlostdate, PartnerType, PoolCurrency, db, IsTest);
    }

    public string LoadComparision(string winlostdate, string ibcTO, string ibcDividendWon)
    {
        var s = new OpenToteSettlementManager();
        return s.LoadComparision(winlostdate, ibcTO, ibcDividendWon);
    }

    #endregion
}