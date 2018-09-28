using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core.KeyValidators
{
    public class JwtValidator : IKeyValidator
    {
        public Task<KeyValidationResult> IsValid(string pubKey, string secret)
        {
            throw new System.NotImplementedException();
        }
    }
}