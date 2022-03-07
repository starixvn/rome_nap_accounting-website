namespace Fanex.Tarus.Customer.Business.InitializingStakeLevel
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.DataAccess.InitializingStakeLevel;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingStakeLevel;

    public interface IInitializingStakeLevelService
    {
        FilterModel GetFilterData();
        IEnumerable<BaseModel> GetSportTypes(int productId);
        IEnumerable<BaseModel> GetBetTypes(int productId, string sportTypes);
    }
    public class InitializingStakeLevelService : IInitializingStakeLevelService
    {
        private const int CreditCurrency = 1;
        private const int DepositCurrency = 2;
        private readonly IInitStakeLevelDataAccess dataAccess;
        public InitializingStakeLevelService()
        {
            dataAccess = new InitStakeLevelDataAccess();
        }

        public IEnumerable<BaseModel> GetBetTypes(int productId, string sportTypes)
        {
            return dataAccess.GetBettypes(productId, sportTypes);
        }

        public FilterModel GetFilterData()
        {
            var products = dataAccess.GetProducts();
            var creditCurrencies = dataAccess.GetCurrency(CreditCurrency);
            var depositCurrencies = dataAccess.GetCurrency(DepositCurrency);

            return new FilterModel
            {
                Products = products,
                CreditCurrencies = creditCurrencies,
                DepositCurrencies = depositCurrencies
            };
        }

        public IEnumerable<BaseModel> GetSportTypes(int productId)
        {
            return dataAccess.GetSportTypes(productId);
        }
    }
}
