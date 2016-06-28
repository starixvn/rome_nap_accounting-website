// --------------------------------------------------------------------------
// <copyright file="GameType.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.AGCasino
{
    using System.Collections.Generic;

    /// <summary>
    /// Game Type.
    /// </summary>
    public class GameType
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public List<SettingLimit> SettingLimitByGroup { get; set; }
    }
}
