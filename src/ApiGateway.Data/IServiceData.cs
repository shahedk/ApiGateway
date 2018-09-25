using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IServiceData: IEntityData<ServiceModel>
    {
        Task<ServiceModel> GetByName(string serviceName);
        Task<ServiceModel> GetByName(string ownerKeyId, string serviceName);
        Task<int> Count();
        Task<bool> Exists(string serviceName);
    }
}
