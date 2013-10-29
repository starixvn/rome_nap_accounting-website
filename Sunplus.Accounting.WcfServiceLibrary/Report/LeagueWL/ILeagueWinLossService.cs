using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Report.LeagueWL;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.LeagueWL
{
    [ServiceContract]
    public interface ILeagueWinLossService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        LeagueWLBySportsDTO SelLeagueWLBySports(string winlostdate, bool isGetNew);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        LeagueWLBettypeDTO SelLeagueWLByBettypes(int sportId, int leagueId, string winlostDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        LeagueWLBettypeDetailBO SelLeagueWLDetailByBettypes(int sportId, int leagueId, int bettypeId, bool isLive,
                                                       bool isHedge, string winlostDate);
    }
}