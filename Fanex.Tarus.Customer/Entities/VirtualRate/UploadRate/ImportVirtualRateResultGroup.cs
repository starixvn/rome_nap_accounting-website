// --------------------------------------------------------------------------
// <copyright file="ImportVirtualRateResultGroup.cs" company="">
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
    /// Import Virtual Rate Result Group.
    /// </summary>
    [Serializable]
    public class ImportVirtualRateResultGroup
    {
        public int GroupdId { get; set; }

        public string GroupName { get; set; }

        public List<ImportVirtualRateResult> ImportVirtualRateResultList { get; set; }
    }
}
