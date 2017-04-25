using System;
using System.Collections;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.ExtraPtGroup
{
    public class InsertGroupByProductCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public byte GroupId { get; set; }

        public byte ProductId { get; set; }

        public decimal ExtraPt { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime EndDate { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_GroupExtraPT_GroupByProduct_Ins";
        }
        
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("userid", UserId);
            param.Add("groupid", GroupId);
            param.Add("productid", ProductId);
            param.Add("extrapt", ExtraPt);
            param.Add("effectivedate", EffectiveDate);
            param.Add("enddate", EndDate);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            return param;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
