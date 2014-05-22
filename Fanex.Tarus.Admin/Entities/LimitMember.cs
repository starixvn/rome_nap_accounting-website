// --------------------------------------------------------------------------
// <copyright file="LimitMember.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Entities
{
    /// <summary>
    /// LimitMember class.
    /// </summary>
    public class LimitMember
    {
        public string Super { get; set; }

        public int MaxPerDay { get; set; }

        public Result OutputResult { get; set; }
    }
}
