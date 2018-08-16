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
                PublicKey = ApiKey,
                Properties = {[ApiHttpHeaders.ApiSecret] = ApiSecret}
            };

            var serviceKey = new KeyModel
            {
                PublicKey = ServiceApiKey,
                Properties = {[ApiHttpHeaders.ApiSecret] = ServiceApiSecret}
            };

            var result = await _keyValidator.IsValid(clientKey, serviceKey, httpMethod, serviceId, apiUrl);

            return result;
        }

    }
}