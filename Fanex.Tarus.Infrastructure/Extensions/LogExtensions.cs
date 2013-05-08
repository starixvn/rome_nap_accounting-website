// -----------------------------------------------------------------------
// <copyright file="LogExtensions.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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

            Fanex.Logging.Logger.Log.Error(ex.Message, ex);
        }
    }
}
