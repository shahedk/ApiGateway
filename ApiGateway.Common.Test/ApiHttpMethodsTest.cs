using ApiGateway.Common.Constants;
using Xunit;

namespace ApiGateway.Common.Test
{
    public class ApiHttpMethodsTest
    {
        [Fact]
        public void CheckCountValue()
        { 
            Assert.Equal(ApiHttpMethods.ToList().Count, ApiHttpMethods.Count);
        }

            
        [Fact]
        public void CheckIsValid()
        { 
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Get));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Put));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Post));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Delete));
            Assert.False(ApiHttpMethods.IsValid("some text"));
        }
    }

}