// --------------------------------------------------------------------------
// <copyright file="LoggingHandler.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.17">Created</change>
// </history>

namespace Fanex.BetList.Accounting.Core
{
    using System;
    using System.Web;
    using Fanex.Logging;
    using Sunplus.Accounting.Core.Extensions;

    public enum LogType : int
    {
        /// <summary>
        /// The information.
        /// </summary>
        Info = 1,

        /// <summary>
        /// The error.
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoggingHandler
    {
        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public static void Log(Exception ex)
        {
            string message = ex.Message +
                             "\r\nSOURCE: " + ex.Source +
                             "\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                             "\r\nREFERRER: " + ex.FormatSensitiveInfo(HttpContext.Current.Request.UrlReferrer.ToString()) +
                             "\r\nFORM: " + ex.FormatSensitiveInfo(HttpContext.Current.Request.Form.ToString()) +
                             "\r\nQUERYSTRING: " + ex.FormatSensitiveInfo(HttpContext.Current.Request.QueryString.ToString()) +
                             "\r\nTARGETSITE: " + ex.TargetSite +
                             "\r\nSTACKTRACE: " + ex.StackTrace +
                             ex.FormatSqlException();

            Fanex.Logging.Logger.Log.Error(message);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            var ex = new Exception(message);
            switch (logType)
            {
                case LogType.Info:
                    LogService.Info(ex.FormatSensitiveInfo(message));
                    break;
                case LogType.Error:
                    LogService.Error(ex.FormatSensitiveInfo(message));
                    break;
            }
        }
    }
}