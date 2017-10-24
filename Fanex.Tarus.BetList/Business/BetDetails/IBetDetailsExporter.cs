namespace Fanex.Tarus.BetList.Business.BetDetails
{
    using System.Collections.Generic;
    using System.IO;
    using Fanex.BetList.Accounting.Models;

    public interface IBetDetailsExporter
    {
        string GetFileNameWithoutExtension(int sportType);

        IEnumerable<TicketDetail> GetTicketDetails(int sportType, long refNo, int custId, byte transType);

        void Export(IEnumerable<TicketDetail> ticketDetails, Stream stream, int sportType);
    }
}
