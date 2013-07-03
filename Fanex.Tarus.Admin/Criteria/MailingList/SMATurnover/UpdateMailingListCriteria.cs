// --------------------------------------------------------------------------
// <copyright file="UpdateMailingListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Criteria.MailingList.SMATurnover
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Admin.Entities.MailingList;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class UpdateMailingListCriteria
    {
        public bool IsUpdate { get; set; }

        public MailLicensee mailLicensee { get; set; }

        public string GetSettingKey()
        {
            return this.IsUpdate ?
                "Acc_MailingListUpd" :
                "Acc_MailingListIns";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
