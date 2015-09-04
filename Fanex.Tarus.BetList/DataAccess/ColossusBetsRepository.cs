namespace Fanex.Tarus.BetList.DataAccess
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.BetList.Criteria;

    public class ColossusBetsRepository : IColossusBetsRepository
    {
        public IEnumerable<ITicket> BetDetails(GetColossusBetsBetDetailsCriteria criteria)
        {
            var result = new List<ITicket>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new Ticket
                        {
                            TransId = Converter.ToInt(reader["transid"]),
                            TransDate = Converter.ToDateTime(reader["transdate"]),
                            BetTypeId = Converter.ToInt(reader["bettype"]),
                            StatusId = Converter.ToInt(reader["statusid"]),
                            Status = Converter.ToString2(reader["status"]),

                            Handicap1 = Converter.ToDecimal(reader["Sold"]),
                            Winlost = Converter.ToDecimal(reader["WinLost"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
