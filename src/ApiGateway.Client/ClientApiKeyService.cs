using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApiGateway.Common;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ApiGateway.Client
{
    public class ClientApiKeyService : IClientApiKeyService
    {
        private readonly ApiGatewaySettings _settings;
        private readonly IHttpClientFactory _clientFactory;

        public ClientApiKeyService(IOptions<ApiGatewaySettings> settings, IHttpClientFactory clientFactory)
        {
            _settings = settings.Value;
            _clientFactory = clientFactory;
        }

        public async Task<KeyValidationResult> IsClientApiKeyValidAsync(string apiKey, string apiSecret,
            string serviceName, string apiName, string httpAction)
        {
            var validationResult = new KeyValidationResult();

            try
            {
                var url = _settings.AuthApiEndPoint;

                if (!url.EndsWith("/", StringComparison.Ordinal))
                {
                    url += "/";
                }

                url += $"{serviceName}?api={apiName}&httpMethod={httpAction}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add(ApiHttpHeaders.ApiKey, apiKey);
                request.Headers.Add(ApiHttpHeaders.ApiSecret, apiSecret);

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                validationResult = JsonConvert.DeserializeObject<KeyValidationResult>(content);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    validationResult.IsValid = false;
                    validationResult.Message += Environment.NewLine + " Unexpected status code: " +
                                                response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                validationResult.IsValid = false;
                validationResult.Message = ex.Message;
            }

            return validationResult;
        }
    }
}