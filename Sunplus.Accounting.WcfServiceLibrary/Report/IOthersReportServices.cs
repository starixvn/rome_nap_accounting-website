using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Report.Others;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [ServiceContract]
    public interface IOthersReportServices
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        NumberOfBetsDTO SelectNumberOfBetsForCreditSites(string fromDate, string toDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        DuplicateTicketsDTO SelDuplicateTickets(string wldate);
    }
}