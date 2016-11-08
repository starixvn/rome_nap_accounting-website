using Fanex.Tarus.Customer.Entities.NG2;

namespace Fanex.Tarus.Customer.DataAccess.NG2
{
    public interface ISyncCurrencyRepository
    {
        SyncCurrencyModel SyncCurrencies();

        SyncCurrencyEntity GetCurrencyByName(string currencyCode);
    }
}