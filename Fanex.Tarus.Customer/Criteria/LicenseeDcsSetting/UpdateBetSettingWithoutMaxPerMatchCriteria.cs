using System.Collections;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class UpdateBetSettingWithoutMaxPerMatchCriteria : NonQueryCriteria
    {
        public UpdateBetSettingWithoutMaxPerMatchCriteria(
            int superId,
            bool applyExistedMem,
            BetSettingWithoutMaxPerMatchEntity betSetting,
            int agentId,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            SuperId = superId;
            ApplyExistedMem = applyExistedMem;
            BetSetting = betSetting;
            AgentId = agentId;
            IsAgentOnly = isAgentOnly;
            UserId = userId;
            IP = ip;
        }

        public int SuperId { get; }

        public bool ApplyExistedMem { get; }

        public BetSettingWithoutMaxPerMatchEntity BetSetting { get; }

        public int AgentId { get; }

        public bool IsAgentOnly { get; }

        public override string GetSettingKey()
            => "Acc_Dep_CustSetting_NewBetLimitFullLineUpd";

        public override bool IsValid()
            => base.IsValid() && BetSetting != null;

        public override Hashtable GetSpParams()
        {
            string betLimit = string.Format(
                "<ROOT><BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\"/></ROOT>",
                BetSetting.SportType,
                BetSetting.BetType,
                BetSetting.MinBet,
                BetSetting.MaxBet);

            return new Hashtable
            {
                { "superid", SuperId },
                { "betlimit", betLimit },
                { "applyexistedmem", ApplyExistedMem },
                { "agentid", AgentId },
                { "isagentonly", IsAgentOnly },
                { "userid", UserId },
                { "ip", IP },
                { "errcode", ErrCode },
                { "errmsg", ErrMsg }
            };
        }
    }
}
