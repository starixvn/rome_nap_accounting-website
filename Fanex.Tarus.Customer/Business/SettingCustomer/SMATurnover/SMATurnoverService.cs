using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
using Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover;
using System.Data;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.Core.Cache;

namespace Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover
{
    public class SMATurnoverService: ISMATurnoverService
    {
        public ISMATurnoverAccess _SMATurnoverAccess;

        public SMATurnoverService()
            : this(null)
        { }

        public SMATurnoverService(ISMATurnoverAccess SMATurnoverAccess)
        {
            _SMATurnoverAccess = SMATurnoverAccess ?? new SMATurnoverAccess();
        }

        public List<BaseModel> GetSportType(int mode)
        {
            var sportList = new List<BaseModel>();
            DataTable dtb = _SMATurnoverAccess.GetSportType(mode);
            foreach (DataRow row in dtb.Rows)
            {
                var sport = new BaseModel()
                {
                    Id = CastAide.AsInt32(row["sporttype"]),
                    Name = CastAide.AsString(row["sportname"])
                };
                sportList.Add(sport);
            }
            return sportList;
        }

        public List<BaseModel> GetLeague(int sportType, DateTime fromDate, DateTime toDate)
        {
            var leagueList = new List<BaseModel>();
            var dtb = _SMATurnoverAccess.GetLeague(sportType, fromDate, toDate);
            foreach (DataRow row in dtb.Rows)
            {
                var league = new BaseModel() { 
                    Id = CastAide.AsInt32(row["leagueid"]),
                    Name = CastAide.AsString(row["leaguename"])
                };
                leagueList.Add(league);
            }
            return leagueList;
        }


        public MatchInfo GetInfoByMatchId(int matchId)
        {
            var matchInfo = _SMATurnoverAccess.GetInfoByMatchId(matchId);
            return matchInfo;
        }

        public List<CustomerTurnoverDetail> GetSMATurnoverList(int sportId, int leagueId, DateTime fromDate, DateTime toDate)
        {
            var leagueList = new List<CustomerTurnoverDetail>();
            var dtb = _SMATurnoverAccess.GetSMATurnoverList(sportId, leagueId, fromDate, toDate);
            var query = from league in dtb.AsEnumerable()
                        group league by league.Field<string>("leaguename") into dtbLeague
                        from match in
                            (from league in dtbLeague.AsEnumerable()
                             group league by league.Field<int>("matchid"))
                        group match by dtbLeague.Key;

            foreach (var leagueGroup in query)
            {
                var League = new CustomerTurnoverDetail();
                League.LeagueName = leagueGroup.Key;
                var matchList = new List<CustomerTurnoverDetail>();
                foreach (var matchGroup in leagueGroup)
                {
                    var Match = new CustomerTurnoverDetail();
                    Match.MatchId = matchGroup.Key.ToString();
                    var customerList = new List<CustomerTurnoverDetail>();
                    var i  = 0;
                    foreach (var detail in matchGroup)
                    {
                        if(i > 0)
                        {
                            Match.EventDate = string.Format("{0:M/d/yyyy}", detail["eventdate"]);
                            Match.HomeTeam = CacheRef.GetTeamName(detail["homeid"]);
                            Match.AwayTeam = CacheRef.GetTeamName(detail["awayid"]);
                            Match.IsSettled = CastAide.AsInt32(detail["ruben"]);
                        }
                        var Customer = new CustomerTurnoverDetail() { 
                            Username = detail["username"].ToString(),
                            Currency = detail["currency"].ToString(),
                            SMATurnover = NumberFormat.DecFormat(CastAide.AsDecimal(detail["Totalurnover"]), 2),
                            MaxTurnover = NumberFormat.DecFormat(CastAide.AsDecimal(detail["MaxTurnover"]), 2)
                        };
                        i++;
                        customerList.Add(Customer);
                    }
                    Match.CustomerList = customerList;
                    matchList.Add(Match);
                }
                League.CustomerList = matchList;
                leagueList.Add(League);
            }

            return leagueList;
        }
    }
}
