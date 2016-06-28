// --------------------------------------------------------------------------
// <copyright file="LimitNewMemberService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Business.LimitNewMember
{
    using System;
    using Fanex.Tarus.Admin.Criteria.LimitNewMember;
    using Fanex.Tarus.Admin.DataAccess.LimitNewMember;
    using Fanex.Tarus.Admin.Entities;
    using Sunplus.Accounting.Core.Extensions;

    /// <summary>
    /// LimitNewMemberService class.
    /// </summary>
    public class LimitNewMemberService : ILimitNewMemberService
    {
        private readonly ILimitNewMemberAccess _limitNewMemberAccess;

        public LimitNewMemberService()
            : this(null)
        { 
        }

        public LimitNewMemberService(ILimitNewMemberAccess limitNewMemberAccess)
        {
            _limitNewMemberAccess = limitNewMemberAccess ?? new LimitNewMemberAccess();
        }

        /// <summary>
        /// Gets the limit new member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The limit member object.</returns>
        public LimitMember GetLimitNewMember(LimitNewMemberCriteria criteria)
        {
            LimitMember limitSuper;
            try
            {
                limitSuper = _limitNewMemberAccess.GetLimitNewMember(criteria);
            }
            catch (Exception ex)
            {
                limitSuper = null;
                LogExtensions.ExceptionHandler(ex);
            }

            return limitSuper;
        }

        /// <summary>
        /// Updates the limit new member.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        public Result UpdateLimitNewMember(LimitNewMemberCriteria criteria)
        {
            Result result = null;
            try
            {
                result = _limitNewMemberAccess.UpdateLimitNewMember(criteria);
            }
            catch (Exception ex)
            {
                result = new Result { ErrorCode = -1, ErrorMessage = ex.Message };
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
