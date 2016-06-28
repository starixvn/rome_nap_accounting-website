// --------------------------------------------------------------------------
// <copyright file="IConfigureDirectAccountService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.ConfigureDirectAccount
{
    using System.Web;
    using Fanex.Tarus.Customer.Criteria.ConfigureDirectAccount;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.ConfigureDirectAccount;

    public interface IConfigureDirectAccountService
    {
        Result UpdateDirectAccountStatus(UpdateDirectAccountStatusCriteria criteria);

        UpdateDirectAccountStatusEffectInfo UpdateDirectAccountStatusEffect(UpdateDirectStatusEffectCriteria criteria);

        void DownloadImportDirectAccountTemplate();

        ImportDirectAccountResult CheckImportDirectAccount(HttpPostedFileBase fileUploaded, int userId);

        Result UpdateImportDirectAccount(UpdateImportDirectAccountCriteria criteria);
    }
}
