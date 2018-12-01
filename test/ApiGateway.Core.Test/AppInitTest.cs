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
            var keyManager =  GetKeyManager();
            var serviceManager =  GetServiceManager();
            var roleManager =  GetRoleManager();
            var apiManager = GetApiManager();
            var mock = new Mock<IStringLocalizer<IAppEnvironment>>();

            AddKey(mock, "There are {0} active service(s). System can not be re-initialized.");
            AddKey(mock, "No service found. Please configure application environment.");
            AddKey(mock, "Total {0} services registered.");
            AddKey(mock,
                "System initialized. Please save the key in secured place. ApiKey: {0} | ApiSecret1: {1} |  ApiSecret2: {2}");

            var env = new AppEnvironment(keyManager, serviceManager, roleManager, apiManager, mock.Object);

            var state = await env.GetApplicationState();

            if (!state.IsConfigured)
            {
                await env.Initialize();
                
                // Check current state after initialization
                state = await env.GetApplicationState();
            }

            Assert.True(state.IsConfigured);
        }

        private void AddKey(Mock<IStringLocalizer<IAppEnvironment>> mock, string key)
        {
            var localizedString = new LocalizedString(key, key);
            mock.Setup(_ => _[key]).Returns(localizedString);

        }
    }
}