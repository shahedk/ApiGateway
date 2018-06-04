using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IApiData : IEntityData<ApiModel>
    {
        Task<ApiModel> Get(KeyModel key, string serviceId, string httpMethod, string apiUrl);
    }
}