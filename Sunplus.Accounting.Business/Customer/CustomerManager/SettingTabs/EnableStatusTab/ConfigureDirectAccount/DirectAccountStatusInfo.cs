// --------------------------------------------------------------------------
// <copyright file="DirectAccountStatusInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Business.Customer.ConfigureDirectAccount
{
    /// <summary>
    /// Direct Account Status Info.
    /// </summary>
    public class DirectAccountStatusInfo
    {
        public bool IsShowDirectStatus { get; set; }

        public bool CanEditDirectStatus { get; set; }

        public bool IsCheckedDirectStatus { get; set; }
    }
}
