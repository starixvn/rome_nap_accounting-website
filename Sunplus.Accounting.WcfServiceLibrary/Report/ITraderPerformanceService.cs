using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Report;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [ServiceContract]
    public interface ITraderPerformanceService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        TraderPerformanceDTO GetTraderPerformance(string fromDate, string toDate, int isSoccer);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void TraderPerformanceExport2Excel(string fromDate, string toDate, int isSoccer);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        TraderPerformanceDetailDTO GetTraderPerformanceReportDetail(string fromDate,
            string toDate,
            int isSoccer,
            int pageIndex,
            int pageSize,
            bool getNewData,
            string sortField,
            int sortFactor,
            string sortDataType);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void TraderPerformanceDetailExport2Excel(string fromDate, string toDate, int isSoccer);
    }
}
