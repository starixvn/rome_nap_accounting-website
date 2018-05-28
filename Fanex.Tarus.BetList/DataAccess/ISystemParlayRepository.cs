namespace Fanex.Tarus.BetList.DataAccess
{
    using Models;
    using NAP.Framework.Data;

    public interface ISystemParlayRepository
    {
        SystemParlayDetail CombinationDetails(ICriteria criteria);

        SystemParlayDetailExcel GetSystemParlayDetailExcel(ICriteria criteria);
    }
}
