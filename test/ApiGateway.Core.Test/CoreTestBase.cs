using System.Threading.Tasks;
using ApiGateway.Core.KeyValidators;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiGateway.Core.Test
{
    public class CoreTestBase : TestBase
    {
        protected IApiKeyValidator GetApiKeyValidator()
        {
            var localizer = new Mock<IStringLocalizer<ApiKeyValidator>>();
            var localizer2 = new Mock<IStringLocalizer<KeySecretValidator>>();
            var logger = new Mock<ILogger<ApiKeyValidator>>();
            var logger2 = new Mock<ILogger<KeySecretValidator>>();
            var apiManager = GetApiManager();
            var keyManager =  GetKeyManager();
            var serviceManager =  GetServiceManager();
            var keyValidator = new KeySecretValidator(keyManager, localizer2.Object, logger2.Object);
            return new ApiKeyValidator(keyValidator, localizer.Object, logger.Object, apiManager, keyManager,serviceManager);
        } 

        protected IKeyValidator GetKeySecretValidator()
        {
            var localizer = new Mock<IStringLocalizer<KeySecretValidator>>();
            var logger = new Mock<ILogger<KeySecretValidator>>();
            var keyManager =  GetKeyManager();
            return new KeySecretValidator(keyManager, localizer.Object, logger.Object);
        }

        protected  IKeyManager GetKeyManager()
        {
            var localizer = new Mock<IStringLocalizer<IKeyManager>>();
            var logger = new Mock<ILogger<IKeyManager>>();
            var roleData = GetRoleData();
            
            return new KeyManager( GetKeyData(), localizer.Object, logger.Object, roleData);
        }

        protected IRoleManager GetRoleManager()
        {
            var localizer = new Mock<IStringLocalizer<RoleManager>>();
            var logger = new Mock<ILogger<RoleManager>>();
            var keyManager =  GetKeyManager();
            
            return new RoleManager( GetRoleData(), localizer.Object, logger.Object, keyManager);
        }
        
        protected IServiceManager GetServiceManager()
        {
            var localizer = new Mock<IStringLocalizer<IServiceManager>>();
            var logger = new Mock<ILogger<IServiceManager>>();
            var keyManager =  GetKeyManager();
            var roleManager = GetRoleManager();
            var apiManager = GetApiManager();

            return new ServiceManager(GetServiceData(), localizer.Object, logger.Object, keyManager, roleManager,
                apiManager);
        }
        
        protected IApiManager GetApiManager()
        {
            var localizer = new Mock<IStringLocalizer<IApiManager>>();
            var logger = new Mock<ILogger<IApiManager>>();
            var keyManager =  GetKeyManager();
            var roleData = GetRoleData();
            
            return new ApiManager( GetApiData(), localizer.Object, logger.Object, keyManager, roleData);
        }
    }
}
