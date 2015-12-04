namespace Fanex.Tarus.Customer.Business.VirtualRate
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Customer.Entities.VirtualRate.SportBetTypeReferences;
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;

    public interface IVirtualRateService
    {
        CustomerVirtualRateModel CustomerVirtualRate(ViewVirtualRateCriteria criteria);

        Result UpdateSettings(UpdateSettingsCriteria criteria);

        HistoryLogReportModel HistoryLog(ViewHistoryLogCriteria criteria);

        void DownloadImportRateTemplate();

        UploadResult UpdateImportRate(UpdateImportRateCriteria criteria);

        Result DeleteTempRate();

        List<SportBetTypeReferencesModel> GetSportBetTypeReferences(GetSportBetTypeReferencesCriteria criteria);
    }
}
