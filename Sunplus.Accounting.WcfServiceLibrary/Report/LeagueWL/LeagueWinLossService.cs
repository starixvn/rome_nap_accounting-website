using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities.Report.LeagueWL;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.LeagueWL
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LeagueWinLossService : ILeagueWinLossService
    {
        public LeagueWLBySportsDTO SelLeagueWLBySports(string winlostdate, bool isGetNew)
        {
            var leagueWL = CommonBootStrapper.Locator.GetInstance<Business.Report.LeagueWL.ILeagueWLService>();
            return leagueWL.SelLeagueWLBySports(winlostdate, isGetNew);
        }

        public LeagueWLBettypeDTO SelLeagueWLByBettypes(int sportId, int leagueId, string winlostDate)
        {
            var leagueWL = CommonBootStrapper.Locator.GetInstance<Business.Report.LeagueWL.ILeagueWLService>();
            return leagueWL.SelLeagueWLByBettypes(sportId, leagueId, winlostDate);
        }

        public LeagueWLBettypeDetailBO SelLeagueWLDetailByBettypes(int sportId, int leagueId, int bettypeId, bool isLive, bool isHedge, string winlostDate)
        {
            var leagueWL = CommonBootStrapper.Locator.GetInstance<Business.Report.LeagueWL.ILeagueWLService>();
            return leagueWL.SelLeagueWLDetailByBettypes(sportId, leagueId, bettypeId, isLive, isHedge, winlostDate);
        }
    }
}