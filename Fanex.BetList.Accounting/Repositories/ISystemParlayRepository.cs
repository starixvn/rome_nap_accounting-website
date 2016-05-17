namespace Fanex.BetList.Accounting.Repositories
{
    using Models;
    using NAP.Framework.Data;

    public interface ISystemParlayRepository
    {
        SystemParlayDetail CombinationDetails(ICriteria criteria);

        SystemParlayDetailExcel GetSystemParlayDetailExcel(ICriteria criteria);
    }
}
