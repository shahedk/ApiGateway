using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Core.Test;
using ApiGateway.WebApi.Controllers;

namespace ApiGateway.WebApi.Test
{
    public class WebApiTestBase : CoreTestBase
    {
        private async Task<IApiRequestHelper> GetApiRequestHelperWithRootKey()
        {
            var rootKey = await GetRootKey();
            return new MockApiRequestHelper(rootKey.PublicKey);
        }

        //private async Task<IApiRequestHelper> GetApiRequestHelperWithUserKey()
        //{
        //    var rootKey = await GetUserKey();
        //    return new MockApiRequestHelper(rootKey.PublicKey);
        //}

        protected async Task<KeyController> GetKeyController()
        {
            return new KeyController(await GetKeyManager(), await GetApiRequestHelperWithRootKey());
        }

        protected async Task<ServiceController> GetServiceController()
        {
            return new ServiceController(await GetServiceManager(), await GetApiRequestHelperWithRootKey());
        }

        protected async Task<ApiController> GetApiController()
        {
            return new ApiController(await GetApiManager(), await GetApiRequestHelperWithRootKey());
        }

        protected async Task<RoleController> GetRoleController()
        {
            return new RoleController(await GetRoleManager(), await GetApiRequestHelperWithRootKey());
        }

        protected async Task<IsValidController> GetIsValidController()
        {
            return new IsValidController(await GetApiKeyValidator(), await GetApiRequestHelperWithRootKey());
        }
    }
}