using System.ServiceModel;
using System.ServiceModel.Web;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
[ServiceContract]
public interface IServicePunterAnalysis
{
    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GeneratePeriodMonthly(string currMonthValue, string currYearValue);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GeneratePeriodAnnual(string currYearValue);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GeneratePeriodLongRange(string currfMonthValue, string currtMonthValue, string currfYearValue,
                                   string currtYearValue);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelTopPunterSummary(string username, string fromdate, string todate, string toprow, string orderby,
                               string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void SelTopPunterSummaryExportOnly(string username, string fromdate, string todate, string toprow, string orderby,
                                       string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelTopPunterByCustId(string custid, string username, string fromdate, string todate, string orderby,
                                string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void SelTopPunterByCustIdExport(string custid, string username, string fromdate, string todate, string orderby,
                                    string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelTopPunterBySporttype(string custid, string username, string sporttype, string fromdate, string todate,
                                   string orderby, string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void SelTopPunterBySporttypeExport(string custid, string username, string sporttype, string fromdate, string todate,
                                       string orderby, string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelTopPunterByBettype(string custid, string username, string sporttype, string bettype, string fromdate,
                                 string todate, string orderby, string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void SelTopPunterByBettypeExport(string custid, string username, string sporttype, string bettype, string fromdate,
                                     string todate, string orderby, string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelIpByCustomer(string custid, string username, string fromdate, string todate, ref string fullpath);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void SelIpByCustomerExport(string custid, string username, string fromdate, string todate);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelBetCountByCustomer(string custid, string username, string fromdate, string todate, ref string fullpath,
                                 string leagueid);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void SelBetCountByCustomerExport(string custid, string username, string fromdate, string todate, string leagueid);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelSports(string fromdate, string todate, string sporttype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelLeagues(string fromdate, string todate, string sporttype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelTopPunterSummaryByLeague(string leagueid, string leaguename, string fromdate, string todate, string toprow,
                                       string orderby, string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void SelTopPunterSummaryByLeagueExport(string leagueid, string leaguename, string fromdate, string todate,
                                           string toprow, string orderby, string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string SelTopPunterByLeagueCustId(string custId, string leagueid, string username, string sporttype, string fromdate,
                                      string todate, string orderby, string ordertype);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    void SelTopPunterByLeagueCustIdExport(string custId, string leagueid, string username, string sporttype,
                                          string fromdate, string todate, string orderby, string ordertype);
}