using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IApiKeyValidator
    {
        Task<KeyValidationResult> IsValid(KeyModel clientKey, string httpMethod, string serviceName,
            string apiNameOrUrl);
    }
}