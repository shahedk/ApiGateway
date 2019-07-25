using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/IsValid")]
    public class IsValidController : ApiControllerBase
    {
        private readonly IApiKeyValidator _keyValidator;

        public IsValidController(IApiKeyValidator keyValidator, IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
            _keyValidator = keyValidator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(KeyValidationResult))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string id,string api, string httpMethod)
        {
            if (string.IsNullOrWhiteSpace(api))
            {
                api = "";
            }
            if (string.IsNullOrWhiteSpace(id) ||                
                string.IsNullOrWhiteSpace(httpMethod))
            {
                return BadRequest();
            }
            
            var serviceName = id.ToLower();
         
            var challenge = new KeyChallenge
            {
                Type = ApiKeyType,
                Properties = {[ApiKeyPropertyNames.ClientSecret1] = ApiSecret, [ApiKeyPropertyNames.PublicKey] = ApiKey}
            };

            var result = await _keyValidator.IsValid(challenge, httpMethod, serviceName, api);

            return Ok(result.ToLite());
        }

    }
}