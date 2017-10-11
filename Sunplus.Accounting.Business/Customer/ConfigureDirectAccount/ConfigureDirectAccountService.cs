// --------------------------------------------------------------------------
// <copyright file="ConfigureDirectAccountService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Business.Customer.ConfigureDirectAccount
{
    using Core.Common;
    using Criteria;
    using DataAccess;
    using DNAide.Web;
    using Entities;
    using Resources;
    using Security;

    /// <summary>
    /// Configure Direct Account Service.
    /// </summary>
    public class ConfigureDirectAccountService
    {
        /// <summary>
        /// Adds the mark direct account setting control.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public static void AddMarkDirectAccountSettingControl(AddMarkDirectAccountSettingControlCriteria criteria)
        {
            var hasPermission = Permissions.HasPermission(Enumerations.Functions.Customer.ConfigureDirectAccount);
            var htmlContent = criteria.Htmlcontent;
            if (!hasPermission || !criteria.DirectAccountStatusInfo.IsShowDirectStatus)
            {
                TemplateHandler.RemoveBlock(ref htmlContent, "directAccountControl");
                return;
            }
            
            TemplateHandler.Assign(ref htmlContent, criteria.ControlValueTag, criteria.DirectAccountStatusInfo.IsCheckedDirectStatus);
            criteria.Htmlcontent = htmlContent;

            var directAccountControlSettingGroup = new SettingGroup();
            directAccountControlSettingGroup.Add(GetSettingControlMarkDirectAccount(criteria));
            var html = directAccountControlSettingGroup.GetHTMLSettingControl();
            TemplateHandler.Assign(ref htmlContent, "{directAccountControl}", directAccountControlSettingGroup.GetHTMLSettingControl());
        }

        /// <summary>
        /// Gets the mark direct account setting control.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The mark direct account setting control.</returns>
        public static SettingControl GetSettingControlMarkDirectAccount(AddMarkDirectAccountSettingControlCriteria criteria)
        {
            string classname = string.Empty;
            string jsonclick = string.Empty;
            string jsonmouseover = string.Empty;
            string jsonmouseout = string.Empty;
            if (criteria.IsManageCustomerAccess)
            {
                if (criteria.DirectAccountStatusInfo.CanEditDirectStatus)
                {
                    classname = "canchange";
                    jsonclick = "return changeDirectAccountStatus('" + criteria.Username + "'" +
                        "," + criteria.UserId +
                        "," + (criteria.DirectAccountStatusInfo.IsCheckedDirectStatus ? 1 : 0).ToString() +
                        ");";
                }
                else
                {
                    classname = "cannotchange";
                    jsonmouseout = "UnTip()";
                    jsonmouseover = string.Format("Tip('{0}', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 8)", Labels.DisabledMarkDirectStatus);
                }
            }

            return new SettingControl(
                Labels.Direct_Account,
                criteria.DirectAccountStatusInfo.IsCheckedDirectStatus,
                false,
                jsonclick,
                jsonmouseout,
                jsonmouseover,
                classname,
                criteria.ControlName,
                criteria.RoleId,
                false,
                criteria.ControlType);
        }
    }
}
