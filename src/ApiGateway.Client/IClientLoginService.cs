using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Client
{
    public interface IClientLoginService
    {
        Task<KeyValidationResult> IsClientApiKeyValidAsync(string apiKey, string apiSecret, 
            string serviceApiKey, string serviceApiSecret, 
            string serviceId, string apiUrl, string httpAction);
    }
}