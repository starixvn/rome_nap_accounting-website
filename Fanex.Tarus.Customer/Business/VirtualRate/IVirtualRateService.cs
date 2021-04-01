namespace Fanex.Tarus.Customer.Business.VirtualRate
{
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Customer.Entities.VirtualRate.SportBetTypeReferences;
    using System.Collections.Generic;

    public interface IVirtualRateService
    {
        CustomerVirtualRateModel CustomerVirtualRate(ViewVirtualRateCriteria criteria);

        Result UpdateSettings(UpdateSettingsCriteria criteria);

        HistoryLogReportModel HistoryLog(ViewHistoryLogCriteria criteria);

        List<SportBetTypeReferencesModel> GetSportBetTypeReferences(GetSportBetTypeReferencesCriteria criteria);

        string GetConfirmUpdateMessage(int custId);
    }
}
