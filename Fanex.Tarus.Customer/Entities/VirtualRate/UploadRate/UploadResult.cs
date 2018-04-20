// --------------------------------------------------------------------------
// <copyright file="UploadResult.cs" company="">
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
    /// Upload Result.
    /// </summary>
    [Serializable]
    public class UploadResult
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public string ResultNote { get; set; }

        public int FileSize { get; set; }

        public string FileName { get; set; }

        public int TotalSuccessRow { get; set; }

        public List<ImportVirtualRateResultGroup> FailedInfoList { get; set; }

        public List<ImportVirtualRateResultGroup> SucceededInfoList { get; set; }

        public List<ImportVirtualRateResult> SuccessRecords { get; set; }
    }
}
