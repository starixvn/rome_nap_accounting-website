// --------------------------------------------------------------------------
// <copyright file="ScoreInformationDataAccess.cs" company="Nexcel Solutions Vietnam">
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
    /// The class ScoreInformationDataAccess.
    /// </summary>
    public class ScoreInformationDataAccess : IScoreInformationDataAccess
    {
        /// <summary>
        /// Gets the score information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of SPORTSBOOKS.</returns>
        public List<Sportsbook> GetScoreInformation(ref GetScoreInformationCriteria criteria)
        {
            var result = new List<Sportsbook>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        result.Add(BindSportsbook(reader, ref criteria));

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result[0].GameDetails.Add(BindGameDetail(reader));
                            }
                        }

                        if (result[0].GameDetails.Count == 0)
                        {
                            result[0].GameDetails.Add(new GameDetail());
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Bind game detail value.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>The game detail value.</returns>
        private static GameDetail BindGameDetail(IDataReader reader)
        {
            return new GameDetail
            {
                MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                ResourceId = reader["ResourceId"] != DBNull.Value ? reader["ResourceId"].ToString() : string.Empty,
                HomeValue = reader["HomeValue"] != DBNull.Value ? reader["HomeValue"].ToString() : string.Empty,
                AwayValue = reader["AwayValue"] != DBNull.Value ? reader["AwayValue"].ToString() : string.Empty,
                ResultStatusID = reader["ResultStatusID"] != DBNull.Value ? int.Parse(reader["ResultStatusID"].ToString()) : 0,
                SettlementStatusID = reader["SettlementStatusID"] != DBNull.Value ? int.Parse(reader["SettlementStatusID"].ToString()) : 0
            };
        }

        /// <summary>
        /// Binds the SPORTSBOOK.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The object of SPORTSBOOK.
        /// </returns>
        private static Sportsbook BindSportsbook(IDataReader reader, ref GetScoreInformationCriteria criteria)
        {
            criteria.SportType = reader["sporttype"] != DBNull.Value ? Convert.ToInt32(reader["sporttype"]) : 0;

            return new Sportsbook()
            {
                HomeId = reader["homeID"] != DBNull.Value ? Convert.ToInt32(reader["homeID"]) : 0,
                AwayId = reader["awayID"] != DBNull.Value ? Convert.ToInt32(reader["awayID"]) : 0,
                HtHomeScore = reader["hthomescore"] != DBNull.Value ? Convert.ToInt32(reader["hthomescore"]) : 0,
                FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? Convert.ToInt32(reader["finalhomescore"]) : 0,
                HtAwayScore = reader["htawayscore"] != DBNull.Value ? Convert.ToInt32(reader["htawayscore"]) : 0,
                FinalAwayScore = reader["finalawayscore"] != DBNull.Value ? Convert.ToInt32(reader["finalawayscore"]) : 0,
                EventStatus = reader["eventstatus"] != DBNull.Value ? Convert.ToString(reader["eventstatus"]) : string.Empty,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? Convert.ToDateTime(reader["kickofftime"].ToString()) : DateTime.MinValue,
                EventDate = reader["eventdate"] != DBNull.Value ? Convert.ToDateTime(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsNeutral = reader["IsNeutral"] != DBNull.Value ? Convert.ToBoolean(reader["IsNeutral"]) : false,
                BetStatus = reader["betstatus"] != DBNull.Value ? Convert.ToString(reader["betstatus"]) : string.Empty,
                IsPending = reader["IsPending"] != DBNull.Value ? Convert.ToBoolean(reader["IsPending"]) : false,
                LeagueId = reader["LeagueID"] != DBNull.Value ? Convert.ToInt32(reader["LeagueID"]) : 0,
                MatchId = criteria.MatchId,
                Color = string.Empty,
                MatchCode = reader["matchcode"] != DBNull.Value ? Convert.ToString(reader["matchcode"]) : string.Empty,
                GoalRecord = string.Empty,
                IsHTCal = false,
                Calculate = false,
                RefNo = string.Empty,
                BetID = criteria.BetId.ToString(),
                Bettype = criteria.BetType,
                GameDetails = new List<GameDetail>()
            };
        }
    }
}
