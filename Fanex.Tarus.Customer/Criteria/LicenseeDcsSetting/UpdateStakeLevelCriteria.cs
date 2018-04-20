using System.Collections;
using System.Linq;
using System.Text;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class UpdateStakeLevelCriteria : NonQueryCriteria
    {
        public UpdateStakeLevelCriteria(
               int productId,
               int superId,
               StakeLevelEntity[] stakeLevels,
               int userId,
               string ip)
        {
            ProductId = productId;
            SuperId = superId;
            StakeLevels = stakeLevels;
            UserId = userId;
            IP = ip;
        }

        public int ProductId { get; set; }

        public StakeLevelEntity[] StakeLevels { get; }

        public override string GetSettingKey()
        {
            return "Acc_Dep_CustSetting_StakeLevelUpd";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable
            {
                { "productid", ProductId },
                { "custid", SuperId },
                { "stakelevel", BuildStakeLevelXml() },
                { "userid", UserId },
                { "ip", IP },
                { "errcode", ErrCode },
                { "errmsg", ErrMsg }
            };
        }

        public override bool IsValid()
        {
            return base.IsValid() && ProductId > 0 && StakeLevels != null && StakeLevels.Length > 0;
        }

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
