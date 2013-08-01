
namespace Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover
{
    using System.Collections.Generic;

    public class MatchInfo : Result
    {
        public string Id { get; set; }

        public string LeagueName { get; set; }

        public string LeagueId { get; set; }

        public string EventDate { get; set; }

        public int HomeTeamId { get; set; }

        public string HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        public string AwayTeam { get; set; }

        public List<MatchInfo> ChildsInfo;
    }
}
