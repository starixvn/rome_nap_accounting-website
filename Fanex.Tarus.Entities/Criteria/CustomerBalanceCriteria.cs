using Fanex.NAP.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Entities.Criteria
{
    public class CustomerBalanceCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int? PageSize { get; set; }

        public int? PageIndex { get; set; }

        public override string GetSettingKey()
        {
            return "CustomerSelClearBalance";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
