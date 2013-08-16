using Sunplus.Accounting.Core.Cache;


namespace Fanex.Tarus.LiveCasino.Business
{
    using Fanex.Caching;
    using Sunplus.Accounting.Core.Common;
    using System;
    using Sunplus.Accounting.Core.Extensions;
    using Fanex.Tarus.LiveCasino.Entities;

    public class BaseManager
    {

        protected readonly ICache CacheService = null;

        public BaseManager()
        {
            CacheService = Fanex.Caching.CacheService.CreateInstance(CacheType.MemCache);
        }

        public bool GetHostPermission(string funcName)
        {
            try
            {
              
                if (AppSession.RoleAccess != null)
                {
                    if (AppSession.RoleAccess.ContainsKey(funcName.ToLower()))
                    {
                        if (AppSession.RoleAccess[funcName.ToLower()] == 3)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }
    }
}
