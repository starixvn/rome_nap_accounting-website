namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using Fanex.NAP.Framework.Data;

    public class CurrencySelCriteria : CriteriaBase
    {
        public CurrencySelCriteria(int mode)
        {
            Mode = mode;
        }

        public int Mode { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_CurrencySel";
    }
}
