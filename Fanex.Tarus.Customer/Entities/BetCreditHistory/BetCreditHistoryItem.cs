using System;

namespace Fanex.Tarus.Customer.Entities.BetCreditHistory
{
    public class BetCreditHistoryItem
    {
        public BetCreditHistoryItem(
            DateTime actionDate,
            DateTime? winLossDate,
            long? refNo,
            decimal? actualStake,
            decimal? winLoss,
            decimal? playerComm,
            decimal? balanceChange,
            decimal? newBalance,
            decimal givenCredit,
            decimal? betCredit,
            string action)
        {
            ActionDate = actionDate;
            WinLossDate = winLossDate;
            RefNo = refNo;
            ActualStake = actualStake;
            WinLoss = winLoss;
            PlayerComm = playerComm;
            BalanceChange = balanceChange;
            NewBalance = newBalance;
            GivenCredit = givenCredit;
            BetCredit = betCredit;
            Action = action;
        }

        public DateTime ActionDate { get; }

        public DateTime? WinLossDate { get; }

        public long? RefNo { get; }

        public bool IsHilightedRow => !BetCredit.HasValue;

        public decimal? ActualStake { get; }

        public decimal? WinLoss { get; }

        public decimal? PlayerComm { get; }

        public decimal? BalanceChange { get; }

        public decimal? NewBalance { get; }

        public decimal GivenCredit { get; }

        public decimal? BetCredit { get; }

        public bool IsReddenBetCredit => BetCredit.HasValue && BetCredit.Value < 0;

        public string Action { get; }
    }
}
