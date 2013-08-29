// --------------------------------------------------------------------------
// <copyright file="GetPTSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.ViewPT
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetPTSettingCriteria
    {
        public string UserName { get; set; }

        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public bool IsLive { get; set; }

        public string GetSettingKey()
        {
            return "Customer_ViewPTSel";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
