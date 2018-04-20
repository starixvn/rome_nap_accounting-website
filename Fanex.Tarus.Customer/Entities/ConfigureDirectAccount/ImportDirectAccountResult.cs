// --------------------------------------------------------------------------
// <copyright file="ImportDirectAccountResult.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.ConfigureDirectAccount
{
    using System.Collections.Generic;

    /// <summary>
    /// Import Direct Account Result.
    /// </summary>
    public class ImportDirectAccountResult
    {
        public int ImportResultCode { get; set; }

        public string ImportResultMessage { get; set; }

        public int UsernameIndex { get; set; }

        public CustomerInfo CustomerInfo { get; set; }

        public List<ImportDirectAccountResult> FailedInfoList { get; set; }

        public List<ImportDirectAccountResult> SucceededInfoList { get; set; }
    }
}
