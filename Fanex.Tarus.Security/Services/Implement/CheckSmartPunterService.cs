// -----------------------------------------------------------------------
// <copyright file="CheckSmartPunterService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Security.Services.Implement
{
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Security.Criteria;
    using Fanex.Tarus.Security.Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckSmartPunterService : ICheckSmartPunterService
    {
        /// <summary>
        /// Check smart punter service.
        /// </summary>
        /// <param name="criteria">Check smart punter criteria.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CheckSmartPunter(CheckSmartPunterCriteria criteria)
        {
            bool isSmartPunter = false;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                Hashtable hash = db.GetOuputParams();
                isSmartPunter  = Converter.To<bool>(hash["issmartpunter"]);
            }

            return isSmartPunter;
        }
    }
}
