using System.Collections;
using System.Linq;
using System.Text;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class UpdateStakeLevelCriteria : NonQueryCriteria
    {
        public UpdateStakeLevelCriteria(
               int productId,
               int custId,
               StakeLevelEntity[] stakeLevels,
               bool isAgentOnly,
               int userId,
               string ip)
        {
            ProductId = productId;
            CustId = custId;
            StakeLevels = stakeLevels;
            IsAgentOnly = isAgentOnly;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public StakeLevelEntity[] StakeLevels { get; }

        public bool IsAgentOnly { get; }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_StakeLevelUpd";

        public override Hashtable GetSpParams()
            => new Hashtable
            {
                { "productid", ProductId },
                { "custid", CustId },
                { "stakelevel", BuildStakeLevelXml() },
                { "isagentonly", IsAgentOnly },
                { "userid", UserId },
                { "ip", IP },
                { "errcode", ErrCode },
                { "errmsg", ErrMsg }
            };

        public override bool IsValid()
            => base.IsValid() && ProductId > 0 && StakeLevels != null && StakeLevels.Length > 0;

        private string BuildStakeLevelXml()
        {
            var paramBuilder = new StringBuilder("<ROOT>");
            var groupStakeLevels = StakeLevels.ToLookup(kvp => new { kvp.SportType, kvp.BetType }, kvp => kvp);

            foreach (var item in groupStakeLevels)
            {
                var checkedItems = item.Where(sl => sl.IsChecked).Select(sl => sl.StakeLevelId);

                paramBuilder.Append("<StakeLevel SportType=\"")
                    .Append(item.Key.SportType)
                    .Append("\" BetType=\"")
                    .Append(item.Key.BetType)
                    .Append("\" StakeLevelList=\"")
                    .Append(string.Join(",", checkedItems))
                    .Append("\"/>");
            }

            paramBuilder.Append("</ROOT>");

            return paramBuilder.ToString();
        }
    }
}
