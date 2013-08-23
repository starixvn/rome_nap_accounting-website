using System.ServiceModel;

namespace Sunplus.Accounting.Business.PT.MiniGame
{
    [ServiceContract]
    public interface IServiceMiniGame
    {
        /*
    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetSuperPTSetting(string username);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string UpdateSuper(int custid, double spt, double minpt, double comm);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetMasterPTSetting(string username);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string UpdateMaster(int custid, double spt,bool isauto, double sautopt, double mpt, double minpt,double comm );

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetAgentPTSetting(string username);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string UpdateAgent(int custid, double mpt, double apt, double minpt, double comm);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetMemberPTSetting(string username);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string UpdateMember(int custid, double apt, double comm);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetSuperOnMemberPTSetting(string username);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string UpdateSuperOnMember(int custid, double spt, bool isautopt, double sautopt);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GetMasterOnMemberPTSetting(string username);

    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string UpdateMasterOnMember(int custid, double mpt);
     * */
    }
}