using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IApiData : IEntityData<ApiModel>
    {
        Task<ApiModel> Get(string ownerPublicKey, string serviceId, string httpMethod, string apiUrl);
    }
}