using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IServiceData
    {
        Task<ServiceModel> SaveService(string ownerKeyId, ServiceModel model);
        Task DeleteService(string ownerKeyId, string id);
    }
}
