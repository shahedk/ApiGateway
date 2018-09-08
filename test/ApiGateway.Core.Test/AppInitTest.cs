using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Moq;
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
            var localizer = new Mock<IStringLocalizer<IAppEnvironment>>();
            
            var env = new AppEnvironment(keyManager, serviceManager, roleManager, apiManager, localizer.Object);

            await env.Initialize();
            
        }
    }
}