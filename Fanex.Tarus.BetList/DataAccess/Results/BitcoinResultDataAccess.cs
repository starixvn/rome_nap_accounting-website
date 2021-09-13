
namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Result.Core.Bitcoin.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    public interface IBitcoinResultDataAccess
    {
        List<Bitcoin> GetResult(GetBitcoinResultCriteria criteria);
    }

    public class BitcoinResultDataAccess : IBitcoinResultDataAccess
    {
        public List<Bitcoin> GetResult(GetBitcoinResultCriteria criteria)
        {
            var result = new List<Bitcoin>();
            var detail = new List<GameDetail>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindBitcoin(reader));
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

            foreach(var item in result)
            {
                var gameDetail = detail.Where(i => i.Match3rdId == item.Match3rdId);
                if(gameDetail!=null)
                {
                    item.GameResults = gameDetail.ToList();
                }
            }

            return result;
        }

        private static Bitcoin BindBitcoin(IDataReader reader)
        {
            return new Bitcoin
            {
                League3rdId = reader["league3rdid"] != DBNull.Value ? int.Parse(reader["league3rdid"].ToString()) : 0,
                Match3rdId = reader["match3rdid"] != DBNull.Value ? int.Parse(reader["match3rdid"].ToString()) : 0,
                EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                BetStatus = reader["betstatus"] != DBNull.Value ? reader["betstatus"].ToString() : string.Empty
            };
        }

        private static GameDetail BindGameDetail(IDataReader reader)
        {
            return new GameDetail
            {
                Match3rdId = reader["match3rdid"] != DBNull.Value ? int.Parse(reader["match3rdid"].ToString()) : 0,
                GameResult = reader["GameResult"] != DBNull.Value ? reader["GameResult"].ToString() : string.Empty
            };
        }
    }
}
