using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class ApiData: IApiData
    {
        public Task<ApiModel> Create(string ownerKeyId, ApiModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiModel> Update(string ownerKeyId, ApiModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiModel> Get(KeyModel key, string serviceId, string httpMethod, string apiUrl)
        {
            throw new System.NotImplementedException();
        }
    }
}