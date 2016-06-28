// --------------------------------------------------------------------------
// <copyright file="NapLoginAreaRegistration.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.NAP.SSO
{
    using System.Web.Mvc;

    /// <summary>
    /// NAP SSO area registration.
    /// </summary>
    public class NapLoginAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "NapLogin";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var controller = new { controller = "NapSso", action = "Index", id = UrlParameter.Optional };
            context.MapRoute(this.AreaName + "_Default", "NapLogin/{controller}/{action}/{id}", controller, new[] { typeof(Fanex.NAP.SSO.NapSsoController).Namespace });
        }
    }
}