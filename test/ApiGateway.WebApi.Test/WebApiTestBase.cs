using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Extensions;
using ApiGateway.Core.Test;
using ApiGateway.WebApi.Controllers;

namespace ApiGateway.WebApi.Test
{
    public class WebApiTestBase : CoreTestBase
    {
        private async Task<IApiRequestHelper> GetApiRequestHelperWithRootKey()
        {
            var rootKey = await GetRootKey();
            return new MockApiRequestHelper(rootKey.PublicKey, rootKey.GetSecret1());
        }

        private async Task<IApiRequestHelper> GetApiRequestHelperWithUserKeyAndServiceKey()
        {
            var userKey = await GetUserKey();
            var serviceKey = await GetRootKey();

            return new MockApiRequestHelper(userKey.PublicKey, userKey.GetSecret1(), serviceKey.PublicKey, serviceKey.GetSecret1());
        }

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
            return new IsValidController(await GetApiKeyValidator(), await GetApiRequestHelperWithUserKeyAndServiceKey());
        }
    }
}