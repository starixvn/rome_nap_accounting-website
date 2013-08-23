using System.Collections.Generic;
using System.Web;
using Fanex.Tarus.Security.Web;

namespace Fanex.Tarus.Security.Permission
{
    internal class PermissionService : Security.Permission.IPermissionService 
    {

       public bool HasPermission(string actionPath, Dictionary<string, int> pageAccess)
        {
           if (pageAccess == null)
           {
               return false;
           }

           return !string.IsNullOrEmpty(actionPath) && pageAccess.ContainsKey(actionPath.ToLower());
        }

        public static bool HasPermission(string functionName)
        {
            var roleAccessSession = AppSessions.RoleAccess;
            if (null == roleAccessSession)
                return false;
            var roleAccess = (Dictionary<string,int>)roleAccessSession;
            return roleAccess.ContainsKey("[" + functionName.Trim().ToLower() + "]");
        }

        public bool GetHostPermission<T>(T function, Dictionary<string, int> roleAccess)
        {
            if (roleAccess == null)
                return false;

            string functionName = "[" + function.ToString() + "]";
            if(roleAccess.ContainsKey(functionName.ToLower()))
            {
                return roleAccess[functionName.ToLower()] == 3;
            }
            return false;
        }

        public bool GetHostPermission(string actionPath, Dictionary<string, int> pageAccess)
        {
            if (pageAccess == null)
                return false;
            if (string.IsNullOrEmpty(actionPath))
                return false;

            if (pageAccess.ContainsKey(actionPath.ToLower()))
            {
                return pageAccess[actionPath.ToLower()] == 3;
            }
            return false;
        }

        
    }
}
