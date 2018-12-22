using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IServiceManager : IManager<ServiceModel>
    {
        Task<IList<ServiceSummaryModel>> GetAllSummary(string ownerPublicKey);
        
        Task<ServiceModel> GetByName(string ownerPublicKey, string serviceName);
        
        Task<int> Count();
    }
}