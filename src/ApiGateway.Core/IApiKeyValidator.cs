using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IApiKeyValidator
    {
        Task<KeyValidationResult> IsValid(KeyChallenge keyChallenge, string httpMethod, string serviceName,
            string apiNameOrUrl);
    }
}