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

    public class Authenticate : IAuthenticate
    {
        public bool AllowRead(string action)
        {
            // Implement permission Read here
            return true;
        }

        public bool AllowUpdate(string action)
        {
            // Implement permission Host here
            return true;
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