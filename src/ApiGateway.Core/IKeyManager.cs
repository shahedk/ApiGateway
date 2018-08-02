using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IKeyManager : IManager<KeyModel>
    {
        Task<KeyModel> GetByPublicKey(string publicKey);
    }
}