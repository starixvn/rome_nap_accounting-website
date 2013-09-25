// -----------------------------------------------------------------------
// <copyright file="LogExtensions.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Web;

namespace Fanex.Tarus.Infrastructure.Extensions
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LogExtensions
    {
        public static void ExceptionHandler(Exception ex)
        {
            string message = ex.Message +
                             "\r\nSOURCE: " + ex.Source +
                             "\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                             "\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                             "\r\nFORM: " + HttpContext.Current.Request.Form.ToString() +
                             "\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString.ToString() +
                             "\r\nTARGETSITE: " + ex.TargetSite +
                             "\r\nSTACKTRACE: " + ex.StackTrace;
            Fanex.Logging.Logger.Log.Error(message);
        }
    }
}
