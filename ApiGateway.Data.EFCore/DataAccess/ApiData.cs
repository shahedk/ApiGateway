using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class ApiData: IApiData
    {
        public Task<ApiModel> SaveApi(KeyModel key, ApiModel model)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteApi(KeyModel key, ApiModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}