using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class UpdatePositionTakingCriteria : NonQueryCriteria
    {
        public UpdatePositionTakingCriteria(
            int productId,
            int superId,
            IEnumerable<PTEntity> PTList,
            int agentId,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            ProductId = productId;
            SuperId = superId;
            this.PTList = PTList;
            AgentId = agentId;
            IsAgentOnly = isAgentOnly;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; }

        public int SuperId { get; }

        [SpParam(Ignored = true)]
        public IEnumerable<PTEntity> PTList { get; }

        public string PT
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("<ROOT>");
                foreach (var entity in PTList)
                {
                    builder.Append("<PT ");
                    builder.Append("SportType = \"" + entity.SportType + "\" ");
                    builder.Append("BetType = \"" + entity.BetType + "\" ");
                    var pt = entity.AgentPT;
                    if (0 <= pt && pt <= 1)
                    {
                        pt *= 100;
                    }

                    builder.Append("PT = \"" + pt + "\" ");
                    builder.Append("IsLive = \"" + (entity.IsLive ? "1" : "0") + "\" ");
                    builder.Append("/>");
                }

                builder.Append("</ROOT>");
                return builder.ToString();
            }
        }
        public int AgentId { get; }

        public bool IsAgentOnly { get; }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_PositionTakingSettingUpd";

        public override bool IsValid()
            => base.IsValid() && ProductId > 0 && AgentId > 0 && PTList != null && PTList.Any();
    }
}
