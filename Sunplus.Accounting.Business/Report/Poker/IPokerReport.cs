using Sunplus.Accounting.Entities.Report.Poker;

namespace Sunplus.Accounting.Business.Report.Poker
{
    public interface IPokerReport
    {
        PokerMainBO SelWinlostP2P(string FromDate, string ToDate);
        PokerMainBO SelWinlostP2PDetail(int currency, string date);
        void ExportExcelWinlostP2P(string fromDate, string toDate, string reportTitle);
        void ExportExcelWinlostP2PDetail(int currency, string date, string reportTitle);
    }
}
