using System.ServiceModel;
using System.ServiceModel.Web;

namespace Sunplus.Accounting.WcfServiceLibrary.Customer
{
    /// <summary>
    /// Summary description for IServiceCustomerList
    /// </summary>
    [ServiceContract]
    public interface IServiceSuspend
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenSuspendListContent(string searchText, string selectAlpha, int PageIndex);
    }
}