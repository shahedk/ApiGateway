using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Key")]
    public class IsValidController : ApiControllerBase
    {
        private readonly IApiKeyValidator _keyValidator;

        public IsValidController(IApiKeyValidator keyValidator, IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
            _keyValidator = keyValidator;
        }

        [HttpGet]
        public async Task<KeyValidationResult> Get(string serviceId, string apiUrl, string httpMethod)
        {
            var clientKey = new KeyModel
            {
                Type = ApiKeyType,
                PublicKey = ApiKey,
                Properties = {[ApiKeyPropertyNames.ClientSecret] = ApiSecret}
            };

            var serviceKey = new KeyModel
            {
                Type = ServiceKeyType,
                PublicKey = ServiceApiKey,
                Properties = {[ApiKeyPropertyNames.ClientSecret] = ServiceApiSecret}
            };

            var result = await _keyValidator.IsValid(clientKey, serviceKey, httpMethod, serviceId, apiUrl);

            return result;
        }

    }
}