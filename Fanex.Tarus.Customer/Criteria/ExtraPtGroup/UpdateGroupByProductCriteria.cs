using System;
using System.Collections;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.ExtraPtGroup
{
    public class UpdateGroupByProductCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public byte GroupId { get; set; }

        public byte ProductId { get; set; }

        public decimal ExtraPt { get; set; }

        public DateTime OldEffectiveDate { get; set; }

        public DateTime OldEndDate { get; set; }

        public DateTime NewEffectiveDate { get; set; }

        public DateTime NewEndDate { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_GroupExtraPT_GroupByProduct_Upd";
        }
        
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("userid", UserId);
            param.Add("groupid", GroupId);
            param.Add("productid", ProductId);
            param.Add("extrapt", ExtraPt);
            param.Add("oldeffectivedate", OldEffectiveDate);
            param.Add("oldenddate", OldEndDate);
            param.Add("neweffectivedate", NewEffectiveDate);
            param.Add("newenddate", NewEndDate);
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
