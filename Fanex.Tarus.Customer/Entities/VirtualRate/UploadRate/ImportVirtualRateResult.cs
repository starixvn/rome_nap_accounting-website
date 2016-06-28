// --------------------------------------------------------------------------
// <copyright file="ImportVirtualRateResult.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate
{
    using System;

    /// <summary>
    /// Import Virtual Rate Result.
    /// </summary>
    [Serializable]
    public class ImportVirtualRateResult
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public int CustomerId { get; set; }

        public string Username { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public int BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public int LiveTypeId { get; set; }

        public string LiveTypeName { get; set; }

        public decimal VirtualRate { get; set; }

        public string VirtualRateString { get; set; }
    }
}
