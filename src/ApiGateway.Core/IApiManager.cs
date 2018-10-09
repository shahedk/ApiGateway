using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IApiManager : IManager<ApiModel>
    {
        Task<ApiModel> Get(string ownerPublicKey, string serviceId, string httpMethod, string apiName);        
    }
}