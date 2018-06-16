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
            var ownerKey = await GetOwnerKey();

            var key = new KeyModel
            {
                OwnerKeyId =  ownerKey.Id,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            key.Properties.Add(ApiKeyPropertyNames.ClientSecret, "Supper secret string");

            // Save key
            await keyData.Create(ownerKey.PublicKey, key);

            // Validate key
            var result = await validator.IsValid(ownerKey.PublicKey, key.PublicKey, key.Properties[ApiKeyPropertyNames.ClientSecret]);

            Assert.True(result.IsValid);
        }
    }
}
