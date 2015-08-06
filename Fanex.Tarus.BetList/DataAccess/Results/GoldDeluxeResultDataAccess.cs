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
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Result.Core.GoldDeluxe.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    /// <summary>
    /// The class ScoreInformationDataAccess.
    /// </summary>
    public class GoldDeluxeResultDataAccess : IGoldDeluxeResultDataAccess
    {
        /// <summary>
        /// Gets the score information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of SPORTSBOOKS.</returns>
        public List<GoldDeluxe> GetGoldDeluxePopUpResult(GetGoldDeluxePopUpResultCriteria criteria)
        {
            var result = new List<GoldDeluxe>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindGoldDeluxeData(reader, criteria.Bettype));
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
        private static GoldDeluxe BindGoldDeluxeData(IDataReader reader, int bettype)
        {
            return new GoldDeluxe
            {
                Bettype = bettype,
                Result = reader["TransDesc"].ToString()
            };
        }
    }
}
