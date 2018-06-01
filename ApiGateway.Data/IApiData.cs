using System.Threading.Tasks;
using ApiGateway.Common.Model;

namespace ApiGateway.Data
{
    public interface IApiData
    {
        Task<ApiModel> SaveApi(KeyModel key, ApiModel model);
        void DeleteApi(KeyModel key, ApiModel model);
    }
}