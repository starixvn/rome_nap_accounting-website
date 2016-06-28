// --------------------------------------------------------------------------
// <copyright file="AddMarkDirectAccountSettingControlCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Business.Criteria
{
    using Sunplus.Accounting.Business.Customer.ConfigureDirectAccount;
    using Sunplus.Accounting.DataAccess;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AddMarkDirectAccountSettingControlCriteria
    {
        public SettingGroup SettingGroup { get; set; }

        public string Htmlcontent { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public int RoleId { get; set; }

        public DirectAccountStatusInfo DirectAccountStatusInfo { get; set; }

        public string ControlName
        {
            get
            {
                return "enableDirectStatus";
            }
        }

        public string ControlType
        {
            get
            {
                return "EnableDirectStatus";
            }
        }

        public string ControlValueTag
        {
            get
            {
                return "{chkEnableDirectStatus}";
            }
        }

        public bool IsManageCustomerAccess { get; set; }
    }
}
