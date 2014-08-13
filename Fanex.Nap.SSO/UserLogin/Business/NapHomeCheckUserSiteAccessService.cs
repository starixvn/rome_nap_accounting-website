// --------------------------------------------------------------------------
// <copyright file="NapHomeCheckUserSiteAccessService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ethan.Nguyen" date="2014.08.14">Created</change>
// </history>

namespace Fanex.Nap.SSO.UserLogin.Business
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Nap.SSO.UserLogin.DataAccess;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class NapHomeCheckUserSiteAccessService : INapHomeCheckUserSiteAccessService
    {
        private readonly INapHomeCheckUserSiteAccessDataAccess _reportRepository;

        public NapHomeCheckUserSiteAccessService ()
            : this(null)
        {
        }

        public NapHomeCheckUserSiteAccessService (INapHomeCheckUserSiteAccessDataAccess reportRepository)
        {
            this._reportRepository = reportRepository ?? new NapHomeCheckUserSiteAccessDataAccess();
        }

        /// <summary>
        /// Check User Site Access
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="environment"></param>
        /// <returns>Return true if having permission</returns>
        public bool NapHomeCheckUserSiteAccess(int userId, string environment)
        {
            return _reportRepository.NapHomeCheckUserSiteAccess(new Criteria.NapHomeCheckUserSiteAccessCriteria(){
                    UserId = userId,
                    Environment = environment
                });
        }
    }
}
