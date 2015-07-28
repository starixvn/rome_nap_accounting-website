// -----------------------------------------------------------------------
// <copyright file="ExceptionExtensions.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Core.Extensions
{
    using System;
    using System.Data.SqlClient;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ExceptionExtensions
    {
        public static string FormatSensitiveInfo(this Exception ex, string info)
        {
            var formatedInfo = string.IsNullOrWhiteSpace(info) ? string.Empty : info.ToLower(); 
            return formatedInfo.Contains("pass") || formatedInfo.Contains("pwd") || formatedInfo.Contains("security") ? string.Empty : formatedInfo;
        }

        public static void WriteLog(this Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }

        public static string FormatSqlException(this Exception ex)
        {
            var sqlEx = ex as SqlException;
            if (sqlEx != null)
            {
                var message = "\r\nProcedure: " + sqlEx.Procedure +
                        "\r\nLine Number: " + sqlEx.LineNumber +
                        "\r\nServer: " + sqlEx.Server;

                return message;
            }

            return string.Empty;
        }
    }
}