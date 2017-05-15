
using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.NAP.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.BetList.Criteria.Lucky3
{
    public class GetLucky3DetailCriteria : CriteriaBase
    {
        public GetLucky3DetailCriteria(GetLucky3DetailParameter commandData)
        {
            criteria = commandData;
        }

        [SpParam(Ignored = true)]
        private GetLucky3DetailParameter criteria { get; set; }

        public long CustId
        {
            get
            {
                return criteria.CustomerId;
            }
        }

        public long TransId
        {
            get
            {
                return criteria.TransactionId;
            }
        }

        public override string GetSettingKey() => "Acc_BetList_Lucky3_Detail";

        public override bool IsValid() => criteria.IsValid();
    }
}
