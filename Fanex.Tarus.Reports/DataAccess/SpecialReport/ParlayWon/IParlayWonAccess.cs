

namespace Fanex.Tarus.Reports.DataAccess.ParlayWon
{
    using System.Data;
    public interface IParlayWonAccess
    {
        DataSet GetParlayWonAccess(string winlostDate, int betType, bool isSoccer, string moveBetsDay, string balanceUpDay);
    }
}
