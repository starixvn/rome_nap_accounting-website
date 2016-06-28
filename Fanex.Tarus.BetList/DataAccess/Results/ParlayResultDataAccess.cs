// --------------------------------------------------------------------------
// <copyright file="ParlayResultDataAccess.cs" company="">
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
    using Fanex.Result.Core.Sportsbook.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    /// <summary>
    /// The class ParlayResultDataAccess.
    /// </summary>
    public class ParlayResultDataAccess : IParlayResultDataAccess
    {
        /// <summary>
        /// Gets the parlay result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of Parlays.</returns>
        public IEnumerable<Parlay> GetParlayResult(GetParlayResultCriteria criteria)
        {
            List<Parlay> results = null;

            using (var db = new ObjectDb(criteria.GetSettingKey(), 600))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    results = GetParlays(reader);
                    List<GameDetail> gameDetails = GetGameDetails(reader);
                    AddGameDetailsToResults(gameDetails, results);
                }
            }

            return results;
        }

        /// <summary>
        /// Adds the game details to results.
        /// </summary>
        /// <param name="gameDetails">The game details.</param>
        /// <param name="results">The results.</param>
        private static void AddGameDetailsToResults(List<GameDetail> gameDetails, List<Parlay> results)
        {
            if (gameDetails.Count > 0)
            {
                foreach (var result in results)
                {
                    foreach (var sb in result.Sportbook)
                    {
                        sb.GameDetails.AddRange(gameDetails.Where(i => i.MatchId == sb.MatchId));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the game details.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The list of game details.</returns>
        private static List<GameDetail> GetGameDetails(IDataReader reader)
        {
            var gameDetails = new List<GameDetail>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    gameDetails.Add(BindGameDetail(reader));
                }
            }

            return gameDetails;
        }

        /// <summary>
        /// Gets the parlays.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The list of Parlays.</returns>
        private static List<Parlay> GetParlays(IDataReader reader)
        {
            var results = new List<Parlay>();
            var leagueId = 0;
            var sportId = 0;
            var sportbooks = new List<Sportsbook>();

            while (reader.Read())
            {
                var newLeagueId = reader["LeagueID"] != DBNull.Value ? int.Parse(reader["LeagueID"].ToString()) : 0;
                var newsportId = reader["sporttype"] != DBNull.Value ? int.Parse(reader["sporttype"].ToString()) : 0;

                if (leagueId != newLeagueId && leagueId > 0)
                {
                    results.Add(new Parlay
                    {
                        SportId = sportId,
                        LeagueId = leagueId,
                        Sportbook = sportbooks,
                    });
                    sportbooks = new List<Sportsbook>();
                }

                sportbooks.Add(BindSportbook(reader));

                leagueId = newLeagueId;
                sportId = newsportId;
            }

            results.Add(new Parlay
            {
                SportId = sportId,
                LeagueId = leagueId,
                Sportbook = sportbooks
            });

            return results;
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
            };
        }

        /// <summary>
        /// Bind data from IDATAREADER to DTO.
        /// </summary>
        /// <param name="reader">The IDATAREADER.</param>
        /// <returns>The SPORTBOOK DTO.</returns>
        private static Sportsbook BindSportbook(System.Data.IDataReader reader)
        {
            var item = new Sportsbook
            {
                AwayId = reader["awayID"] != DBNull.Value ? int.Parse(reader["awayID"].ToString()) : 0,
                HomeId = reader["homeID"] != DBNull.Value ? int.Parse(reader["homeID"].ToString()) : 0,
                BetStatus = reader["betstatus"] != DBNull.Value ? reader["betstatus"].ToString() : string.Empty,
                LeagueId = reader["leagueid"] != DBNull.Value ? int.Parse(reader["leagueid"].ToString()) : 0,
                MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                FinalAwayScore = reader["finalawayscore"] != DBNull.Value ? int.Parse(reader["finalawayscore"].ToString()) : new int?(),
                HtAwayScore = reader["htawayscore"] != DBNull.Value ? int.Parse(reader["htawayscore"].ToString()) : new int?(),
                Color = ColumnExisted(reader, "lcolor") && reader["lcolor"] != DBNull.Value ? reader["lcolor"].ToString() : string.Empty,
                FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? int.Parse(reader["finalhomescore"].ToString()) : new int?(),
                HtHomeScore = reader["hthomescore"] != DBNull.Value ? int.Parse(reader["hthomescore"].ToString()) : new int?(),
                EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsNeutral = reader["IsNeutral"] != DBNull.Value ? bool.Parse(reader["IsNeutral"].ToString()) : false,
                IsPending = reader["IsPending"] != DBNull.Value ? bool.Parse(reader["IsPending"].ToString()) : false,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                MatchCode = reader["matchcode"] != DBNull.Value ? reader["matchcode"].ToString() : string.Empty,
                GoalRecord = reader["goalrecord"] != DBNull.Value ? reader["goalrecord"].ToString() : string.Empty,
                IsHTCal = reader["IsHTCal"] != DBNull.Value ? Convert.ToBoolean(reader["IsHTCal"]) : false,
                Calculate = reader["Calculate"] != DBNull.Value ? Convert.ToBoolean(reader["Calculate"]) : false,
                RefNo = string.Empty,
                GameDetails = new List<GameDetail>(),
                Bettype = GetBetTypeFromToken(reader),
                BetID = GetBetIDFromToken(reader)
            };

            return item;
        }

        /// <summary>
        /// Check the column existed in data reader.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>True or False.</returns>
        private static bool ColumnExisted(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static int GetBetTypeFromToken(IDataReader reader)
        {
            const string BettypeColumn = "bettype";
            if (ColumnExisted(reader, BettypeColumn) && reader[BettypeColumn] != DBNull.Value)
            {
                return int.Parse(reader[BettypeColumn].ToString());
            }

            return 0;
        }

        private static string GetBetIDFromToken(IDataReader reader)
        {
            const string BetcheckColumn = "betcheck";
            if (ColumnExisted(reader, BetcheckColumn) && reader[BetcheckColumn] != DBNull.Value)
            {
                return reader[BetcheckColumn].ToString();
            }

            return string.Empty;
        }
    }
}
