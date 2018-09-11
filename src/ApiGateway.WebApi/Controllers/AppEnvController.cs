using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("sys/AppEnv")]
    public class AppEnvController : ApiControllerBase
    {
        private readonly IAppEnvironment _appEnvironment;

        public AppEnvController(IApiRequestHelper apiRequestHelper, IAppEnvironment appEnvironment) : base(apiRequestHelper)
        {
            _appEnvironment = appEnvironment;
            
        }
        
        [HttpGet]
        public async Task<AppState> Get()
        {
            return await _appEnvironment.GetApplicationState();
        }

        [HttpPost("Initialize")]
        public async Task<AppState> Post()
        {
            return await _appEnvironment.Initialize();
        }
    }
}