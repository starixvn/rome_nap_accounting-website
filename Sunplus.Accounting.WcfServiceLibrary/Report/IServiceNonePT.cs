using System.ServiceModel;
using System.ServiceModel.Web;
using ExcelUtil.Spreadsheet;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [ServiceContract]
    public interface IServiceNonePT
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string LoadHeader();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string getNonePTMembers(string username, string currpage);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExcelFile ExportToExcel_NonePTMemSummary(string username, string currpage);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string getNonePTLastestBet(string username, string currency, string bettype, string fromstake, string tostake,
                                   string pageindex, string pagesize);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string getAllBettypes();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string getAllCurrencies();
    }
}