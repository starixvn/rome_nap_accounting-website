using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover
{
    public class MatchInfo
    {
        public string LeagueName { get; set; }

        public string LeagueId { get; set; }

        public string EventDate { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
