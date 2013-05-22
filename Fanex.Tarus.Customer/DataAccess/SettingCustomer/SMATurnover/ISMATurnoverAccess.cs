using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
using Fanex.Tarus.Customer.Entities;

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover
{
    public interface ISMATurnoverAccess
    {
        DataTable GetSportType(int mode);
        DataTable GetLeague(int sport, DateTime fromDate, DateTime toDate);
        MatchInfo GetInfoByMatchId(int matchId);
        DataTable GetSMATurnoverList(int sportId, int leagueId, DateTime fromDate, DateTime toDate);
        Result AddSMATurnover(int matchId, string custName, double maxTurnover, int userId, string ip);
        Result UpdateSMATurnover(int matchId, int custId, double maxTurnover, int userId, string ip);
        Result CheckMaxTurnoverValue(int matchId, string custName, double maxTurnover, int mode);
    }
}
