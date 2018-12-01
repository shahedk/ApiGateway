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
            var validator =  GetKeySecretValidator();
            var keyData =  GetKeyData();
            var ownerKey = await GetRootKey();

            var key = new KeyModel
            {
                OwnerKeyId =  ownerKey.Id,
                PublicKey = ModelHelper.GeneratePublicKey(),
                Type = ApiKeyTypes.ClientSecret
            };

            var secret = "Supper secret string";

            key.Properties.Add(ApiKeyPropertyNames.ClientSecret1, secret);

            // Save key
            await keyData.Create(key);

            // Validate key
            var result = await validator.IsValid(key.PublicKey, secret);

            Assert.True(result.IsValid);
        }
    }
}
