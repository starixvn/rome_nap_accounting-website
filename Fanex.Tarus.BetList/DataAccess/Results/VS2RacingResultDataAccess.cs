namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Result.Core.VirtualRacing.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.Infrastructure.Helper;

    public class VS2RacingResultDataAccess : IVS2RacingResultDataAccess
    {
        public IList<VirtualRacing> GetRacingPopupResult(GetVS2RacingResultCriteria criteria)
        {
            List<VirtualRacing> results = null;

            using (var db = new ObjectDb(criteria.GetSettingKey(), 600))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    results = GetVirtualRacing(reader);
                    List<VirtualRacingDetail> raceDetails = GetRaceDetails(reader);
                    AddRaceDetailsToResults(raceDetails, results);
                }
            }

            return results;
        }

        private static List<VirtualRacing> GetVirtualRacing(IDataReader reader)
        {
            var result = new List<VirtualRacing>();
            while (reader.Read())
            {
                result.Add(BindVirtualRacing(reader));
            }

            return result;
        }

        private static void AddRaceDetailsToResults(List<VirtualRacingDetail> gameDetails, List<VirtualRacing> results)
        {
            if (gameDetails.Count > 0)
            {
                foreach (var result in results)
                {
                    result.Details.AddRange(gameDetails.Where(i => i.EventId == result.EventId));
                }
            }
        }

        private static List<VirtualRacingDetail> GetRaceDetails(IDataReader reader)
        {
            var gameDetails = new List<VirtualRacingDetail>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    gameDetails.Add(BindVirtualRacingDetail(reader));
                }
            }

            return gameDetails;
        }

        private static VirtualRacing BindVirtualRacing(IDataReader reader)
        {
            return new VirtualRacing
            {
                EventId = reader["eventid"] != DBNull.Value ? DataUtils.ToInt32(reader["eventid"].ToString()) : 0,
                LeagueId = reader["leagueid"] != DBNull.Value ? DataUtils.ToInt32(reader["leagueid"].ToString()) : 0,
                VendorMatchId = reader["vendormatchid"].ToString(),
                Distance = reader["distance"].ToString(),
                EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                BetStatus = reader["betstatus"] != DBNull.Value ? reader["betstatus"].ToString() : string.Empty,
                EventTime = reader["eventtime"] != DBNull.Value ? DataUtils.ToDateTime(reader["eventtime"].ToString()) : DateTime.MinValue,
            };
        }


        private static VirtualRacingDetail BindVirtualRacingDetail(System.Data.IDataReader reader)
        {
            var item = new VirtualRacingDetail
            {
                EventId = reader["eventid"] != DBNull.Value ? int.Parse(reader["eventid"].ToString()) : 0,
                Placing = reader["placing"] != DBNull.Value ? DataUtils.ToInt32(reader["placing"].ToString()) : 0,
                RacerId = reader["racerid"] != DBNull.Value ? DataUtils.ToInt32(reader["racerid"].ToString()) : 0,
                Orid = reader["orid"] != DBNull.Value ? DataUtils.ToInt32(reader["orid"].ToString()) : 0,
                Lane = reader["lane"] != DBNull.Value ? reader["lane"].ToString() : string.Empty,
            };

            return item;
        }
    }
}
