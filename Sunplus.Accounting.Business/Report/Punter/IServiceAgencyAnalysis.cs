using System.ServiceModel;
using System.ServiceModel.Web;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
[ServiceContract]
public interface IServiceAgencyAnalysis
{
    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GeneratePeriodLongRange(string currfMonthValue, string currfYearValue, string currtMonthValue,
                                   string currtYearValue);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GeneratePeriodLongRangeNone();

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string DirectCustomerReport(string fMonth, string fYear, string tMonth, string tYear, string statistic,
                                string username);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void DirectCustomerReportExport(string fMonth, string fYear, string tMonth, string tYear, string statistic,
                                    string username);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void GenerateChartByCustIdExport(string custId, string username, string fMonth, string fYear, string tMonth,
                                     string tYear);
}