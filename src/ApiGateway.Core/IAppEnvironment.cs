using System.Threading.Tasks;

namespace ApiGateway.Core
{
    public interface IAppEnvironment
    {
        Task Initialize();
    }
}