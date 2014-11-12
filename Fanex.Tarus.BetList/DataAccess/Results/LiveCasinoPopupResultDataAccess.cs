// --------------------------------------------------------------------------
// <copyright file="LiveCasinoPopupResultDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    using Fanex.Result.Core.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    /// <summary>
    /// The class LiveCasinoPopupResultDataAccess.
    /// </summary>
    public class LiveCasinoPopupResultDataAccess : ILiveCasinoPopupResultDataAccess
    {
        /// <summary>
        /// Get live casino POPUP result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The live casino POPUP data.</returns>
        public IList<LiveCasino> GetLiveCasinoPopupResult(GetLiveCasinoPopupResultCriteria criteria)
        {
            var result = new List<LiveCasino>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindLiveCasino(reader, criteria.BetType));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Bind data from IDATAREADER to DTO.
        /// </summary>
        /// <param name="reader">The IDATAREADER.</param>
        /// <param name="betType">Type of the bet.</param>
        /// <returns>
        /// The live casino DTO.
        /// </returns>
        private static LiveCasino BindLiveCasino(IDataReader reader, int betType)
        {
            return new LiveCasino
            {
                ShowTime = reader["showtime"] != DBNull.Value ? reader["showtime"].ToString() : string.Empty,
                LeagueId = reader["leagueid"] != DBNull.Value ? int.Parse(reader["leagueid"].ToString()) : 0,
                EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                FinalAwayScore = reader["finalawayscore"] != DBNull.Value ? int.Parse(reader["finalawayscore"].ToString()) : new int?(),
                FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? int.Parse(reader["finalhomescore"].ToString()) : new int?(),
                EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsPending = reader["IsPending"] != DBNull.Value ? bool.Parse(reader["IsPending"].ToString()) : false,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                IsHTCal = reader["IsHTCal"] != DBNull.Value ? Convert.ToBoolean(reader["IsHTCal"]) : false,
                Calculate = reader["Calculate"] != DBNull.Value ? Convert.ToBoolean(reader["Calculate"]) : false,
                BetStatus = reader["betstatus"] != DBNull.Value ? reader["betstatus"].ToString() : string.Empty,
                Bettype = betType,
                MatchCode = reader["matchcode"] != DBNull.Value ? reader["matchcode"].ToString() : string.Empty
            };
        }
    }
}
