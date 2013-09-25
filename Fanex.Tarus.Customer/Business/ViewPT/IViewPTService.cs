// --------------------------------------------------------------------------
// <copyright file="IViewPTService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.ViewPT
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Customer.Criteria.ViewPT;
    using Fanex.Tarus.Customer.Entities.ViewPT;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IViewPTService
    {
        List<PostionTakingInfo> GetPTSetting(GetPTSettingCriteria criteria);
    }
}
