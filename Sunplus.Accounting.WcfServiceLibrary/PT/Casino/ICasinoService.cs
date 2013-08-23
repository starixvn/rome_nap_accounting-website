using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Casino;

namespace Sunplus.Accounting.WcfServiceLibrary.PT.Casino
{
    [ServiceContract]
    public interface ICasinoService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void AutoInsertCasinoAccount(CSCustomerDetail customerInfo, int userId, string userName, string ip);
    }
}