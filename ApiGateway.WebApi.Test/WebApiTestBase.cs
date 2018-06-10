using System.Threading.Tasks;
using ApiGateway.Core.Test;
using ApiGateway.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using Moq;

namespace ApiGateway.WebApi.Test
{
    public class WebApiTestBase : CoreTestBase
    {
        public async Task<ServiceController> GetServiceController()
        {
            var localizer = new Mock<IStringLocalizer<ServiceController>>();
            var keyData = await GetKeyData();
            var keyValidator = await GetApiKeyValidator();
            var serviceData = await GetServiceData();
            var serviceController = new ServiceController(serviceData, keyValidator, keyData, localizer.Object);

            return serviceController;
        }
    }
}