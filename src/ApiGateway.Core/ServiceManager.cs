using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data;

namespace ApiGateway.Core
{
    public class ServiceManager : IServiceManager
    {
        private readonly IServiceData _serviceData;

        public ServiceManager(IServiceData serviceData)
        {
            _serviceData = serviceData;
        }

        public Task<ServiceModel> Create(string ownerPublicKey, ServiceModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceModel> Update(string ownerPublicKey, ServiceModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceModel> Get(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}