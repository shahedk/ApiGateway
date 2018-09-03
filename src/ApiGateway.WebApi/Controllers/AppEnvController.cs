using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;


namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/AppEnv")]
    public class AppEnvController : ApiControllerBase
    {
        private readonly IAppEnvironment _appEnvironment;
        private readonly IStringLocalizer<IAppEnvironment> _stringLocalizer;

        public AppEnvController(IApiRequestHelper apiRequestHelper, IAppEnvironment appEnvironment, IStringLocalizer<IAppEnvironment> stringLocalizer) : base(apiRequestHelper)
        {
            _appEnvironment = appEnvironment;
            _stringLocalizer = stringLocalizer;
        }
        
        [HttpGet]
        public async Task<string> Get()
        {
            var serviceCount = await _appEnvironment.GetServiceCount();

            if (serviceCount > 0)
            {
                var msgTemplate = _stringLocalizer["Total {0} services registered."];
                var msg = string.Format(msgTemplate, serviceCount);

                return msg;
            }
            else
            {
                var msg = _stringLocalizer["No service found. Please configure application environment."];
                
                return msg;
            }
        }

        [HttpPost("Initialize")]
        public async Task<string> Post()
        {
            await _appEnvironment.Initialize();

            return _stringLocalizer["System successfully initialized"];
        }

    }
}