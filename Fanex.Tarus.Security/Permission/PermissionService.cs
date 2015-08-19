namespace Fanex.Tarus.Security.Permission
{
    using System.Collections.Generic;
    using Fanex.Tarus.Security.Web;
    using Sunplus.Accounting.DataAccess.IBCDLL;
    using System.Data;
    using DNAide.Core;
    using System;
    using DNAide.Data;

    /// <summary>
    /// Class PermissionService.
    /// </summary>
    public class PermissionService : Security.Permission.IPermissionService
    {
        /// <summary>
        /// Determines whether the specified function name has permission.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns><c>true</c> if the specified function name has permission; otherwise, <c>false</c>.</returns>
        public static bool HasPermission(string functionName)
        {
            var roleAccessSession = AppSessions.RoleAccess;
            if (null == roleAccessSession)
            {
                return false;
            }

            var roleAccess = (Dictionary<string, int>)roleAccessSession;
            return roleAccess.ContainsKey("[" + functionName.Trim().ToLower() + "]");
        }

        /// <summary>
        /// Determines whether the specified action path has permission.
        /// </summary>
        /// <param name="actionPath">The action path.</param>
        /// <param name="pageAccess">The page access.</param>
        /// <returns><c>true</c> if the specified action path has permission; otherwise, <c>false</c>.</returns>
        public bool HasPermission(string actionPath, Dictionary<string, int> pageAccess)
        {
            if (pageAccess == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(actionPath) && pageAccess.ContainsKey(actionPath.ToLower());
        }

        /// <summary>
        /// Gets the host permission.
        /// </summary>
        /// <typeparam name="T">The function type.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="roleAccess">The role access.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GetHostPermission<T>(T function, Dictionary<string, int> roleAccess)
        {
            if (roleAccess == null)
            {
                return false;
            }

            string functionName = "[" + function.ToString() + "]";
            if (roleAccess.ContainsKey(functionName.ToLower()))
            {
                return roleAccess[functionName.ToLower()] == 3;
            }

            return false;
        }

        /// <summary>
        /// Gets the host permission.
        /// </summary>
        /// <param name="actionPath">The action path.</param>
        /// <param name="pageAccess">The page access.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool GetHostPermission(string actionPath, Dictionary<string, int> pageAccess)
        {
            if (pageAccess == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(actionPath))
            {
                return false;
            }

            if (pageAccess.ContainsKey(actionPath.ToLower()))
            {
                return pageAccess[actionPath.ToLower()] == 3;
            }

            return false;
        }

        /// <summary>
        /// Belong to #40994, we need to auth in real time not the session.
        /// This method is help to retrieve PageAccess and RoleAccess real time from DB.
        /// </summary>
        /// <param name="custid">Customer Id.</param>
        /// <param name="pageAcess">PageAccess output.</param>
        /// <param name="roleAccess">RoleAccess output.</param>
        public void GetPageAccessAndRoleAccess(int custid, Dictionary<string, int> pageAcess, Dictionary<string, int> roleAccess, ref int groupId)
        {
            char[] delimiterChars = { ';' };
            DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.Acc_Con_FileNameSelByCustID(custid);
            if (ds != null)
            {
                string filenames = "";
                if (ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    groupId = CastAide.AsInt32(ds.ResultSet.Tables[0].Rows[0]["groupid"], 0);
                }

                foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                {
                    filenames = CastAide.AsString(row["filenames"], string.Empty).ToLower();

                    string functionName = CastAide.AsString(row["funcname"], string.Empty).ToLower();
                    int actualPolicy = CastAide.AsInt32(row["actual_policy"], 0);
                    if (!roleAccess.ContainsKey(functionName))
                    {
                        roleAccess.Add(functionName, actualPolicy);
                    }

                    if (!string.IsNullOrEmpty(filenames))
                    {
                        string[] fileList = filenames.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string file in fileList)
                        {
                            var cleanFilePath = file.Trim();
                            if (!cleanFilePath.IndexOf("?").Equals(-1))
                            {
                                cleanFilePath = file.Substring(0, file.IndexOf('?'));
                                cleanFilePath = cleanFilePath.Trim();
                            }

                            if (!pageAcess.ContainsKey(cleanFilePath))
                                pageAcess.Add(cleanFilePath, actualPolicy);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Belong to #40994, we need to auth in real time not the session.
        /// This method is help to retrieve PageAccess real time from DB.
        /// </summary>
        /// <param name="custid">Customer Id.</param>
        public Dictionary<string, int> GetPageAccess(int custid)
        {
            Dictionary<string, int> pageAcess = new Dictionary<string, int>();
            char[] delimiterChars = { ';' };
            DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.Acc_Con_FileNameSelByCustID(custid);
            if (ds != null)
            {
                string filenames = "";
                foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                {
                    filenames = CastAide.AsString(row["filenames"], string.Empty).ToLower();
                    int actualPolicy = CastAide.AsInt32(row["actual_policy"], 0);
                    if (!string.IsNullOrEmpty(filenames))
                    {
                        string[] fileList = filenames.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string file in fileList)
                        {
                            var cleanFilePath = file.Trim();
                            if (!cleanFilePath.IndexOf("?").Equals(-1))
                            {
                                cleanFilePath = file.Substring(0, file.IndexOf('?'));
                                cleanFilePath = cleanFilePath.Trim();
                            }

                            if (!pageAcess.ContainsKey(cleanFilePath))
                                pageAcess.Add(cleanFilePath, actualPolicy);
                        }
                    }
                }
            }

            return pageAcess;
        }

        /// <summary>
        /// Belong to #40994, we need to auth in real time not the session.
        /// This method is help to retrieve  RoleAccess real time from DB.
        /// </summary>
        /// <param name="custid">Customer Id.</param>
        public Dictionary<string, int> GetRoleAccess(int custid)
        {
            Dictionary<string, int> roleAccess = new Dictionary<string, int>();
            char[] delimiterChars = { ';' };
            DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.Acc_Con_FileNameSelByCustID(custid);
            if (ds != null)
            {
                string filenames = "";
                foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                {
                    filenames = CastAide.AsString(row["filenames"], string.Empty).ToLower();
                    string functionName = CastAide.AsString(row["funcname"], string.Empty).ToLower();
                    int actualPolicy = CastAide.AsInt32(row["actual_policy"], 0);
                    if (!roleAccess.ContainsKey(functionName))
                    {
                        roleAccess.Add(functionName, actualPolicy);
                    }
                }
            }

            return roleAccess;
        }
    }
}