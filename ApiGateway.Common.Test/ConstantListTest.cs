using System;
using ApiGateway.Common.Constants;
using Xunit;

namespace ApiGateway.Common.Test
{
    public class ConstantListTest
    {
        [Fact]
        public void Check_ApiKeyTypes()
        { 
            Assert.Equal(ApiKeyTypes.ToList().Count, ApiKeyTypes.Count);

            Assert.True(ApiKeyTypes.IsValid(ApiKeyTypes.ClientSecret));
            Assert.True(ApiKeyTypes.IsValid(ApiKeyTypes.JwtToken));
            Assert.False(ApiKeyTypes.IsValid("some text"));
        }

        [Fact]
        public void Check_ApiHttpMethods()
        {
            Assert.Equal(ApiHttpMethods.ToList().Count, ApiHttpMethods.Count);
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Get));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Put));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Post));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Delete));
            Assert.False(ApiHttpMethods.IsValid("some text"));
        }

        
        [Fact]
        public void Check_KeyPropertyDataTypes()
        {
            Assert.Equal(KeyPropertyDataTypes.ToList().Count, KeyPropertyDataTypes.Count);
            Assert.True(KeyPropertyDataTypes.IsValid(KeyPropertyDataTypes.Boolean));
            Assert.True(KeyPropertyDataTypes.IsValid(KeyPropertyDataTypes.Float));
            Assert.True(KeyPropertyDataTypes.IsValid(KeyPropertyDataTypes.Int));
            Assert.True(KeyPropertyDataTypes.IsValid(KeyPropertyDataTypes.StringArray));
            Assert.True(KeyPropertyDataTypes.IsValid(KeyPropertyDataTypes.String));
            Assert.False(KeyPropertyDataTypes.IsValid("some text"));
        }
    }
}
