// --------------------------------------------------------------------------
// <copyright file="LoginRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.SSO.Repositories
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.SSO.Criteria;

    /// <summary>
    /// Login Repository.
    /// </summary>
    public class LoginRepository
    {
        /// <summary>
        /// Checks the user site access.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Check result.</returns>
        public bool CheckUserSiteAccess(CheckUserSiteAccessCriteria criteria)
        {
            bool isAccess;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                isAccess = Converter.ToBool(outputParams["isaccess"]);
            }

            return isAccess;
        }
    }
}
