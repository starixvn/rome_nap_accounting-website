using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
using Fanex.Tarus.Customer.Entities;

namespace Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover
{
    public interface ISMATurnoverService
    {
        List<Sport> GetSportType(int mode);
        List<League> GetLeague(int sportType, DateTime fromDate, DateTime toDate);
        MatchInfo GetInfoByMatchId(int matchId);
        List<CustomerTurnoverDetail> GetSMATurnoverList(int sportId, int leagueId, DateTime fromDate, DateTime toDate);
        Result AddSMATurnover(int matchId, string custName, decimal maxTurnover);
        Result UpdateSMATurnover(int matchId, int custId, decimal maxTurnover);
        Result CheckMaxTurnoverValue(int matchId, string custName, decimal maxTurnover, int mode);
    }
}
