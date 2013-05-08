// -----------------------------------------------------------------------
// <copyright file="DataUtils.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Infrastructure.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DataUtils
    {
        public static bool IsExistKey(DataRow dr, string key)
        {
            try
            {
                string value = dr[key].ToString();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsExistKey(Dictionary<string, double> dr, string key)
        {
            try
            {
                string value = dr[key].ToString();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
