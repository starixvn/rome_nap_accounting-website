namespace Fanex.Tarus.Finance.DataAccess.Transfer
{
    using System.Data;
    using Fanex.Tarus.Finance.Entities;

    public interface ICrossTransferAccess
    {
        Result CustomerCrossTransferInsert(int fromCustID, int toCustID, double fromAmount, double toAmount, double fromExRate, double toExRate, string forexUsername, string remark, int custid);

        Result BettransInsertTransfer(int _accId, int _fromCustId, string _toUser, double _exRate, double _amount, string _remark, string _ip);

        CustomerExRate SelectCustomerExRate(string username);

        DataSet SelectAllForexAccount();

        DataSet SelectAllExchangeRate();
    }
}