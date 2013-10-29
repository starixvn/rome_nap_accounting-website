using System.ServiceModel;
using System.ServiceModel.Web;
using ExcelUtil.Spreadsheet;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
[ServiceContract]
public interface IServiceOpenToteSettlement
{
    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string LoadHeader(string winlostdate, string db);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string WinlostHR_OpenTote(string winlostdate, string db, bool IsTest);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string WinlostHR_OpenTote_Details(string winlostdate, string partnertype, string poolcurrency, string db,
                                      bool IsTest);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string ViewBetList(string leagueid, string raceno, string currencyid, string winlostdate, string db, bool IsTest);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    ExcelFile Export2ExcelWinlostHR_OpenTote(string winlostdate, string db, bool IsTest);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    ExcelFile ExportToExcel_OpenToteDetail(string winlostdate, string PartnerType, string PoolCurrency, string db,
                                           bool IsTest);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string LoadComparision(string winlostdate, string ibcTO, string ibcDividendWon);
}