using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IApiManager : IManager<ApiModel>
    {
        Task<ApiModel> GetByApiName(string ownerPublicKey, string serviceId, string httpMethod, string apiName);
        Task<ApiModel> GetByApiUrl(string ownerPublicKey, string serviceId, string httpMethod, string apiUrl);
    }
}