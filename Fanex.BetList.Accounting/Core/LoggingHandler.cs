// --------------------------------------------------------------------------
// <copyright file="LoggingHandler.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.17">Created</change>
// </history>

namespace Fanex.BetList.Accounting.Core
{
    using System;
    using Fanex.Module.Logs.Services;
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
            ex.WriteLog();
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            var ex = new Exception(message);
            ex.Write();
        }
    }
}