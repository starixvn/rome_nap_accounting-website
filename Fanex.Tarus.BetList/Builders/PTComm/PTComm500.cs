namespace Fanex.Tarus.BetList.Builders.PTComm
{
    using System.Collections.Generic;
    using Constants;
    using Fanex.BetList.Core.Entities;

    public class PTComm500 : PTComm
    {
        public PTComm500(ITicket ticket, int roleId)
            : base(ticket, roleId)
        {
        }

        protected override IDictionary<int, object> CommSettingByRoleId
            => new Dictionary<int, object>
            {
                { RoleIdConstants.Super, Ticket.SuperDiscount - Ticket.MasterDiscount },
                { RoleIdConstants.Master, Ticket.MasterDiscount - Ticket.AgentDiscount },
                { RoleIdConstants.Agent, -Ticket.AgentDiscount }
            };
    }
}
