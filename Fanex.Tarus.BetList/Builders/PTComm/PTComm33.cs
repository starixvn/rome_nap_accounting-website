namespace Fanex.Tarus.BetList.Builders.PTComm
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class PTComm33 : PTComm
    {
        public PTComm33(ITicket ticket, List<ITicketData> ticketData, int roleId)
            : base(ticket, roleId)
        {
            TicketData = ticketData;
            PositionTakingWin = GetPositionTakingByUserLevel(roleId, isWin: true);
            PositionTakingPlace = GetPositionTakingByUserLevel(roleId, isWin: false);
        }

        public string PositionTakingWin { get; }

        public string PositionTakingPlace { get; }

        public int WinBetId => !IsVirtualRacing ? 31 : 1231;

        public int PlaceBetId => !IsVirtualRacing ? 32 : 1232;

        private bool IsVirtualRacing => Ticket.BetTypeId == 1233;

        protected List<ITicketData> TicketData { get; }

        private string GetPositionTakingByUserLevel(int roleId, bool isWin)
        {
            var refTicketData = IsVirtualRacing ?
                TicketData.FindAll(item => item.RefNo.Equals(Ticket.TransId.ToString())) :
                TicketData.FindAll(item => item.RefNo.Equals(Ticket.RefNo));
            var refTicketDataItem = refTicketData.LastOrDefault(item => item.BetTypeId == (isWin ? WinBetId : PlaceBetId));

            decimal positionTaking = 0;

            if (refTicketDataItem != null)
            {
                if (roleId.Equals(RoleIdConstants.Super))
                {
                    positionTaking = refTicketDataItem.SuperPT;
                }
                else if (roleId.Equals(RoleIdConstants.Master))
                {
                    positionTaking = refTicketDataItem.MasterPT;
                }
                else
                {
                    positionTaking = refTicketDataItem.AgentPT;
                }
            }

            return Formatter.FormatNumber2(positionTaking, null) + "%";
        }
    }
}
