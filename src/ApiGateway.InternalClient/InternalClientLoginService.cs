using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;

namespace ApiGateway.InternalClient
{
    public class InternalClientLoginService : IClientLoginService
    {
        public Task<KeyValidationResult> IsClientApiKeyValidAsync(string apiKey, string apiSecret, string serviceApiKey, string serviceApiSecret,
            string serviceId, string apiUrl, string httpAction)
        {
            throw new NotImplementedException();
        }
    }
}
