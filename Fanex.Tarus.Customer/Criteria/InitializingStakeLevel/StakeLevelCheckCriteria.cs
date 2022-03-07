namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using Fanex.NAP.Framework.Data;

    public class StakeLevelCheckCriteria : CriteriaBase
    {
        public StakeLevelCheckCriteria(string inputData)
        {
            InputData = inputData;
        }

        public string InputData { get; set; }

        public int NotiCode { get; set; }

        public string NotiMsg { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_StakelevelCheck";
    }
}
