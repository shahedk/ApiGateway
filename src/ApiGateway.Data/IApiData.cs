using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IApiData : IEntityData<ApiModel>
    {
        Task<ApiModel> Get(string ownerKeyId, string serviceId, string httpMethod, string apiUrl);
    }
}