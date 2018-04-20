namespace Fanex.Tarus.BetList.Business.BetDetails
{
    using System.Collections.Generic;
    using System.IO;
    using Models;

    public interface IBetDetailsExporter
    {
        string GetFileNameWithoutExtension(int sportType);

        IEnumerable<TicketDetail> GetTicketDetails(int sportType, long refNo, int custId, byte transType);

        IEnumerable<TicketDetail> GetJackpotDetails(long transId, int custId, int betType);

        void Export(IEnumerable<TicketDetail> ticketDetails, Stream stream, int sportType);
    }
}
