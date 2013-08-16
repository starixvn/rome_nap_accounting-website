using System.Collections.Generic;

namespace Fanex.Tarus.Security.Permission
{
    internal interface IPermissionService
    {
        bool HasPermission(string actionPath, Dictionary<string, int> pageAccess);

        bool GetHostPermission<T>(T function, Dictionary<string, int> roleAccess);

        bool GetHostPermission(string actionPath, Dictionary<string, int> pageAccess);
    }
}