namespace Fanex.Tarus.BetList.DataAccess
{
    using System.Collections.Generic;
    using Fanex.BetList.Accounting.Models;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.BetList.Criteria;

    public class MicroGamingRepository : IMicroGamingRepository
    {
        public IEnumerable<TicketDetail> BetDetails(GetMicroGamingBetDetailsCriteria criteria)
        {
            var result = new List<TicketDetail>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new TicketDetail
                        {
                            Ticket = new Ticket
                           {
                               TransId = Converter.ToInt(reader["transid"]),
                               SportTypeId = Converter.ToInt(reader["SportType"]),
                               BetTypeId = Converter.ToInt(reader["BetType"]),
                               TransDate = Converter.ToDateTime(reader["transdate"]),
                               WinlostDate = Converter.ToDateTime(reader["winlostdate"]),

                               Status = Converter.ToString2(reader["status"]),
                               StatusId = Converter.ToInt(reader["statusid"]),

                               Odds = Converter.ToDecimal(reader["odds"]),
                               Stake = Converter.ToDecimal(reader["stake"]),
                               ActualStake = Converter.ToDecimal(reader["actual_stake"]),
                               Winlost = Converter.ToDecimal(reader["WinLost"]),
                               Comm = Converter.ToDecimal(reader["comm"]),
                               PlayerComm = Converter.ToDecimal(reader["playercomm"]),
                               IP = Converter.ToString2(reader["ip"]),
                               TransDesc = Converter.ToString2(reader["transdesc"]),
                               BetCheck = Converter.ToString2(reader["betcheck"])
                           },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty
                        });
                    }
                }
            }

            return result;
        }
    }
}
