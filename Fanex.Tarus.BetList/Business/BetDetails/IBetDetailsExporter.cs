namespace Fanex.Tarus.BetList.Business.BetDetails
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;

    public interface IBetDetailsExporter
    {
        string GetFileNameWithoutExtension(int sportType, string sportName);

        IEnumerable<TicketDetail> GetOthersTicketDetails(int sportType, long refNo, int custId, byte transType, DateTime winlostDate);

        IEnumerable<TicketDetail> GetRNGTicketDetails(long refNo, int custId, byte transType, DateTime winlostDate);

        IEnumerable<TicketDetail> GetJackpotDetails(long transId, int custId, int betType, DateTime winlostDate);

        void Export(IEnumerable<TicketDetail> ticketDetails, Stream stream, int sportType, bool hideSabaText);
    }
}
