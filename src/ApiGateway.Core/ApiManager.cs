using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data;

namespace ApiGateway.Core
{
    public class ApiManager : IApiManager
    {
        private readonly IApiData _apiData;

        public ApiManager(IApiData apiData)
        {
            _apiData = apiData;
        }

        public Task<ApiModel> Create(string ownerPublicKey, ApiModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiModel> Update(string ownerPublicKey, ApiModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiModel> Get(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}