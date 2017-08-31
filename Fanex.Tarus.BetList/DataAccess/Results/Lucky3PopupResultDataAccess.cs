using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Result.Core.Lucky3.DTO;

namespace Fanex.Tarus.BetList.DataAccess.Results
{
    public interface ILucky3PopupResultDataAccess
    {
        IList<Lucky3> GetLucky3PopupResult(ICriteria criteria);
    }

    public class Lucky3PopupResultDataAccess : ILucky3PopupResultDataAccess
    {
        public IList<Lucky3> GetLucky3PopupResult(ICriteria criteria)
        {
            IList<Lucky3> result = new List<Lucky3>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        BindLucky3(result, reader);
                    }
                }
            }

            return result;
        }

        private static void BindLucky3(IList<Lucky3> result, IDataReader reader)
        {
            var matchId = reader["matchid"] != DBNull.Value ? Convert.ToInt32(reader["matchid"]) : 0;
            var existMatch = result.SingleOrDefault(i => i.MatchId == matchId);

            if (existMatch != null)
            {
                existMatch.Seq.Add(reader["seq"] != DBNull.Value ? Convert.ToInt32(reader["seq"]) : 0);
                existMatch.Ball.Add(reader["number"] != DBNull.Value ? Convert.ToInt32(reader["number"]) : 0);
            }
            else
            {
                result.Add(new Lucky3
                {
                    LeagueId = reader["leagueid"] != DBNull.Value ? Convert.ToInt32(reader["leagueid"]) : 0,
                    MatchId = reader["matchid"] != DBNull.Value ? Convert.ToInt32(reader["matchid"]) : 0,
                    EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                    EventDate = reader["eventdate"] != DBNull.Value ? Convert.ToDateTime(reader["eventdate"]) : DateTime.MinValue,
                    KickOffTime = reader["kickofftime"] != DBNull.Value ? Convert.ToDateTime(reader["kickofftime"]) : DateTime.MinValue,
                    MatchCode = reader["matchcode"] != DBNull.Value ? reader["matchcode"].ToString() : string.Empty,
                    FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? Convert.ToInt32(reader["finalhomescore"]) : 0,
                    LiveHomeScore = reader["livehomescore"] != DBNull.Value ? Convert.ToInt32(reader["livehomescore"]) : 0,
                    LiveAwayScore = reader["liveawayscore"] != DBNull.Value ? Convert.ToInt32(reader["liveawayscore"]) : 0,
                    Seq = new List<int> { reader["seq"] != DBNull.Value ? Convert.ToInt32(reader["seq"]) : 0 },
                    Ball = new List<int> { reader["number"] != DBNull.Value ? Convert.ToInt32(reader["number"]) : 0 }
                });
            }
        }
    }
}
