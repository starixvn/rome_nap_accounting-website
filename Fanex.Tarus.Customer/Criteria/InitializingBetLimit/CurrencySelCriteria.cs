namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
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
            => "Acc_InitializingGlobal_CurrencySel";
    }
}
