// --------------------------------------------------------------------------
// <copyright file="ViewResources.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.AGCasino
{
    using System.Collections.Generic;

    /// <summary>
    /// View Resources.
    /// </summary>
    public class ViewResources
    {
        public string CustomerId { get; set; }

        public int CurrentRole { get; set; }

        public bool HasHostPermission { get; set; }

        public string PageMD5Key { get; set; }

        public Dictionary<string, string> Labels { get; set; }
    }
}
