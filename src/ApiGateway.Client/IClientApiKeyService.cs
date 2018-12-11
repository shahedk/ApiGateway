using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Client
{
    public interface IClientApiKeyService
    {
        Task<KeyValidationResult> IsClientApiKeyValidAsync(string apiKey, string apiSecret,  
            string serviceName, string apiName, string httpAction);
    }
}