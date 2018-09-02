using System.Threading.Tasks;
using Xunit;

namespace ApiGateway.Core.Test
{
    public class AppInitTest : CoreTestBase
    {
        [Fact]
        public async Task RunInit()
        {
            var keyManager = await GetKeyManager();
            var serviceManager = await GetServiceManager();
            var roleManager = await GetRoleManager();
            var apiManager = await GetApiManager();
            
            var env = new AppEnvironment(keyManager, serviceManager, roleManager, apiManager);

            await env.Initialize();
            
        }
    }
}