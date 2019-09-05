using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IApiData : IEntityData<ApiModel>
    {
        Task<ApiModel> GetByUrl(string ownerKeyId, string serviceId, string httpMethod, string apiUrl);
        Task<ApiModel> GetByName(string ownerKeyId, string serviceId, string httpMethod, string name);
        Task<ApiModel> GetByName(string serviceId, string httpMethod, string name);
        Task<bool> ExistsByName(string ownerKeyId, string serviceId, string httpMethod, string name);
        Task<bool> ExistsByUrl(string ownerKeyId, string serviceId, string httpMethod, string url);
        Task<int> Count(string ownerKeyId, string serviceId);
        Task<IList<ApiModel>> GetByService(string ownerKeyId, string serviceId);
    }
}