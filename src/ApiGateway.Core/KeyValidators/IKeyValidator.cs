using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core.KeyValidators
{
    public interface IKeyValidator
    {
        Task<KeyValidationResult> IsValid(string ownerKeyId, string pubKey, string secret);
    }
}