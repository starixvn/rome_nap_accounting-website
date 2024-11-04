
namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    public interface IBetbyResultDataAccess
    {
        List<Sportsbook> GetResult(GetBitcoinResultCriteria criteria);
    }

    public class BetbyResultDataAccess : IBetbyResultDataAccess
    {
        public List<Sportsbook> GetResult(GetBitcoinResultCriteria criteria)
        {
            var result = new List<Sportsbook>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindSportbook(reader));
                    }
                }
            }

            return result;
        }

        private static Sportsbook BindSportbook(IDataReader reader)
        {
            return new Sportsbook
            {
                LeagueId = reader["league3rdid"] != DBNull.Value ? int.Parse(reader["league3rdid"].ToString()) : 0,
                MatchId = reader["match3rdid"] != DBNull.Value ? int.Parse(reader["match3rdid"].ToString()) : 0,
                EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                BetStatus = reader["betstatus"] != DBNull.Value ? reader["betstatus"].ToString() : string.Empty,
                MatchCode = reader["MatchCode"] != DBNull.Value ? reader["MatchCode"].ToString() : string.Empty,
                HtHomeScore = reader["HtHomeScore"] != DBNull.Value ? int.Parse(reader["HtHomeScore"].ToString()) : (int?)null,
                HtAwayScore = reader["HtAwayScore"] != DBNull.Value ? int.Parse(reader["HtAwayScore"].ToString()) : (int?)null,
                FinalHomeScore = reader["FinalHomeScore"] != DBNull.Value ? int.Parse(reader["FinalHomeScore"].ToString()) : (int?)null,
                FinalAwayScore = reader["FinalAwayScore"] != DBNull.Value ? int.Parse(reader["FinalAwayScore"].ToString()) : (int?)null,
                HomeId = reader["HomeId"] != DBNull.Value ? int.Parse(reader["HomeId"].ToString()) : 0,
                AwayId = reader["AwayId"] != DBNull.Value ? int.Parse(reader["AwayId"].ToString()) : 0
            };
        }
    }
}
