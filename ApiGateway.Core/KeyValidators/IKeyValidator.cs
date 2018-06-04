using ApiGateway.Common.Models;

namespace ApiGateway.Core.KeyValidators
{
    public interface IKeyValidator
    {
        KeyValidationResult IsValid(KeyModel key);
    }
}