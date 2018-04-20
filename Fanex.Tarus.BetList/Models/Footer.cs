namespace Fanex.Tarus.BetList.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class Footer
    {
        private static readonly string[] ExcludingStatus =
        {
            BetStatus.Running,
            BetStatus.Waiting,
            BetStatus.Void,
            BetStatus.Reject
        };

        public Footer(IList<ITicket> tickets)
        {
            var totalWinLoss = tickets.Sum(ticket =>
                IsNotCricket(ticket.BetTypeId) &&
                NotContainExcludingStatus(ticket, ExcludingStatus) ? ticket.Winlost : 0);

            var totalComm = tickets.Sum(ticket =>
                NotContainExcludingStatus(ticket, ExcludingStatus) ? ticket.PlayerComm : 0);

            SubTotalWinLoss = ConvertByBetType.Winloss(totalWinLoss);
            SubTotalCommission = ConvertByBetType.Comm(totalComm);
            Total = ConvertByBetType.Winloss(totalWinLoss + totalComm);
        }

        public string SubTotalWinLoss { get; }

        public string SubTotalCommission { get; }

        public string Total { get; }

        private static bool IsNotCricket(int betType) => betType != 500;

        private static bool NotContainExcludingStatus(ITicket ticket, string[] excludingStatus)
        {
            var excludeStatus = excludingStatus ?? new string[] { };

            return !excludeStatus.Contains(ticket.Status.ToLowerInvariant());
        }
    }
}
