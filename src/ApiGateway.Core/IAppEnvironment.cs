using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IAppEnvironment
    {
        Task<AppState> GetApplicationState();
        Task<int> GetServiceCount();
        Task<AppState> Initialize();
    }
}