// --------------------------------------------------------------------------
// <copyright file="CheckUserSiteAccessCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.SSO.Criteria
{
    using System.Configuration;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Infrastructure.Common;

    /// <summary>
    /// Check User Site Access Criteria.
    /// </summary>
    public class CheckUserSiteAccessCriteria : CriteriaBase
    {
        public int UserId
        {
            get
            {
                return AppSession.AccId;
            }
        }

        public string Environment
        {
            get
            {
                return ConfigurationManager.AppSettings["Environment"].ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "NAP_Home_CheckUserSiteAccess";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
