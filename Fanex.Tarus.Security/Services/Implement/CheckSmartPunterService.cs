// -----------------------------------------------------------------------
// <copyright file="CheckSmartPunterService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Security.Services.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Fanex.Tarus.Security.Criteria;
    using Fanex.Tarus.Security.Services.Interface;
    using Fanex.Data;
    using System.Collections;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckSmartPunterService : ICheckSmartPunterService
    {
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
