using System.Threading.Tasks;
using ApiGateway.Core.KeyValidators;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiGateway.Core.Test
{
    public class CoreTestBase : TestBase
    {
        protected async Task<IApiKeyValidator> GetApiKeyValidator()
        {
            var localizer = new Mock<IStringLocalizer<ApiKeyValidator>>();
            var localizer2 = new Mock<IStringLocalizer<KeySecretValidator>>();
            var logger = new Mock<ILogger<ApiKeyValidator>>();
            var logger2 = new Mock<ILogger<KeySecretValidator>>();
            var apiManager = await GetApiManager();
            var keyManager = await GetKeyManager();
            var serviceManager = await GetServiceManager();
            var keyValidator = new KeySecretValidator(keyManager, localizer2.Object, logger2.Object);
            return new ApiKeyValidator(keyValidator, localizer.Object, logger.Object, apiManager, keyManager,serviceManager);
        } 

        protected async Task<IKeyValidator> GetKeySecretValidator()
        {
            var localizer = new Mock<IStringLocalizer<KeySecretValidator>>();
            var logger = new Mock<ILogger<KeySecretValidator>>();
            var keyManager = await GetKeyManager();
            return new KeySecretValidator(keyManager, localizer.Object, logger.Object);
        }

        protected  async Task <IKeyManager> GetKeyManager()
        {
            var localizer = new Mock<IStringLocalizer<IKeyManager>>();
            var logger = new Mock<ILogger<IKeyManager>>();

            return new KeyManager(await GetKeyData(), localizer.Object, logger.Object);
        }

        protected  async Task <IRoleManager> GetRoleManager()
        {
            var localizer = new Mock<IStringLocalizer<RoleManager>>();
            var logger = new Mock<ILogger<RoleManager>>();
            var keyManager = await GetKeyManager();
            
            return new RoleManager(await GetRoleData(), localizer.Object, logger.Object, keyManager);
        }
        
        protected  async Task <IServiceManager> GetServiceManager()
        {
            var localizer = new Mock<IStringLocalizer<IServiceManager>>();
            var logger = new Mock<ILogger<IServiceManager>>();
            var keyManager = await GetKeyManager();

            return new ServiceManager(await GetServiceData(), localizer.Object, logger.Object, keyManager);
        }
        
        protected  async Task <IApiManager> GetApiManager()
        {
            var localizer = new Mock<IStringLocalizer<IApiManager>>();
            var logger = new Mock<ILogger<IApiManager>>();
            var keyManager = await GetKeyManager();
            
            return new ApiManager(await GetApiData(), localizer.Object, logger.Object, keyManager);
        }
    }
}
