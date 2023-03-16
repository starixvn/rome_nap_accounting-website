// --------------------------------------------------------------------------
// <copyright file="ScoreInformationDataAccess.cs" company="">
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
    using Criteria.BetBuilder;
    using Fanex.Data;
    using Fanex.Result.Core.Sportsbook.DTO;
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
        public List<Sportsbook> GetScoreInformation(GetScoreInformationCriteria criteria, int handicap2)
        {
            var result = new List<Sportsbook>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        result.Add(BindSportsbook(reader, criteria));

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result[0].GameDetails.Add(BindGameDetail(reader));
                                result[0].Handicap2 = handicap2;
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

        public List<Sportsbook> GetScoreInformation(GetResultBetBuilderDetailCriteria criteria)
        {
            var result = new List<Sportsbook>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        result.Add(BindSportsbook(reader, criteria));

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result[0].BetBuilderDetails.Add(BindBetBuilderDetail(reader));
                            }
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
                SettlementStatusID = reader["SettlementStatusID"] != DBNull.Value ? int.Parse(reader["SettlementStatusID"].ToString()) : 0,
                OtherInfo = reader["OtherInfo"] != DBNull.Value ? reader["OtherInfo"].ToString() : string.Empty,
                BettypeId = reader["BetType"] != DBNull.Value ? int.Parse(reader["BetType"].ToString()) : 0,
            };
        }

        private static BetBuilderDetail BindBetBuilderDetail(IDataReader reader)
            => new BetBuilderDetail
            {
                SelectionType = int.Parse(reader["selectiontype"].ToString()),
                Result = reader["result"].ToString()
            };

        /// <summary>
        /// Binds the SPORTSBOOK.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The object of SPORTSBOOK.
        /// </returns>
        private static Sportsbook BindSportsbook(IDataReader reader, GetScoreInformationCriteria criteria)
        {
            return new Sportsbook()
            {
                HomeId = reader["homeID"] != DBNull.Value ? Convert.ToInt32(reader["homeID"]) : 0,
                AwayId = reader["awayID"] != DBNull.Value ? Convert.ToInt32(reader["awayID"]) : 0,
                HtHomeScore = reader["hthomescore"] != DBNull.Value ? Convert.ToInt32(reader["hthomescore"]) : new int?(),
                FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? Convert.ToInt32(reader["finalhomescore"]) : new int?(),
                HtAwayScore = reader["htawayscore"] != DBNull.Value ? Convert.ToInt32(reader["htawayscore"]) : new int?(),
                FinalAwayScore = reader["finalawayscore"] != DBNull.Value ? Convert.ToInt32(reader["finalawayscore"]) : new int?(),
                EventStatus = reader["eventstatus"] != DBNull.Value ? Convert.ToString(reader["eventstatus"]) : string.Empty,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? Convert.ToDateTime(reader["kickofftime"].ToString()) : DateTime.MinValue,
                EventDate = reader["eventdate"] != DBNull.Value ? Convert.ToDateTime(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsNeutral = reader["IsNeutral"] != DBNull.Value ? Convert.ToBoolean(reader["IsNeutral"]) : false,
                BetStatus = reader["betstatus"] != DBNull.Value ? Convert.ToString(reader["betstatus"]) : string.Empty,
                LeagueId = reader["LeagueID"] != DBNull.Value ? Convert.ToInt32(reader["LeagueID"]) : 0,
                MatchId = criteria.MatchId,
                Color = string.Empty,
                MatchCode = reader["matchcode"] != DBNull.Value ? Convert.ToString(reader["matchcode"]) : string.Empty,
                GoalRecord = reader["GoalRecord"] != DBNull.Value ? Convert.ToString(reader["GoalRecord"]) : string.Empty,
                LiveHomeScore = reader["livehomescore"] != DBNull.Value ? Convert.ToInt32(reader["livehomescore"]) : new int?(),
                LiveAwayScore = reader["liveawayscore"] != DBNull.Value ? Convert.ToInt32(reader["liveawayscore"]) : new int?(),
                IsHTCal = reader["IsHTCal"] != DBNull.Value && Convert.ToBoolean(reader["IsHTCal"]),
                Calculate = reader["Calculate"] != DBNull.Value && Convert.ToBoolean(reader["Calculate"]),
                RefNo = string.Empty,
                BetID = criteria.BetId.ToString(),
                Bettype = criteria.BetType,
                GameDetails = new List<GameDetail>(),
                SettlementStatus = reader["SettlementStatus"] != DBNull.Value ? Convert.ToByte(reader["SettlementStatus"]) : default(byte),
                NoHTScore = reader["NoHTScore"] != DBNull.Value ? Convert.ToBoolean(reader["NoHTScore"]) : false,
            };
        }

        private static Sportsbook BindSportsbook(IDataReader reader, GetResultBetBuilderDetailCriteria criteria)
        {
            return new Sportsbook()
            {
                HomeId = reader["homeID"] != DBNull.Value ? Convert.ToInt32(reader["homeID"]) : 0,
                AwayId = reader["awayID"] != DBNull.Value ? Convert.ToInt32(reader["awayID"]) : 0,
                HtHomeScore = reader["hthomescore"] != DBNull.Value ? Convert.ToInt32(reader["hthomescore"]) : new int?(),
                FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? Convert.ToInt32(reader["finalhomescore"]) : new int?(),
                HtAwayScore = reader["htawayscore"] != DBNull.Value ? Convert.ToInt32(reader["htawayscore"]) : new int?(),
                FinalAwayScore = reader["finalawayscore"] != DBNull.Value ? Convert.ToInt32(reader["finalawayscore"]) : new int?(),
                EventStatus = reader["eventstatus"] != DBNull.Value ? Convert.ToString(reader["eventstatus"]) : string.Empty,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? Convert.ToDateTime(reader["kickofftime"].ToString()) : DateTime.MinValue,
                EventDate = reader["eventdate"] != DBNull.Value ? Convert.ToDateTime(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsNeutral = reader["IsNeutral"] != DBNull.Value ? Convert.ToBoolean(reader["IsNeutral"]) : false,
                BetStatus = reader["betstatus"] != DBNull.Value ? Convert.ToString(reader["betstatus"]) : string.Empty,
                LeagueId = reader["LeagueID"] != DBNull.Value ? Convert.ToInt32(reader["LeagueID"]) : 0,
                MatchId = criteria.MatchId,
                Color = string.Empty,
                MatchCode = reader["matchcode"] != DBNull.Value ? Convert.ToString(reader["matchcode"]) : string.Empty,
                GoalRecord = reader["GoalRecord"] != DBNull.Value ? Convert.ToString(reader["GoalRecord"]) : string.Empty,
                LiveHomeScore = reader["livehomescore"] != DBNull.Value ? Convert.ToInt32(reader["livehomescore"]) : new int?(),
                LiveAwayScore = reader["liveawayscore"] != DBNull.Value ? Convert.ToInt32(reader["liveawayscore"]) : new int?(),
                IsHTCal = reader["IsHTCal"] != DBNull.Value && Convert.ToBoolean(reader["IsHTCal"]),
                Calculate = reader["Calculate"] != DBNull.Value && Convert.ToBoolean(reader["Calculate"]),
                RefNo = string.Empty,
                BetID = criteria.MarketId.ToString(),
                Bettype = criteria.BetType,
                GameDetails = new List<GameDetail>(),
                SettlementStatus = reader["SettlementStatus"] != DBNull.Value ? Convert.ToByte(reader["SettlementStatus"]) : default(byte)
            };
        }
    }
}
