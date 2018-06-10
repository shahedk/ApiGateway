using System.Threading.Tasks;
using ApiGateway.Common.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class ApiData: IApiData
    {
        public ApiData(ApiGatewayContext context, IStringLocalizer<ApiData> localizer, ILogger<ApiData> logger)
        {

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

        public Task<ApiModel> Get(KeyModel key, string serviceId, string httpMethod, string apiUrl)
        {
            throw new System.NotImplementedException();
        }
    }
}