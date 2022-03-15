namespace Sunplus.Accounting.Business.ExChangeRate
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;

    internal class CasinoApiService
    {
        private string apiUrl = ConfigurationManager.AppSettings["CasinoAPI:Url"];

        internal CasinoApiResponse UpdateExchange(CasinoApiRequest request)
        {
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                return new CasinoApiResponse() { Result = new List<CasinoApiResult> { new CasinoApiResult { ErrorCode = "1", Description = "CasinoAPI:Url cannot be null or empty." } } };
            }

            //HttpClient client = new HttpClient();
            //client.BaseAddress = 
            return new CasinoApiResponse();
        }
    }

    internal class CasinoApiRequest
    {
        internal DateTime TimeStamp => DateTime.Now;

        internal string Seq => Guid.NewGuid().ToString();

        internal CasinoApiAuth Auth { get; set; }

        internal List<CasinoApiCurrencyParam> Call { get; set; }
    }

    internal class CasinoApiAuth
    {
        internal string Username => ConfigurationManager.AppSettings["CasinoAPI:Username"];

        internal string Password => ConfigurationManager.AppSettings["CasinoAPI:Password"];
    }

    internal class CasinoApiCurrencyParam
    {
        internal string CasinoNo => "01";

        internal int IBCExchangeId { get; set; }

        internal string Currency { get; set; }

        internal double ActualRate { get; set; }

        internal string ISOCode { get; set; }
    }

    internal class CasinoApiResponse
    {
        internal DateTime TimeStamp { get; set; }

        internal string Seq { get; set; }

        internal bool IsSuccess => this.Result != null && this.Result.Any(r => !r.ErrorCode.Equals("0"));

        internal List<CasinoApiResult> Result { get; set; }
    }

    internal class CasinoApiResult
    {
        internal string ErrorCode { get; set; }

        internal string Description { get; set; }
    }
}
