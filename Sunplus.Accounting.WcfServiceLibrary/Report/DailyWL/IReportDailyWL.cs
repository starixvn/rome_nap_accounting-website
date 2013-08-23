using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Report.DailyWL;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.DailyWL
{
    [ServiceContract]
    public interface IReportDailyWL
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        DailyWLBO GetWinlossRacingReport(string winlossdate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        DailyWLBO DailyWLDirectRacingBySport(string sporttype, string winlossdate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        DailyWLDTO DailyWLDirectRacingBySportDetails(string winlossdate, string sporttype, string bettype);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void Export2ExcelDailyWLDirectRacing(string winlossDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void Export2ExcelDailyWLDirectRacingBySport(string sporttype, string winlossDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void Export2ExcelDailyWLDirectRacingBySportDetails(string sporttype, string bettype, string winlossDate);
    }
}