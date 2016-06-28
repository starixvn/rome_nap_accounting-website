// --------------------------------------------------------------------------
// <copyright file="ILimitNewMemberAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.Tarus.Admin.DataAccess.LimitNewMember
{
    using Fanex.Tarus.Admin.Criteria.LimitNewMember;
    using Fanex.Tarus.Admin.Entities;

    /// <summary>
    /// ILimitNewMemberAccess interface.
    /// </summary>
    public interface ILimitNewMemberAccess
    {
        LimitMember GetLimitNewMember(LimitNewMemberCriteria criteria);

        Result UpdateLimitNewMember(LimitNewMemberCriteria criteria);
    }
}
