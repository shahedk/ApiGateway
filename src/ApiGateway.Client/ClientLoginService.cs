using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApiGateway.Common;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ApiGateway.Client
{
    public class ClientLoginService:IClientLoginService
    {
        private readonly ApiGatewaySettings _settings;

        public ClientLoginService(IOptions<ApiGatewaySettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<KeyValidationResult> IsClientApiKeyValidAsync(string apiKey, string apiSecret, string serviceApiKey, string serviceApiSecret,
            string serviceName, string apiUrl, string httpAction)
        {
            var validationResult = new KeyValidationResult();

            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add(ApiHttpHeaders.ApiKey, apiKey);
                client.DefaultRequestHeaders.Add(ApiHttpHeaders.ApiSecret, apiSecret);
                client.DefaultRequestHeaders.Add(ApiHttpHeaders.ServiceApiKey, serviceApiKey);
                client.DefaultRequestHeaders.Add(ApiHttpHeaders.ServiceApiSecret, serviceApiSecret);
                
                var url = _settings.AuthApiEndPoint;

                if (!url.EndsWith("/"))
                {
                    url += "/";
                }

                url += $"{serviceName}?apiurl={apiUrl}&httpMethod={httpAction}";
                
                var responseMessage =
                    await client.GetAsync(url);

                var content = await responseMessage.Content.ReadAsStringAsync();
                validationResult = JsonConvert.DeserializeObject<KeyValidationResult>(content);

                if (responseMessage.StatusCode != HttpStatusCode.OK)
                {
                    validationResult.IsValid = false;
                    validationResult.Message += Environment.NewLine + " Unexpected status code: " +
                                                responseMessage.StatusCode;
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