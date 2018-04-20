using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.NG2
{
    public class SyncCurrencyModel
    {
        public SyncCurrencyModel()
        {
            this.Currencies = new List<SyncCurrencyEntity>();
        }

        public IList<SyncCurrencyEntity> Currencies { get; set; }
    }
}
