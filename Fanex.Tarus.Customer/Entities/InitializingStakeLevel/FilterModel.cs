namespace Fanex.Tarus.Customer.Entities.InitializingStakeLevel
{
    using System.Collections.Generic;

    public class FilterModel
    {
        public IEnumerable<BaseModel> Products { get; set; }

        public IEnumerable<BaseModel> DepositCurrencies { get; set; }

        public IEnumerable<BaseModel> CreditCurrencies { get; set; }
    }
}
