using Sunplus.Accounting.Core.Cache;

namespace Fanex.Tarus.LiveCasino.Business
{
    using Fanex.Caching;

    public class BaseManager
    {

        protected readonly ICache CacheService = null;

        public BaseManager()
        {
            CacheService = Fanex.Caching.CacheService.CreateInstance(CacheType.MemCache);
        }

    }
}
