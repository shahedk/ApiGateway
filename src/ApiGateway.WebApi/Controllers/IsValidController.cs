using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Key")]
    public class IsValidController : Controller
    {
        private readonly IApiKeyValidator _keyValidator;

        public IsValidController(IApiKeyValidator keyValidator)
        {
            _keyValidator = keyValidator;
        }

        [HttpGet]
        public string Get()
        {
            return ":)";
        }

        public async Task<KeyValidationResult> Get(string serviceId, string apiUrl, string httpMethod, 
            [FromHeader] string apiKey, [FromHeader] string apiSecret, [FromHeader] string serviceApiKey, [FromHeader] string serviceApiSecret)
        {
            var clientKey = new KeyModel
            {
                PublicKey = apiKey,
                Properties = {[ApiHttpHeaders.ApiSecret] = apiSecret}
            };

            var serviceKey = new KeyModel
            {
                PublicKey = serviceApiKey,
                Properties = {[ApiHttpHeaders.ApiSecret] = serviceApiSecret}
            };

            var result = await _keyValidator.IsValid(clientKey, serviceKey, httpMethod, serviceId, apiUrl);

            return result;
        }

    }
}