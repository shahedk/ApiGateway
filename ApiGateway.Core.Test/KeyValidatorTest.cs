using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Core.KeyValidators;
using Xunit;

namespace ApiGateway.Core.Test
{
    public class KeyValidatorTest : CoreTestBase
    {
        [Fact]
        public async Task ClientSecretKeyTest()
        {
            var validator = await GetKeySecretValidator();
            var keyData = await GetKeyData();

            var ownerId = ModelHelper.GenerateNewId();
            
            var key = new KeyModel
            {
                OwnerKeyId =  ownerId,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            key.Properties.Add(ApiKeyPropertyNames.ClientSecret, "Supper secret string");

            // Save key
            await keyData.Create(ownerId, key);

            // Validate key
            var result = await validator.IsValid(ownerId, key.PublicKey, key.Properties[ApiKeyPropertyNames.ClientSecret]);

            Assert.True(result.IsValid);
        }
    }
}
