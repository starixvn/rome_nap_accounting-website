// --------------------------------------------------------------------------
// <copyright file="VirtualTennisResultDataAccess.cs" company="">
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
    using Fanex.Data;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    /// <summary>
    /// The class VirtualTennisResultDataAccess.
    /// </summary>
    public class VirtualTennisResultDataAccess : IVirtualTennisResultDataAccess
    {
        /// <summary>
        /// Get racing POPUP result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The racing POPUP data.</returns>
        public IList<Sportsbook> GetVirtualTennisPopupResult(GetVirtualTennisResultCriteria criteria)
        {
            var result = new List<Sportsbook>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindVirtualTennis(reader, criteria.MatchId));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Binds the virtual tennis.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>The SPORTSBOOK DTO.</returns>
        private static Sportsbook BindVirtualTennis(IDataReader reader, int matchId)
        {
            return new Sportsbook
            {
                HomeId = reader["homeID"] != DBNull.Value ? Convert.ToInt32(reader["homeID"]) : 0,
                AwayId = reader["awayID"] != DBNull.Value ? Convert.ToInt32(reader["awayID"]) : 0,
                FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? Convert.ToInt32(reader["finalhomescore"]) : 0,
                FinalAwayScore = reader["finalawayscore"] != DBNull.Value ? Convert.ToInt32(reader["finalawayscore"]) : 0,
                EventStatus = reader["eventstatus"] != DBNull.Value ? Convert.ToString(reader["eventstatus"]) : string.Empty,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? Convert.ToDateTime(reader["kickofftime"].ToString()) : DateTime.MinValue,
                EventDate = reader["kickofftime"] != DBNull.Value ? Convert.ToDateTime(reader["kickofftime"].ToString()) : DateTime.MinValue,
                LeagueId = reader["LeagueID"] != DBNull.Value ? Convert.ToInt32(reader["LeagueID"]) : 0,
                MatchId = matchId,
                Color = string.Empty,
                GoalRecord = string.Empty,
                IsHTCal = false,
                Calculate = false,
                RefNo = string.Empty
            };
        }
    }
}
