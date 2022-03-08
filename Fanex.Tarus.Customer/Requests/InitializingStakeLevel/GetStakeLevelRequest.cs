namespace Fanex.Tarus.Customer.Requests.InitializingStakeLevel
{
    public class GetStakeLevelRequest
    {
        public int ProductID { get; set; }

        public string SportTypes { get; set; }

        public string BetTypes { get; set; }

        public string DepositCurrency { get; set; }

        public string CreditCurrency { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
