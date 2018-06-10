using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.Data.EFCore.Test
{
    public class KeyDataTest : TestBase
    {
        [Fact]
        public async Task CreateKey()
        {
            var data = await GetKeyData();

            var key = new KeyModel() {Id = ModelHelper.GenerateNewId(), PublicKey = ModelHelper.GenerateNewId(), Type = ApiKeyTypes.ClientSecret};

            var savedKey = await data.Create(string.Empty, key);

            Assert.Equal(key.PublicKey, savedKey.PublicKey);
        }
        
    }
}