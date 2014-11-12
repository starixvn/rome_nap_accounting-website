// --------------------------------------------------------------------------
// <copyright file="VirtualRacingResultDataAccess.cs" company="Nexcel Solutions Vietnam">
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
    /// The class VirtualRacingResultDataAccess.
    /// </summary>
    public class VirtualRacingResultDataAccess : IVirtualRacingResultDataAccess
    {
        /// <summary>
        /// Get racing POPUP result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The racing POPUP data.</returns>
        public IList<Racing> GetRacingPopupResult(GetVirtualRacingResultCriteria criteria)
        {
            var result = new List<Racing>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindRacing(reader));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Bind data from IDATAREADER to DTO.
        /// </summary>
        /// <param name="reader">The IDATAREADER.</param>
        /// <returns>The Racing DTO.</returns>
        private static Racing BindRacing(IDataReader reader)
        {
            return new Racing
            {
                LeagueId = reader["meetingid"] != DBNull.Value ? int.Parse(reader["meetingid"].ToString()) : 0,
                EngineId = reader["HorseId"] != DBNull.Value ? reader["HorseId"].ToString() : string.Empty,
                EngineNo = reader["HorseNo"] != DBNull.Value ? reader["HorseNo"].ToString() : string.Empty,
                IsFavor = reader["IsFavor"] != DBNull.Value && reader["IsFavor"].ToString().Equals("1") ? true : false,
                IsSCR = reader["IsSCR"] != DBNull.Value && reader["IsSCR"].ToString().Equals("1") ? true : false,
                Place = reader["place"] != DBNull.Value ? int.Parse(reader["place"].ToString()) : 0,
                PlaceOdds = reader["placeodds"] != DBNull.Value ? decimal.Parse(reader["placeodds"].ToString()) : 0,
                WinOdds = reader["odds"] != DBNull.Value ? decimal.Parse(reader["odds"].ToString()) : 0,
                RaceName = reader["racename"] != DBNull.Value ? int.Parse(reader["racename"].ToString()) : 0,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                Favourite = reader["IsFavor"] != DBNull.Value ? reader["IsFavor"].ToString() : string.Empty,
                RowCount = reader["RowCount"] != DBNull.Value ? int.Parse(reader["RowCount"].ToString()) : 0,
                SCROfMatch = reader["SCR"] != DBNull.Value ? reader["SCR"].ToString() : string.Empty,
                TFOfMatch = reader["TF"] != DBNull.Value ? reader["TF"].ToString() : string.Empty
            };
        }
    }
}
