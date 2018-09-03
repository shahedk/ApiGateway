using ApiGateway.Client;


namespace ApiGateway.WebApi.Controllers
{
    public class AppEnvController : ApiControllerBase
    {
        public AppEnvController(IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
        }
    }
}