using System.ServiceModel;
using System.ServiceModel.Web;
using ExcelUtil.Spreadsheet;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
[ServiceContract]
public interface IServiceHRGroupDaily
{
    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string HRGroupDaily_ToteOnly(string fromdate, string todate, bool IsSendEmail);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string HRGroupDaily(string fromdate, string todate, bool IsSendEmail);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    ExcelFile ExportExcelHRGroupDaily(string fromdate, string todate);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    ExcelFile ExportExcelHRGroupDaily_ToteOnly(string fromdate, string todate);
}