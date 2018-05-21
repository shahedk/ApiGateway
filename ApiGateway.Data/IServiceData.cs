using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Common.Model;

namespace ApiGateway.Data
{
    public interface IServiceData
    {
        Task<ServiceModel> SaveService(string ownerKeyId, ServiceModel model);
        Task DeleteService(string ownerKeyId, string id);
    }
}
