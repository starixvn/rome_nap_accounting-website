// --------------------------------------------------------------------------
// <copyright file="BetListByRefNoCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

using System;
    using CustomerSetting.Module.Core.Web;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Criteria
{
    public class BetListByRefNoCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public long RefNo { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }
        }

        public override string GetSettingKey()
        {
            return "BetList_SelByRefNo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}