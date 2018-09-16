using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using Xunit;

namespace ApiGateway.WebApi.Test
{
    public class KeyValidatorTest: WebApiTestBase
    {
        [Fact]
        public async Task IsKeySecrectValid()
        {
            var service = await GetServiceModel();
            var userKey = await GetUserKey();
            var rootKey = await GetRootKey();
            var api = await GetApiModel();
            var role  = await GetRoleModel();
            var roleData = await GetRoleData();

            // Assign api in role
            await roleData.AddApiInRole(rootKey.Id, role.Id, api.Id);

            // Assign key in role
            await roleData.AddKeyInRole(rootKey.Id, role.Id, userKey.Id);

            var controller = await GetIsValidController();

            var result = await controller.Get(service.Name, api.Url, ApiHttpMethods.Get);

            Assert.True(result.IsValid);

        }

    }
}