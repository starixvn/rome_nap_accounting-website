using System;
using System.Threading.Tasks;
using Chatbot.Client;
using RestSharp;

namespace Sunplus.Accounting.Business.Helpers
{
    public class RestClientHelper : Chatbot.Client.Helpers.IRestClient
    {
        private readonly IRestClient restClient;

        public RestClientHelper(IRestClient restClient = null)
        {
            this.restClient = restClient ?? CreateDefaultClient();
        }

        public static IRestClient CreateDefaultClient()
           => new RestClient
           {
               BaseUrl = new Uri(ChatbotClientManager.BotClientConfigs.ApiBaseUrl)
           };

        public async Task<T> ExecuteGetAsync<T>(string endPoint, object data = null)
            => await ExecuteAsync<T>(endPoint, Method.GET, data);

        public async Task<T> ExecutePostAsync<T>(string endPoint, object data = null)
            => await ExecuteAsync<T>(endPoint, Method.POST, data);

        private async Task<T> ExecuteAsync<T>(string resource, Method method, object data)
        {
            ValidateResource(resource);

            var request = new RestRequest(resource, method);

            if (data != null)
            {
                request.AddJsonBody(data);
            }

            var response = await restClient.ExecuteTaskAsync<T>(request).ConfigureAwait(false);

            if (IsSuccessfulResponse(response))
            {
                return response.Data;
            }

            throw new InvalidOperationException(
                string.Join(
                    "\r\n",
                    $"Content: {response.Content}",
                    $"ErrorMessage: {response.ErrorMessage}"));
        }

        private static void ValidateResource(string endPoint)
        {
            if (string.IsNullOrWhiteSpace(endPoint))
            {
                throw new ArgumentNullException(nameof(endPoint));
            }
        }

        private static bool IsSuccessfulResponse(IRestResponse response)
            => (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299
                && response.ResponseStatus == ResponseStatus.Completed;
    }
}