using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Common;



namespace Sunplus.Accounting.Core.Extensions
{
    using System.Web;

    /**
     * public class Log Extensions
     * @Author: bobby.nguyen
     * @Date: 11012011
     **/
    public static class LogExtensions
    {
        private static BackgroundWorker _worker = null;
        private static Queue<Exception> _queueEx = new Queue<Exception>();
        /**
         * Function: Exception Handler
         */
        public static void ExceptionHandler(Exception ex)
        {
            string message = ex.Message +
                             "<br>\r\nSOURCE: " + ex.Source +
                             "<br>\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                             "<br>\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                             "<br>\r\nFORM: " + HttpContext.Current.Request.Form.ToString() +
                             "<br>\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString.ToString() +
                             "<br>\r\nTARGETSITE: " + ex.TargetSite +
                             "<br>\r\nSTACKTRACE: " + ex.StackTrace;
            Fanex.Logging.Logger.Log.Error(message);
        }

    }
}
