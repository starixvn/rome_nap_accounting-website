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

using Fanex.CustomerSetting.Security.Authenticate;
using Fanex.Tarus.Security.Web;
using IPermissionService = Fanex.Tarus.Security.Permission.IPermissionService;
using PermissionService = Fanex.Tarus.Security.Permission.PermissionService;


namespace Fanex.Tarus.Security.Authenticate
{
    public class CustomerSetting : IAuthenticate
    {
        private readonly IPermissionService _permissionService = new PermissionService();
        public bool AllowRead(string action)
        {
            return _permissionService.HasPermission(action, AppSessions.PageAccess);
        }

        public bool AllowUpdate(string action)
        {
            return _permissionService.GetHostPermission(action, AppSessions.PageAccess);
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