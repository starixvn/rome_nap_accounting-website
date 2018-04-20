namespace Fanex.Tarus.Customer.DataAccess.VirtualRate
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Customer.Entities.VirtualRate.SportBetTypeReferences;

    public interface IVirtualRateRepository
    {
        CustomerVirtualRateModel CustomerVirtualRate(ViewVirtualRateCriteria criteria);

        Result UpdateSettings(UpdateSettingsCriteria criteria);

        List<HistoryLogEntity> HistoryLog(ViewHistoryLogCriteria criteria);

        List<SportBetTypeReferencesEntity> GetSportBetTypeReferences(GetSportBetTypeReferencesCriteria criteria);
    }
}
