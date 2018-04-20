// --------------------------------------------------------------------------
// <copyright file="GameTypeGroup.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.AGCasino
{
    using System;

    /// <summary>
    /// Game Type Group.
    /// </summary>
    [Serializable]
    public class GameTypeGroup
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Enable { get; set; }

        public bool Checked { get; set; }

        public bool OldChecked { get; set; }
    }
}
