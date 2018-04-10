// --------------------------------------------------------------------------
// <copyright file="NumberGamePopupResultDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    /// <summary>
    /// The class NumberGamePopupResultDataAccess.
    /// </summary>
    public class NumberGamePopupResultDataAccess : INumberGamePopupResultDataAccess
    {
        /// <summary>
        /// Gets the number game popup result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of number games.</returns>
        public IList<NumberGame> GetNumberGamePopupResult(GetNumberGamePopupResultCriteria criteria)
        {
            var result = new List<NumberGame>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        BindNumberGame(result, reader);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Bind data from IDATAREADER to DTO.
        /// </summary>
        /// <param name="result">The result list.</param>
        /// <param name="reader">The IDATAREADER.</param>
        private static void BindNumberGame(List<NumberGame> result, IDataReader reader)
        {
            var matchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0;
            var existMatch = result.SingleOrDefault(i => i.MatchId == matchId);
            if (existMatch != null)
            {
                existMatch.Seg.Add(reader["seq"] != DBNull.Value ? int.Parse(reader["seq"].ToString()) : 0);
                existMatch.Ball.Add(reader["number"] != DBNull.Value ? int.Parse(reader["number"].ToString()) : 0);
            }
            else
            {
                result.Add(new NumberGame
                {
                    LeagueId = reader["leagueid"] != DBNull.Value ? int.Parse(reader["leagueid"].ToString()) : 0,
                    MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                    EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                    EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                    KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                    MatchCode = reader["matchcode"] != DBNull.Value ? reader["matchcode"].ToString() : string.Empty,
                    Seg = new List<int> { reader["seq"] != DBNull.Value ? int.Parse(reader["seq"].ToString()) : 0 },
                    Ball = new List<int> { reader["number"] != DBNull.Value ? int.Parse(reader["number"].ToString()) : 0 }
                });
            }
        }
    }
}
