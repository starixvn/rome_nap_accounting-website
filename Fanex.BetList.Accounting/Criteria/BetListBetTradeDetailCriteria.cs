// --------------------------------------------------------------------------
// <copyright file="BetListByRefNoCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Criteria
{
    public class BetListBetTradeDetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_BetTradeDetails";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}