using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.NG2
{
    public class SyncCurrencyModel
    {
        public SyncCurrencyModel()
        {
            this.Model = new List<SyncCurrencyEntity>();
        }

        public IList<SyncCurrencyEntity> Model { get; set; }
    }
}
