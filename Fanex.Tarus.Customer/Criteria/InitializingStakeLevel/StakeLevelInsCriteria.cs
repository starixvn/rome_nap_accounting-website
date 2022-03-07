namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using Fanex.NAP.Framework.Data;

    public class StakeLevelInsCriteria : CriteriaBase
    {
        public StakeLevelInsCriteria(string inputData)
        {
            InputData = inputData;
        }

        public string InputData { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_StakelevelIns";
    }
}
