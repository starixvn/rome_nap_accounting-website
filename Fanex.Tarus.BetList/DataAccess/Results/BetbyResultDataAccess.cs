
namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
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
            var detail = new List<GameDetail>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindSportbook(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            detail.Add(BindGameDetail(reader));
                        }
                    }
                }
            }

            foreach (var item in result)
            {
                var gameDetail = detail.Where(i => i.MatchId == item.MatchId);
                if (gameDetail != null)
                {
                    item.GameDetails = gameDetail.ToList();
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
                HomeId = reader["HomeId"] != DBNull.Value ? int.Parse(reader["HomeId"].ToString()) : 0,
                AwayId = reader["AwayId"] != DBNull.Value ? int.Parse(reader["AwayId"].ToString()) : 0
            };
        }

        private static GameDetail BindGameDetail(IDataReader reader)
        {
            return new GameDetail
            {
                MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                HomeValue = reader["Custom1"] != DBNull.Value ? reader["Custom1"].ToString() : string.Empty,
                AwayValue = reader["Custom2"] != DBNull.Value ? reader["Custom2"].ToString() : string.Empty,
                SettlementStatusID = reader["SettlementStatusID"] != DBNull.Value ? int.Parse(reader["SettlementStatusID"].ToString()) : 0,
                ResourceId = reader["ResourceId"] != DBNull.Value ? reader["ResourceId"].ToString() : string.Empty
            };
        }
    }
}
