namespace Sunplus.Accounting.Business.ExChangeRate
{
    using Fanex.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    internal class CasinoApiService
    {
        private readonly string apiUrl = ConfigurationManager.AppSettings["CasinoAPI:Url"];
        
        internal CasinoApiResponse UpdateExchange(CasinoApiRequest request)
            => Task.Run(() => InvokeAPI(request)).Result;

        internal async Task<CasinoApiResponse> InvokeAPI(CasinoApiRequest request)
        {
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                Logger.Log.Error("Casino API Error: </br>Request: " + SerializeCasinoAPIObject(request) + "</br>Response: CasinoAPI:Url cannot be null or empty.");
                return new CasinoApiResponse() { Result = new List<CasinoApiResult> { new CasinoApiResult { ErrorCode = "1", Description = "CasinoAPI:Url cannot be null or empty." } } };
            }

            var content = new StringContent(SerializeCasinoAPIObject(request), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var casinoApiResponse = JsonConvert.DeserializeObject<CasinoApiResponse>(responseString);
            request.Auth = null; // Remove sensitive info.
            if (!casinoApiResponse.IsSuccess)
            {
                Logger.Log.Error("Casino API Error: </br>Request: " + SerializeCasinoAPIObject(request) + "</br>Response: " + responseString);
            }
            else
            {
                Logger.Log.Info("Casino API Called: </br>Request: " + SerializeCasinoAPIObject(request) + "</br>Response: " + responseString);
            }

            return casinoApiResponse;
        }

        internal string SerializeCasinoAPIObject(object obj)
        {

            JsonSerializerSettings serializeSetting = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd hh:mm:ss.fff",
                
            };

            return JsonConvert.SerializeObject(obj, serializeSetting);
        }
    }

    internal class CasinoApiRequest
    {
        internal CasinoApiRequest()
        {
            Call = new List<CasinoApiCurrencyParam>();
            Auth = new CasinoApiAuth();
        }

        [JsonProperty]
        internal DateTime TimeStamp => DateTime.Now;

        [JsonProperty]
        internal string Seq => Guid.NewGuid().ToString();

        [JsonProperty]
        internal CasinoApiAuth Auth { get; set; }

        [JsonProperty]
        internal List<CasinoApiCurrencyParam> Call { get; set; }
    }

    internal class CasinoApiAuth
    {
        [JsonProperty]
        internal string Username => ConfigurationManager.AppSettings["CasinoAPI:Username"];

        [JsonProperty]
        internal string Password => ConfigurationManager.AppSettings["CasinoAPI:Password"];
    }

    internal class CasinoApiCurrencyParam
    {
        [JsonProperty]
        internal string CasinoNo => "01";

        [JsonProperty]
        internal int IBCExchangeId { get; set; }

        [JsonProperty]
        internal string Currency { get; set; }

        [JsonProperty]
        internal double ActualRate { get; set; }

        [JsonProperty]
        internal string ISOCode { get; set; }
    }

    internal class CasinoApiResponse
    {
        [JsonProperty]
        internal DateTime TimeStamp { get; set; }

        [JsonProperty]
        internal string Seq { get; set; }

        internal bool IsSuccess => this.Result != null && !this.Result.Any(r => !r.ErrorCode.Equals("0"));

        [JsonProperty]
        internal List<CasinoApiResult> Result { get; set; }
    }

    internal class CasinoApiResult
    {
        [JsonProperty]
        internal string ErrorCode { get; set; }

        [JsonProperty]
        internal string Description { get; set; }
    }
}
