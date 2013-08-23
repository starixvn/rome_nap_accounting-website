// -----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Web;
using System.Collections.Generic;

namespace Fanex.Tarus.Security.Web
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class AppSessions
    {
        public static Dictionary<string, int> RoleAccess
        {
            get { return (Dictionary<string, int>)HttpContext.Current.Session["RoleAccess"]; }
            set { HttpContext.Current.Session["RoleAccess"] = value; }
        }
        public static Dictionary<string, int> PageAccess
        {
            get { return (Dictionary<string, int>)HttpContext.Current.Session["PageAccess"]; }
            set { HttpContext.Current.Session["PageAccess"] = value; }
        }
    }
}
