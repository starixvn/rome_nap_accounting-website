// --------------------------------------------------------------------------
// <copyright file="UserLoginAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.SSO
{
    using System.Web.Mvc;

    /// <summary>
    /// User login area registration.
    /// </summary>
    public class UserLoginAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserLogin";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var controller = new { controller = "Login", action = "Index", id = UrlParameter.Optional };
            context.MapRoute(this.AreaName + "_Default", "UserLogin/{controller}/{action}/{id}", controller, new[] { typeof(Fanex.NAP.SSO.LoginController).Namespace });
        }
    }
}