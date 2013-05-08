namespace Fanex.Tarus.Finance.Business.Transfer
{
    using Fanex.Tarus.Finance.Entities;
    using Fanex.Tarus.Finance.Model.Transfer;
    using System.Collections.Generic;

    public interface ICrossTransferService
    {
        Result TransferYesterdayTransaction(int accId, string fromCustName, string toCustName, double exRate, double amount, string remark);
        Result CrossCurrencyTransferInsert(FinanceCrossCurrencyTransfer param);
        CustomerExRate SelectCustomerExRate(string username);
        List<ForexAccount> SelectAllForexAccount();
        List<ExchangeRate> SelectAllExchangeRate();
    }
}
