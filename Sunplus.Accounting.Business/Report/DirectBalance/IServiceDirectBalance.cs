using System.ServiceModel;
using System.ServiceModel.Web;
using ExcelUtil.Spreadsheet;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
[ServiceContract]
public interface IServiceDirectBalance
{
    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string LoadHeader();

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetDirectBalance(string displayType);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetDirectBalance_Downline(int custid, int pcustid, int roleid, string user);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    ExcelFile ExportToExcel();

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string getHeaderDownLine(int roleid, string user, string custid, string puser, int pcustid);
}