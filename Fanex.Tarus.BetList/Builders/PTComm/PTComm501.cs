namespace Fanex.Tarus.BetList.Builders.PTComm
{
    using Fanex.BetList.Core.Entities;

    public class PTComm501 : PTComm
    {
        public PTComm501(ITicket ticket, int roleId)
            : base(ticket, roleId)
        {
        }

        public override bool VisibleCommision => !IsIMBCMatch && base.VisibleCommision;

        public override bool VisibileCommisionPercent => !IsIMBCMatch && base.VisibileCommisionPercent;

        private bool IsIMBCMatch => Ticket.SuperDiscount < 0 || Ticket.MasterDiscount < 0 || Ticket.AgentDiscount < 0;
    }
}
