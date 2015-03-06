// --------------------------------------------------------------------------
// <copyright file="IUploadVirtualRateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.VirtualRate.UploadRate
{
    using System.Web;
    using Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;

    public interface IUploadVirtualRateService
    {
        UploadResult CheckImportVirtualRate(HttpPostedFileBase fileUploaded, int userId);

        void DownloadImportRateTemplate();

        UploadResult UpdateImportRate(UpdateImportRateCriteria criteria);

        Result DeleteTempRate();
    }
}
