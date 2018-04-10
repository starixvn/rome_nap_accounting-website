namespace Fanex.Tarus.BetList.Business
{
    using System.IO;
    using Criteria;

    public interface ISystemParlayExcel
    {
        void ExportCombinationDetails(BetListSystemParlayByRefnoCriteria criteria, Stream stream);

        void ExportSystemParlayDetails(BetListSystemParlayDetailsCriteria criteria, Stream stream);
    }
}
