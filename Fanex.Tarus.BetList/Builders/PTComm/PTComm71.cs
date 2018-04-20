namespace Fanex.Tarus.BetList.Builders.PTComm
{
    using Fanex.BetList.Core.Entities;

    public class PTComm71 : PTComm
    {
        public PTComm71(ITicket ticket, int roleId)
            : base(ticket, roleId)
        {
        }

        public override bool VisibilePositionTakingPercent => false;

        public override bool VisibileCommisionPercent => false;
    }
}
