// --------------------------------------------------------------------------
// <copyright file="ProductInfo.cs" company="">
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
    /// Product Information.
    /// </summary>
    public class ProductInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool EnableUpline { get; set; }

        public bool Enable { get; set; }
        
        public bool EnableDownlines { get; set; }

        public List<GameTypeGroup> GameTypeGroup { get; set; }

        public List<GameType> GameTypes { get; set; }
    }
}
