namespace Fanex.Tarus.Finance.Business.Transfer
{
    using System.Collections.Generic;
    using Fanex.Tarus.Finance.Entities;
    using Fanex.Tarus.Finance.Model.Transfer;

    public interface ICrossTransferService
    {
        Result TransferYesterdayTransaction(int accId, string fromCustName, string toCustName, double exRate, double amount, string remark);

        Result CrossCurrencyTransferInsert(FinanceCrossCurrencyTransfer param);

        CustomerExRate SelectCustomerExRate(string username);

        List<ForexAccount> SelectAllForexAccount();

        List<ExchangeRate> SelectAllExchangeRate();
    }
}