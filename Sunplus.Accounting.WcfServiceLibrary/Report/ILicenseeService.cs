using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Report.Deposit;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [ServiceContract]
    public interface ILicenseeService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ConcurrentUserDTO GetConcurrentUserList(string siteName, string fromDate, string toDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void PeakExport2Excel(string siteName, string fromDate, string toDate);
    }
}