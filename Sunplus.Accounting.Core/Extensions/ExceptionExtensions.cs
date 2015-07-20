// -----------------------------------------------------------------------
// <copyright file="ExceptionExtensions.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Core.Extensions
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ExceptionExtensions
    {
        public static string FormatSensitiveInfo(this Exception ex, string info)
        {
            var formatedInfo = string.IsNullOrWhiteSpace(info) ? string.Empty : info;
            var infos = formatedInfo.Split('&');

            foreach (var item in infos)
            {
                if (item.ToLower().Contains("pass") || item.ToLower().Contains("pwd"))
                {
                    return string.Empty;
                }
            }

            return formatedInfo;
        }

        public static void WriteLog(this Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }
}