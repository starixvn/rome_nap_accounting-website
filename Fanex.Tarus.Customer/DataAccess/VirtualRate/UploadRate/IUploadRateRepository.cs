// --------------------------------------------------------------------------
// <copyright file="IUploadRateRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.VirtualRate.UploadRate
{
    using Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;

    public interface IUploadRateRepository
    {
        CheckImportResult InsertTempRate(InsertTempRateCriteria criteria);

        CheckImportResult CheckImport(CheckImportCriteria criteria);

        Result DeleteTempRate(DeleteTempRateCriteria criteria);

        CheckImportResult UpdateImportRate(UpdateImportRateCriteria criteria);
    }
}
