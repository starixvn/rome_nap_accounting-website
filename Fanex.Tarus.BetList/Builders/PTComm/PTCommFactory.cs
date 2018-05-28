namespace Fanex.Tarus.BetList.Builders.PTComm
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    public static class PTCommFactory
    {
        public static PTComm CreateInstance(ITicket ticket, List<ITicketData> ticketData, int roleId)
        {
            if (ticket.BetTypeId == 33 || ticket.BetTypeId == 1233)
            {
                return new PTComm33(ticket, ticketData, roleId);
            }

            if (ticket.BetTypeId == 500)
            {
                return new PTComm500(ticket, roleId);
            }

            if (ticket.BetTypeId == 501)
            {
                return new PTComm501(ticket, roleId);
            }

            if (ticket.BetTypeId == 71 || ticket.BetTypeId == 73)
            {
                return new PTComm71(ticket, roleId);
            }

            if (ticket.BetTypeId == 2198 || ticket.BetTypeId == 2398 || ticket.BetTypeId == 2998)
            {
                return new PTComm2198(ticket, roleId);
            }

            return new PTComm(ticket, roleId);
        }
    }
}
