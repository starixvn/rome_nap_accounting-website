// --------------------------------------------------------------------------
// <copyright file="GoldDeluxeRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2015.03.13">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.GoldDeluxe
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.GoldDeluxe;
    using Fanex.Tarus.Customer.Resources;

    /// <summary>
    /// Gold Deluxe repository.
    /// </summary>
    public class GoldDeluxeRepository : IGoldDeluxeRepository
    {
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Gold Deluxe status object.</returns>
        public GoldDeluxeStatus GetStatus(ICriteria criteria)
        {
            GoldDeluxeStatus goldDeluxe = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var outputParams = db.GetOuputParams();
                    if (outputParams != null && outputParams.Count > 0)
                    {
                        goldDeluxe = new GoldDeluxeStatus
                        {
                            Enable = (bool)outputParams["supperstatus"],
                            DownlineEnable = (bool)outputParams["downlinestatus"]
                        };
                    }
                }
            }

            return goldDeluxe;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        public Result UpdateStatus(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                
                var outputParams = db.GetOuputParams();
                string message = outputParams["errmsg"].ToString();
                return string.IsNullOrEmpty(message) ? 
                    new Result(0, Customer_Labels.GoldDeluxeStatusUpdatedSuccessfully) : 
                    new Result(-1, message);
            }
        }
    }
}
