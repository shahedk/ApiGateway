using ApiGateway.Common.Constants;
using Xunit;

namespace ApiGateway.Common.Test.Constants
{
    public class ApiHttpMethodsTest
    {
        [Fact]
        public void CheckCountValue()
        { 
            Assert.Equal(ApiHttpMethods.ToList().Count, ApiHttpMethods.Count);
        }

            
        [Fact]
        public void Check_IsValid_With_Valid_Input()
        { 
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Get));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Put));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Post));
            Assert.True(ApiHttpMethods.IsValid(ApiHttpMethods.Delete));
        }

        [Fact]
        public void Check_IsValid_With_Invalid_Input()
        { 
            Assert.False(ApiHttpMethods.IsValid("some invalid text"));
        }
    }

}