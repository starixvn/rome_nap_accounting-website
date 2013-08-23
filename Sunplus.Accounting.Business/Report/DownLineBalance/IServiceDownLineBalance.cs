using System.ServiceModel;
using System.ServiceModel.Web;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Entities.Report.DownLinceBalance;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
[ServiceContract]
public interface IServiceDownLineBalance
{
    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string LoadHeader();

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string MasterBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch, string firstletter,
                         string currency);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SuperBalance(string db, bool isYesterday, string usernameSearch, string firstletter, string currency);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetCompanyBalanceHistory();

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    ExcelFile ExportToExcel();

    CurrencyDTO GetCurrenciesBySystem();

    DownLineBalanceDTO GetSuperBalance(string db, bool isYesterday, string usernameSearch, string firstletter, string currency, bool isViewTotalOnly);

    DownLineBalanceDTO GetMasterBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch,
                                         string firstletter, string currency);

    DownLineBalanceDTO GetAgentBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch,
                                         string firstletter, string currency);

    DownLineBalanceDTO GetMemberBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch,
                                         string firstletter, string currency);

    void ExportExcelDownLineBalance(int roleid, int custid, bool isYesterday, string usernameSearch, string firstletter,
                                    string currency, string reportTitle, bool isViewTotalOnly);

    DifferenceBalanceInfo GetDifferenceBalance();
}