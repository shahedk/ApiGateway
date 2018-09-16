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
            var mock = new Mock<IStringLocalizer<IAppEnvironment>>();

            AddKey(mock, "There are {0} active service(s). System can not be re-initialized.");
            AddKey(mock, "No service found. Please configure application environment.");
            AddKey(mock, "Total {0} services registered.");

            var env = new AppEnvironment(keyManager, serviceManager, roleManager, apiManager, mock.Object);

            var state = await env.GetApplicationState();

            if (!state.IsConfigured)
            {
                await env.Initialize();
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