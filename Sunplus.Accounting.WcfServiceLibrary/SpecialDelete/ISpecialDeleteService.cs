using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.SpecialDelete;
using Sunplus.Accounting.Entities.SpecialDelete.EarlyKO;
using Sunplus.Accounting.Entities.SpecialDelete.Mixparlay;

namespace Sunplus.Accounting.WcfServiceLibrary.SpecialDelete
{
    /// <summary>
    /// Summary description for IMenuManager
    /// </summary>
    /// 
    [ServiceContract]
    public interface ISpecialDeleteService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        SpecialDeleteDTO GetSpecialDeleteNG(int matchId, long fromRefNo, long toRefNo, string live, string dead, string userName);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result DeleteSpecialTransactionNumberGame(int matchId, long frefno, long trefno, string live, string dead,
                     int DelOption, string transids,
            int DelOption_BD, string transids_BD, int DelOption_12EU, string transids_12EU, int DelOption_MHC, string transids_MHC,
            Message message, string reason);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result DeleteSpecialTransaction(string matchId, string frefno, string trefno, string bettype,
                                          string betteam1378, string betteam515, string hdp, string uname,
                                          string userId, string pass, string DelOption, string transids,
                                          string Liveindicator, string DelOption_BD, string transids_BD,
                                          string DelOption_12EU, string transids_12EU,
                                          string DelOption_MHC, string transids_MHC, string betteam430,
                                          string betteam6, string betteam14, string betteam16, Message message,
                                          string homescore, string awayscore, string Fromtransdate, string Totransdate, int SAFilter,
                                          string reason);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        SpecialDeleteDTO SelRecoverBets(string transid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result RecoverBet(string transid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        SpecialDeleteDTO BettransSelSpecialDel(SpecialDeleteParam param);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        SpecialDeleteDTO GetEarlyKOBetList(SpecialDeleteParamEarlyKO param);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MixparlayChoice GetEarlyKOMixparlay(string refno, string transid, string currency, string subtransid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result DeleteSpecialEarlyKO(string matchId, string bettype, string betteam1378, string betteam515, 
            string hdp, string uname, string userId, string pass, string DelOption, string transids, string kickoffdate, 
            string DelOption_BD, string transids_BD, string DelOption_12EU, string transids_12EU, string DelOption_MHC,
            string transids_MHC, string betteam430, string betteam6, string betteam14, string betteam16, Message message, int SAFilter);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Dictionary<int, string> GetBettype(int type);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MixParlayBO GetMPList(SpecialDeleteParam param);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Dictionary<string, string> GetMPTicket(string transids, string matchid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result DelMPList(string matchId, int Type, string HomeScore, string AwayScore, string bettypes,
            string betteam1378, string betteam515, string hdp,
            string uname, string userId, string pass,
            string DelOption, string transids,
            string DelOption_D, string transids_D,
            string DelOption_BD, string transids_BD,
            string DelOption_12EU, string transids_12EU,
            string DelOption_MHC, string transids_MHC,
            string betteam430, string betteam6, string betteam14, string betteam16, string Fromtransdate, string Totransdate, string ParlayType, 
            MessageMP message);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MixParlayBOPager GetMPListPager(SpecialDeleteParamPager param);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Dictionary<int, string> CheckRefno(string matchid, string fromRefno, string toRefno, string type);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Dictionary<string, string> UploadMsg(Message pmsg, string TransIds);
    }
}
