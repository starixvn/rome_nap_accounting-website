namespace Fanex.Tarus.BetList.Core
{
    using Logging.Extensions.Web;
    using System;

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
            ExceptionHandler.Handle(ex);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            var ex = new Exception(message);
            ExceptionHandler.Handle(ex);
        }
    }
}