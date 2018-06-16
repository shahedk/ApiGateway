using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IKeyData : IEntityData<KeyModel>
    {
        Task<KeyModel> GetByPublicKey(string publicKey);
    }
}