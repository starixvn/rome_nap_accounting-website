using Sunplus.Accounting.Entities.Report.LeagueWL;

namespace Sunplus.Accounting.Business.Report.LeagueWL
{
    public interface ILeagueWLService
    {
        LeagueWLBySportsDTO SelLeagueWLBySports(string winlostdate, bool isGetNew);

        LeagueWLBettypeDTO SelLeagueWLByBettypes(int sportId, int leagueId, string winlostDate);

        LeagueWLBettypeDetailBO SelLeagueWLDetailByBettypes(int sportId, int leagueId, int bettypeId, bool isLive,
                                                       bool isHedge, string winlostDate);

        void ExportExcelLeagueWLBySports(string winlostDate, string reportTitle);

        void ExportExcelLeagueWLByBettypes(string winlostDate, int sportId, int leagueId, string reportTitle);

        void ExportExcelLeagueWLDetailByBettype(int sportId, int leagueId, int bettypeId, bool isLive,
                                                       bool isHedge, string winlostDate, string reportTitle);
    }
}