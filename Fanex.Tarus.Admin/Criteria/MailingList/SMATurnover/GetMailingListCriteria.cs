// --------------------------------------------------------------------------
// <copyright file="GetMailingListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Criteria.MailingList.SMATurnover
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetMailingListCriteria
    {
        public string GetSettingKey()
        {
            return "Acc_MailingListSel";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
