using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class UpdateChargePositionTakingCriteria : NonQueryCriteria
    {
        public UpdateChargePositionTakingCriteria(
            int productId,
            int custId,
            IEnumerable<ChargePTEntity> PTList,
            int userId,
            string ip)
        {
            ProductId = productId;
            this.PTList = PTList;
            CustId = custId;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public IEnumerable<ChargePTEntity> PTList { get; }

        public string PT
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("<ROOT>");
                foreach (var entity in PTList)
                {
                    builder.Append("<PT ");
                    builder.Append("SportType=\"" + entity.SportType + "\" ");
                    var pt = entity.PT;
                    if (0 <= pt && pt <= 1)
                    {
                        pt *= 100;
                    }

                    builder.Append("PT=\"" + pt + "\" ");
                    builder.Append("/>");
                }

                builder.Append("</ROOT>");
                return builder.ToString();
            }
        }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_ChargePTUpd";

        public override bool IsValid()
            => base.IsValid() && ProductId > 0 && CustId > 0 && PTList != null && PTList.Any();
    }
}
