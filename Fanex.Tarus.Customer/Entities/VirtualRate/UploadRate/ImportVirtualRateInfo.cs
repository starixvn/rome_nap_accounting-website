// --------------------------------------------------------------------------
// <copyright file="ImportVirtualRateInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate
{
    using System;

    /// <summary>
    /// Import Virtual Rate Information.
    /// </summary>
    [Serializable]
    public class ImportVirtualRateInfo
    {
        public string Username { get; set; }

        public int SportType { get; set; }

        public int LiveType { get; set; }

        public int BetType { get; set; }

        public decimal VirtualRate { get; set; }

        public string SportName { get; set; }

        public string BetTypeName { get; set; }

        public string LiveTypeName { get; set; }
    }
}
