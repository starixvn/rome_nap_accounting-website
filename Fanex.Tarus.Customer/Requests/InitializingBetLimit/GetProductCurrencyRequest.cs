namespace Fanex.Tarus.Customer.Requests.InitializingBetLimit
{
    using System;


    [Serializable]
    public class GetProductCurrencyRequest
    {
        public string ProductIds { get; set; }

        public string Currencies { get; set; }
    }
}
