using System.Threading.Tasks;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.WebApi.Test
{
    public class KeyValidatorTest: WebApiTestBase
    {
        [Fact]
        public async Task IsKeySecrectValid()
        {
            var rootKey = await GetRootKey();
            var userKey = await GetUserKey();
            var service = await GetServiceModel();

            var controller = await GetIsValidController();

        }

    }
}