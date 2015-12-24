namespace Fanex.Tarus.Infrastructure.Business
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;

    public interface IServiceBase
    {
        IEnumerable<T> GetData<T>(ICriteria criteria);
    }
}
