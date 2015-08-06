namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.BetList.Criteria.Results;

    public class MetricGamingResultDataAccess : IMetricGamingResultDataAccess
    {
        public List<Fanex.Result.Core.MetricGaming.DTO.MetricGaming> GetResult(GetMetricGamingResultCriteria criteria)
        {
            var result = new List<Fanex.Result.Core.MetricGaming.DTO.MetricGaming>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindResultData(reader, criteria));
                    }
                }
            }

            return result;
        }

        private static Fanex.Result.Core.MetricGaming.DTO.MetricGaming BindResultData(IDataReader reader, GetMetricGamingResultCriteria criteria)
        {
            return new Fanex.Result.Core.MetricGaming.DTO.MetricGaming
            {
                LeagueId = Converter.ToInt(reader["Leagueid"]),
                MatchId = Converter.ToInt(reader["MatchID"]),
                GameId = Converter.ToInt(reader["GameID"]),
                BetTypeTime = Converter.ToString2(reader["Custom1"]),
                SettlementStatusId = Converter.ToInt(reader["SettlementStatusID"]),
                GameResult = Converter.ToString2(reader["GameResult"]),
                OtherInfo = Converter.ToString2(reader["OtherInfo"]),

                HomeId = Converter.ToString2(reader["HomeId"]),
                AwayId = Converter.ToString2(reader["awayid"])
            };
        }
    }
}
