using ApiGateway.Common.Constants;
using Xunit;

namespace ApiGateway.Common.Test
{
    public class KeyPropertyDataTypesTest
    {
        [Fact]
        public void CheckCountValue()
        {
            Assert.Equal(PropertyDataTypes.ToList().Count, PropertyDataTypes.Count);
        }


        [Fact]
        public void CheckIsValid()
        {
            Assert.True(PropertyDataTypes.IsValid(PropertyDataTypes.Boolean));
            Assert.True(PropertyDataTypes.IsValid(PropertyDataTypes.Float));
            Assert.True(PropertyDataTypes.IsValid(PropertyDataTypes.Int));
            Assert.True(PropertyDataTypes.IsValid(PropertyDataTypes.StringArray));
            Assert.True(PropertyDataTypes.IsValid(PropertyDataTypes.String));
            Assert.False(PropertyDataTypes.IsValid("some text"));
        }
    }
}