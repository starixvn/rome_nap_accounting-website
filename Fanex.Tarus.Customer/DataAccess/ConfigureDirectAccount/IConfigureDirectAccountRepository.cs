// --------------------------------------------------------------------------
// <copyright file="IConfigureDirectAccountRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.AGCasino
{
    using Fanex.Tarus.Customer.Criteria.ConfigureDirectAccount;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.ConfigureDirectAccount;

    public interface IConfigureDirectAccountRepository
    {
        Result UpdateDirectAccountStatus(UpdateDirectAccountStatusCriteria criteria);

        UpdateDirectAccountStatusEffectInfo UpdateDirectAccountStatusEffect(UpdateDirectStatusEffectCriteria criteria);

        ImportDirectAccountResult CheckImportDirectAccount(CheckImportDirectAccountFileCriteria criteria);

        Result UpdateImportDirectAccount(UpdateImportDirectAccountCriteria criteria);
    }
}
