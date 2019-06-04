using Fanex.NAP.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.Customer.Criteria.CN88
{
    public class UpdateCreditCriteria : CriteriaBase
    {
        public UpdateCreditCriteria(
            int searchAccountId,
            decimal creditAmount,
            int actorId,
            string ip)
        {
            SearchAccountId = searchAccountId;
            CreditAmount = creditAmount;
            ActorId = actorId;
            IP = ip;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        [SpParam(SqlParamName = "UpdCredit")]
        public decimal CreditAmount { get; }

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "SubAccId")]
        public int SubAccountId => 0;

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "CS_Customer_Credit_Upd";

        public override bool IsValid() => SearchAccountId > 0 && ActorId > 0;

    }
}
