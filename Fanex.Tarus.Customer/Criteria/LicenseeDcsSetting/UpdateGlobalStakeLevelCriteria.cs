using System.Collections;
using System.Text;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class UpdateGlobalStakeLevelCriteria : NonQueryCriteria
    {
        public UpdateGlobalStakeLevelCriteria(
            int superId,
            GlobalStakeLevelEntity[] globalStakeLevels,
            int userId,
            string ip)
        {
            SuperId = superId;
            GlobalStakeLevels = globalStakeLevels;
            UserId = userId;
            IP = ip;
        }

        public GlobalStakeLevelEntity[] GlobalStakeLevels { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Dep_CustSetting_GlobalStakeLevelUpdate";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable
            {
                { "stakelevel", BuildStakeLevelXml() },
                { "custid", SuperId },
                { "userid", UserId },
                { "ip", IP },
                { "errcode", ErrCode },
                { "errmsg", ErrMsg }
            };
        }

        public override bool IsValid()
        {
            return base.IsValid() && GlobalStakeLevels != null && GlobalStakeLevels.Length > 0;
        }

        private string BuildStakeLevelXml()
        {
            var paramBuilder = new StringBuilder("<ROOT>");

            foreach (var item in GlobalStakeLevels)
            {
                paramBuilder.Append("<StakeLevel StakeLevelID=\"")
                    .Append(item.StakeLevelId)
                    .Append("\" GameTypeID=\"")
                    .Append(item.GameTypeId)
                    .Append("\" MinLabel=\"")
                    .Append(item.MinLabel)
                    .Append("\" MaxLabel=\"")
                    .Append(item.MaxLabel)
                    .Append("\" MinStake=\"")
                    .Append(item.MinStake)
                    .Append("\" MaxStake=\"")
                    .Append(item.MaxStake)
                    .Append("\"/>");
            }

            paramBuilder.Append("</ROOT>");

            return paramBuilder.ToString();
        }
    }
}
