using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data;
using System.Collections.Generic;

namespace Sunplus.Accounting.WcfServiceLibrary.Customer
{
    /// <summary>
    /// Summary description for IServiceCustomerList
    /// </summary>
    [ServiceContract]
    public interface IServiceCustomerList
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenSuperListContent(string searchText, string selectAlpha, string selectCurreny, int type, string status,
                                   int PageIndex);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenMasterListContent(string superId, string hr, string searchText, string selectAlpha, string selectCurreny,
                                    int type, string status, int PageIndex, string synccasinoUpline);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenAgentListContent(string masterId, string hr, int PageIndex, string syncCasinoUpline);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenMemberListContent(string agentId, string hr, string isInternal, int PageIndex, string isSynccasino);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenOperatorListContent(string searchText, string selectAlpha, int PageIndex);
    }
}