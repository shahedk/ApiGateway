using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IAccountData : IEntityData<AccountModel>
    {
        Task<AccountModel> GetByLoginName(string ownerKeyId, string loginName);
    }
}
