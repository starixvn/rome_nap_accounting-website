// --------------------------------------------------------------------------
// <copyright file="IVirtualRateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.VirtualRate
{
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;

    public interface IVirtualRateService
    {
        CustomerVirtualRateModel CustomerVirtualRate(ViewVirtualRateCriteria criteria);

        Result UpdateSettings(UpdateSettingsCriteria criteria);

        HistoryLogReportModel HistoryLog(ViewHistoryLogCriteria criteria);
    }
}
