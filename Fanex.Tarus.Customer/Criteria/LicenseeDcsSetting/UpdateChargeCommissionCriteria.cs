
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class UpdateChargeCommissionCriteria : NonQueryCriteria
    {
        public UpdateChargeCommissionCriteria(
            int productId,
            int custId,
            IEnumerable<ChargeCommissionEntity> CommissionList,
            int userId,
            string ip)
        {
            ProductId = productId;
            this.CommissionList = CommissionList;
            CustId = custId;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public IEnumerable<ChargeCommissionEntity> CommissionList { get; }

        public string Discount
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("<ROOT>");
                foreach (var entity in CommissionList)
                {
                    builder.Append($"<Discount SportType=\"{entity.SportType}\" BetType=\"{entity.BetType}\" Discount=\"{entity.Discount}\" />");
                }

                builder.Append("</ROOT>");
                return builder.ToString();
            }
        }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_ChargeCommUpd";

        public override bool IsValid()
            => base.IsValid() && ProductId > 0 && CustId > 0 && CommissionList != null && CommissionList.Any();
    }
}
