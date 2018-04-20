// --------------------------------------------------------------------------
// <copyright file="CheckImportResult.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Check Import Result.
    /// </summary>
    [Serializable]
    public class CheckImportResult
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public string ResultNote { get; set; }

        public List<ImportVirtualRateResult> FailedInfoList { get; set; }

        public List<ImportVirtualRateResult> SucceededInfoList { get; set; }
    }
}
