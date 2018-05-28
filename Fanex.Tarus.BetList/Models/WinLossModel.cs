namespace Fanex.Tarus.BetList.Models
{
    using Constants;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class WinLossModel
    {
        private const string Hyphen = "-";
        private const string NetWin = "(NW) ";
        private const string ZeroValue = "0.00";

        public WinLossModel(ITicket ticket)
        {
            ShowComm = IsShowComm(ticket);
            SetData(ticket);
        }

        public bool ShowComm { get; }

        public string WinLoss { get; private set; }

        public string Comm { get; private set; }

        private static bool IsShowComm(ITicket ticket)
        {
            var isIMBCMatch = ticket.SuperDiscount < 0 || ticket.MasterDiscount < 0 || ticket.AgentDiscount < 0;

            return (ticket.BetTypeId != 501 && !BetTypeContants.IsJackpotContribution(ticket.BetTypeId)) || !isIMBCMatch;
        }

        private void SetData(ITicket ticket)
        {
            if (BetTypeContants.IsJackpotContribution(ticket.BetTypeId))
            {
                WinLoss = Hyphen;
                Comm = string.Empty;

                return;
            }

            var status = ticket.Status.ToLowerInvariant();
            switch (status)
            {
                case BetStatus.Running:
                case BetStatus.Waiting:
                    WinLoss = Hyphen;
                    Comm = string.Empty;
                    break;

                case BetStatus.Void:
                    WinLoss = ticket.BetTypeId == 500 ? NetWin + ZeroValue : string.Empty;
                    Comm = ZeroValue;
                    break;

                case BetStatus.Reject:
                    WinLoss = ZeroValue;
                    Comm = string.Empty;
                    break;

                default:
                    WinLoss = ticket.BetTypeId == 500 ? NetWin + ConvertByBetType.Winloss(ticket.ActualStake) : ConvertByBetType.Winloss(ticket.Winlost);
                    Comm = ConvertByBetType.Comm(ticket.PlayerComm);
                    break;
            }
        }
    }
}
