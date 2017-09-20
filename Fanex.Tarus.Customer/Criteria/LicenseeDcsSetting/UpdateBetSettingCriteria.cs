using System.Collections;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public sealed class UpdateBetSettingCriteria : NonQueryCriteria
    {
        public UpdateBetSettingCriteria(
            int superId,
            bool applyExistedMem,
            BetSetting betSetting,
            int userId,
            string ip)
        {
            SuperId = superId;
            ApplyExistedMem = applyExistedMem;
            BetSetting = betSetting;
            UserId = userId;
            IP = ip;
        }

        public bool ApplyExistedMem { get; }

        public BetSetting BetSetting { get; }

        public override string GetSettingKey()
        {
            return "Acc_Dep_CustSetting_BetLimitFullLineUpd";
        }

        public override bool IsValid()
        {
            return base.IsValid() && BetSetting != null;
        }

        public override Hashtable GetSpParams()
        {
            string betLimit = string.Format(
                "<ROOT><BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\"/></ROOT>",
                BetSetting.SportType,
                BetSetting.BetType,
                BetSetting.MinBet,
                BetSetting.MaxBet,
                BetSetting.MaxPerMatch);

            return new Hashtable
            {
                { "custid", SuperId },
                { "betlimit", betLimit },
                { "applyexistedmem", ApplyExistedMem },
                { "userid", UserId },
                { "ip", IP },
                { "errcode", ErrCode },
                { "errmsg", ErrMsg }
            };
        }
    }
}
