using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Core.KeyValidators;
using ApiGateway.Data;
using ApiGateway.Data.EFCore;
using ApiGateway.Data.EFCore.DataAccess;
using ApiGateway.Data.EFCore.Test;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            var logger = new Mock<ILogger<ApiKeyValidator>>();
            var apiData = await GetApiData();
            var keyData = await GetKeyData();
            return new ApiKeyValidator(null, localizer.Object, logger.Object, apiData, keyData);
        } 

        protected async Task<IKeyValidator> GetKeySecretValidator()
        {
            var localizer = new Mock<IStringLocalizer<KeySecretValidator>>();
            var logger = new Mock<ILogger<KeySecretValidator>>();
            var keyData = await GetKeyData();
            return new KeySecretValidator(keyData, localizer.Object, logger.Object);
        }
    }
}
