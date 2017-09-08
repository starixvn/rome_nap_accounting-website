using Fanex.NAP.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Entities.Criteria.Admin
{
    public class GetTicketToHeadlineSupportedProductCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_TicketGroupHeadline_ProductSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
