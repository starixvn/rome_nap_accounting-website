using System;
using Fanex.BetList.Accounting.Constants;
using Fanex.BetList.Accounting.Resources;

namespace Fanex.BetList.Accounting.Models
{
    public class CashOutTicket
    {
        public int BetType { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public string Status { get; set; }

        public decimal Stake { get; set; }

        public DateTime TransDate { get; set; }

        public int CustId { get; set; }

        public decimal Price { get; set; }

        public decimal WinLost { get; set; }

        public string StatusHtml
        {
            get
            {
                string upperStatus = Status.ToUpperInvariant();

                if (BetType == BetTypeContants.CashOut)
                {
                    return BuildCashOutStatus(BetListResources.CashOut, upperStatus);
                }

                if (BetType == BetTypeContants.CashOutBought)
                {
                    return BuildCashOutStatus(BetListResources.Bought, upperStatus);
                }

                return upperStatus;
            }
        }

        private string BuildCashOutStatus(string cashOutLabel, string upperStatus)
        {
            if (upperStatus == "WON" || upperStatus == "LOSE" || upperStatus == "DRAW")
            {
                return cashOutLabel.ToUpperInvariant();
            }

            if (upperStatus == "VOID" || upperStatus == "REFUND")
            {
                return string.Format("{0}<br/>({1})", upperStatus, cashOutLabel.ToUpperInvariant());
            }

            return upperStatus;
        }
    }
}
