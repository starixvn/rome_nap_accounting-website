using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Report;

namespace Sunplus.Accounting.Business.Report.Other
{
    public interface ITraderPerformanceManager
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        TraderPerformanceDTO GetTraderPerformance(string fromDate, string toDate, int isSoccer);

        [OperationContract]
        void TraderPerformanceExport2Excel(string  fromDate, string toDate, int isSoccer);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        TraderPerformanceDetailDTO GetTraderPerformanceDetail(string fromDate, string toDate, int isSoccer, bool isGetNewData);

        [OperationContract]
        void TraderPerformanceDetailExport2Excel(string fromDate, string toDate, int isSoccer);
    }
}
