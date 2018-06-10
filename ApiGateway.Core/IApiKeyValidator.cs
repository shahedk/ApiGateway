using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IApiKeyValidator
    {
        Task<KeyValidationResult> IsValid(KeyModel clientKey, KeyModel serviceKey, string httpMethod, string serviceId,
            string apiUrl);

        Task<KeyValidationResult> IsAllowedToManageApiGateway(KeyModel clientKey);
    }
}