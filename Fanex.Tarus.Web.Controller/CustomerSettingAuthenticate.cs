// --------------------------------------------------------------------------
// <copyright file="Authenticate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// <summary>
// Controller for account
// </summary>
// --------------------------------------------------------------------------
// <history>
//   <change who="Andy.Nguyen" date="2013.8.8">Created</change>
// </history>

namespace Fanex.Tarus.Web.Controllers
{
    using Fanex.CustomerSetting.Security.Authenticate;
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Business.Interface;
    using Sunplus.Accounting.Core.Common;
    using DNAide.Core;
    using Sunplus.Accounting.Core.Configurations;

    public class CustomerSettingAuthenticate : IAuthenticate
    {
        private readonly IPermissionService _permissionService = new PermissionService();
        private int groupId = CastAide.AsInt32(WebSettings.ExternalBettingLimitConfig.Split(new char[] { '|' })[0], 0);

        public bool AllowRead(string action)
        {
            if (groupId != AppSession.GroupID)
            {
                return false;
            }

            return this._permissionService.HasPermission(action, AppSession.PageAccess);
        }

        public bool AllowUpdate(string action)
        {
            if (groupId != AppSession.GroupID)
            {
                return false;
            }

            return this._permissionService.GetHostPermission(action, AppSession.PageAccess);
        }

        public string Function
        {
            get;
            set;
        }

        public AuthenticateAction.AuthenticationType Policy
        {
            get;
            set;
        }
    }
}